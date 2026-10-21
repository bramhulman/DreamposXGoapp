Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks
Imports DreamposXGoapp.Models
Imports Newtonsoft.Json

Namespace Services
    Public Partial Class GoappApiClient
        Private Shared ReadOnly SubmissionLock As New Object()
        Private Shared ReadOnly Submissions As New Dictionary(Of String, OrderSubmissionJournal.Entry)()
        Private Shared _nextCacheCleanup As DateTime = DateTime.MinValue

        ''' <summary>
        ''' Kirim transaksi tanpa member. Memerlukan SQL log; tidak memanggil API burn point.
        ''' Identitas member harus kosong; Goapp menentukan dukungan claim dan reward pada server.
        ''' </summary>
        Public Async Function PushGuestSalesOrderAsync(order As PushOrderRequest) As Task(Of ApiResponse(Of PushOrderResponse))
            Return Await PushGuestSalesOrderAsync(order, _config.CalculateEstimatePoint)
        End Function

        ''' <summary>Mengirim transaksi tanpa member dengan pilihan ringkasan estimasi poin.</summary>
        Public Async Function PushGuestSalesOrderAsync(order As PushOrderRequest, calculateEstimatePoint As Boolean) As Task(Of ApiResponse(Of PushOrderResponse))
            Return Await SubmitSalesOrderAsync(order, True, calculateEstimatePoint)
        End Function

        Private Shared Function SubmissionHash(value As String) As String
            Using sha = SHA256.Create()
                Return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", "").ToLowerInvariant()
            End Using
        End Function

        Private Async Function SubmitSalesOrderAsync(input As PushOrderRequest, guest As Boolean, calculateEstimatePoint As Boolean) As Task(Of ApiResponse(Of PushOrderResponse))
            If input Is Nothing Then Return OrderError("Data transaksi wajib diisi.")
            ' Snapshot mencegah perubahan input oleh pemanggil saat request sedang berjalan.
            Dim order = JsonConvert.DeserializeObject(Of PushOrderRequest)(JsonConvert.SerializeObject(input))
            If String.IsNullOrWhiteSpace(order.ProviderRef) Then order.ProviderRef = order.OrderNo
            If String.IsNullOrWhiteSpace(order.ProviderRef) OrElse order.ProviderRef.Length > 50 Then Return OrderError("provider_ref wajib diisi, maksimal 50 karakter. Gunakan nomor struk tetap untuk transaksi yang sama.")
            order.ProviderRef = order.ProviderRef.Trim()
            If String.IsNullOrWhiteSpace(order.OrderNo) Then order.OrderNo = order.ProviderRef
            If guest AndAlso order.Member IsNot Nothing Then Return OrderError("Transaksi tanpa member harus mengosongkan Member, termasuk UID dan nomor HP.")
            If guest AndAlso order.Payments IsNot Nothing AndAlso order.Payments.Any(Function(p) p IsNot Nothing AndAlso String.Equals(If(p.PaymentType, "").Trim(), "POINT", StringComparison.OrdinalIgnoreCase)) Then Return OrderError("Transaksi tanpa member tidak boleh menggunakan pembayaran POINT.")
            If guest AndAlso (order.Lines Is Nothing OrElse order.Lines.Count = 0 OrElse order.TotalPaid <= 0 OrElse order.TotalInclTax <= 0) Then Return OrderError("Transaksi harus berisi detail produk dan nilai transaksi lebih dari 0.")
            If guest AndAlso String.IsNullOrWhiteSpace(DbConnectionString) Then Return OrderError("Transaksi memerlukan DbConnectionString agar kedua log SQL tersimpan.")
            If order.Store Is Nothing Then
                order.Store = New OrderStoreRef With {.Uid = _config.StoreUid, .Name = _config.StoreName, .StoreCode = _config.StoreCode}
            End If
            Dim requestJson = JsonConvert.SerializeObject(order)
            ' Kedua metode memakai key yang sama agar order tidak terkirim lagi dengan mode member berbeda.
            Dim scope = _config.ChannelBaseUrl.TrimEnd("/"c) & "|" & _config.ChannelUid & "|" & If(order.Store.Uid.HasValue, "UID:" & order.Store.Uid.Value.ToString(), "CODE:" & order.Store.StoreCode) & "|" & order.ProviderRef
            Dim key = SubmissionHash(scope)
            Dim entry As OrderSubmissionJournal.Entry = Nothing
            Dim journal As OrderSubmissionJournal = Nothing
            If Not String.IsNullOrWhiteSpace(DbConnectionString) Then
                journal = New OrderSubmissionJournal(_redeemRepo)
                Try
                    entry = Await journal.ReserveAsync(key, order, requestJson, guest)
                Catch ex As Exception
                    Return OrderError("Log SQL/reservasi transaksi gagal; transaksi belum dikirim: " & ex.Message)
                End Try
            Else
                SyncLock SubmissionLock
                    If DateTime.UtcNow >= _nextCacheCleanup Then
                        Dim cutoff = DateTime.UtcNow.AddMonths(-3)
                        Dim expired = Submissions.Where(Function(pair) pair.Value.CreatedAtUtc < cutoff).Select(Function(pair) pair.Key).ToList()
                        For Each expiredKey In expired
                            Submissions.Remove(expiredKey)
                        Next
                        _nextCacheCleanup = DateTime.UtcNow.AddDays(1)
                    End If
                    If Not Submissions.TryGetValue(key, entry) Then
                        entry = New OrderSubmissionJournal.Entry With {.IsNew = True, .State = "Pending", .RequestJson = requestJson}
                        Submissions.Add(key, entry)
                    Else
                        entry = New OrderSubmissionJournal.Entry With {.State = entry.State, .RequestJson = entry.RequestJson, .ResponseJson = entry.ResponseJson, .StatusCode = entry.StatusCode}
                    End If
                End SyncLock
            End If
            If Not entry.IsNew Then
                If entry.RequestJson <> requestJson Then Return OrderError("Nomor transaksi sudah digunakan dengan payload berbeda. Request tidak dikirim ulang.", True)
                If entry.State = "Succeeded" Then
                    Dim cached = New ApiResponse(Of PushOrderResponse) With {
                        .IsSuccess = True, .IsDuplicate = True, .StatusCode = entry.StatusCode,
                        .RawJson = entry.ResponseJson, .RequestJson = requestJson,
                        .Message = "Transaksi sudah terkirim. Menggunakan hasil tersimpan tanpa POST ulang.",
                        .TransactionLogSaved = (journal IsNot Nothing), .ApiLogSaved = entry.ApiLogSaved
                    }
                    cached.Data = JsonConvert.DeserializeObject(Of PushOrderResponse)(entry.ResponseJson)
                    AddRewardSummaryWarning(cached, calculateEstimatePoint, guest)
                    Return cached
                End If
                Return OrderError("Transaksi sudah pernah dicoba (" & entry.State & "). Periksa log/Goapp sebelum mengirim ulang; tidak ada retry otomatis.", True)
            End If

            Log($"Mengirim sales order Ref={order.ProviderRef}, Mode={If(guest, "TanpaMember", "Member")}, Submission={key}.")
            Dim url = $"{_config.ChannelBaseUrl.TrimEnd("/"c)}/sales/order/?calculate_estimate_point={If(calculateEstimatePoint, "1", "0")}"
            Dim result = Await SendAuthorizedRequestAsync(Of PushOrderResponse)(HttpMethod.Post, url, order)
            result.RequiresReconciliation = result.RequiresReconciliation OrElse result.StatusCode = 0 OrElse result.StatusCode >= 500
            If result.IsSuccess AndAlso result.Data Is Nothing Then
                result.RequiresReconciliation = True
                result.WarningMessage = "Goapp menerima HTTP request tetapi tidak mengembalikan detail transaksi; periksa log sebelum mengirim ulang."
            End If
            If guest AndAlso result.Data IsNot Nothing AndAlso result.Data.TotalEarnedPoints <> 0 Then
                result.WarningMessage = "Goapp mengembalikan reward pada transaksi tanpa member. Periksa konfigurasi server; jangan kirim ulang."
                result.RequiresReconciliation = True
            End If
            If journal IsNot Nothing Then
                Try
                    Await journal.CompleteAsync(entry.Id, result)
                    result.TransactionLogSaved = True
                Catch ex As Exception
                    result.WarningMessage = (If(result.WarningMessage, "") & " Update log SQL gagal: " & ex.Message).Trim()
                    result.RequiresReconciliation = True
                End Try
            Else
                SyncLock SubmissionLock
                    entry.State = If(result.IsSuccess AndAlso Not result.RequiresReconciliation, "Succeeded", "Unknown")
                    entry.ResponseJson = result.RawJson
                    entry.StatusCode = result.StatusCode
                End SyncLock
                result.WarningMessage = "SQL belum dikonfigurasi; perlindungan kirim ulang hanya berlaku selama proses aplikasi ini hidup."
            End If
            If Not result.ApiLogSaved AndAlso Not String.IsNullOrWhiteSpace(DbConnectionString) Then
                result.WarningMessage = (If(result.WarningMessage, "") & " Log request/response API belum tersimpan. Jangan kirim ulang transaksi untuk memperbaiki log.").Trim()
            End If
            AddRewardSummaryWarning(result, calculateEstimatePoint, guest)
            Return result
        End Function

        Private Shared Sub AddRewardSummaryWarning(result As ApiResponse(Of PushOrderResponse), requested As Boolean, guest As Boolean)
            If Not requested OrElse guest OrElse Not result.IsSuccess OrElse result.Data Is Nothing Then Return
            If result.Data.RewardSummary IsNot Nothing AndAlso result.Data.RewardSummary.Amount.HasValue Then Return
            Dim warning = "reward_summary tidak tersedia; total poin memakai rincian reward."
            If result.IsDuplicate Then warning &= " Respons tersimpan dipakai tanpa POST ulang."
            result.WarningMessage = (If(result.WarningMessage, "") & " " & warning).Trim()
        End Sub

        Private Shared Function OrderError(message As String, Optional duplicate As Boolean = False) As ApiResponse(Of PushOrderResponse)
            Return New ApiResponse(Of PushOrderResponse) With {.IsSuccess = False, .StatusCode = If(duplicate, 409, 400), .Message = message, .IsDuplicate = duplicate, .RequiresReconciliation = duplicate}
        End Function
    End Class
End Namespace