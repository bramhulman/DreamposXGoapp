Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports DreamposXGoapp.Models
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace Services
    ''' <summary>
    ''' Client Integrasi API Goapp CRM untuk DreamPOS.
    ''' Menangani autentikasi otomatis, caching token, auto-retry, pencarian member,
    ''' push transaksi, burn point, dan voucher.
    ''' </summary>
    Public Class GoappApiClient
        Private ReadOnly _config As GoappConfig
        Private ReadOnly _httpClient As HttpClient
        Private ReadOnly _tokenLock As New SemaphoreSlim(1, 1)
        Private ReadOnly _dbLogger As New DatabaseLogger()

        Private _cachedToken As String = String.Empty
        Private _tokenExpiryEpoch As Double = 0

        ''' <summary>
        ''' Event logger untuk monitoring transaksi POS
        ''' </summary>
        Public Event OnLog(message As String)

        Public Sub New(Optional config As GoappConfig = Nothing)
            _config = If(config, New GoappConfig())

            ' Pastikan TLS 1.2 aktif (krusial untuk koneksi HTTPS ke API modern)
            Try
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or CType(768, SecurityProtocolType) Or SecurityProtocolType.Tls
            Catch
                ' Abaikan jika platform mengelola security protocol secara default
            End Try

            Dim handler As New HttpClientHandler()
            ' Toleransi sertifikat untuk testing / dev environment jika diperlukan
            handler.ServerCertificateCustomValidationCallback = Function(message, cert, chain, errors) True

            _httpClient = New HttpClient(handler) With {
                .Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds)
            }
        End Sub

        Private Sub Log(msg As String)
            RaiseEvent OnLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}")
        End Sub

#Region "Authentication & Token Management"

        ''' <summary>
        ''' Mendapatkan token aktif dari cache atau melakukan request baru ke Goapp Auth API
        ''' </summary>
        Public Async Function GetValidTokenAsync(Optional forceRefresh As Boolean = False) As Task(Of String)
            Await _tokenLock.WaitAsync()
            Try
                Dim currentEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                ' Gunakan token jika masih valid (buffer 60 detik sebelum expired)
                If Not forceRefresh AndAlso Not String.IsNullOrEmpty(_cachedToken) AndAlso _tokenExpiryEpoch > (currentEpoch + 60) Then
                    Return _cachedToken
                End If

                Log("Meminta Access Token baru ke Goapp Auth API...")
                Dim authUrl = $"{_config.AuthBaseUrl.TrimEnd("/"c)}/token-auth/"
                Dim payload = New With {
                    .username = _config.ApiKey,
                    .password = _config.ApiSecret
                }

                Dim jsonBody = JsonConvert.SerializeObject(payload)
                Dim content = New StringContent(jsonBody, Encoding.UTF8, "application/json")

                Dim response = Await _httpClient.PostAsync(authUrl, content)
                Dim resContent = Await response.Content.ReadAsStringAsync()

                If Not response.IsSuccessStatusCode Then
                    Log($"Gagal mendapatkan token: {response.StatusCode} - {resContent}")
                    Throw New Exception($"Goapp Auth Gagal ({response.StatusCode}): {resContent}")
                End If

                Dim tokenObj = JsonConvert.DeserializeObject(Of AuthTokenResponse)(resContent)
                If tokenObj Is Nothing OrElse String.IsNullOrEmpty(tokenObj.Token) Then
                    Throw New Exception("Format response token dari Goapp tidak valid.")
                End If

                _cachedToken = tokenObj.Token
                _tokenExpiryEpoch = If(tokenObj.ExpiredAt > 0, tokenObj.ExpiredAt, currentEpoch + 3600)
                Log("Access Token Goapp berhasil didapatkan dan disimpan di memori.")

                Return _cachedToken
            Finally
                _tokenLock.Release()
            End Try
        End Function

#End Region

#Region "Channel Info"

        ''' <summary>
        ''' Memeriksa informasi channel/toko yang terdaftar pada kredensial saat ini
        ''' </summary>
        Public Async Function GetChannelDirectoryAsync() As Task(Of ApiResponse(Of ChannelDirectoryInfo))
            Return Await SendAuthorizedRequestAsync(Of ChannelDirectoryInfo)(
                HttpMethod.Get,
                $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/directory/channel/0/"
            )
        End Function

