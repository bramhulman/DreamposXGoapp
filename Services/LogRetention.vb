Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Threading.Tasks

' Retensi bersama untuk jalur insert sinkron dan asinkron.
Friend NotInheritable Class LogRetention
    Private Shared ReadOnly CleanupLock As New Object()
    Private Shared ReadOnly NextCleanup As New Dictionary(Of String, DateTime)()
    Private Shared ReadOnly Running As New HashSet(Of String)()

    Private Sub New()
    End Sub

    Public Shared Sub Cleanup(conn As SqlConnection, tableName As String, dateColumn As String)
        CleanupAsync(conn, tableName, dateColumn).GetAwaiter().GetResult()
    End Sub

    Public Shared Async Function CleanupAsync(conn As SqlConnection, tableName As String, dateColumn As String) As Task
        ' Pisahkan jadwal untuk setiap koneksi, tabel, dan kolom tanggal.
        Dim key = conn.ConnectionString & vbNullChar & tableName & vbNullChar & dateColumn
        SyncLock CleanupLock
            Dim nextRun As DateTime
            If Running.Contains(key) OrElse
                (NextCleanup.TryGetValue(key, nextRun) AndAlso DateTime.UtcNow < nextRun) Then Return
            Running.Add(key)
        End SyncLock

        Dim succeeded As Boolean = False
        Try
            ' Waktu SQL Server, batas tetap selama batch; tanggal tepat pada batas tetap disimpan.
            Dim sql = "SET NOCOUNT ON; " &
                      "DECLARE @Cutoff datetime = DATEADD(MONTH, -3, GETDATE()); " &
                      "WHILE 1 = 1 BEGIN " &
                      $"DELETE TOP (1000) FROM {tableName} WHERE {dateColumn} < @Cutoff; " &
                      "IF @@ROWCOUNT < 1000 BREAK; END;"
            Using cmd As New SqlCommand(sql, conn)
                Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
            End Using
            succeeded = True
        Catch ex As Exception
            ' Kegagalan retensi tidak menggagalkan insert yang sudah berhasil.
            Console.WriteLine("Log retention failed: " & ex.Message)
        Finally
            SyncLock CleanupLock
                Running.Remove(key)
                ' Coba ulang 5 menit jika gagal; setelah berhasil cukup sekali sehari.
                NextCleanup(key) = DateTime.UtcNow.AddMinutes(If(succeeded, 1440, 5))
            End SyncLock
        End Try
    End Function
End Class