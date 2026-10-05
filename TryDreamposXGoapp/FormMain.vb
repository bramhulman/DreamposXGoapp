Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services
Imports Newtonsoft.Json

Public Class FormMain
    Private _client As GoappApiClient
    Private _currentMember As MemberResponse
    Private _lastPaymentRef As String = String.Empty

    Private Class SchemeComboItem
        Public Property Uid As Long
        Public Property Name As String

        Public Sub New(u As Long, n As String)
            Uid = u
            Name = n
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Name} (UID: {Uid})"
        End Function
    End Class

    Private Async Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtApiKey.Text = System.Configuration.ConfigurationManager.AppSettings("Goapp.ApiKey")
        txtApiSecret.Text = System.Configuration.ConfigurationManager.AppSettings("Goapp.ApiSecret")
        InitClient()
        AppendLog("Test Harness DreamPOS x Goapp siap digunakan.")
        Await LoadSchemesAsync()
    End Sub

    Private Async Function LoadSchemesAsync() As Task
        cmbRegScheme.Items.Clear()
        ' Skema default standar jika offline / belum koneksi
        Dim defaultItem As New SchemeComboItem(138348545946696, "Go Member")
        cmbRegScheme.Items.Add(defaultItem)
        cmbRegScheme.SelectedIndex = 0

        Try
            Dim resp = Await _client.GetMemberSchemesAsync()
            If resp.IsSuccess AndAlso resp.Data IsNot Nothing AndAlso resp.Data.Count > 0 Then
                cmbRegScheme.Items.Clear()
                For Each sc In resp.Data
                    cmbRegScheme.Items.Add(New SchemeComboItem(sc.Uid, sc.Name))
                Next
                cmbRegScheme.SelectedIndex = 0
                AppendLog($"[SCHEME] Berhasil memuat {resp.Data.Count} tier membership Goapp.")
            End If
        Catch ex As Exception
            AppendLog($"[SCHEME] Info skema membership: {ex.Message}")
        End Try
    End Function

    Private Sub btnGoToRegister_Click(sender As Object, e As EventArgs) Handles btnGoToRegister.Click
        tabControl.SelectedTab = tabRegister
        If Not String.IsNullOrWhiteSpace(txtInputMember.Text) Then
            txtRegMobile.Text = txtInputMember.Text.Trim()
        End If
        txtRegFirstName.Focus()
    End Sub

    Private Sub InitClient()
        Dim apiKey As String = txtApiKey.Text.Trim()
        If String.IsNullOrEmpty(apiKey) Then
            apiKey = System.Configuration.ConfigurationManager.AppSettings("Goapp.ApiKey")
        End If
        Dim apiSecret As String = txtApiSecret.Text.Trim()
        If String.IsNullOrEmpty(apiSecret) Then
            apiSecret = System.Configuration.ConfigurationManager.AppSettings("Goapp.ApiSecret")
        End If

        Dim channelUidStr As String = System.Configuration.ConfigurationManager.AppSettings("Goapp.ChannelUid")
        Dim channelUid As Long = 0
        If Not String.IsNullOrEmpty(channelUidStr) Then
            Long.TryParse(channelUidStr, channelUid)
        End If

        Dim authUrl As String = System.Configuration.ConfigurationManager.AppSettings("Goapp.AuthBaseUrl")
        If String.IsNullOrEmpty(authUrl) Then authUrl = "https://account.goapp.co.id/auth"

        Dim channelUrl As String = System.Configuration.ConfigurationManager.AppSettings("Goapp.ChannelBaseUrl")
        If String.IsNullOrEmpty(channelUrl) Then channelUrl = "https://api.goapp.co.id/channel/v1"

        Dim storeUidStr As String = System.Configuration.ConfigurationManager.AppSettings("Goapp.StoreUid")
        Dim storeUid As Long = 0
        Dim finalStoreUid As Long? = Nothing
        If Not String.IsNullOrEmpty(storeUidStr) AndAlso Long.TryParse(storeUidStr, storeUid) Then
            finalStoreUid = storeUid
        End If

        Dim storeName As String = System.Configuration.ConfigurationManager.AppSettings("Goapp.StoreName")
        If String.IsNullOrEmpty(storeName) Then storeName = "Default POS Store"

        Dim config As New GoappConfig With {
            .ApiKey = apiKey,
            .ApiSecret = apiSecret,
            .ChannelUid = channelUid,
            .AuthBaseUrl = authUrl,
            .ChannelBaseUrl = channelUrl,
            .StoreUid = finalStoreUid,
            .StoreName = storeName,
            .TimeoutSeconds = 30,
            .MaxRetryAttempts = 3,
            .RetryDelayMilliseconds = 1500
        }

        _client = New GoappApiClient(config)
        AddHandler _client.OnLog, AddressOf OnClientLog
    End Sub

    Private Sub OnClientLog(msg As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() AppendLog(msg))
        Else
            AppendLog(msg)
        End If
    End Sub

    Private Sub AppendLog(msg As String)
        txtLogs.AppendText(msg & Environment.NewLine)
        txtLogs.SelectionStart = txtLogs.TextLength
        txtLogs.ScrollToCaret()
    End Sub

    Private Sub btnClearLogs_Click(sender As Object, e As EventArgs) Handles btnClearLogs.Click
        txtLogs.Clear()
    End Sub

