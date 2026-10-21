Imports System.Data.SqlClient
Imports System.Threading.Tasks

Public Class DatabaseLogger
    Private Shared _tableChecked As Boolean = False
    Private Shared ReadOnly _tableCheckLock As New Object()

    Public Property ConnectionString As String = String.Empty
    Public Property TableName As String = "[dbo].[GoappApiLog]"

    Public Sub New(Optional customConnectionString As String = Nothing, Optional tableName As String = "[dbo].[GoappApiLog]")
        Me.TableName = If(String.IsNullOrWhiteSpace(tableName), "[dbo].[GoappApiLog]", tableName)
        Me.ConnectionString = If(customConnectionString, String.Empty)
    End Sub

    Private Function GetCreateTableSql() As String
        Dim tblName = If(String.IsNullOrWhiteSpace(Me.TableName), "[dbo].[GoappApiLog]", Me.TableName)
        Return $"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{tblName}') AND type in (N'U')) " &
               "BEGIN " &
               $"    CREATE TABLE {tblName}( " &
               "        [Id] [bigint] IDENTITY(1,1) NOT NULL, " &
               "        [LogDate] [date] NOT NULL, " &
               "        [LogDateTime] [datetime2](7) NOT NULL, " &
               "        [HttpMethod] [varchar](10) NOT NULL, " &
               "        [EndpointUrl] [varchar](2048) NOT NULL, " &
               "        [RequestPayload] [nvarchar](max) NULL, " &
               "        [ResponsePayload] [nvarchar](max) NULL, " &
               "        [StatusCode] [int] NULL, " &
               "        [IsSuccess] [bit] NOT NULL, " &
               "        [ErrorMessage] [nvarchar](max) NULL, " &
               "        [DurationMs] [int] NOT NULL, " &
               "        [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()), " &
               $"       CONSTRAINT [PK_GoappApiLog] PRIMARY KEY CLUSTERED ([Id] ASC) " &
               "    ); " &
               $"    CREATE NONCLUSTERED INDEX [IX_GoappApiLog_LogDate] ON {tblName}([LogDate] ASC); " &
               "END"
    End Function

    ''' <summary>
    ''' Memastikan tabel [dbo].[GoappApiLog] telah terbuat di database SQL Server secara sinkron.
    ''' </summary>
    Public Sub EnsureTableExists(Optional customConnStr As String = Nothing)
        If _tableChecked Then Return
        Dim connStr = If(Not String.IsNullOrWhiteSpace(customConnStr), customConnStr, Me.ConnectionString)
        If String.IsNullOrWhiteSpace(connStr) Then Return

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Using cmd As New SqlCommand(GetCreateTableSql(), conn)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            SyncLock _tableCheckLock
                _tableChecked = True
            End SyncLock
        Catch ex As Exception
            Console.WriteLine("EnsureTableExists [GoappApiLog] error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Memastikan tabel [dbo].[GoappApiLog] telah terbuat di database SQL Server secara asinkron.
    ''' </summary>
    Public Async Function EnsureTableExistsAsync(Optional customConnStr As String = Nothing) As Task
        If _tableChecked Then Return
        Dim connStr = If(Not String.IsNullOrWhiteSpace(customConnStr), customConnStr, Me.ConnectionString)
        If String.IsNullOrWhiteSpace(connStr) Then Return

        Try
            Using conn As New SqlConnection(connStr)
                Await conn.OpenAsync().ConfigureAwait(False)
                Using cmd As New SqlCommand(GetCreateTableSql(), conn)
                    Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
                End Using
            End Using
            SyncLock _tableCheckLock
                _tableChecked = True
            End SyncLock
        Catch ex As Exception
            Console.WriteLine("EnsureTableExistsAsync [GoappApiLog] error: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Mencatat request dan response API ke database SQL Server secara asynchronous.
    ''' Connection string dapat dioper secara eksplisit via parameter customConnectionString (misal dari aplikasi POS),
    ''' atau otomatis fallback ke properti Me.ConnectionString.
    ''' </summary>
    Public Async Function LogApiAsync(method As String, endpoint As String, requestJson As String, responseJson As String, statusCode As Integer, isSuccess As Boolean, errorMessage As String, durationMs As Integer, Optional customConnectionString As String = Nothing) As Task
        Await LogApiWithResultAsync(method, endpoint, requestJson, responseJson, statusCode, isSuccess, errorMessage, durationMs, customConnectionString).ConfigureAwait(False)
    End Function

    Public Async Function LogApiWithResultAsync(method As String, endpoint As String, requestJson As String, responseJson As String, statusCode As Integer, isSuccess As Boolean, errorMessage As String, durationMs As Integer, Optional customConnectionString As String = Nothing) As Task(Of Boolean)
        Dim effectiveConnStr = If(Not String.IsNullOrWhiteSpace(customConnectionString), customConnectionString, Me.ConnectionString)
        If String.IsNullOrWhiteSpace(effectiveConnStr) Then Return False

        Await EnsureTableExistsAsync(effectiveConnStr).ConfigureAwait(False)

        Dim targetTable = If(String.IsNullOrWhiteSpace(Me.TableName), "[dbo].[GoappApiLog]", Me.TableName)
        Dim query = $"INSERT INTO {targetTable} " &
                    "([LogDate], [LogDateTime], [HttpMethod], [EndpointUrl], [RequestPayload], [ResponsePayload], [StatusCode], [IsSuccess], [ErrorMessage], [DurationMs]) " &
                    "VALUES (@LogDate, @LogDateTime, @HttpMethod, @EndpointUrl, @RequestPayload, @ResponsePayload, @StatusCode, @IsSuccess, @ErrorMessage, @DurationMs)"

        Try
            Using conn As New SqlConnection(effectiveConnStr)
                Await conn.OpenAsync()
                Using cmd As New SqlCommand(query, conn)
                    Dim now = DateTime.Now
                    cmd.Parameters.AddWithValue("@LogDate", now.Date)
                    cmd.Parameters.AddWithValue("@LogDateTime", now)
                    cmd.Parameters.AddWithValue("@HttpMethod", method)
                    cmd.Parameters.AddWithValue("@EndpointUrl", endpoint)
                    cmd.Parameters.AddWithValue("@RequestPayload", If(String.IsNullOrEmpty(requestJson), DBNull.Value, requestJson))
                    cmd.Parameters.AddWithValue("@ResponsePayload", If(String.IsNullOrEmpty(responseJson), DBNull.Value, responseJson))
                    cmd.Parameters.AddWithValue("@StatusCode", statusCode)
                    cmd.Parameters.AddWithValue("@IsSuccess", isSuccess)
                    cmd.Parameters.AddWithValue("@ErrorMessage", If(String.IsNullOrEmpty(errorMessage), DBNull.Value, errorMessage))
                    cmd.Parameters.AddWithValue("@DurationMs", durationMs)

                    Await cmd.ExecuteNonQueryAsync()
                End Using
                Await LogRetention.CleanupAsync(conn, targetTable, "[LogDateTime]").ConfigureAwait(False)
            End Using
        Catch ex As Exception
            ' Silently ignore or log to file if DB logging fails
            Console.WriteLine("DB Logging failed: " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Menguji apakah koneksi database SQL Server dapat dibuka dengan baik.
    ''' Mengembalikan Tuple(Of Boolean, String) berisi (Status Sukses, Pesan).
    ''' </summary>
    Public Async Function TestConnectionAsync(Optional customConnectionString As String = Nothing) As Task(Of Tuple(Of Boolean, String))
        Dim effectiveConnStr = If(Not String.IsNullOrWhiteSpace(customConnectionString), customConnectionString, Me.ConnectionString)
        If String.IsNullOrWhiteSpace(effectiveConnStr) Then
            Return Tuple.Create(False, "Connection string database belum ditentukan / kosong.")
        End If

        Try
            Using conn As New SqlConnection(effectiveConnStr)
                Await conn.OpenAsync()
                Return Tuple.Create(True, "Koneksi database SQL Server berhasil terhubung.")
            End Using
        Catch ex As Exception
            Return Tuple.Create(False, "Koneksi database SQL Server gagal: " & ex.Message)
        End Try
    End Function
End Class