#End Region

#Region "Member Search & Validation"

        ''' <summary>
        ''' Mencari dan memvalidasi member Goapp berdasarkan Nomor Handphone atau Member ID
        ''' </summary>
        ''' <param name="mobileNoOrMemberId">Nomor HP (misal: 08123456789) atau Member ID</param>
        Public Async Function GetMemberAsync(mobileNoOrMemberId As String) As Task(Of ApiResponse(Of MemberResponse))
            If String.IsNullOrWhiteSpace(mobileNoOrMemberId) Then
                Return New ApiResponse(Of MemberResponse) With {
                    .IsSuccess = False,
                    .Message = "Nomor HP atau Member ID tidak boleh kosong."
                }
            End If

            Dim cleanParam = mobileNoOrMemberId.Trim()
            ' Standardisasi jika format 62 diawali 08 atau sebaliknya jika dibutuhkan
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/member/{WebUtility.UrlEncode(cleanParam)}/"
            Log($"Mencari data member: {cleanParam}")

            Dim result = Await SendAuthorizedRequestAsync(Of MemberResponse)(HttpMethod.Get, url)

            If result.IsSuccess AndAlso result.Data IsNot Nothing Then
                Log($"Member ditemukan: {result.Data.FullName}, Poin: {result.Data.AvailablePoints}, IDR: {result.Data.PointsInRupiah:N0}")
            Else
                Log($"Pencarian member gagal: {result.Message}")
            End If

            Return result
        End Function

        ''' <summary>
        ''' Mendaftarkan (Save/Set) data member baru langsung dari kasir POS ke Goapp CRM
        ''' </summary>
        Public Async Function RegisterMemberAsync(firstName As String, lastName As String, mobileNo As String, Optional email As String = Nothing, Optional schemeUid As Long = 138348545946696, Optional schemeName As String = "Go Member") As Task(Of ApiResponse(Of MemberResponse))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/member/"
            Dim payload As New CreateMemberRequest With {
                .FirstName = firstName.Trim(),
                .LastName = If(String.IsNullOrEmpty(lastName), "-", lastName.Trim()),
                .MobileNo = mobileNo.Trim(),
                .Email = email,
                .Scheme = New MemberScheme With {.Uid = schemeUid, .Name = schemeName}
            }

            Log($"Mendaftarkan member baru: {firstName} ({mobileNo}) ke Scheme '{schemeName}'...")
            Dim result = Await SendAuthorizedRequestAsync(Of MemberResponse)(HttpMethod.Post, url, payload)

            If result.IsSuccess AndAlso result.Data IsNot Nothing Then
                Log($"Registrasi member berhasil! UID: {result.Data.Uid}, Nama: {result.Data.FullName}")
            Else
                Log($"Registrasi member gagal: {result.Message}")
            End If

            Return result
        End Function

        ''' <summary>
        ''' Mengambil daftar Tier/Scheme Member CRM yang aktif
        ''' </summary>
        Public Async Function GetMemberSchemesAsync() As Task(Of ApiResponse(Of List(Of MemberScheme)))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/scheme/"
            Return Await SendAuthorizedRequestAsync(Of List(Of MemberScheme))(HttpMethod.Get, url)
        End Function

        ''' <summary>
        ''' Menampilkan daftar seluruh voucher promo (Deals) yang sedang aktif di channel toko ini
        ''' </summary>
        Public Async Function GetAvailableDealsAsync() As Task(Of ApiResponse(Of List(Of DirectDealInfo)))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/deal/"
            Return Await SendAuthorizedRequestAsync(Of List(Of DirectDealInfo))(HttpMethod.Get, url)
        End Function

        ''' <summary>
        ''' Mengambil daftar voucher/deal code aktif milik spesifik member
        ''' </summary>
        Public Async Function GetMemberDealCodesAsync(mobileNoOrMemberId As String) As Task(Of ApiResponse(Of List(Of MemberDealCodeResponse)))
            If String.IsNullOrWhiteSpace(mobileNoOrMemberId) Then
                Return New ApiResponse(Of List(Of MemberDealCodeResponse)) With {.IsSuccess = False, .Message = "No HP / Member ID tidak boleh kosong."}
            End If

            Dim cleanParam = mobileNoOrMemberId.Trim()
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/member/{System.Net.WebUtility.UrlEncode(cleanParam)}/deal_codes/?status=not_used"
            Return Await SendAuthorizedRequestAsync(Of List(Of MemberDealCodeResponse))(System.Net.Http.HttpMethod.Get, url)
        End Function

