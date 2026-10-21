Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks

Namespace Services
    ''' <summary>
    ''' Model data untuk entitas tabel [dbo].[GoappRedeemLog]
    ''' </summary>
    Public Class GoappRedeemLogItem
        Public Property Id As Long = 0
        Public Property Waktu As DateTime = DateTime.Now
        Public Property Jenis As String = String.Empty ' 'RedeemVoucher' atau 'BurnPoint'
        Public Property Kode As String = String.Empty ' Kode voucher atau payment_ref
        Public Property NamaVoucher As String = String.Empty ' Nama voucher atau 'GoappPoint'
        Public Property NilaiPoint As Decimal = 0D ' Nilai diskon atau jumlah poin yang digunakan
        Public Property Status As Short = 0 ' 0 = Digunakan/Burn, 1 = Cancel
        Public Property StatusPos As Short = 0 ' 0 = Belum masuk POS, 1 = Sudah masuk POS
        Public Property TransactionRef As String = String.Empty ' Nomor transaksi POS (SM_Number)
        Public Property MemberUid As String = String.Empty
        Public Property MemberName As String = String.Empty
        Public Property CreatedAt As DateTime = DateTime.Now
        Public Property UpdatedAt As DateTime = DateTime.Now

        Public ReadOnly Property StatusText As String
            Get
                Return If(Status = 0, "Digunakan", "Dibatalkan")
            End Get
        End Property

        Public ReadOnly Property StatusPosText As String
            Get
                Return If(StatusPos = 0, "Belum Masuk POS", "Sudah Masuk POS")
            End Get
        End Property

        Public ReadOnly Property CanCancelFromDll As Boolean
            Get
                Return (Status = 0)
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Repository untuk mengelola riwayat pencatatan dan status Redeem Voucher & Burn Point
    ''' pada tabel [dbo].[GoappRedeemLog] di database POS.
    ''' Menyediakan metode Synchronous dan Asynchronous (ConfigureAwait False) untuk mencegah UI deadlock.
    ''' </summary>
    Public Class GoappRedeemRepository
        Private Shared _tableChecked As Boolean = False
        Private Shared ReadOnly _tableCheckLock As New Object()

        Public Property ConnectionString As String = String.Empty
        Public Property TableName As String = "[dbo].[GoappRedeemLog]"

        Public Sub New(Optional customConnectionString As String = Nothing, Optional tableName As String = "[dbo].[GoappRedeemLog]")
            Me.ConnectionString = If(customConnectionString, String.Empty)
            Me.TableName = If(String.IsNullOrWhiteSpace(tableName), "[dbo].[GoappRedeemLog]", tableName)
        End Sub

        Private Function GetEffectiveConnStr(customConnStr As String) As String
            Return If(Not String.IsNullOrWhiteSpace(customConnStr), customConnStr, Me.ConnectionString)
        End Function

        Private Function GetCreateTableSql() As String
            Return $"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{Me.TableName}') AND type in (N'U')) " &
                   "BEGIN " &
                   $"    CREATE TABLE {Me.TableName}( " &
                   "        [Id] [bigint] IDENTITY(1,1) NOT NULL, " &
                   "        [Waktu] [datetime] NOT NULL DEFAULT (getdate()), " &
                   "        [Jenis] [varchar](20) NOT NULL, " &
                   "        [Kode] [varchar](100) NOT NULL, " &
                   "        [NamaVoucher] [varchar](150) NOT NULL, " &
                   "        [NilaiPoint] [decimal](18, 2) NOT NULL DEFAULT (0), " &
                   "        [Status] [smallint] NOT NULL DEFAULT (0), " &
                   "        [StatusPos] [smallint] NOT NULL DEFAULT (0), " &
                   "        [TransactionRef] [varchar](50) NULL, " &
                   "        [MemberUid] [varchar](50) NULL, " &
                   "        [MemberName] [varchar](100) NULL, " &
                   "        [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()), " &
                   "        [UpdatedAt] [datetime] NOT NULL DEFAULT (getdate()), " &
                   $"       CONSTRAINT [PK_GoappRedeemLog] PRIMARY KEY CLUSTERED ([Id] ASC) " &
                   "    ); " &
                   $"    CREATE NONCLUSTERED INDEX [IX_GoappRedeemLog_StatusPos] ON {Me.TableName}([StatusPos] ASC, [Status] ASC); " &
                   $"    CREATE NONCLUSTERED INDEX [IX_GoappRedeemLog_TxRef] ON {Me.TableName}([TransactionRef] ASC); " &
                   $"    CREATE NONCLUSTERED INDEX [IX_GoappRedeemLog_Kode] ON {Me.TableName}([Kode] ASC, [Jenis] ASC); " &
                   "END"
        End Function

        ' =========================================================================
        ' SYNCHRONOUS METHODS (Bebas Deadlock pada UI Thread POS)
        ' =========================================================================

        ''' <summary>
        ''' Memastikan tabel [dbo].[GoappRedeemLog] telah terbuat di database SQL Server POS secara sinkron.
        ''' </summary>
        Public Sub EnsureTableExists(Optional customConnStr As String = Nothing)
            If _tableChecked Then Return

            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) Then Return

            Dim sql = GetCreateTableSql()

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                SyncLock _tableCheckLock
                    _tableChecked = True
                End SyncLock
            Catch ex As Exception
                Console.WriteLine("EnsureTableExists error: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Menyimpan pencatatan baru secara sinkron.
        ''' </summary>
        Public Function InsertRedeemLog(item As GoappRedeemLogItem, Optional customConnStr As String = Nothing) As Long
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) OrElse item Is Nothing Then Return 0

            EnsureTableExists(connStr)

            Dim sql = $"INSERT INTO {Me.TableName} " &
                      "([Waktu], [Jenis], [Kode], [NamaVoucher], [NilaiPoint], [Status], [StatusPos], [TransactionRef], [MemberUid], [MemberName], [CreatedAt], [UpdatedAt]) " &
                      "VALUES (@Waktu, @Jenis, @Kode, @NamaVoucher, @NilaiPoint, @Status, @StatusPos, @TransactionRef, @MemberUid, @MemberName, @CreatedAt, @UpdatedAt); " &
                      "SELECT SCOPE_IDENTITY();"

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Waktu", item.Waktu)
                        cmd.Parameters.AddWithValue("@Jenis", item.Jenis)
                        cmd.Parameters.AddWithValue("@Kode", item.Kode)
                        cmd.Parameters.AddWithValue("@NamaVoucher", item.NamaVoucher)
                        cmd.Parameters.AddWithValue("@NilaiPoint", item.NilaiPoint)
                        cmd.Parameters.AddWithValue("@Status", item.Status)
                        cmd.Parameters.AddWithValue("@StatusPos", item.StatusPos)
                        cmd.Parameters.AddWithValue("@TransactionRef", If(String.IsNullOrEmpty(item.TransactionRef), DBNull.Value, item.TransactionRef))
                        cmd.Parameters.AddWithValue("@MemberUid", If(String.IsNullOrEmpty(item.MemberUid), DBNull.Value, item.MemberUid))
                        cmd.Parameters.AddWithValue("@MemberName", If(String.IsNullOrEmpty(item.MemberName), DBNull.Value, item.MemberName))
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now)
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now)

                        Dim insertedId = cmd.ExecuteScalar()
                        If insertedId IsNot Nothing AndAlso Not Convert.IsDBNull(insertedId) Then
                            item.Id = Convert.ToInt64(insertedId)
                            LogRetention.Cleanup(conn, Me.TableName, "[Waktu]")
                            Return item.Id
                        End If
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("InsertRedeemLog error: " & ex.Message)
            End Try

            Return 0
        End Function

        ''' <summary>
        ''' Mengambil daftar transaksi redeem voucher / burn point yang berstatus AKTIF dan BELUM masuk ke POS (StatusPos = 0, Status = 0)
        ''' secara sinkron (100% aman dipanggil langsung di UI thread POS).
        ''' </summary>
        Public Function GetActiveLogsByTxRef(txRef As String, Optional customConnStr As String = Nothing) As List(Of GoappRedeemLogItem)
            Dim list As New List(Of GoappRedeemLogItem)()
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) Then Return list

            EnsureTableExists(connStr)

            Dim sql = $"SELECT [Id], [Waktu], [Jenis], [Kode], [NamaVoucher], [NilaiPoint], [Status], [StatusPos], [TransactionRef], [MemberUid], [MemberName], [CreatedAt], [UpdatedAt] " &
                      $"FROM {Me.TableName} " &
                      "WHERE [Status] = 0 " &
                      If(Not String.IsNullOrEmpty(txRef), "AND [TransactionRef] = @TransactionRef ", "") &
                      "ORDER BY [Id] DESC"

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        If Not String.IsNullOrEmpty(txRef) Then
                            cmd.Parameters.AddWithValue("@TransactionRef", txRef)
                        End If

                        Using rdr = cmd.ExecuteReader()
                            While rdr.Read()
                                Dim itm As New GoappRedeemLogItem With {
                                    .Id = Convert.ToInt64(rdr("Id")),
                                    .Waktu = Convert.ToDateTime(rdr("Waktu")),
                                    .Jenis = rdr("Jenis").ToString(),
                                    .Kode = rdr("Kode").ToString(),
                                    .NamaVoucher = rdr("NamaVoucher").ToString(),
                                    .NilaiPoint = Convert.ToDecimal(rdr("NilaiPoint")),
                                    .Status = Convert.ToInt16(rdr("Status")),
                                    .StatusPos = Convert.ToInt16(rdr("StatusPos")),
                                    .TransactionRef = If(Convert.IsDBNull(rdr("TransactionRef")), "", rdr("TransactionRef").ToString()),
                                    .MemberUid = If(Convert.IsDBNull(rdr("MemberUid")), "", rdr("MemberUid").ToString()),
                                    .MemberName = If(Convert.IsDBNull(rdr("MemberName")), "", rdr("MemberName").ToString()),
                                    .CreatedAt = Convert.ToDateTime(rdr("CreatedAt")),
                                    .UpdatedAt = Convert.ToDateTime(rdr("UpdatedAt"))
                                }
                                list.Add(itm)
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("GetActiveLogsByTxRef error: " & ex.Message)
            End Try

            Return list
        End Function

        ''' <summary>
        ''' Membatalkan status penggunaan voucher/poin di database: Status = 1 (Cancel) secara sinkron.
        ''' </summary>
        Public Function UpdateStatusCancel(kode As String, Optional jenis As String = Nothing, Optional customConnStr As String = Nothing) As Boolean
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) OrElse String.IsNullOrWhiteSpace(kode) Then Return False

            EnsureTableExists(connStr)

            Dim sql = $"UPDATE {Me.TableName} " &
                      "SET [Status] = 1, [UpdatedAt] = GETDATE() " &
                      "WHERE [Kode] = @Kode AND [Status] = 0 " &
                      If(Not String.IsNullOrEmpty(jenis), "AND [Jenis] = @Jenis", "")

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Kode", kode.Trim())
                        If Not String.IsNullOrEmpty(jenis) Then
                            cmd.Parameters.AddWithValue("@Jenis", jenis.Trim())
                        End If
                        Dim rowsAffected = cmd.ExecuteNonQuery()
                        Return (rowsAffected > 0)
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("UpdateStatusCancel error: " & ex.Message)
            End Try

            Return False
        End Function

        ''' <summary>
        ''' Mengunci status voucher/poin setelah berhasil ditarik dan diterapkan ke POS: StatusPos = 1 secara sinkron.
        ''' </summary>
        Public Function UpdateStatusPos(kode As String, Optional txRef As String = Nothing, Optional customConnStr As String = Nothing) As Boolean
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) OrElse String.IsNullOrWhiteSpace(kode) Then Return False

            EnsureTableExists(connStr)

            Dim sql = $"UPDATE {Me.TableName} " &
                      "SET [StatusPos] = 1, [UpdatedAt] = GETDATE() " &
                      "WHERE [Kode] = @Kode " &
                      If(Not String.IsNullOrEmpty(txRef), "AND [TransactionRef] = @TransactionRef", "")

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Kode", kode.Trim())
                        If Not String.IsNullOrEmpty(txRef) Then
                            cmd.Parameters.AddWithValue("@TransactionRef", txRef.Trim())
                        End If
                        Dim rowsAffected = cmd.ExecuteNonQuery()
                        Return (rowsAffected > 0)
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("UpdateStatusPos error: " & ex.Message)
            End Try

            Return False
        End Function

        ' =========================================================================
        ' ASYNCHRONOUS METHODS (Dengan ConfigureAwait(False) untuk Non-UI / DLL)
        ' =========================================================================

        ''' <summary>
        ''' Memastikan tabel [dbo].[GoappRedeemLog] telah terbuat di database SQL Server POS secara asinkron.
        ''' </summary>
        Public Async Function EnsureTableExistsAsync(Optional customConnStr As String = Nothing) As Task
            If _tableChecked Then Return

            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) Then Return

            Dim sql = GetCreateTableSql()

            Try
                Using conn As New SqlConnection(connStr)
                    Await conn.OpenAsync().ConfigureAwait(False)
                    Using cmd As New SqlCommand(sql, conn)
                        Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
                    End Using
                End Using
                SyncLock _tableCheckLock
                    _tableChecked = True
                End SyncLock
            Catch ex As Exception
                Console.WriteLine("EnsureTableExistsAsync error: " & ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' Menyimpan pencatatan baru secara asinkron.
        ''' </summary>
        Public Async Function InsertRedeemLogAsync(item As GoappRedeemLogItem, Optional customConnStr As String = Nothing) As Task(Of Long)
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) OrElse item Is Nothing Then Return 0

            Await EnsureTableExistsAsync(connStr).ConfigureAwait(False)

            Dim sql = $"INSERT INTO {Me.TableName} " &
                      "([Waktu], [Jenis], [Kode], [NamaVoucher], [NilaiPoint], [Status], [StatusPos], [TransactionRef], [MemberUid], [MemberName], [CreatedAt], [UpdatedAt]) " &
                      "VALUES (@Waktu, @Jenis, @Kode, @NamaVoucher, @NilaiPoint, @Status, @StatusPos, @TransactionRef, @MemberUid, @MemberName, @CreatedAt, @UpdatedAt); " &
                      "SELECT SCOPE_IDENTITY();"

            Try
                Using conn As New SqlConnection(connStr)
                    Await conn.OpenAsync().ConfigureAwait(False)
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Waktu", item.Waktu)
                        cmd.Parameters.AddWithValue("@Jenis", item.Jenis)
                        cmd.Parameters.AddWithValue("@Kode", item.Kode)
                        cmd.Parameters.AddWithValue("@NamaVoucher", item.NamaVoucher)
                        cmd.Parameters.AddWithValue("@NilaiPoint", item.NilaiPoint)
                        cmd.Parameters.AddWithValue("@Status", item.Status)
                        cmd.Parameters.AddWithValue("@StatusPos", item.StatusPos)
                        cmd.Parameters.AddWithValue("@TransactionRef", If(String.IsNullOrEmpty(item.TransactionRef), DBNull.Value, item.TransactionRef))
                        cmd.Parameters.AddWithValue("@MemberUid", If(String.IsNullOrEmpty(item.MemberUid), DBNull.Value, item.MemberUid))
                        cmd.Parameters.AddWithValue("@MemberName", If(String.IsNullOrEmpty(item.MemberName), DBNull.Value, item.MemberName))
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now)
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now)

                        Dim insertedId = Await cmd.ExecuteScalarAsync().ConfigureAwait(False)
                        If insertedId IsNot Nothing AndAlso Not Convert.IsDBNull(insertedId) Then
                            item.Id = Convert.ToInt64(insertedId)
                            Await LogRetention.CleanupAsync(conn, Me.TableName, "[Waktu]").ConfigureAwait(False)
                            Return item.Id
                        End If
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("InsertRedeemLogAsync error: " & ex.Message)
            End Try

            Return 0
        End Function

        ''' <summary>
        ''' Mengambil daftar transaksi redeem voucher / burn point yang berstatus AKTIF dan BELUM masuk ke POS (StatusPos = 0, Status = 0)
        ''' untuk nomor transaksi/bill tertentu secara asinkron.
        ''' </summary>
        Public Async Function GetActiveLogsByTxRefAsync(txRef As String, Optional customConnStr As String = Nothing) As Task(Of List(Of GoappRedeemLogItem))
            Dim list As New List(Of GoappRedeemLogItem)()
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) Then Return list

            Await EnsureTableExistsAsync(connStr).ConfigureAwait(False)

            Dim sql = $"SELECT [Id], [Waktu], [Jenis], [Kode], [NamaVoucher], [NilaiPoint], [Status], [StatusPos], [TransactionRef], [MemberUid], [MemberName], [CreatedAt], [UpdatedAt] " &
                      $"FROM {Me.TableName} " &
                      "WHERE [Status] = 0 " &
                      If(Not String.IsNullOrEmpty(txRef), "AND [TransactionRef] = @TransactionRef ", "") &
                      "ORDER BY [Id] DESC"

            Try
                Using conn As New SqlConnection(connStr)
                    Await conn.OpenAsync().ConfigureAwait(False)
                    Using cmd As New SqlCommand(sql, conn)
                        If Not String.IsNullOrEmpty(txRef) Then
                            cmd.Parameters.AddWithValue("@TransactionRef", txRef)
                        End If

                        Using rdr = Await cmd.ExecuteReaderAsync().ConfigureAwait(False)
                            While Await rdr.ReadAsync().ConfigureAwait(False)
                                Dim itm As New GoappRedeemLogItem With {
                                    .Id = Convert.ToInt64(rdr("Id")),
                                    .Waktu = Convert.ToDateTime(rdr("Waktu")),
                                    .Jenis = rdr("Jenis").ToString(),
                                    .Kode = rdr("Kode").ToString(),
                                    .NamaVoucher = rdr("NamaVoucher").ToString(),
                                    .NilaiPoint = Convert.ToDecimal(rdr("NilaiPoint")),
                                    .Status = Convert.ToInt16(rdr("Status")),
                                    .StatusPos = Convert.ToInt16(rdr("StatusPos")),
                                    .TransactionRef = If(Convert.IsDBNull(rdr("TransactionRef")), "", rdr("TransactionRef").ToString()),
                                    .MemberUid = If(Convert.IsDBNull(rdr("MemberUid")), "", rdr("MemberUid").ToString()),
                                    .MemberName = If(Convert.IsDBNull(rdr("MemberName")), "", rdr("MemberName").ToString()),
                                    .CreatedAt = Convert.ToDateTime(rdr("CreatedAt")),
                                    .UpdatedAt = Convert.ToDateTime(rdr("UpdatedAt"))
                                }
                                list.Add(itm)
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("GetActiveLogsByTxRefAsync error: " & ex.Message)
            End Try

            Return list
        End Function

        ''' <summary>
        ''' Membatalkan status penggunaan voucher/poin di database: Status = 1 (Cancel) secara asinkron.
        ''' </summary>
        Public Async Function UpdateStatusCancelAsync(kode As String, Optional jenis As String = Nothing, Optional customConnStr As String = Nothing) As Task(Of Boolean)
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) OrElse String.IsNullOrWhiteSpace(kode) Then Return False

            Await EnsureTableExistsAsync(connStr).ConfigureAwait(False)

            Dim sql = $"UPDATE {Me.TableName} " &
                      "SET [Status] = 1, [UpdatedAt] = GETDATE() " &
                      "WHERE [Kode] = @Kode AND [Status] = 0 " &
                      If(Not String.IsNullOrEmpty(jenis), "AND [Jenis] = @Jenis", "")

            Try
                Using conn As New SqlConnection(connStr)
                    Await conn.OpenAsync().ConfigureAwait(False)
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Kode", kode.Trim())
                        If Not String.IsNullOrEmpty(jenis) Then
                            cmd.Parameters.AddWithValue("@Jenis", jenis.Trim())
                        End If
                        Dim rowsAffected = Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
                        Return (rowsAffected > 0)
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("UpdateStatusCancelAsync error: " & ex.Message)
            End Try

            Return False
        End Function

        ''' <summary>
        ''' Mengunci status voucher/poin setelah berhasil ditarik dan diterapkan ke POS: StatusPos = 1 secara asinkron.
        ''' </summary>
        Public Async Function UpdateStatusPosAsync(kode As String, Optional txRef As String = Nothing, Optional customConnStr As String = Nothing) As Task(Of Boolean)
            Dim connStr = GetEffectiveConnStr(customConnStr)
            If String.IsNullOrWhiteSpace(connStr) OrElse String.IsNullOrWhiteSpace(kode) Then Return False

            Await EnsureTableExistsAsync(connStr).ConfigureAwait(False)

            Dim sql = $"UPDATE {Me.TableName} " &
                      "SET [StatusPos] = 1, [UpdatedAt] = GETDATE() " &
                      "WHERE [Kode] = @Kode " &
                      If(Not String.IsNullOrEmpty(txRef), "AND [TransactionRef] = @TransactionRef", "")

            Try
                Using conn As New SqlConnection(connStr)
                    Await conn.OpenAsync().ConfigureAwait(False)
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Kode", kode.Trim())
                        If Not String.IsNullOrEmpty(txRef) Then
                            cmd.Parameters.AddWithValue("@TransactionRef", txRef.Trim())
                        End If
                        Dim rowsAffected = Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
                        Return (rowsAffected > 0)
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("UpdateStatusPosAsync error: " & ex.Message)
            End Try

            Return False
        End Function
    End Class
End Namespace