#Region "1. Test Authentication & Channel Info"

    Private Async Sub btnTestAuth_Click(sender As Object, e As EventArgs) Handles btnTestAuth.Click
        btnTestAuth.Enabled = False
        lblStatusChannel.Text = "Status: Menguji koneksi..."
        lblStatusChannel.ForeColor = Color.OrangeRed

        Try
            InitClient()
            ' Cek token dan info channel toko
            Dim channelResp = Await _client.GetChannelDirectoryAsync()

            If channelResp.IsSuccess AndAlso channelResp.Data IsNot Nothing Then
                lblStatusChannel.Text = $"Terkoneksi: {channelResp.Data.Name} ({channelResp.Data.ChannelType})"
                lblStatusChannel.ForeColor = Color.DarkGreen
                Await LoadSchemesAsync()
                MessageBox.Show($"Koneksi Berhasil!{Environment.NewLine}Channel: {channelResp.Data.Name}{Environment.NewLine}UID: {channelResp.Data.Uid}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblStatusChannel.Text = "Status: Gagal Terkoneksi"
                lblStatusChannel.ForeColor = Color.Red
                MessageBox.Show($"Gagal terkoneksi ke Goapp:{Environment.NewLine}{channelResp.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            lblStatusChannel.Text = "Status: Terjadi Kesalahan"
            lblStatusChannel.ForeColor = Color.Red
            MessageBox.Show($"Error: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnTestAuth.Enabled = True
        End Try
    End Sub

#End Region

#Region "2. Pencarian & Validasi Member (Async/Await)"

    ''' <summary>
    ''' Contoh implementasi pencarian member secara Asynchronous (Non-Blocking UI)
    ''' Memvalidasi member melalui No HP atau Member ID
    ''' </summary>
    Private Async Sub btnSearchMember_Click(sender As Object, e As EventArgs) Handles btnSearchMember.Click
        Dim inputQuery = txtInputMember.Text.Trim()
        If String.IsNullOrWhiteSpace(inputQuery) Then
            MessageBox.Show("Silakan masukkan Nomor HP atau Member ID!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtInputMember.Focus()
            Return
        End If

        ' Disable UI selama proses pencarian agar kasir tidak klik berulang kali
        btnSearchMember.Enabled = False
        btnSearchMember.Text = "Mencari..."
        Cursor = Cursors.WaitCursor

        Try
            ' Panggil fungsi Async dari DLL DreamposXGoapp
            Dim response As ApiResponse(Of MemberResponse) = Await _client.GetMemberAsync(inputQuery)

            If response.IsSuccess AndAlso response.Data IsNot Nothing Then
                _currentMember = response.Data

                ' Tampilkan detail member ke UI Form
                lblMemberName.Text = $"Nama Member : {_currentMember.FullName}"
                lblMemberPhone.Text = $"No Handphone : {_currentMember.MobileNo}"
                lblMemberLevel.Text = $"Level / Scheme : {If(_currentMember.Level IsNot Nothing, _currentMember.Level.Name, "-")} / {If(_currentMember.Scheme IsNot Nothing, _currentMember.Scheme.Name, "-")}"
                lblMemberPoints.Text = $"Sisa Poin : {_currentMember.AvailablePoints:N0} Pts"
                lblMemberRupiah.Text = $"Nilai Rupiah Poin : Rp {_currentMember.PointsInRupiah:N0}"
                lblMemberReferral.Text = $"Referral Code : {If(String.IsNullOrEmpty(_currentMember.ReferralCode), "-", _currentMember.ReferralCode)}"

                ' Tampilkan raw JSON response di box sebelah kanan
                txtRawJson.Text = FormatJson(response.RawJson)

                AppendLog($"[SUCCESS] Member {_currentMember.FullName} berhasil divalidasi. Poin aktif: {_currentMember.AvailablePoints}")
            Else
                _currentMember = Nothing
                ResetMemberLabels()
                txtRawJson.Text = response.RawJson
                MessageBox.Show($"Member tidak ditemukan atau terjadi kesalahan:{Environment.NewLine}{response.Message}", "Hasil Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show($"Terjadi kesalahan saat memanggil Goapp API:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnSearchMember.Enabled = True
            btnSearchMember.Text = "Cek Member (Async)"
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ResetMemberLabels()
        lblMemberName.Text = "Nama Member : -"
        lblMemberPhone.Text = "No Handphone : -"
        lblMemberLevel.Text = "Level / Scheme : -"
        lblMemberPoints.Text = "Sisa Poin : 0 Pts"
        lblMemberRupiah.Text = "Nilai Rupiah Poin : Rp 0"
        lblMemberReferral.Text = "Referral Code : -"
    End Sub

#End Region

#Region "3. Voucher (Validasi & Use dengan Auto-Retry)"

    Private Async Sub btnValidateVoucher_Click(sender As Object, e As EventArgs) Handles btnValidateVoucher.Click
        Dim code = txtVoucherCode.Text.Trim()
        If String.IsNullOrEmpty(code) Then
            MessageBox.Show("Masukkan kode voucher terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnValidateVoucher.Enabled = False
        lblVoucherStatus.Text = "Status: Memvalidasi voucher ke Goapp..."
        lblVoucherStatus.ForeColor = Color.OrangeRed

        Try
            Dim memberUid As Long? = If(_currentMember IsNot Nothing, _currentMember.Uid, CType(Nothing, Long?))
            Dim resp = Await _client.ValidateVoucherAsync(code, memberUid)

            If resp.IsSuccess Then
                lblVoucherStatus.Text = $"Status: Voucher VALID! {resp.Message}"
                lblVoucherStatus.ForeColor = Color.DarkGreen
                txtRawJson.Text = FormatJson(resp.RawJson)
            Else
                lblVoucherStatus.Text = $"Status: Voucher TIDAK VALID! ({resp.Message})"
                lblVoucherStatus.ForeColor = Color.Red
            End If
        Finally
            btnValidateVoucher.Enabled = True
        End Try
    End Sub

    Private Async Sub btnUseVoucher_Click(sender As Object, e As EventArgs) Handles btnUseVoucher.Click
        Dim code = txtVoucherCode.Text.Trim()
        Dim txRef = txtVoucherTxRef.Text.Trim()

        If String.IsNullOrEmpty(code) OrElse String.IsNullOrEmpty(txRef) Then
            MessageBox.Show("Kode Voucher dan No Transaksi (Tx Ref) harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnUseVoucher.Enabled = False
        lblVoucherStatus.Text = "Status: Memproses voucher dengan Auto-Retry..."
        lblVoucherStatus.ForeColor = Color.DarkBlue

        Try
            Dim memberUid As Long? = If(_currentMember IsNot Nothing, _currentMember.Uid, CType(Nothing, Long?))
            ' Panggil fungsi Auto-Retry dari DLL
            Dim resp = Await _client.UseVoucherWithRetryAsync(code, txRef, memberUid, customMaxRetry:=3)

            If resp.IsSuccess Then
                lblVoucherStatus.Text = $"Status: Voucher BERHASIL DIGUNAKAN pada transaksi {txRef}!"
                lblVoucherStatus.ForeColor = Color.DarkGreen
                MessageBox.Show("Voucher sukses diterapkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblVoucherStatus.Text = $"Status: Gagal menggunakan voucher! ({resp.Message})"
                lblVoucherStatus.ForeColor = Color.Red
                MessageBox.Show($"Voucher gagal diterapkan:{Environment.NewLine}{resp.Message}", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtRawJson.Text = FormatJson(resp.RawJson)
        Finally
            btnUseVoucher.Enabled = True
        End Try
    End Sub

    Private Async Sub btnCancelVoucher_Click(sender As Object, e As EventArgs) Handles btnCancelVoucher.Click
        Dim code = txtVoucherCode.Text.Trim()
        Dim txRef = txtVoucherTxRef.Text.Trim()

        If String.IsNullOrEmpty(code) OrElse String.IsNullOrEmpty(txRef) Then
            MessageBox.Show("Kode Voucher dan No Transaksi (Tx Ref) harus diisi untuk membatalkan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnCancelVoucher.Enabled = False
        lblVoucherStatus.Text = "Status: Membatalkan pemakaian voucher ke Goapp..."
        lblVoucherStatus.ForeColor = Color.OrangeRed

        Try
            Dim memberUid As Long? = If(_currentMember IsNot Nothing, _currentMember.Uid, CType(Nothing, Long?))
            Dim resp = Await _client.CancelVoucherAsync(code, txRef, memberUid)

            If resp.IsSuccess Then
                lblVoucherStatus.Text = $"Status: Voucher {code} BERHASIL DIBATALKAN (Void)!"
                lblVoucherStatus.ForeColor = Color.DarkGreen
                MessageBox.Show("Pembatalan pemakaian voucher berhasil! Kupon kembali aktif.", "Sukses Void Voucher", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblVoucherStatus.Text = $"Status: Gagal membatalkan voucher. {resp.Message}"
                lblVoucherStatus.ForeColor = Color.Red
                MessageBox.Show($"Pembatalan voucher gagal:{Environment.NewLine}{resp.Message}", "Gagal Void", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtRawJson.Text = FormatJson(resp.RawJson)
        Finally
            btnCancelVoucher.Enabled = True
        End Try
    End Sub

#End Region

#Region "4. Burn Point & Push Sales Order"

    Private Async Sub btnBurnPoint_Click(sender As Object, e As EventArgs) Handles btnBurnPoint.Click
        If _currentMember Is Nothing Then
            MessageBox.Show("Silakan cari dan pilih member terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim burnPt As Decimal
        Dim billTotal As Decimal
        If Not Decimal.TryParse(txtBurnPoint.Text, burnPt) OrElse burnPt <= 0 Then
            MessageBox.Show("Jumlah poin yang dibakar tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Decimal.TryParse(txtBillTotal.Text, billTotal)

        ' Cek saldo poin member
        If burnPt > _currentMember.AvailablePoints Then
            MessageBox.Show($"Poin member ({_currentMember.AvailablePoints:N0}) tidak cukup untuk membakar {burnPt:N0} poin!", "Saldo Kurang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnBurnPoint.Enabled = False
        Try
            Dim txRef = "TRX-" & DateTime.Now.ToString("yyMMddHHmmss")
            Dim orderAmountIdr = burnPt * 1000D ' Estimasi rasio 1 poin = Rp 1.000

            Dim resp = Await _client.CreatePointPaymentAsync(_currentMember.Uid, burnPt, orderAmountIdr, txRef)
            If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                _lastPaymentRef = resp.Data.PaymentRef
                lblLastPaymentRef.Text = $"Last Payment Ref: {_lastPaymentRef} (Status: {resp.Data.Status})"
                MessageBox.Show($"Burn Point Berhasil!{Environment.NewLine}Ref: {_lastPaymentRef}{Environment.NewLine}Status: {resp.Data.Status}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Refresh data member
                btnSearchMember.PerformClick()
            Else
                MessageBox.Show($"Burn Point Gagal:{Environment.NewLine}{resp.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtRawJson.Text = FormatJson(resp.RawJson)
        Finally
            btnBurnPoint.Enabled = True
        End Try
    End Sub

    Private Async Sub btnCancelBurnPoint_Click(sender As Object, e As EventArgs) Handles btnCancelBurnPoint.Click
        If String.IsNullOrEmpty(_lastPaymentRef) Then
            MessageBox.Show("Belum ada Payment Ref transaksi poin yang bisa dibatalkan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnCancelBurnPoint.Enabled = False
        Try
            Dim resp = Await _client.CancelPointPaymentAsync(_lastPaymentRef, "Pembatalan oleh Kasir POS")
            If resp.IsSuccess Then
                lblLastPaymentRef.Text = $"Last Payment Ref: {_lastPaymentRef} (Status: VOID/CANCEL)"
                MessageBox.Show("Pembatalan poin berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSearchMember.PerformClick()
            Else
                MessageBox.Show($"Gagal membatalkan poin:{Environment.NewLine}{resp.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtRawJson.Text = FormatJson(resp.RawJson)
        Finally
            btnCancelBurnPoint.Enabled = True
        End Try
    End Sub

    Private Async Sub btnPushSalesOrder_Click(sender As Object, e As EventArgs) Handles btnPushSalesOrder.Click
        btnPushSalesOrder.Enabled = False
        Try
            Dim orderNo = "POS-" & DateTime.Now.ToString("yyMMdd-HHmmss")
            Dim billTotal As Decimal
            Decimal.TryParse(txtBillTotal.Text, billTotal)
            If billTotal <= 0 Then billTotal = 50000D

            Dim burnPoint As Decimal = 0
            Decimal.TryParse(txtBurnPoint.Text, burnPoint)
            
            Dim pointPayAmount = If(burnPoint <= billTotal, burnPoint, billTotal)
            Dim remainingTotal = billTotal - pointPayAmount

            Dim item1Price = Math.Round(billTotal * 0.6D)
            Dim item2Price = billTotal - item1Price

            Dim pay1Amount = Math.Round(remainingTotal * 0.7D)
            Dim pay2Amount = remainingTotal - pay1Amount

            Dim orderReq As New PushOrderRequest With {
                .OrderNo = orderNo,
                .ProviderRef = orderNo,
                .OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sszzz"),
                .LinesTotal = billTotal,
                .LinesTax = 0,
                .TotalInclTax = billTotal,
                .TotalPaid = billTotal,
                .PaidAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sszzz"),
                .CompletedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sszzz"),
                .Lines = New List(Of OrderLineItem) From {
                    New OrderLineItem With {
                        .Product = New OrderProductInfo With {
                            .Sku = "SKFTRR02-S",
                            .Name = "Baju FT RR Small"
                        },
                        .Quantity = 1,
                        .PriceBeforeDiscount = item1Price,
                        .Price = item1Price
                    },
                    New OrderLineItem With {
                        .Product = New OrderProductInfo With {
                            .Sku = "SKFTRR03-M",
                            .Name = "Baju FT RR Medium"
                        },
                        .Quantity = 1,
                        .PriceBeforeDiscount = item2Price,
                        .Price = item2Price
                    }
                },
                .Payments = New List(Of OrderPaymentItem)()
            }

            If pointPayAmount > 0 Then
                orderReq.Payments.Add(New OrderPaymentItem With {
                    .PaymentMethodName = "Point",
                    .PaymentType = "POINT",
                    .Amount = pointPayAmount
                })
            End If

            If pay1Amount > 0 Then
                orderReq.Payments.Add(New OrderPaymentItem With {
                    .PaymentMethodName = "EDC BCA",
                    .PaymentType = "EDC",
                    .Amount = pay1Amount
                })
            End If

            If pay2Amount > 0 Then
                orderReq.Payments.Add(New OrderPaymentItem With {
                    .PaymentMethodName = "Gopay",
                    .PaymentType = "EWALLET",
                    .Amount = pay2Amount
                })
            End If

            If _currentMember IsNot Nothing Then
                orderReq.Member = New OrderMemberRef With {
                    .Uid = _currentMember.Uid,
                    .MobileNo = _currentMember.MobileNo
                }
            End If

            Dim requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(orderReq, Newtonsoft.Json.Formatting.Indented, New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore})
            txtOrderRawJson.Text = "=== REQUEST JSON ===" & Environment.NewLine & requestJson & Environment.NewLine & Environment.NewLine

            Dim resp = Await _client.PushSalesOrderAsync(orderReq)
            If resp.IsSuccess Then
                Dim earnedPts = If(resp.Data?.Reward IsNot Nothing AndAlso resp.Data.Reward.Count > 0, resp.Data.Reward(0).Amount, 0)
                MessageBox.Show($"Transaksi {orderNo} berhasil dikirim ke Goapp!{Environment.NewLine}Order UID: {resp.Data?.Uid}{Environment.NewLine}Poin Diperoleh: {earnedPts} Pts", "Earning Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show($"Gagal mengirim transaksi:{Environment.NewLine}{resp.Message}", "Gagal Push Order", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtOrderRawJson.Text &= "=== RESPONSE JSON ===" & Environment.NewLine & FormatJson(resp.RawJson)
        Finally
            btnPushSalesOrder.Enabled = True
        End Try
    End Sub

#End Region

#Region "5. Struk POS & Survey QR Preview"

    Private Sub btnGenerateReceipt_Click(sender As Object, e As EventArgs) Handles btnGenerateReceipt.Click
        Dim orderNo = "STRUK-" & DateTime.Now.ToString("yyMMdd-HHmmss")
        Dim billTotal As Decimal
        Dim burnPt As Decimal
        Decimal.TryParse(txtBillTotal.Text, billTotal)
        Decimal.TryParse(txtBurnPoint.Text, burnPt)
        If billTotal <= 0 Then billTotal = 50000D

        Dim paidTotal = billTotal - (burnPt * 1000D)
        If paidTotal < 0 Then paidTotal = 0

        Dim receiptContent = ReceiptGenerator.GenerateReceiptText(
            storeName:="DREAMPOS STORE JAKARTA",
            orderNo:=orderNo,
            cashierName:="Kasir 01",
            member:=_currentMember,
            earnedPoints:=Math.Floor(billTotal / 10000D),
            burnedPoints:=burnPt,
            orderTotal:=billTotal,
            paidTotal:=paidTotal,
            surveyUrlBase:="https://survey.goapp.co.id"
        )

        txtReceiptPreview.Text = receiptContent
        AppendLog($"[RECEIPT] Preview struk transaksi {orderNo} berhasil di-generate.")

        ' Reconstruct URL to generate QR preview
        Dim surveyUrl As String = $"https://survey.goapp.co.id/?ref={orderNo}"
        If _currentMember IsNot Nothing Then
            surveyUrl &= $"&member_uid={_currentMember.Uid}&mobile={_currentMember.MobileNo}"
        End If

        Try
            picQrCode.Load($"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={Uri.EscapeDataString(surveyUrl)}")
        Catch ex As Exception
            AppendLog("[ERROR] Gagal merender QR Code preview: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "6. Pendaftaran Member Baru (Save/Register Member CRM)"

    Private Async Sub btnSubmitRegister_Click(sender As Object, e As EventArgs) Handles btnSubmitRegister.Click
        Dim firstName = txtRegFirstName.Text.Trim()
        Dim lastName = txtRegLastName.Text.Trim()
        Dim mobileNo = txtRegMobile.Text.Trim()
        Dim email = txtRegEmail.Text.Trim()

        If String.IsNullOrWhiteSpace(firstName) Then
            MessageBox.Show("Nama depan member wajib diisi!", "Validasi Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRegFirstName.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(mobileNo) Then
            MessageBox.Show("Nomor Handphone member wajib diisi!", "Validasi Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRegMobile.Focus()
            Return
        End If

        Dim selectedSchemeUid As Long = 138348545946696
        Dim selectedSchemeName As String = "Go Member"
        If cmbRegScheme.SelectedItem IsNot Nothing AndAlso TypeOf cmbRegScheme.SelectedItem Is SchemeComboItem Then
            Dim sc = DirectCast(cmbRegScheme.SelectedItem, SchemeComboItem)
            selectedSchemeUid = sc.Uid
            selectedSchemeName = sc.Name
        End If

        btnSubmitRegister.Enabled = False
        lblRegStatus.Text = "Status: Mengirim data member baru ke Goapp..."
        lblRegStatus.ForeColor = Color.DarkOrange
        Cursor = Cursors.WaitCursor

        Try
            Dim emailParam = If(String.IsNullOrWhiteSpace(email), Nothing, email)
            Dim resp = Await _client.RegisterMemberAsync(firstName, lastName, mobileNo, emailParam, selectedSchemeUid, selectedSchemeName)

            If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                lblRegStatus.Text = $"Status: Registrasi BERHASIL! UID: {resp.Data.Uid}"
                lblRegStatus.ForeColor = Color.DarkGreen
                AppendLog($"[REGISTER SUCCESS] Member {resp.Data.FullName} ({resp.Data.MobileNo}) terdaftar dengan UID {resp.Data.Uid}")

                MessageBox.Show($"Pendaftaran Member Baru Berhasil!{Environment.NewLine}" & _
                                $"Nama: {resp.Data.FullName}{Environment.NewLine}" & _
                                $"No HP: {resp.Data.MobileNo}{Environment.NewLine}" & _
                                $"UID Goapp: {resp.Data.Uid}", "Registrasi Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Kosongkan input form registrasi
                txtRegFirstName.Clear()
                txtRegLastName.Clear()
                txtRegEmail.Clear()

                ' Pindahkan input ke Tab 1 dan otomatis trigger pencarian
                txtInputMember.Text = mobileNo
                tabControl.SelectedTab = tabMember
                btnSearchMember.PerformClick()
            Else
                lblRegStatus.Text = $"Status: Gagal mendaftarkan member. {resp.Message}"
                lblRegStatus.ForeColor = Color.Red
                MessageBox.Show($"Pendaftaran member baru gagal:{Environment.NewLine}{resp.Message}", "Registrasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            txtRawJson.Text = FormatJson(resp.RawJson)
        Catch ex As Exception
            lblRegStatus.Text = $"Status: Terjadi exception: {ex.Message}"
            lblRegStatus.ForeColor = Color.Red
            MessageBox.Show($"Terjadi kesalahan:{Environment.NewLine}{ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnSubmitRegister.Enabled = True
            Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region "7. Master & Siklus Voucher (Insert, List Deals, & Void Code)"

    Private Async Sub btnCreateDeal_Click(sender As Object, e As EventArgs) Handles btnCreateDeal.Click
        Dim name = txtDealName.Text.Trim()
        Dim sku = txtDealSku.Text.Trim()
        Dim amountText = txtDealAmount.Text.Trim()
        Dim redeemCode = txtDealCode.Text.Trim()
        Dim discountType = If(cmbDealType.SelectedIndex = 1, "percentage", If(cmbDealType.SelectedIndex = 2, "free_item", "amount"))

        If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(sku) Then
            MessageBox.Show("Nama Promo dan Reward SKU wajib diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim amount As Decimal = 0
        Decimal.TryParse(amountText, amount)

        btnCreateDeal.Enabled = False
        lblCreateDealStatus.Text = "Status: Mengirim pendaftaran Master Voucher ke Goapp..."
        lblCreateDealStatus.ForeColor = Color.DarkOrange
        Cursor = Cursors.WaitCursor

        Try
            Dim startTime = DateTime.Now
            Dim endTime = DateTime.Now.AddMonths(3)

            Dim resp = Await _client.CreateDealAsync(
                name:=name,
                rewardSku:=sku,
                startTime:=startTime,
                endTime:=endTime,
                discountType:=discountType,
                discountAmount:=amount,
                redeemCode:=redeemCode
            )

            If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                lblCreateDealStatus.Text = $"Status: Sukses! UID Master Voucher: {resp.Data.Uid}"
                lblCreateDealStatus.ForeColor = Color.DarkGreen
                MessageBox.Show($"Master Voucher Berhasil Dibuat!{Environment.NewLine}Nama: {resp.Data.Name}{Environment.NewLine}UID: {resp.Data.Uid}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnLoadDeals.PerformClick()
            Else
                lblCreateDealStatus.Text = $"Status: Gagal membuat voucher. {resp.Message}"
                lblCreateDealStatus.ForeColor = Color.Red
                MessageBox.Show($"Pembuatan voucher via API gagal:{Environment.NewLine}{resp.Message}{Environment.NewLine}{Environment.NewLine}Catatan Arsitektur Goapp:{Environment.NewLine}- Master deal promosi umumnya dibuat melalui CRM Admin Dashboard.{Environment.NewLine}- Jika via API, field 'reward_sku' harus terdaftar di master catalog toko.", "Info Goapp CRM", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            txtRawJson.Text = FormatJson(resp.RawJson)
        Catch ex As Exception
            lblCreateDealStatus.Text = $"Status: Exception: {ex.Message}"
            lblCreateDealStatus.ForeColor = Color.Red
            MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnCreateDeal.Enabled = True
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnLoadDeals_Click(sender As Object, e As EventArgs) Handles btnLoadDeals.Click
        btnLoadDeals.Enabled = False
        btnLoadDeals.Text = "Memuat Promo..."
        lstDeals.Items.Clear()

        Try
            Dim resp = Await _client.GetAvailableDealsAsync()
            If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                For Each d In resp.Data
                    Dim kodePromo = If(Not String.IsNullOrEmpty(d.RedeemCode), d.RedeemCode, d.Uid.ToString())
                    Dim desc = $"[KODE: {kodePromo}] {d.Name} | Tipe: {d.RewardSku}"
                    lstDeals.Items.Add(desc)
                Next
                AppendLog($"[DEALS] Berhasil memuat {resp.Data.Count} promo deal aktif dari Goapp.")
                txtRawJson.Text = FormatJson(resp.RawJson)
                If resp.Data.Count = 0 Then
                    lstDeals.Items.Add("(Tidak ada promo aktif di channel ini)")
                End If
            Else
                MessageBox.Show($"Gagal memuat daftar deal: {resp.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            btnLoadDeals.Enabled = True
            btnLoadDeals.Text = "Muat Promo Aktif Toko (GET /deal/)"
        End Try
    End Sub

    Private Sub lstDeals_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDeals.SelectedIndexChanged
        If lstDeals.SelectedItem IsNot Nothing Then
            Dim selected = lstDeals.SelectedItem.ToString()
            Dim match = System.Text.RegularExpressions.Regex.Match(selected, "UID:\s*(\d+)")
            If match.Success Then
                Dim dealUid = match.Groups(1).Value
                txtCancelVoucherCode.Text = dealUid
                txtVoucherCode.Text = dealUid
            End If
        End If
    End Sub

    Private Async Sub btnSubmitCancelVoucher_Click(sender As Object, e As EventArgs) Handles btnSubmitCancelVoucher.Click
        Dim code = txtCancelVoucherCode.Text.Trim()
        Dim txRef = txtCancelTxRef.Text.Trim()

        If String.IsNullOrWhiteSpace(code) OrElse String.IsNullOrWhiteSpace(txRef) Then
            MessageBox.Show("Kode Voucher dan Transaction Ref POS wajib diisi!", "Validasi Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnSubmitCancelVoucher.Enabled = False
        lblCancelVoucherStatus.Text = "Status: Mengirim pembatalan voucher (cancel_code)..."
        lblCancelVoucherStatus.ForeColor = Color.DarkOrange
        Cursor = Cursors.WaitCursor

        Try
            Dim memberUid As Long? = If(_currentMember IsNot Nothing, _currentMember.Uid, CType(Nothing, Long?))
            Dim resp = Await _client.CancelVoucherAsync(code, txRef, memberUid)

            If resp.IsSuccess Then
                lblCancelVoucherStatus.Text = $"Status: Voucher {code} BERHASIL DIBATALKAN (Void)!"
                lblCancelVoucherStatus.ForeColor = Color.DarkGreen
                MessageBox.Show($"Pembatalan Voucher Berhasil!{Environment.NewLine}Voucher {code} pada transaksi {txRef} kini telah dibatalkan dan dapat digunakan kembali.", "Void Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblCancelVoucherStatus.Text = $"Status: Gagal membatalkan voucher. {resp.Message}"
                lblCancelVoucherStatus.ForeColor = Color.Red
                MessageBox.Show($"Pembatalan voucher gagal:{Environment.NewLine}{resp.Message}", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            txtRawJson.Text = FormatJson(resp.RawJson)
        Catch ex As Exception
            lblCancelVoucherStatus.Text = $"Status: Exception: {ex.Message}"
            lblCancelVoucherStatus.ForeColor = Color.Red
            MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnSubmitCancelVoucher.Enabled = True
            Cursor = Cursors.Default
        End Try
    End Sub

#End Region

    Private Function FormatJson(raw As String) As String
        If String.IsNullOrWhiteSpace(raw) Then Return String.Empty
        Try
            Dim parsed = JsonConvert.DeserializeObject(raw)
            Return JsonConvert.SerializeObject(parsed, Formatting.Indented)
        Catch
            Return raw
        End Try
    End Function

End Class