#End Region

#Region "Vouchers with Auto-Retry"

        ''' <summary>
        ''' Validasi voucher Goapp sebelum digunakan pada transaksi POS
        ''' </summary>
        Public Async Function ValidateVoucherAsync(dealCode As String, Optional memberUid As Long? = Nothing) As Task(Of ApiResponse(Of VoucherResult))
            If String.IsNullOrWhiteSpace(dealCode) Then
                Return New ApiResponse(Of VoucherResult) With {.IsSuccess = False, .Message = "Kode voucher harus diisi."}
            End If

            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/deal/validate/"
            Dim payload = New VoucherValidateRequest With {
                .DealCode = dealCode.Trim()
            }
            If memberUid.HasValue AndAlso memberUid.Value > 0 Then
                payload.Member = New VoucherMemberRef With {.Uid = memberUid.Value}
            End If

            Return Await SendAuthorizedRequestAsync(Of VoucherResult)(HttpMethod.Post, url, payload)
        End Function

        ''' <summary>
        ''' Menggunakan voucher dengan mekanisme Auto-Retry jika terjadi kendala jaringan / server busy
        ''' </summary>
        Public Async Function UseVoucherWithRetryAsync(dealCode As String, transactionRef As String, Optional memberUid As Long? = Nothing, Optional customMaxRetry As Integer = -1) As Task(Of ApiResponse(Of VoucherResult))
            Dim maxRetry = If(customMaxRetry > 0, customMaxRetry, _config.MaxRetryAttempts)
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/deal/use/"
            Dim payload = New VoucherUseRequest With {
                .DealCode = dealCode.Trim(),
                .TransactionRef = transactionRef,
                .Member = If(memberUid.HasValue AndAlso memberUid.Value > 0, New VoucherMemberRef With {.Uid = memberUid.Value}, Nothing)
            }

            Dim lastResult As ApiResponse(Of VoucherResult) = Nothing

            For attempt As Integer = 1 To maxRetry
                Log($"Menggunakan voucher '{dealCode}' (Percobaan {attempt}/{maxRetry})...")
                lastResult = Await SendAuthorizedRequestAsync(Of VoucherResult)(HttpMethod.Post, url, payload)

                If lastResult.IsSuccess Then
                    Log($"Voucher '{dealCode}' berhasil digunakan pada transaksi {transactionRef}.")
                    Return lastResult
                End If

                ' Jika error 400 (Bad Request - voucher tidak valid/kadaluarsa), jangan retry karena hasil tidak akan berubah
                If lastResult.StatusCode = 400 Then
                    Log($"Voucher ditolak sistem: {lastResult.Message}. Menghentikan retry.")
                    Return lastResult
                End If

                ' Jika error jaringan atau server error 5xx, lakukan delay dan retry
                If attempt < maxRetry Then
                    Dim delay = _config.RetryDelayMilliseconds * attempt
                    Log($"Gagal menggunakan voucher (Status: {lastResult.StatusCode}). Menunggu {delay}ms sebelum retry...")
                    Await Task.Delay(delay)
                End If
            Next

            Return lastResult
        End Function

        ''' <summary>
        ''' Membatalkan penggunaan voucher pada suatu transaksi POS (Void / Cancel Voucher)
        ''' </summary>
        Public Async Function CancelVoucherAsync(dealCode As String, transactionRef As String, Optional memberUid As Long? = Nothing) As Task(Of ApiResponse(Of VoucherResult))
            If String.IsNullOrWhiteSpace(dealCode) OrElse String.IsNullOrWhiteSpace(transactionRef) Then
                Return New ApiResponse(Of VoucherResult) With {.IsSuccess = False, .Message = "Kode voucher dan transaction_ref harus diisi."}
            End If

            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/deal/cancel_code/"
            Dim payload = New VoucherCancelRequest With {
                .DealCode = dealCode.Trim(),
                .TransactionRef = transactionRef.Trim(),
                .Member = If(memberUid.HasValue AndAlso memberUid.Value > 0, New VoucherMemberRef With {.Uid = memberUid.Value}, Nothing)
            }

            Log($"Membatalkan pemakaian voucher '{dealCode}' pada transaksi {transactionRef}...")
            Dim result = Await SendAuthorizedRequestAsync(Of VoucherResult)(HttpMethod.Post, url, payload)
            If result.IsSuccess Then
                Log($"Pembatalan voucher '{dealCode}' berhasil.")
            Else
                Log($"Gagal membatalkan voucher: {result.Message}")
            End If
            Return result
        End Function

        ''' <summary>
        ''' Mencoba membuat Master Voucher / Promo Deal baru via API
        ''' Catatan: Master deal umumnya dikonfigurasi melalui Web Dashboard CRM.
        ''' Bila melalui API, reward_sku harus merupakan SKU yang valid di master catalog.
        ''' </summary>
        Public Async Function CreateDealAsync(name As String, rewardSku As String, startTime As DateTime, endTime As DateTime, Optional discountType As String = "amount", Optional discountAmount As Decimal = 0, Optional minPurchase As Decimal = 0, Optional redeemCode As String = "") As Task(Of ApiResponse(Of DirectDealInfo))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/deal/"
            Dim payload As New CreateDealRequest With {
                .Name = name.Trim(),
                .RewardSku = rewardSku.Trim(),
                .RedeemCode = If(String.IsNullOrEmpty(redeemCode), Nothing, redeemCode.Trim()),
                .StartTime = startTime.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                .EndTime = endTime.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                .RewardChannel = New ChannelRef With {.Uid = _config.ChannelUid},
                .RewardData = New RewardDataInfo With {
                    .Name = name.Trim(),
                    .DiscountType = discountType,
                    .DiscountAmount = discountAmount,
                    .MinPurchase = minPurchase
                }
            }

            Log($"Mengirim pendaftaran Master Voucher '{name}' (SKU: {rewardSku})...")
            Dim result = Await SendAuthorizedRequestAsync(Of DirectDealInfo)(HttpMethod.Post, url, payload)
            If result.IsSuccess Then
                Log($"Master Voucher '{name}' berhasil dibuat! UID: {result.Data?.Uid}")
            Else
                Log($"Gagal membuat Master Voucher: {result.Message}")
            End If
            Return result
        End Function

