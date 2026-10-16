Imports System.Data.SqlClient
Imports System.Threading.Tasks

Public Class DatabaseLogger
    Public Property ConnectionString As String = String.Empty
    Public Property TableName As String = "[dbo].[GoappApiLog]"

    Public Sub New(Optional customConnectionString As String = Nothing, Optional tableName As String = "[dbo].[GoappApiLog]")
        Me.TableName = If(String.IsNullOrWhiteSpace(tableName), "[dbo].[GoappApiLog]", tableName)
        Me.ConnectionString = If(customConnectionString, String.Empty)
    End Sub

    ''' <summary>
    ''' Mencatat request dan response API ke database SQL Server secara asynchronous.
    ''' Connection string dapat dioper secara eksplisit via parameter customConnectionString (misal dari aplikasi POS),
    ''' atau otomatis fallback ke properti Me.ConnectionString.
    ''' </summary>
    Public Async Function LogApiAsync(method As String, endpoint As String, requestJson As String, responseJson As String, statusCode As Integer, isSuccess As Boolean, errorMessage As String, durationMs As Integer, Optional customConnectionString As String = Nothing) As Task
        Dim effectiveConnStr = If(Not String.IsNullOrWhiteSpace(customConnectionString), customConnectionString, Me.ConnectionString)
        If String.IsNullOrWhiteSpace(effectiveConnStr) Then Return ' Lewati jika tidak ada DB yang dikonfigurasi

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
        End Try
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
