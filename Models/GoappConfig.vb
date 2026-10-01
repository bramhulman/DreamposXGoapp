Namespace Models
    ''' <summary>
    ''' Konfigurasi integrasi API Goapp CRM.
    ''' WAJIB diisi oleh aplikasi pemanggil (POS) sebelum instansiasi GoappApiClient.
    ''' Jangan pernah menyimpan ApiKey/ApiSecret langsung di source code — baca dari
    ''' file konfigurasi terenkripsi, environment variable, atau Windows Credential Store.
    ''' </summary>
    Public Class GoappConfig
        ''' <summary>API Key channel (Channel UID dari dashboard Goapp CRM). WAJIB diisi.</summary>
        Public Property ApiKey As String = String.Empty

        ''' <summary>API Secret channel. WAJIB diisi. Jaga kerahasiaan nilai ini.</summary>
        Public Property ApiSecret As String = String.Empty

        ''' <summary>UID numerik channel — biasanya sama dengan ApiKey. WAJIB diisi.</summary>
        Public Property ChannelUid As Long = 0

        ''' <summary>Base URL server autentikasi Goapp. Default: production Goapp.</summary>
        Public Property AuthBaseUrl As String = "https://account.goapp.co.id/auth"

        ''' <summary>Base URL Channel API Goapp. Default: production Goapp.</summary>
        Public Property ChannelBaseUrl As String = "https://api.goapp.co.id/channel/v1"

        ''' <summary>Timeout HTTP request dalam detik.</summary>
        Public Property TimeoutSeconds As Integer = 30

        ''' <summary>Jumlah maksimal percobaan ulang saat request gagal.</summary>
        Public Property MaxRetryAttempts As Integer = 3

        ''' <summary>Jeda antar percobaan ulang dalam milidetik.</summary>
        Public Property RetryDelayMilliseconds As Integer = 1000

        ''' <summary>
        ''' Validasi bahwa semua field wajib sudah diisi sebelum digunakan.
        ''' Lemparkan InvalidOperationException jika ada yang kosong/nol.
        ''' </summary>
        Public Sub Validate()
            If String.IsNullOrWhiteSpace(ApiKey) Then
                Throw New InvalidOperationException("GoappConfig.ApiKey belum diisi. Isi API Key dari konfigurasi aplikasi Anda.")
            End If
            If String.IsNullOrWhiteSpace(ApiSecret) Then
                Throw New InvalidOperationException("GoappConfig.ApiSecret belum diisi. Isi API Secret dari konfigurasi aplikasi Anda.")
            End If
            If ChannelUid = 0 Then
                Throw New InvalidOperationException("GoappConfig.ChannelUid belum diisi. Isi UID channel numerik dari dashboard Goapp CRM.")
            End If
            If String.IsNullOrWhiteSpace(AuthBaseUrl) Then
                Throw New InvalidOperationException("GoappConfig.AuthBaseUrl belum diisi.")
            End If
            If String.IsNullOrWhiteSpace(ChannelBaseUrl) Then
                Throw New InvalidOperationException("GoappConfig.ChannelBaseUrl belum diisi.")
            End If
        End Sub
    End Class
End Namespace
