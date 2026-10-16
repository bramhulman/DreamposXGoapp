Imports DreamposXGoapp.Forms
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services
Imports Newtonsoft.Json

Public Class FormMain
    Private _config As GoappConfig
    Private _client As GoappApiClient
    Private _currentMember As MemberResponse
    Private _lastPaymentRef As String = String.Empty
    Private ReadOnly _salesRef As New TextBox With {.Location = New Point(450, 22), .Size = New Size(250, 27), .MaxLength = 50}
    Private ReadOnly _allowWithoutMember As New CheckBox With {.Text = "Ijinkan tanpa member", .Location = New Point(440, 153), .AutoSize = True}
    Private _salesOrderTime As String = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:sszzz")

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
        _salesRef.Text = "TRX-" & Guid.NewGuid().ToString("N")
        tabTransaction.Controls.Add(New Label With {.Text = "Nomor struk tetap:", .Location = New Point(325, 25), .AutoSize = True})
        tabTransaction.Controls.Add(_salesRef)
        Dim newSales As New Button With {.Text = "Transaksi baru", .Location = New Point(715, 20), .Size = New Size(180, 30)}
        AddHandler newSales.Click, Sub()
                                      _salesRef.Text = "TRX-" & Guid.NewGuid().ToString("N")
                                      _salesOrderTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:sszzz")
                                  End Sub
        tabTransaction.Controls.Add(newSales)
        tabTransaction.Controls.Add(_allowWithoutMember)
        AddHandler _allowWithoutMember.CheckedChanged, Sub()
                                                         txtBurnPoint.Enabled = Not _allowWithoutMember.Checked
                                                         txtBurnAuthCode.Enabled = Not _allowWithoutMember.Checked
                                                         btnBurnPoint.Enabled = Not _allowWithoutMember.Checked
                                                         btnCancelBurnPoint.Enabled = Not _allowWithoutMember.Checked
                                                     End Sub
        btnPushSalesOrder.Text = "Kirim Transaksi Selesai"
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

        Dim storeCode As String = System.Configuration.ConfigurationManager.AppSettings("Goapp.StoreCode")
        If String.IsNullOrEmpty(storeCode) Then storeCode = ""

        Dim dbConnStr As String = String.Empty
        Dim connStrSetting = System.Configuration.ConfigurationManager.ConnectionStrings("POSDatabase")
        If connStrSetting IsNot Nothing Then
            dbConnStr = connStrSetting.ConnectionString
        End If

        _config = New GoappConfig With {
            .DbConnectionString = dbConnStr,
            .ApiKey = apiKey,
            .ApiSecret = apiSecret,
            .ChannelUid = channelUid,
            .AuthBaseUrl = authUrl,
            .ChannelBaseUrl = channelUrl,
            .StoreUid = finalStoreUid,
            .StoreName = storeName,
            .StoreCode = storeCode,
            .TimeoutSeconds = 30,
            .MaxRetryAttempts = 3,
            .RetryDelayMilliseconds = 1500
        }

        _client = New GoappApiClient(_config)
        _client.DbConnectionString = dbConnStr
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

            ' Uji koneksi SQL Server Logging yang dioper dari aplikasi POS
            Dim dbTestResult = Await _client.Logger.TestConnectionAsync(_client.DbConnectionString)
            If dbTestResult.Item1 Then
                AppendLog($"[DATABASE] {dbTestResult.Item2}")
            Else
                AppendLog($"[DATABASE WARNING] {dbTestResult.Item2}")
            End If

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
                ' Promo logic removed

                ' Tampilkan raw JSON response di box sebelah kanan
                                ' Load Member Deal Codes
                lstMemberPromos.Items.Clear()
                Dim dealResp = Await _client.GetMemberDealCodesAsync(txtInputMember.Text)
                If dealResp.IsSuccess AndAlso dealResp.Data IsNot Nothing AndAlso dealResp.Data.Count > 0 Then
                    lstMemberPromos.Items.Add($"Ada {dealResp.Data.Count} Benefit Aktif:")
                    For Each v In dealResp.Data
                        Dim dealName = If(v.Deal IsNot Nothing AndAlso Not String.IsNullOrEmpty(v.Deal.Name), v.Deal.Name, "-")
                        lstMemberPromos.Items.Add($"- [{v.Code}] {dealName}")
                    Next
                Else
                    lstMemberPromos.Items.Add("Belum ada benefit / promo aktif.")
                End If

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


    ''' <summary>
    ''' Contoh pembukaan FormSearchMember langsung dari DLL dengan RaiseEvent sendMemberData
    ''' </summary>
    Private Sub btnSearchMemberDialog_Click(sender As Object, e As EventArgs) Handles btnSearchMemberDialog.Click
        InitClient()
        Dim g_frmLoyal As New FormSearchMember(_client)

        ' Pasang listener event dari DLL ke Sub penampung di POS
        AddHandler g_frmLoyal.sendMemberData, AddressOf GetLoyalID_MemberData

        g_frmLoyal.ShowDialog(Me)
    End Sub

    ''' <summary>
    ''' Sub penampung event di POS yang otomatis dipanggil saat kasir memilih member di Form DLL
    ''' </summary>
    Private Sub GetLoyalID_MemberData(sender As Object, e As MemberDataEventArgs)
        AppendLog("[EVENT sendMemberData] Diterima dari FormSearchMember:")
        AppendLog($" - Nama: {e.FullName}")
        AppendLog($" - No HP: {e.MobileNo}")
        AppendLog($" - Status Aktif: {e.IsActive} ({e.StatusDescription})")
        AppendLog($" - Poin: {e.AvailablePoints:N0} Pts (Rp {e.PointsInRupiah:N0})")
        AppendLog($" - Level / Scheme: {e.LevelName} / {e.SchemeName}")

        ' Terapkan data member ke form kasir
        txtInputMember.Text = e.MobileNo
        lblMemberName.Text = $"Nama Member : {e.FullName}"
        lblMemberPhone.Text = $"No Handphone : {e.MobileNo}"
        lblMemberLevel.Text = $"Level / Scheme : {e.LevelName} / {e.SchemeName}"
        lblMemberPoints.Text = $"Sisa Poin : {e.AvailablePoints:N0} Pts"
        lblMemberRupiah.Text = $"Nilai Rupiah Poin : Rp {e.PointsInRupiah:N0}"
        lblMemberReferral.Text = $"Referral Code : {e.ReferralCode}"

        lstMemberPromos.Items.Clear()
        If e.DealCodes IsNot Nothing AndAlso e.DealCodes.Count > 0 Then
            lstMemberPromos.Items.Add($"Ada {e.DealCodes.Count} Benefit Aktif:")
            For Each v In e.DealCodes
                Dim dealName = If(v.Deal IsNot Nothing AndAlso Not String.IsNullOrEmpty(v.Deal.Name), v.Deal.Name, "-")
                lstMemberPromos.Items.Add($"- [{v.Code}] {dealName}")
            Next
        Else
            lstMemberPromos.Items.Add("Belum ada benefit / promo aktif.")
        End If

        If e.RawMember IsNot Nothing Then
            _currentMember = e.RawMember
        End If

        If Not e.IsActive Then
            MessageBox.Show($"Perhatian: Member {e.FullName} ({e.MobileNo}) sedang {e.StatusDescription}!", "Status Member", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            MessageBox.Show($"Member {e.FullName} ({e.MobileNo}) berhasil dipilih via Form DLL!", "Member Terpilih", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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

                        If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                Dim v = resp.Data
                Dim desc = If(String.IsNullOrEmpty(v.Description), "-", v.Description)
                Dim rd = v.RewardData

                ' 1. Susun detail pada Label
                Dim sbLabel As New System.Text.StringBuilder()
                sbLabel.AppendLine($"Status: VALID (Kode: {v.DealCode})")
                sbLabel.AppendLine($"[DEAL] {v.Name} | {desc}")
                If rd IsNot Nothing Then
                    sbLabel.AppendLine($"[REWARD_DATA]:")
                    For Each line In rd.GetDetailLines()
                        sbLabel.AppendLine("  " & line)
                    Next
                Else
                    sbLabel.AppendLine($"Tipe: {v.DiscountType} ({v.DiscountAmount:N0})")
                End If

                lblVoucherStatus.Text = sbLabel.ToString().TrimEnd()
                lblVoucherStatus.ForeColor = Color.DarkGreen

                ' 2. Susun detail lengkap setiap variabel 'reward_data' untuk MessageBox
                Dim sbMsg As New System.Text.StringBuilder()
                sbMsg.AppendLine("==========================================")
                sbMsg.AppendLine("           DETAIL VALIDASI VOUCHER")
                sbMsg.AppendLine("==========================================")
                sbMsg.AppendLine($"Kode Voucher  : {v.DealCode}")
                sbMsg.AppendLine($"ID Voucher    : {v.Id}")
                sbMsg.AppendLine($"Nama Deal     : {v.Name}")
                sbMsg.AppendLine($"Deskripsi     : {desc}")
                sbMsg.AppendLine($"Status Code   : {If(String.IsNullOrEmpty(v.StatusCode), "active", v.StatusCode)}")
                sbMsg.AppendLine()
                sbMsg.AppendLine("--- DETAIL OBJEK 'reward_data' (TIAP VARIABEL) ---")
                If rd IsNot Nothing Then
                    For Each line In rd.GetDetailLines()
                        sbMsg.AppendLine(line)
                    Next
                Else
                    sbMsg.AppendLine("(Objek reward_data tidak ditemukan pada respons)")
                End If
                sbMsg.AppendLine("==========================================")

                MessageBox.Show(sbMsg.ToString(), "Detail Voucher & Objek reward_data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtVoucherRawJson.Text = FormatJson(resp.RawJson)
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
            Dim authCode = txtVoucherAuthCode.Text.Trim()
            ' Panggil fungsi Auto-Retry dari DLL (authCode bisa terisi, random, atau kosong)
            Dim resp = Await _client.UseVoucherWithRetryAsync(code, txRef, memberUid, customMaxRetry:=3, authorizationCode:=authCode)

            If resp.IsSuccess Then
                lblVoucherStatus.Text = $"Status: Voucher BERHASIL DIGUNAKAN pada transaksi {txRef}!"
                lblVoucherStatus.ForeColor = Color.DarkGreen
                MessageBox.Show("Voucher sukses diterapkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblVoucherStatus.Text = $"Status: Gagal menggunakan voucher! ({resp.Message})"
                lblVoucherStatus.ForeColor = Color.Red
                MessageBox.Show($"Voucher gagal diterapkan:{Environment.NewLine}{resp.Message}", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtVoucherRawJson.Text = FormatJson(resp.RawJson)
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
            txtVoucherRawJson.Text = FormatJson(resp.RawJson)
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
            Dim orderAmountIdr = burnPt ' 1 poin = 1 Rupiah
            Dim authCode = txtBurnAuthCode.Text.Trim()

            ' Panggil DLL Burn Point (authCode bisa terisi, random, atau kosong; totalOrderAmount adalah billTotal transaksi)
            Dim resp = Await _client.CreatePointPaymentAsync(_currentMember.Uid, burnPt, orderAmountIdr, txRef, authorizationCode:=authCode, totalOrderAmount:=billTotal)
            If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                _lastPaymentRef = resp.Data.PaymentRef
                lblLastPaymentRef.Text = $"Last Payment Ref: {_lastPaymentRef} (Status: {resp.Data.Status})"
                MessageBox.Show($"Burn Point Berhasil!{Environment.NewLine}Ref: {_lastPaymentRef}{Environment.NewLine}Status: {resp.Data.Status}{Environment.NewLine}Order Amount (Poin): Rp {orderAmountIdr:N0}{Environment.NewLine}Total Order Amount: Rp {billTotal:N0}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    ' Data uji memakai SKU deterministik agar referensi sama selalu menghasilkan payload sama.
    Private Shared Function CreateTestOrderLines(total As Decimal, transactionRef As String) As List(Of OrderLineItem)
        Dim lines As New List(Of OrderLineItem)()
        Dim count = If(total >= 3D, 3, 1)
        Dim unitPrice = Decimal.Truncate(total / count)
        For index = 1 To count
            Dim price = If(index = count, total - unitPrice * (count - 1), unitPrice)
            Dim sku As String
            Using sha = System.Security.Cryptography.SHA256.Create()
                sku = "SKU-" & BitConverter.ToString(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(transactionRef & "|" & index))).Replace("-", "").Substring(0, 12)
            End Using
            lines.Add(New OrderLineItem With {
                .Product = New OrderProductInfo With {.Sku = sku, .Name = "Produk " & index},
                .Quantity = 1, .Price = price, .PriceBeforeDiscount = price
            })
        Next
        Return lines
    End Function

    Private Async Sub btnPushSalesOrder_Click(sender As Object, e As EventArgs) Handles btnPushSalesOrder.Click
        Dim withoutMember = _allowWithoutMember.Checked
        Dim member = _currentMember
        If Not withoutMember AndAlso member Is Nothing Then
            MessageBox.Show("Silakan cari dan pilih member terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim billTotal As Decimal
        Decimal.TryParse(txtBillTotal.Text, billTotal)
        If billTotal <= 0 Then
            MessageBox.Show("Nilai transaksi harus lebih dari 0.")
            Return
        End If

        Dim burnPoint As Decimal = 0
        If Not withoutMember Then Decimal.TryParse(txtBurnPoint.Text, burnPoint)

        Dim authCode = txtBurnAuthCode.Text.Trim()
        Dim providerRef = _salesRef.Text.Trim()
        Dim memberBefore As ApiResponse(Of MemberResponse) = Nothing

        btnPushSalesOrder.Enabled = False
        _allowWithoutMember.Enabled = False
        txtOrderRawJson.Clear()

        Try
            If Not withoutMember Then memberBefore = Await _client.GetMemberAsync(member.Uid.ToString())
            ' 1. SELALU KIRIM TRANSAKSI / SALES ORDER KE POST /sales/order/
            ' Seluruh payment dibuat 1 saja ('Total Paid' / total_amount)
            ' Hitung diskon poin & voucher
            Dim pointDiscount = burnPoint
            Dim linesDiscountTotal = pointDiscount ' Jika ada voucher diskon total order, ditambahkan ke sini

            ' Hitung total bayar kasir (bersih setelah dipotong poin)
            Dim cashPaid = billTotal - pointDiscount
            If cashPaid < 0 Then cashPaid = 0

            Dim paymentItems As New List(Of OrderPaymentItem)()
            If cashPaid > 0 OrElse pointDiscount = 0 Then
                paymentItems.Add(New OrderPaymentItem With {
                    .PaymentMethodName = "CASH",
                    .PaymentType = "CASH",
                    .Amount = cashPaid
                })
            End If
            If pointDiscount > 0 Then
                paymentItems.Add(New OrderPaymentItem With {
                    .PaymentMethodName = "Point",
                    .PaymentType = "POINT",
                    .Amount = pointDiscount
                })
            End If

            Dim orderReq As New PushOrderRequest With {
                .Store = New OrderStoreRef With {
                    .Uid = If(_config.StoreUid.HasValue, _config.StoreUid.Value, CType(Nothing, Long?)),
                    .Name = _config.StoreName,
                    .StoreCode = _config.StoreCode
                },
                .OrderNo = providerRef,
                .ProviderRef = providerRef,
                .OrderDate = _salesOrderTime,
                .LinesTotal = billTotal,
                .LinesTax = 0,
                .TotalInclTax = billTotal,
                .TotalPaid = cashPaid,
                .PaidAt = _salesOrderTime,
                .CompletedAt = _salesOrderTime,
                .ServiceFee = 0,
                .LinesDiscount = If(linesDiscountTotal > 0, linesDiscountTotal, CType(Nothing, Decimal?)),
                .Member = If(withoutMember, Nothing, New OrderMemberRef With {
                    .Uid = If(member Is Nothing, 0, member.Uid),
                    .MobileNo = If(member Is Nothing, Nothing, member.MobileNo)
                }),
                .Lines = CreateTestOrderLines(billTotal, providerRef),
                .Payments = paymentItems
            }

            Dim orderReqJson = Newtonsoft.Json.JsonConvert.SerializeObject(orderReq, Newtonsoft.Json.Formatting.Indented, New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore})
            txtOrderRawJson.Text = "=== [1/2] REQUEST JSON (POST /sales/order/) ===" & Environment.NewLine & orderReqJson & Environment.NewLine & Environment.NewLine

            Dim salesOrderResp As ApiResponse(Of PushOrderResponse)
            If withoutMember Then
                salesOrderResp = Await _client.PushGuestSalesOrderAsync(orderReq)
            Else
                salesOrderResp = Await _client.PushSalesOrderAsync(orderReq)
            End If
            txtOrderRawJson.Text &= "=== [1/2] RESPONSE JSON (POST /sales/order/) ===" & Environment.NewLine & FormatJson(salesOrderResp.RawJson) & Environment.NewLine & Environment.NewLine

            Dim msgBuilder As New System.Text.StringBuilder()
            msgBuilder.AppendLine($"HTTP: {salesOrderResp.StatusCode}; GoappApiLog: {salesOrderResp.ApiLogSaved}; GoappRedeemLog: {salesOrderResp.TransactionLogSaved}")
            If salesOrderResp.IsDuplicate Then msgBuilder.AppendLine("Transaksi sudah terkirim; POST tidak diulang.")
            If Not String.IsNullOrWhiteSpace(salesOrderResp.WarningMessage) Then msgBuilder.AppendLine(salesOrderResp.WarningMessage)

            If salesOrderResp.IsSuccess AndAlso salesOrderResp.Data IsNot Nothing Then
                Dim earnedPts = salesOrderResp.Data.TotalEarnedPoints
                msgBuilder.AppendLine($"Transaksi Penjualan Sukses Dikirim ke /sales/order/!")
                msgBuilder.AppendLine($"Order UID: {salesOrderResp.Data.Uid}")
                msgBuilder.AppendLine($"Total Paid (Bersih): Rp {cashPaid:N0}")
                If linesDiscountTotal > 0 Then
                    msgBuilder.AppendLine($"Diskon Poin/Lines: Rp {linesDiscountTotal:N0}")
                End If
                msgBuilder.AppendLine($"Poin Diperoleh (Earning): {earnedPts:N0} Pts")
            Else
                msgBuilder.AppendLine($"Transaksi /sales/order/ Gagal: {salesOrderResp.Message}")
            End If

            ' 2. JIKA ADA BURN POINT (burnPoint > 0) -> HIT JUGA API POST /member/payment/
            Dim burnSucceeded As Boolean = (burnPoint <= 0)
            If burnPoint > 0 AndAlso salesOrderResp.IsSuccess AndAlso Not salesOrderResp.IsDuplicate Then
                Dim paymentReqObj As New PaymentTransactionRequest With {
                    .Member = New PaymentMemberRef With {.Uid = member.Uid},
                    .Amount = burnPoint,
                    .OrderAmount = burnPoint,
                    .TotalOrderAmount = billTotal,
                    .OrderCurrency = "idr",
                    .ProviderRef = providerRef,
                    .AuthorizationCode = If(String.IsNullOrWhiteSpace(authCode), Nothing, authCode)
                }

                Dim payReqJson = Newtonsoft.Json.JsonConvert.SerializeObject(paymentReqObj, Newtonsoft.Json.Formatting.Indented, New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore})
                txtOrderRawJson.Text &= "=== [2/2] REQUEST JSON (POST /member/payment/) ===" & Environment.NewLine & payReqJson & Environment.NewLine & Environment.NewLine

                Dim payResp = Await _client.CreatePointPaymentAsync(member.Uid, burnPoint, burnPoint, providerRef, authorizationCode:=authCode, totalOrderAmount:=billTotal)
                txtOrderRawJson.Text &= "=== [2/2] RESPONSE JSON (POST /member/payment/) ===" & Environment.NewLine & FormatJson(payResp.RawJson) & Environment.NewLine

                msgBuilder.AppendLine()
                If payResp.IsSuccess AndAlso payResp.Data IsNot Nothing Then
                    burnSucceeded = True
                    _lastPaymentRef = payResp.Data.PaymentRef
                    lblLastPaymentRef.Text = $"Last Payment Ref: {_lastPaymentRef} (Status: {payResp.Data.Status})"
                    msgBuilder.AppendLine($"Burn Point ({burnPoint:N0} Pts) Berhasil Dikirim ke /member/payment/!")
                    msgBuilder.AppendLine($"Payment Ref: {_lastPaymentRef} (Status: {payResp.Data.Status})")
                Else
                    msgBuilder.AppendLine($"Burn Point Gagal Dikirim ke /member/payment/: {payResp.Message}")
                End If
            End If

            ' Tampilkan pesan hasil & refresh data member
            Dim isAllOk = salesOrderResp.IsSuccess AndAlso burnSucceeded
            MessageBox.Show(msgBuilder.ToString(), If(isAllOk, "Sukses Transaksi", "Hasil Transaksi"), MessageBoxButtons.OK, If(isAllOk, MessageBoxIcon.Information, MessageBoxIcon.Warning))
            If withoutMember Then Return
            Dim memberAfter = Await _client.GetMemberAsync(member.Uid.ToString())
            If memberBefore.IsSuccess AndAlso memberBefore.Data IsNot Nothing AndAlso memberAfter.IsSuccess AndAlso memberAfter.Data IsNot Nothing Then
                Dim delta = memberAfter.Data.AvailablePoints - memberBefore.Data.AvailablePoints
                Dim expectedEarn = If(salesOrderResp.Data Is Nothing, 0D, salesOrderResp.Data.TotalEarnedPoints)
                Dim detail = $"[AUDIT POIN] Ref={providerRef}; Sebelum={memberBefore.Data.AvailablePoints}; Sesudah={memberAfter.Data.AvailablePoints}; Selisih aktual={delta}; Reward respons={expectedEarn}; Burn diminta={burnPoint}; Duplicate={salesOrderResp.IsDuplicate}"
                txtOrderRawJson.AppendText(Environment.NewLine & detail)
                AppendLog(detail)
                _currentMember = memberAfter.Data
                lblMemberPoints.Text = $"Sisa Poin : {_currentMember.AvailablePoints:N0} Pts"
                If burnPoint = 0 AndAlso salesOrderResp.IsSuccess AndAlso Not salesOrderResp.IsDuplicate AndAlso delta <> expectedEarn Then
                    MessageBox.Show(detail & Environment.NewLine & "Saldo dari server berbeda dari reward respons. Periksa ledger/rule Goapp; jangan kirim ulang transaksi.", "Selisih Poin", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"Terjadi exception saat proses transaksi:{Environment.NewLine}{ex.Message}", "Error Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnPushSalesOrder.Enabled = True
            _allowWithoutMember.Enabled = True
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

    Private Sub lblMemberReferral_Click(sender As Object, e As EventArgs) Handles lblMemberReferral.Click

    End Sub

    Private Sub lblMemberRupiah_Click(sender As Object, e As EventArgs) Handles lblMemberRupiah.Click

    End Sub

    Private Sub lblMemberPoints_Click(sender As Object, e As EventArgs) Handles lblMemberPoints.Click

    End Sub

    Private Sub grpAuth_Enter(sender As Object, e As EventArgs) Handles grpAuth.Enter

    End Sub
End Class