#End Region

#Region "Point Payment (Burn Point)"

        ''' <summary>
        ''' Melakukan pemotongan (Burn) poin member untuk pembayaran transaksi POS
        ''' </summary>
        Public Async Function CreatePointPaymentAsync(memberUid As Long, pointAmount As Decimal, orderAmountIdr As Decimal, providerRef As String) As Task(Of ApiResponse(Of PaymentTransactionResponse))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/payment/"
            Dim payload = New PaymentTransactionRequest With {
                .Member = New PaymentMemberRef With {.Uid = memberUid},
                .Amount = pointAmount,
                .OrderAmount = orderAmountIdr,
                .ProviderRef = providerRef
            }
            
            If _config.StoreUid.HasValue Then
                payload.Store = New OrderStoreRef With {
                    .Uid = _config.StoreUid.Value,
                    .Name = _config.StoreName,
                    .StoreCode = If(String.IsNullOrEmpty(_config.StoreCode), Nothing, _config.StoreCode)
                }
            End If

            Log($"Melakukan Burn Point: Member UID {memberUid}, Poin: {pointAmount}, Nilai Rp: {orderAmountIdr:N0}, Ref: {providerRef}")
            Return Await SendAuthorizedRequestAsync(Of PaymentTransactionResponse)(HttpMethod.Post, url, payload)
        End Function

        ''' <summary>
        ''' Membatalkan transaksi pemotongan poin jika POS membatalkan transaksi / kasir void
        ''' </summary>
        Public Async Function CancelPointPaymentAsync(paymentRef As String, Optional cancelReason As String = "Kasir Cancel") As Task(Of ApiResponse(Of PaymentTransactionResponse))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/member/payment/{paymentRef}/cancel/"
            Dim payload = New With {.cancel_reason = cancelReason}

            Log($"Membatalkan pembayaran poin: Ref {paymentRef}, Alasan: {cancelReason}")
            Return Await SendAuthorizedRequestAsync(Of PaymentTransactionResponse)(HttpMethod.Post, url, payload)
        End Function

