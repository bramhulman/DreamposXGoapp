Imports System.Data.SqlClient
Imports System.Threading.Tasks

Public Class DatabaseLogger
    Public Property ConnectionString As String = String.Empty
    Public Property TableName As String = "[dbo].[GoappApiLog]"

    Public Sub New(Optional customConnectionString As String = Nothing, Optional tableName As String = "[dbo].[GoappApiLog]")
        Me.TableName = If(String.IsNullOrWhiteSpace(tableName), "[dbo].[GoappApiLog]", tableName)
        Me.ConnectionString = If(customConnectionString, String.Empty)
    End Sub

    Public Async Function LogApiAsync(method As String, endpoint As String, requestJson As String, responseJson As String, statusCode As Integer, isSuccess As Boolean, errorMessage As String, durationMs As Integer) As Task
        If String.IsNullOrWhiteSpace(Me.ConnectionString) Then Return ' Lewati jika tidak ada DB yang dikonfigurasi

        Dim targetTable = If(String.IsNullOrWhiteSpace(Me.TableName), "[dbo].[GoappApiLog]", Me.TableName)
        Dim query = $"INSERT INTO {targetTable} " &
                    "([LogDate], [LogDateTime], [HttpMethod], [EndpointUrl], [RequestPayload], [ResponsePayload], [StatusCode], [IsSuccess], [ErrorMessage], [DurationMs]) " &
                    "VALUES (@LogDate, @LogDateTime, @HttpMethod, @EndpointUrl, @RequestPayload, @ResponsePayload, @StatusCode, @IsSuccess, @ErrorMessage, @DurationMs)"

        Try
            Using conn As New SqlConnection(Me.ConnectionString)
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
            End Using
        Catch ex As Exception
            ' Silently ignore or log to file if DB logging fails
            Console.WriteLine("DB Logging failed: " & ex.Message)
        End Try
    End Function
End Class
