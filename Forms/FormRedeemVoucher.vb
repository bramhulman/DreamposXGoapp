Imports System
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services

Namespace Forms
    ''' <summary>
    ''' Form dialog modal visual untuk memvalidasi dan menggunakan voucher Goapp CRM.
    ''' Mendukung input manual / scan barcode QR serta pemilihan langsung dari daftar voucher member.
    ''' </summary>
    Public Class FormRedeemVoucher
        Private ReadOnly _client As GoappApiClient
        Private ReadOnly _presetDeal As MemberDealCodeResponse
        Private ReadOnly _initialCode As String

        ''' <summary>
        ''' Data hasil redeem voucher yang berhasil digunakan (untuk dipassing ke POS)
        ''' </summary>
        Public Property RedeemedResult As RedeemedVoucherInfo

        ''' <summary>
        ''' Nomor referensi transaksi dari POS (misal: nomor order / SM_Number)
        ''' </summary>
        Public Property TransactionRef As String = String.Empty

        ''' <summary>
        ''' UID member Goapp yang sedang aktif (jika ada)
        ''' </summary>
        Public Property MemberUid As Long? = Nothing

        ''' <summary>
        ''' Nama member Goapp yang sedang aktif (jika ada)
        ''' </summary>
        Public Property MemberName As String = String.Empty

        ''' <summary>
        ''' Subtotal transaksi POS saat ini sebelum pajak & service
        ''' </summary>
        Public Property CurrentSubtotal As Decimal = 0D

        ''' <summary>
        ''' Inisialisasi FormRedeemVoucher
        ''' </summary>
        ''' <param name="client">Instance GoappApiClient</param>
        ''' <param name="dealCode">Kode voucher (bisa kosong jika dari scan QR / input manual)</param>
        ''' <param name="presetDeal">Objek deal dari list voucher member (opsional)</param>
        ''' <param name="memberUid">UID member aktif (opsional)</param>
        ''' <param name="txRef">Nomor transaksi POS (opsional)</param>
        ''' <param name="subtotal">Subtotal transaksi saat ini sebelum pajak & service (opsional)</param>
        ''' <param name="memberName">Nama member Goapp (opsional)</param>
        Public Sub New(client As GoappApiClient,
                       Optional dealCode As String = "",
                       Optional presetDeal As MemberDealCodeResponse = Nothing,
                       Optional memberUid As Long? = Nothing,
                       Optional txRef As String = "",
                       Optional subtotal As Decimal = 0D,
                       Optional memberName As String = "")
            If client Is Nothing Then
                Throw New ArgumentNullException(NameOf(client), "GoappApiClient tidak boleh Nothing.")
            End If

            Me.InitializeComponent()
            _client = client
            _initialCode = If(dealCode, String.Empty).Trim()
            _presetDeal = presetDeal
            Me.MemberUid = memberUid
            Me.MemberName = If(memberName, String.Empty).Trim()
            Me.TransactionRef = If(txRef, String.Empty).Trim()
            Me.CurrentSubtotal = subtotal
        End Sub

        Private Sub FormRedeemVoucher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            txtVoucherCode.Text = _initialCode

            If Me.CurrentSubtotal > 0 Then
                lblHeaderSubtitle.Text = $"Validasi & Gunakan Voucher | Subtotal POS: Rp {Me.CurrentSubtotal:N0}"
            End If

            ' Tampilkan informasi awal jika dipanggil dari item list voucher member
            If _presetDeal IsNot Nothing AndAlso _presetDeal.Deal IsNot Nothing Then
                Dim d = _presetDeal.Deal
                lblDealName.Text = If(Not String.IsNullOrEmpty(d.Name), d.Name, "Promo Member")

                Dim rd = d.RewardData
                If rd IsNot Nothing Then
                    If String.Equals(rd.DiscountType, "free_items", StringComparison.OrdinalIgnoreCase) Then
                        Dim rawSku = If(rd.Item IsNot Nothing AndAlso rd.Item.Skus IsNot Nothing AndAlso rd.Item.Skus.Count > 0, rd.Item.Skus(0), rd.EffectiveItemSkus)
                        Dim cleanSku = If(rawSku.Contains("-"), rawSku.Split("-"c)(0).Trim(), rawSku.Trim())
                        Dim qty = If(rd.EffectiveItemQuantity > 0, rd.EffectiveItemQuantity, 1)
                        lblRewardInfo.Text = $"🎁 Free Item: {cleanSku} (Qty: {qty}) - Rp 0"
                    Else
                        Dim minPurchase = rd.EffectiveMinPurchase
                        Dim minPurchaseText = If(minPurchase > 0, $" | Min. Rp {minPurchase:N0}", "")
                        If rd.EffectiveDiscountType.ToLower().Contains("percent") OrElse String.Equals(rd.DiscountType, "percentage", StringComparison.OrdinalIgnoreCase) Then
                            Dim pct = If(rd.EffectiveDiscountAmount <= 1D AndAlso rd.EffectiveDiscountAmount > 0D, rd.EffectiveDiscountAmount * 100D, rd.EffectiveDiscountAmount)
                            Dim estDiscAmt = Math.Round((pct / 100D) * Me.CurrentSubtotal, MidpointRounding.AwayFromZero)
                            If rd.EffectiveDiscountMaxAmount.HasValue AndAlso estDiscAmt > rd.EffectiveDiscountMaxAmount.Value Then
                                estDiscAmt = rd.EffectiveDiscountMaxAmount.Value
                            End If
                            lblRewardInfo.Text = $"🏷 Diskon {pct:N0}%" & If(Me.CurrentSubtotal > 0, $" (Est: Rp {estDiscAmt:N0})", "") & minPurchaseText
                        Else
                            lblRewardInfo.Text = $"🏷 Diskon Rp {rd.EffectiveDiscountAmount:N0}{minPurchaseText}"
                        End If

                        If minPurchase > 0 AndAlso Me.CurrentSubtotal > 0 AndAlso Me.CurrentSubtotal < minPurchase Then
                            lblRewardInfo.ForeColor = Color.OrangeRed
                        End If
                    End If
                Else
                    lblRewardInfo.Text = If(Not String.IsNullOrEmpty(d.Description), d.Description, "-")
                End If
            Else
                lblDealName.Text = "(Menunggu validasi kode voucher)"
                lblRewardInfo.Text = "-"
            End If

            ' Fokus ke textbox jika kode belum terisi
            If String.IsNullOrEmpty(txtVoucherCode.Text) Then
                txtVoucherCode.Focus()
            Else
                btnUseVoucher.Focus()
            End If
        End Sub

        Private Sub txtVoucherCode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtVoucherCode.KeyDown
            ' Mendukung aksi otomatis dari Barcode / QR Scanner (yang biasanya diakhiri Enter)
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                btnUseVoucher.PerformClick()
            End If
        End Sub

        Private Sub txtAuthCode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAuthCode.KeyDown
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                btnUseVoucher.PerformClick()
            End If
        End Sub

        Private Async Sub btnUseVoucher_Click(sender As Object, e As EventArgs) Handles btnUseVoucher.Click
            Dim code = txtVoucherCode.Text.Trim()
            If String.IsNullOrWhiteSpace(code) Then
                FormMessageBox.Show(Me, "Silakan ketik atau scan kode voucher terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtVoucherCode.Focus()
                Return
            End If

            SetProcessingState(True)

            Try
                ' 1. Validasi voucher ke Goapp CRM
                lblStatus.Text = $"Memvalidasi voucher '{code}' ke Goapp CRM..."
                lblStatus.ForeColor = Color.FromArgb(13, 110, 253)

                Dim valResp = Await _client.ValidateVoucherAsync(code, Me.MemberUid)
                If Not valResp.IsSuccess Then
                    lblStatus.Text = $"Voucher tidak valid: {valResp.Message}"
                    lblStatus.ForeColor = Color.Red
                    FormMessageBox.Show(Me, $"Voucher tidak valid:{Environment.NewLine}{valResp.Message}", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtVoucherCode.Focus()
                    Return
                End If

                ' Update tampilan dengan data hasil validasi
                Dim v = valResp.Data
                Dim rdPreview = If(v IsNot Nothing AndAlso v.RewardData IsNot Nothing,
                                   v.RewardData,
                                   If(_presetDeal IsNot Nothing AndAlso _presetDeal.Deal IsNot Nothing, _presetDeal.Deal.RewardData, Nothing))

                If v IsNot Nothing Then
                    lblDealName.Text = If(Not String.IsNullOrEmpty(v.Name), v.Name, "Promo Member")
                    If rdPreview IsNot Nothing Then
                        If String.Equals(rdPreview.DiscountType, "free_items", StringComparison.OrdinalIgnoreCase) Then
                            Dim rawSku = If(rdPreview.Item IsNot Nothing AndAlso rdPreview.Item.Skus IsNot Nothing AndAlso rdPreview.Item.Skus.Count > 0, rdPreview.Item.Skus(0), rdPreview.EffectiveItemSkus)
                            Dim cleanSku = If(rawSku.Contains("-"), rawSku.Split("-"c)(0).Trim(), rawSku.Trim())
                            Dim previewQty = If(rdPreview.EffectiveItemQuantity > 0, rdPreview.EffectiveItemQuantity, 1)
                            lblRewardInfo.Text = $"🎁 Free Item: {cleanSku} (Qty: {previewQty}) - Rp 0"
                        Else
                            Dim minP = rdPreview.EffectiveMinPurchase
                            Dim minPText = If(minP > 0, $" | Min. Rp {minP:N0}", "")
                            If rdPreview.EffectiveDiscountType.ToLower().Contains("percent") OrElse String.Equals(rdPreview.DiscountType, "percentage", StringComparison.OrdinalIgnoreCase) Then
                                Dim pct = If(rdPreview.EffectiveDiscountAmount <= 1D AndAlso rdPreview.EffectiveDiscountAmount > 0D, rdPreview.EffectiveDiscountAmount * 100D, rdPreview.EffectiveDiscountAmount)
                                Dim estDiscAmt = Math.Round((pct / 100D) * Me.CurrentSubtotal, MidpointRounding.AwayFromZero)
                                If rdPreview.EffectiveDiscountMaxAmount.HasValue AndAlso estDiscAmt > rdPreview.EffectiveDiscountMaxAmount.Value Then
                                    estDiscAmt = rdPreview.EffectiveDiscountMaxAmount.Value
                                End If
                                lblRewardInfo.Text = $"🏷 Diskon {pct:N0}%" & If(Me.CurrentSubtotal > 0, $" (Est: Rp {estDiscAmt:N0})", "") & minPText
                            Else
                                lblRewardInfo.Text = $"🏷 Diskon Rp {rdPreview.EffectiveDiscountAmount:N0}{minPText}"
                            End If
                        End If
                    End If
                End If

                ' 2. Validasi Minimum Belanja (min_purchase) sebelum memanggil UseVoucher
                Dim minPurchase As Decimal = If(rdPreview IsNot Nothing, rdPreview.EffectiveMinPurchase, 0D)
                If minPurchase > 0D AndAlso Me.CurrentSubtotal < minPurchase Then
                    Dim selisih As Decimal = minPurchase - Me.CurrentSubtotal
                    lblStatus.Text = $"Subtotal transaksi belum memenuhi syarat minimum pembelian voucher!"
                    lblStatus.ForeColor = Color.Red
                    FormMessageBox.Show(Me, $"Voucher ini membutuhkan minimum pembelian sebesar Rp {minPurchase:N0}." & Environment.NewLine &
                                        $"Subtotal transaksi saat ini: Rp {Me.CurrentSubtotal:N0} (kurang Rp {selisih:N0})." & Environment.NewLine &
                                        "Voucher belum dapat digunakan untuk transaksi ini.",
                                        "Minimum Pembelian Belum Terpenuhi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' 3. Gunakan voucher ke Goapp CRM dengan Authentication Code jika ada
                lblStatus.Text = $"Menerapkan voucher '{code}' ke transaksi..."
                Dim useTxRef = If(Not String.IsNullOrEmpty(Me.TransactionRef), Me.TransactionRef, "TRX-" & DateTime.Now.ToString("yyyyMMddHHmmss"))
                Dim authCode = txtAuthCode.Text.Trim()
                Dim useResp = Await _client.UseVoucherWithRetryAsync(code, useTxRef, Me.MemberUid, customMaxRetry:=3, authorizationCode:=authCode)

                If Not useResp.IsSuccess Then
                    lblStatus.Text = $"Gagal menggunakan voucher: {useResp.Message}"
                    lblStatus.ForeColor = Color.Red
                    FormMessageBox.Show(Me, $"Gagal menggunakan voucher:{Environment.NewLine}{useResp.Message}", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' 4. Ekstraksi objek reward_data akhir
                Dim vFinal = If(useResp.Data, valResp.Data)
                Dim rd = If(vFinal IsNot Nothing AndAlso vFinal.RewardData IsNot Nothing,
                            vFinal.RewardData,
                            rdPreview)

                Dim skuRaw As String = ""
                Dim skuClean As String = ""
                Dim qty As Integer = 1
                Dim discType As String = ""
                Dim discRate As Decimal = 0D
                Dim discAmount As Decimal = 0D

                If rd IsNot Nothing Then
                    discType = If(rd.DiscountType, "")
                    Dim isFreeItem As Boolean = String.Equals(discType, "free_items", StringComparison.OrdinalIgnoreCase)
                    Dim isPercentage As Boolean = String.Equals(discType, "percentage", StringComparison.OrdinalIgnoreCase) OrElse (rd.EffectiveDiscountType IsNot Nothing AndAlso rd.EffectiveDiscountType.ToLower().Contains("percent"))
                    Dim isAmount As Boolean = String.Equals(discType, "amount", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(discType, "nominal", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(discType, "fixed", StringComparison.OrdinalIgnoreCase)

                    If isFreeItem Then
                        If rd.Item IsNot Nothing AndAlso rd.Item.Skus IsNot Nothing AndAlso rd.Item.Skus.Count > 0 Then
                            skuRaw = rd.Item.Skus(0).Trim()
                        ElseIf Not String.IsNullOrEmpty(rd.EffectiveItemSkus) AndAlso rd.EffectiveItemSkus <> "-" Then
                            skuRaw = rd.EffectiveItemSkus.Split(","c)(0).Trim()
                        End If
                        qty = If(rd.EffectiveItemQuantity > 0, rd.EffectiveItemQuantity, 1)

                        ' Aturan pemotongan SKU: jika "SKFTRR03-M" maka ambil sebelum '-' -> "SKFTRR03"
                        If Not String.IsNullOrEmpty(skuRaw) Then
                            If skuRaw.Contains("-") Then
                                skuClean = skuRaw.Split("-"c)(0).Trim()
                            Else
                                skuClean = skuRaw.Trim()
                            End If
                        End If
                    ElseIf isPercentage Then
                        Dim rawDisc = rd.EffectiveDiscountAmount
                        If rawDisc <= 1D AndAlso rawDisc > 0D Then
                            discRate = rawDisc
                        ElseIf rawDisc > 1D Then
                            discRate = rawDisc / 100D
                        End If

                        discAmount = discRate * Me.CurrentSubtotal
                        If rd.EffectiveDiscountMaxAmount.HasValue AndAlso discAmount > rd.EffectiveDiscountMaxAmount.Value Then
                            discAmount = rd.EffectiveDiscountMaxAmount.Value
                        End If
                        discAmount = Math.Round(discAmount, MidpointRounding.AwayFromZero)
                    ElseIf isAmount Then
                        discAmount = Math.Min(rd.EffectiveDiscountAmount, Me.CurrentSubtotal)
                        discAmount = Math.Round(discAmount, MidpointRounding.AwayFromZero)
                    End If
                End If

                ' Susun hasil redeem
                Me.RedeemedResult = New RedeemedVoucherInfo With {
                    .VoucherCode = code,
                    .VoucherName = If(vFinal IsNot Nothing AndAlso Not String.IsNullOrEmpty(vFinal.Name), vFinal.Name, If(_presetDeal IsNot Nothing AndAlso _presetDeal.Deal IsNot Nothing, _presetDeal.Deal.Name, "Voucher Promo")),
                    .DiscountType = discType,
                    .ItemSku = skuClean,
                    .RawSku = skuRaw,
                    .Quantity = qty,
                    .Price = 0D,
                    .DiscountRate = discRate,
                    .DiscountAmount = discAmount,
                    .MinPurchase = minPurchase,
                    .RawRewardData = rd
                }

                ' Catat ke tabel database [dbo].[GoappRedeemLog] dengan Status = 0 (Digunakan) dan StatusPos = 0 (Belum Masuk POS)
                Try
                    Dim logItem As New GoappRedeemLogItem With {
                        .Waktu = DateTime.Now,
                        .Jenis = "RedeemVoucher",
                        .Kode = code,
                        .NamaVoucher = Me.RedeemedResult.VoucherName,
                        .NilaiPoint = Me.RedeemedResult.DiscountAmount,
                        .Status = 0,
                        .StatusPos = 0,
                        .TransactionRef = useTxRef,
                        .MemberUid = If(Me.MemberUid.HasValue, Me.MemberUid.Value.ToString(), ""),
                        .MemberName = If(Not String.IsNullOrEmpty(Me.MemberName), Me.MemberName, If(Me.MemberUid.HasValue, "UID " & Me.MemberUid.Value.ToString(), "Pelanggan"))
                    }
                    Dim logTask = _client.RedeemRepo.InsertRedeemLogAsync(logItem)
                Catch exLog As Exception
                    Console.WriteLine("Gagal menyimpan log redeem voucher: " & exLog.Message)
                End Try

                lblStatus.Text = "Voucher BERHASIL digunakan!"
                lblStatus.ForeColor = Color.FromArgb(25, 135, 84)

                If Not String.IsNullOrEmpty(skuClean) Then
                    FormMessageBox.Show(Me, $"Voucher {code} berhasil digunakan!{Environment.NewLine}Item gratis: '{skuClean}' (Qty: {qty}) akan ditambahkan ke transaksi POS seharga Rp 0.", "Voucher Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf discAmount > 0 Then
                    Dim discInfoStr = If(discRate > 0, $"{discRate * 100:N0}% (Rp {discAmount:N0})", $"Rp {discAmount:N0}")
                    FormMessageBox.Show(Me, $"Voucher {code} berhasil digunakan!{Environment.NewLine}Diskon {discInfoStr} berhasil diterapkan ke transaksi POS.", "Voucher Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    FormMessageBox.Show(Me, $"Voucher {code} berhasil digunakan!", "Voucher Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                lblStatus.Text = "Terjadi kesalahan: " & ex.Message
                lblStatus.ForeColor = Color.Red
                FormMessageBox.Show(Me, $"Terjadi kesalahan saat memproses voucher:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                SetProcessingState(False)
            End Try
        End Sub

        Private Sub SetProcessingState(isProcessing As Boolean)
            btnUseVoucher.Enabled = Not isProcessing
            btnCancel.Enabled = Not isProcessing
            txtVoucherCode.Enabled = Not isProcessing
            txtAuthCode.Enabled = Not isProcessing
            btnUseVoucher.Text = If(isProcessing, "Memproses...", "✔ GUNAKAN VOUCHER")
            Cursor = If(isProcessing, Cursors.WaitCursor, Cursors.Default)
        End Sub
    End Class
End Namespace