#End Region

#Region "Push Sales Order (Earning Point)"

        ''' <summary>
        ''' Mengirim data transaksi selesai ke Goapp untuk proses penambahan poin (Earning Point)
        ''' </summary>
        Public Async Function PushSalesOrderAsync(order As PushOrderRequest) As Task(Of ApiResponse(Of PushOrderResponse))
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/sales/order/"
            Log($"Mengirim transaksi {order.OrderNo} ke Goapp untuk Earning Point...")
            Return Await SendAuthorizedRequestAsync(Of PushOrderResponse)(HttpMethod.Post, url, order)
        End Function

#End Region

#Region "HTTP Helper Core"

        Private Async Function SendAuthorizedRequestAsync(Of T)(method As HttpMethod, url As String, Optional payload As Object = Nothing, Optional retryOn401 As Boolean = True) As Task(Of ApiResponse(Of T))
            Dim result As New ApiResponse(Of T)()
            Dim sw = System.Diagnostics.Stopwatch.StartNew()
            Dim requestJson As String = ""
            Dim responseJson As String = ""
            Dim errMsgForLog As String = ""

            Try
                Dim token = Await GetValidTokenAsync()
                Dim request As New HttpRequestMessage(method, url)
                request.Headers.Authorization = New AuthenticationHeaderValue("Bearer", token)

                If payload IsNot Nothing Then
                    requestJson = JsonConvert.SerializeObject(payload)
                    request.Content = New StringContent(requestJson, Encoding.UTF8, "application/json")
                End If

                Dim response = Await _httpClient.SendAsync(request)
                responseJson = Await response.Content.ReadAsStringAsync()
                result.StatusCode = CInt(response.StatusCode)
                result.RawJson = responseJson

                ' Jika token kadaluarsa (401 Unauthorized), auto-refresh token dan ulangi 1 kali
                If response.StatusCode = HttpStatusCode.Unauthorized AndAlso retryOn401 Then
                    Log("Token tidak valid/kedaluwarsa (401). Mengambil token baru dan mencoba ulang...")
                    Await GetValidTokenAsync(forceRefresh:=True)
                    Return Await SendAuthorizedRequestAsync(Of T)(method, url, payload, retryOn401:=False)
                End If

                If response.IsSuccessStatusCode Then
                    result.IsSuccess = True
                    If Not String.IsNullOrWhiteSpace(responseJson) Then
                        result.Data = JsonConvert.DeserializeObject(Of T)(responseJson)
                    End If
                    result.Message = "OK"
                Else
                    result.IsSuccess = False
                    ' Coba ekstrak pesan error dari JSON Goapp jika ada field 'detail' atau 'message'
                    Dim errMsg = response.ReasonPhrase
                    Try
                        Dim jObj = JObject.Parse(responseJson)
                        If jObj("detail") IsNot Nothing Then
                            errMsg = jObj("detail").ToString()
                        ElseIf jObj("message") IsNot Nothing Then
                            errMsg = jObj("message").ToString()
                        End If
                    Catch
                    End Try
                    result.Message = $"[{result.StatusCode}] {errMsg}"
                    errMsgForLog = result.Message
                End If

            Catch ex As Exception
                result.IsSuccess = False
                result.StatusCode = 0
                result.Message = $"Exception: {ex.Message}"
                errMsgForLog = ex.Message
                Log($"HTTP Request Error ({url}): {ex.Message}")
            Finally
                sw.Stop()
                ' Log to Database asynchronously (fire and forget)
                Dim logTask = _dbLogger.LogApiAsync(method.Method, url, requestJson, responseJson, result.StatusCode, result.IsSuccess, errMsgForLog, CInt(sw.ElapsedMilliseconds))
            End Try

            Return result
        End Function

#End Region

    End Class
End Namespace



