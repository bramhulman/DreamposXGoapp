Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports DreamposXGoapp.Models

Namespace Services
    ' Memakai GoappRedeemLog agar tidak menambah tabel ketiga.
    Friend Class OrderSubmissionJournal
        Friend Class Entry
            Public Property CreatedAtUtc As DateTime = DateTime.UtcNow
            Public Property Id As Long
            Public Property IsNew As Boolean
            Public Property State As String
            Public Property RequestJson As String
            Public Property ResponseJson As String
            Public Property StatusCode As Integer
            Public Property ApiLogSaved As Boolean
        End Class

        Private ReadOnly _repository As GoappRedeemRepository
        Private Shared ReadOnly SchemaLock As New Threading.SemaphoreSlim(1, 1)
        Private Shared ReadOnly CheckedSchemas As New HashSet(Of String)()

        Public Sub New(repository As GoappRedeemRepository)
            _repository = repository
        End Sub

        Private Async Function EnsureSchemaAsync() As Task
            Dim schemaKey = _repository.ConnectionString & vbNullChar & _repository.TableName
            Await SchemaLock.WaitAsync().ConfigureAwait(False)
            Try
                If CheckedSchemas.Contains(schemaKey) Then Return
                Await _repository.EnsureTableExistsAsync().ConfigureAwait(False)
                Using conn As New SqlConnection(_repository.ConnectionString)
                    Await conn.OpenAsync().ConfigureAwait(False)
                    Dim table = _repository.TableName
                    ' Serialisasi migrasi antar proses menggunakan application lock SQL Server.
                    Dim sql = "SET XACT_ABORT ON; BEGIN TRAN; DECLARE @LockResult int; " &
                              "EXEC @LockResult = sys.sp_getapplock @Resource=@SchemaLock, @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=10000; " &
                              "IF @LockResult < 0 BEGIN ROLLBACK; RAISERROR('Order log schema lock failed',16,1); RETURN; END; " &
                              $"IF COL_LENGTH('{table}', 'OrderSubmissionKey') IS NULL ALTER TABLE {table} ADD [OrderSubmissionKey] varchar(64) NULL; " &
                              $"IF COL_LENGTH('{table}', 'OrderSubmissionState') IS NULL ALTER TABLE {table} ADD [OrderSubmissionState] varchar(16) NULL; " &
                              $"IF COL_LENGTH('{table}', 'OrderRequestJson') IS NULL ALTER TABLE {table} ADD [OrderRequestJson] nvarchar(max) NULL; " &
                              $"IF COL_LENGTH('{table}', 'OrderResponseJson') IS NULL ALTER TABLE {table} ADD [OrderResponseJson] nvarchar(max) NULL; " &
                              $"IF COL_LENGTH('{table}', 'OrderHttpStatus') IS NULL ALTER TABLE {table} ADD [OrderHttpStatus] int NULL; " &
                              $"IF COL_LENGTH('{table}', 'OrderApiLogSaved') IS NULL ALTER TABLE {table} ADD [OrderApiLogSaved] bit NULL; " &
                              $"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID('{table}') AND name='UX_OrderSubmissionKey') " &
                              $"EXEC('CREATE UNIQUE INDEX [UX_OrderSubmissionKey] ON {table}([OrderSubmissionKey]) WHERE [OrderSubmissionKey] IS NOT NULL'); COMMIT;"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@SchemaLock", "GoappOrderSchema:" & table)
                        Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
                    End Using
                End Using
                CheckedSchemas.Add(schemaKey)
            Finally
                SchemaLock.Release()
            End Try
        End Function

        Public Async Function ReserveAsync(key As String, order As PushOrderRequest, requestJson As String, guest As Boolean) As Task(Of Entry)
            Await EnsureSchemaAsync().ConfigureAwait(False)
            Using conn As New SqlConnection(_repository.ConnectionString)
                Await conn.OpenAsync().ConfigureAwait(False)
                Dim sql = $"INSERT INTO {_repository.TableName} " &
                          "([Waktu],[Jenis],[Kode],[NamaVoucher],[NilaiPoint],[Status],[StatusPos],[TransactionRef],[MemberUid],[CreatedAt],[UpdatedAt],[OrderSubmissionKey],[OrderSubmissionState],[OrderRequestJson]) " &
                          "VALUES (GETDATE(),@Jenis,@Ref,@Name,0,0,1,@Ref,@Member,GETDATE(),GETDATE(),@Key,'Pending',@Json); SELECT CONVERT(bigint,SCOPE_IDENTITY());"
                Dim duplicate As Boolean = False
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Jenis", "SalesOrder")
                    cmd.Parameters.AddWithValue("@Ref", order.ProviderRef)
                    cmd.Parameters.AddWithValue("@Name", "Sales order")
                    cmd.Parameters.AddWithValue("@Member", If(order.Member Is Nothing, CType(DBNull.Value, Object), Convert.ToString(order.Member.Uid)))
                    cmd.Parameters.AddWithValue("@Key", key)
                    cmd.Parameters.AddWithValue("@Json", requestJson)
                    Try
                        Dim id = Convert.ToInt64(Await cmd.ExecuteScalarAsync().ConfigureAwait(False))
                        Return New Entry With {.Id = id, .IsNew = True, .State = "Pending", .RequestJson = requestJson}
                    Catch ex As SqlException When ex.Number = 2601 OrElse ex.Number = 2627
                        duplicate = True
                    End Try
                End Using
                If duplicate Then
                    Using cmd As New SqlCommand($"SELECT [Id],[OrderSubmissionState],[OrderRequestJson],[OrderResponseJson],[OrderHttpStatus],[OrderApiLogSaved] FROM {_repository.TableName} WHERE [OrderSubmissionKey]=@Key", conn)
                        cmd.Parameters.AddWithValue("@Key", key)
                        Using reader = Await cmd.ExecuteReaderAsync().ConfigureAwait(False)
                            If Await reader.ReadAsync().ConfigureAwait(False) Then
                                Return New Entry With {
                                    .Id = reader.GetInt64(0), .State = Convert.ToString(reader(1)),
                                    .RequestJson = Convert.ToString(reader(2)), .ResponseJson = Convert.ToString(reader(3)),
                                    .StatusCode = If(reader.IsDBNull(4), 0, reader.GetInt32(4)), .ApiLogSaved = Not reader.IsDBNull(5) AndAlso reader.GetBoolean(5)
                                }
                            End If
                        End Using
                    End Using
                End If
                Throw New InvalidOperationException("Reservasi transaksi tidak dapat dibaca; request tidak dikirim.")
            End Using
        End Function

        Public Async Function CompleteAsync(id As Long, result As ApiResponse(Of PushOrderResponse)) As Task
            Dim state = If(result.RequiresReconciliation OrElse result.StatusCode = 0 OrElse result.StatusCode >= 500, "Unknown", If(result.IsSuccess, "Succeeded", "Failed"))
            Using conn As New SqlConnection(_repository.ConnectionString)
                Await conn.OpenAsync().ConfigureAwait(False)
                Using cmd As New SqlCommand($"UPDATE {_repository.TableName} SET [OrderSubmissionState]=@State,[OrderResponseJson]=@Json,[OrderHttpStatus]=@Status,[OrderApiLogSaved]=@ApiLogSaved,[NilaiPoint]=@Points,[UpdatedAt]=GETDATE() WHERE [Id]=@Id", conn)
                    cmd.Parameters.AddWithValue("@Id", id)
                    cmd.Parameters.AddWithValue("@State", state)
                    cmd.Parameters.AddWithValue("@Json", If(result.RawJson, String.Empty))
                    cmd.Parameters.AddWithValue("@Status", result.StatusCode)
                    cmd.Parameters.AddWithValue("@ApiLogSaved", result.ApiLogSaved)
                    cmd.Parameters.AddWithValue("@Points", If(result.Data Is Nothing, 0D, result.Data.TotalEarnedPoints))
                    If Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False) <> 1 Then Throw New InvalidOperationException("Update jurnal transaksi gagal.")
                End Using
                Await LogRetention.CleanupAsync(conn, _repository.TableName, "[Waktu]").ConfigureAwait(False)
            End Using
        End Function
    End Class
End Namespace