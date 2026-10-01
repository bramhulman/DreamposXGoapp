Namespace Models
    ''' <summary>
    ''' Konfigurasi integrasi API Goapp CRM
    ''' </summary>
    Public Class GoappConfig
        Public Property ApiKey As String = "138350315235400"
        Public Property ApiSecret As String = "cbf5b1c921b36bfb06ca988c6d98f608482c40f1"
        Public Property AuthBaseUrl As String = "https://account.goapp.co.id/auth"
        Public Property ChannelBaseUrl As String = "https://api.goapp.co.id/channel/v1"
        Public Property TimeoutSeconds As Integer = 30
        Public Property MaxRetryAttempts As Integer = 3
        Public Property RetryDelayMilliseconds As Integer = 1000
    End Class
End Namespace
