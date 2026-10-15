Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services

Namespace Forms
    ''' <summary>
    ''' Form visual untuk pencarian dan validasi member Goapp CRM langsung dari DLL DreamposXGoapp.
    ''' Dilengkapi touchscreen keypad numerik (0-9, Clear, Hapus), tampilan info member dan daftar benefit,
    ''' serta mengembalikan data ke aplikasi POS via RaiseEvent sendMemberData.
    ''' </summary>
    Public Class FormSearchMember
        Private ReadOnly _client As GoappApiClient
        Private _currentMember As MemberResponse
        Private _memberDeals As List(Of MemberDealCodeResponse)
        Private _isMemberActive As Boolean = True
        Private _activeLogs As New List(Of GoappRedeemLogItem)()

        ''' <summary>
        ''' Event yang ditembakkan ke aplikasi POS pemanggil saat kasir memilih member.
        ''' POS dapat menangkapnya via: AddHandler g_frmLoyal.sendMemberData, AddressOf GetLoyalID_MemberData
        ''' </summary>
        Public Event sendMemberData(sender As Object, e As MemberDataEventArgs)

        ''' <summary>
        ''' Data member terpilih (jika POS ingin membaca langsung dari properti)
        ''' </summary>
        Public Property SelectedMember As MemberResponse
            Get
                Return _currentMember
            End Get
            Private Set(value As MemberResponse)
                _currentMember = value
            End Set
        End Property

        ''' <summary>
        ''' Daftar benefit / promo aktif milik member terpilih
        ''' </summary>
        Public Property SelectedDeals As List(Of MemberDealCodeResponse)
            Get
                Return _memberDeals
            End Get
            Private Set(value As List(Of MemberDealCodeResponse))
                _memberDeals = value
            End Set
        End Property

        ''' <summary>
        ''' Nomor transaksi POS yang dipassing dari FPaymentLatest
        ''' </summary>
        Public Property TransactionRef As String = String.Empty

        ''' <summary>
        ''' Subtotal transaksi POS sebelum pajak & service yang dipassing dari FPaymentLatest
        ''' </summary>
        Public Property Subtotal As Decimal = 0D

        ''' <summary>
        ''' Total transaksi POS (m_colSM.Total) yang dipassing dari FPaymentLatest
        ''' </summary>
        Public Property TotalTransaction As Decimal = 0D

        ''' <summary>
        ''' Nama toko / cabang POS
        ''' </summary>
        Public Property StoreName As String = String.Empty

        ''' <summary>
        ''' Kode toko / outlet POS
        ''' </summary>
        Public Property StoreCode As String = String.Empty

        ''' <summary>
        ''' Voucher yang berhasil diredeem (jika kasir menggunakan voucher pada form ini)
        ''' </summary>
        Public Property RedeemedVoucher As RedeemedVoucherInfo = Nothing

        ''' <summary>
        ''' Poin yang berhasil diredeem (jika kasir menggunakan burn point pada form ini)
        ''' </summary>
        Public Property RedeemedPoint As RedeemedPointInfo = Nothing

        ''' <summary>
        ''' Inisialisasi FormSearchMember dengan passing instance GoappApiClient dari POS
        ''' </summary>
        Public Sub New(client As GoappApiClient)
            If client Is Nothing Then
                Throw New ArgumentNullException(NameOf(client), "GoappApiClient tidak boleh Nothing. Pastikan sudah diinisialisasi dari aplikasi POS.")
            End If
            Me.InitializeComponent()
            _client = client
            If _client.Config IsNot Nothing Then
                Me.StoreName = _client.Config.StoreName
                Me.StoreCode = _client.Config.StoreCode
            End If
            AttachNumpadEvents()
        End Sub

        ''' <summary>
        ''' Inisialisasi FormSearchMember dengan passing konfigurasi GoappConfig dari POS
        ''' </summary>
        Public Sub New(config As GoappConfig)
            If config Is Nothing Then
                Throw New ArgumentNullException(NameOf(config), "GoappConfig tidak boleh Nothing.")
            End If
            Me.InitializeComponent()
            _client = New GoappApiClient(config)
            Me.StoreName = config.StoreName
            Me.StoreCode = config.StoreCode
            AttachNumpadEvents()
        End Sub

        Private Sub AttachNumpadEvents()
            ' Pasang event handler klik tombol angka 0-9
            Dim numButtons As Button() = {btnNum0, btnNum1, btnNum2, btnNum3, btnNum4, btnNum5, btnNum6, btnNum7, btnNum8, btnNum9, btnPlus}
            For Each btn In numButtons
                AddHandler btn.Click, AddressOf OnNumberButtonClick
            Next
        End Sub

        Private Async Sub FormSearchMember_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ResetDisplay()
            txtPhoneNo.Focus()
            Await RefreshActiveRedeemBadgeAsync()
        End Sub

        Private Sub OnNumberButtonClick(sender As Object, e As EventArgs)
            Dim btn = TryCast(sender, Button)
            If btn IsNot Nothing Then
                txtPhoneNo.AppendText(btn.Text)
                txtPhoneNo.Focus()
            End If
        End Sub

        Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
            txtPhoneNo.Clear()
            ResetDisplay()
            txtPhoneNo.Focus()
        End Sub

        Private Sub btnBackspace_Click(sender As Object, e As EventArgs) Handles btnBackspace.Click
            If txtPhoneNo.TextLength > 0 Then
                Dim selStart = txtPhoneNo.SelectionStart
                If selStart > 0 Then
                    txtPhoneNo.Text = txtPhoneNo.Text.Remove(selStart - 1, 1)
                    txtPhoneNo.SelectionStart = selStart - 1
                Else
                    txtPhoneNo.Text = txtPhoneNo.Text.Substring(0, txtPhoneNo.TextLength - 1)
                    txtPhoneNo.SelectionStart = txtPhoneNo.TextLength
                End If
            End If
            txtPhoneNo.Focus()
        End Sub

        Private Sub txtPhoneNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPhoneNo.KeyDown
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                btnSearch.PerformClick()
            End If
        End Sub

        Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
            Dim query = txtPhoneNo.Text.Trim()
            If String.IsNullOrWhiteSpace(query) Then
                FormMessageBox.Show(Me, "Silakan masukkan Nomor HP atau Member ID!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPhoneNo.Focus()
                Return
            End If

            SetSearchingState(True)
            lblSearchStatus.Text = "Menghubungi Goapp CRM..."
            lblSearchStatus.ForeColor = Color.FromArgb(13, 110, 253)

            Try
                ' 1. Cari data profil member
                Dim memberResp = Await _client.GetMemberAsync(query)

                If memberResp.IsSuccess AndAlso memberResp.Data IsNot Nothing Then
                    _currentMember = memberResp.Data

                    ' Evaluasi status member (Active / Inactive)
                    _isMemberActive = _currentMember.IsMemberActive
                    Dim statusDesc = If(Not String.IsNullOrEmpty(_currentMember.Status), _currentMember.Status.ToUpper(), If(_isMemberActive, "AKTIF", "TIDAK AKTIF"))

                    ' Update tampilan info profil
                    lblMemberName.Text = _currentMember.FullName
                    lblMemberPhone.Text = _currentMember.MobileNo
                    lblMemberLevel.Text = $"{If(_currentMember.Level IsNot Nothing, _currentMember.Level.Name, "-")} ({If(_currentMember.Scheme IsNot Nothing, _currentMember.Scheme.Name, "-")})"
                    lblMemberPoints.Text = $"{_currentMember.AvailablePoints:N0} Pts (Rp {_currentMember.PointsInRupiah:N0})"

                    If _isMemberActive Then
                        lblStatusBadge.Text = "🟢 " & statusDesc
                        lblStatusBadge.BackColor = Color.FromArgb(212, 237, 218)
                        lblStatusBadge.ForeColor = Color.FromArgb(21, 87, 36)
                        btnSelect.Enabled = True
                        btnUsePoint.Enabled = (_currentMember.AvailablePoints > 0D)
                    Else
                        lblStatusBadge.Text = "🔴 " & statusDesc
                        lblStatusBadge.BackColor = Color.FromArgb(248, 215, 218)
                        lblStatusBadge.ForeColor = Color.FromArgb(114, 28, 36)
                        btnSelect.Enabled = True ' Tetap diizinkan agar POS bisa memutuskan apakah boleh atau tidak
                        btnUsePoint.Enabled = False
                    End If

                    ' 2. Ambil daftar benefit / deal_codes aktif milik member
                    lblSearchStatus.Text = "Memuat benefit promo member..."
                    lstDeals.Items.Clear()
                    Dim dealResp = Await _client.GetMemberDealCodesAsync(query)
                    If dealResp.IsSuccess AndAlso dealResp.Data IsNot Nothing AndAlso dealResp.Data.Count > 0 Then
                        _memberDeals = dealResp.Data
                        For Each d In _memberDeals
                            Dim dealName = If(d.Deal IsNot Nothing AndAlso Not String.IsNullOrEmpty(d.Deal.Name), d.Deal.Name, "Promo Member")
                            lstDeals.Items.Add($"[{d.Code}] {dealName}")
                        Next
                        lblSearchStatus.Text = $"Member ditemukan. ({_memberDeals.Count} benefit aktif)"
                    Else
                        _memberDeals = New List(Of MemberDealCodeResponse)()
                        lstDeals.Items.Add("(Belum ada promo / voucher aktif untuk member ini)")
                        lblSearchStatus.Text = "Member ditemukan. Tidak ada promo aktif."
                    End If

                    lblSearchStatus.ForeColor = Color.FromArgb(25, 135, 84)
                Else
                    ResetDisplay()
                    lblStatusBadge.Text = "TIDAK DITEMUKAN"
                    lblStatusBadge.BackColor = Color.FromArgb(254, 237, 237)
                    lblStatusBadge.ForeColor = Color.FromArgb(220, 53, 69)
                    lblSearchStatus.Text = $"Pencarian gagal: {memberResp.Message}"
                    lblSearchStatus.ForeColor = Color.FromArgb(220, 53, 69)
                    FormMessageBox.Show(Me, $"Member tidak ditemukan:{Environment.NewLine}{memberResp.Message}", "Hasil Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Catch ex As Exception
                ResetDisplay()
                lblSearchStatus.Text = "Terjadi kesalahan sistem."
                lblSearchStatus.ForeColor = Color.Red
                FormMessageBox.Show(Me, $"Terjadi kesalahan saat memanggil API:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                SetSearchingState(False)
            End Try
        End Sub

        Private Sub lstDeals_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstDeals.DrawItem
            If e.Index < 0 Then Return

            Dim isSelected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim g = e.Graphics
            Dim bounds = e.Bounds

            ' Background
            Dim bgBrush As Brush
            If isSelected Then
                bgBrush = New SolidBrush(Color.FromArgb(220, 235, 255))
            Else
                bgBrush = If(e.Index Mod 2 = 0, Brushes.White, New SolidBrush(Color.FromArgb(248, 250, 252)))
            End If
            g.FillRectangle(bgBrush, bounds)

            ' Cek apakah ini item voucher asli atau teks placeholder
            If _memberDeals IsNot Nothing AndAlso e.Index < _memberDeals.Count Then
                Dim d = _memberDeals(e.Index)
                Dim codeStr = If(Not String.IsNullOrEmpty(d.Code), d.Code, "VOUCHER")
                Dim dealName = If(d.Deal IsNot Nothing AndAlso Not String.IsNullOrEmpty(d.Deal.Name), d.Deal.Name, "Promo Member")

                ' Ringkasan reward
                Dim rewardDesc = "Diskon / Benefit Member"
                If d.Deal IsNot Nothing AndAlso d.Deal.RewardData IsNot Nothing Then
                    Dim rd = d.Deal.RewardData
                    If String.Equals(rd.DiscountType, "free_items", StringComparison.OrdinalIgnoreCase) Then
                        Dim rawSku = If(rd.Item IsNot Nothing AndAlso rd.Item.Skus IsNot Nothing AndAlso rd.Item.Skus.Count > 0, rd.Item.Skus(0), rd.EffectiveItemSkus)
                        Dim cleanSku = If(rawSku.Contains("-"), rawSku.Split("-"c)(0).Trim(), rawSku.Trim())
                        Dim qty = If(rd.EffectiveItemQuantity > 0, rd.EffectiveItemQuantity, 1)
                        rewardDesc = $"🎁 Free Item: {cleanSku} (Qty: {qty})"
                    Else
                        Dim minTxt = If(rd.EffectiveMinPurchase > 0, $" | Min. Rp {rd.EffectiveMinPurchase:N0}", "")
                        If rd.EffectiveDiscountType.ToLower().Contains("percent") OrElse String.Equals(rd.DiscountType, "percentage", StringComparison.OrdinalIgnoreCase) Then
                            Dim pct = If(rd.EffectiveDiscountAmount <= 1D AndAlso rd.EffectiveDiscountAmount > 0D, rd.EffectiveDiscountAmount * 100D, rd.EffectiveDiscountAmount)
                            rewardDesc = $"🏷 Diskon {pct:N0}%{minTxt}"
                        Else
                            rewardDesc = $"🏷 Diskon Rp {rd.EffectiveDiscountAmount:N0}{minTxt}"
                        End If
                    End If
                End If

                ' 1. Badge Kode Voucher (Kiri)
                Dim codeBadgeRect As New Rectangle(bounds.Left + 8, bounds.Top + 8, 120, 42)
                Using badgeBrush As New SolidBrush(If(isSelected, Color.FromArgb(13, 110, 253), Color.FromArgb(235, 240, 250)))
                    g.FillRectangle(badgeBrush, codeBadgeRect)
                End Using
                Using borderPen As New Pen(If(isSelected, Color.FromArgb(10, 88, 202), Color.FromArgb(200, 210, 225)))
                    g.DrawRectangle(borderPen, codeBadgeRect)
                End Using

                Using codeFont As New Font("Segoe UI", 9.5!, FontStyle.Bold)
                    Dim sfCode As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    Dim codeColor = If(isSelected, Color.White, Color.FromArgb(13, 110, 253))
                    Using codeBrush As New SolidBrush(codeColor)
                        g.DrawString(codeStr, codeFont, codeBrush, codeBadgeRect, sfCode)
                    End Using
                End Using

                ' 2. Nama Voucher & Info Reward (Tengah)
                Dim textLeft = bounds.Left + 138
                Dim textWidth = bounds.Width - 146 - 80
                Dim nameRect As New Rectangle(textLeft, bounds.Top + 8, textWidth, 22)
                Dim descRect As New Rectangle(textLeft, bounds.Top + 30, textWidth, 20)

                Using nameFont As New Font("Segoe UI", 10.5!, FontStyle.Bold)
                    Using nameBrush As New SolidBrush(Color.FromArgb(30, 40, 55))
                        Dim sfName As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                        g.DrawString(dealName, nameFont, nameBrush, nameRect, sfName)
                    End Using
                End Using

                Using descFont As New Font("Segoe UI", 9.0!, FontStyle.Regular)
                    Using descBrush As New SolidBrush(Color.FromArgb(90, 100, 115))
                        Dim sfDesc As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                        g.DrawString(rewardDesc, descFont, descBrush, descRect, sfDesc)
                    End Using
                End Using

                ' 3. Tombol Visual "PILIH 👉" (Kanan)
                Dim btnRect As New Rectangle(bounds.Right - 76, bounds.Top + 14, 68, 30)
                Using btnBgBrush As New SolidBrush(If(isSelected, Color.FromArgb(25, 135, 84), Color.FromArgb(240, 243, 248)))
                    g.FillRectangle(btnBgBrush, btnRect)
                End Using
                Using btnPen As New Pen(If(isSelected, Color.FromArgb(20, 108, 67), Color.FromArgb(215, 220, 230)))
                    g.DrawRectangle(btnPen, btnRect)
                End Using
                Using btnFont As New Font("Segoe UI", 8.5!, FontStyle.Bold)
                    Dim sfBtn As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    Dim btnTextColor = If(isSelected, Color.White, Color.FromArgb(70, 80, 95))
                    Using btnTextBrush As New SolidBrush(btnTextColor)
                        g.DrawString("PILIH 👉", btnFont, btnTextBrush, btnRect, sfBtn)
                    End Using
                End Using

            Else
                ' Placeholder / Kosong
                Dim itemText = If(e.Index < lstDeals.Items.Count, lstDeals.Items(e.Index).ToString(), "")
                Using phFont As New Font("Segoe UI", 9.5!, FontStyle.Italic)
                    Using phBrush As New SolidBrush(Color.FromArgb(120, 130, 145))
                        Dim sfPh As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                        g.DrawString(itemText, phFont, phBrush, bounds, sfPh)
                    End Using
                End Using
            End If

            ' Garis pemisah bawah antar item
            Using linePen As New Pen(Color.FromArgb(230, 235, 242))
                g.DrawLine(linePen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1)
            End Using

            If Not isSelected AndAlso bgBrush IsNot Nothing AndAlso bgBrush IsNot Brushes.White Then
                bgBrush.Dispose()
            End If
        End Sub

        Private Sub lstDeals_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDeals.SelectedIndexChanged
            Dim hasSelection = (lstDeals.SelectedIndex >= 0 AndAlso _memberDeals IsNot Nothing AndAlso lstDeals.SelectedIndex < _memberDeals.Count)
            btnUseSelectedVoucher.Enabled = hasSelection
        End Sub

        Private Sub btnUseSelectedVoucher_Click(sender As Object, e As EventArgs) Handles btnUseSelectedVoucher.Click
            If lstDeals.SelectedIndex < 0 OrElse _memberDeals Is Nothing OrElse lstDeals.SelectedIndex >= _memberDeals.Count Then Return
            Dim selectedDeal = _memberDeals(lstDeals.SelectedIndex)
            OpenRedeemVoucherDialog(selectedDeal.Code, selectedDeal)
        End Sub

        Private Sub lstDeals_DoubleClick(sender As Object, e As EventArgs) Handles lstDeals.DoubleClick
            If lstDeals.SelectedIndex < 0 OrElse _memberDeals Is Nothing OrElse _memberDeals.Count = 0 Then Return
            If lstDeals.SelectedIndex >= _memberDeals.Count Then Return

            Dim selectedDeal = _memberDeals(lstDeals.SelectedIndex)
            OpenRedeemVoucherDialog(selectedDeal.Code, selectedDeal)
        End Sub

        Private Sub btnScanQrVoucher_Click(sender As Object, e As EventArgs) Handles btnScanQrVoucher.Click
            OpenRedeemVoucherDialog(String.Empty, Nothing)
        End Sub

        Private Async Sub OpenRedeemVoucherDialog(dealCode As String, presetDeal As MemberDealCodeResponse)
            Try
                Dim memberUid As Long? = If(_currentMember IsNot Nothing AndAlso _currentMember.Uid > 0, _currentMember.Uid, CType(Nothing, Long?))
                Dim memberName = If(_currentMember IsNot Nothing, _currentMember.FullName, "")
                Using frmRedeem As New FormRedeemVoucher(_client, dealCode, presetDeal, memberUid, Me.TransactionRef, Me.Subtotal, memberName:=memberName)
                    frmRedeem.TopMost = True
                    If frmRedeem.ShowDialog(Me) = DialogResult.OK AndAlso frmRedeem.RedeemedResult IsNot Nothing Then
                        Me.RedeemedVoucher = frmRedeem.RedeemedResult
                        Await RefreshActiveRedeemBadgeAsync()

                        FormMessageBox.Show(Me, $"Voucher '{Me.RedeemedVoucher.VoucherName}' berhasil di-redeem!{Environment.NewLine}{Environment.NewLine}" &
                                                $"Klik tombol '✓ MASUKKAN KE POS' untuk menerapkan diskon ke struk POS,{Environment.NewLine}" &
                                                $"atau batalkan pada daftar '📋 Voucher & Poin Terpakai' di samping jika ingin membatalkannya.",
                                                "Voucher Berhasil Di-redeem", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            Catch ex As Exception
                FormMessageBox.Show(Me, "Gagal membuka form redeem voucher: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Async Sub btnUsePoint_Click(sender As Object, e As EventArgs) Handles btnUsePoint.Click
            If _currentMember Is Nothing OrElse _currentMember.Uid <= 0 Then
                FormMessageBox.Show(Me, "Silakan cari member terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _currentMember.AvailablePoints <= 0 Then
                FormMessageBox.Show(Me, "Member tidak memiliki saldo poin untuk digunakan!", "Poin Kosong", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Try
                Using frmBurn As New FormRedeemPoint(_client, _currentMember.Uid, _currentMember.FullName, _currentMember.AvailablePoints, Me.TotalTransaction, Me.TransactionRef, storeName:=Me.StoreName, storeCode:=Me.StoreCode)
                    frmBurn.TopMost = True
                    If frmBurn.ShowDialog(Me) = DialogResult.OK AndAlso frmBurn.RedeemedResult IsNot Nothing Then
                        Me.RedeemedPoint = frmBurn.RedeemedResult

                        ' Hitung sisa poin setelah dibakar
                        Dim remainingPts = Math.Max(0D, _currentMember.AvailablePoints - frmBurn.RedeemedResult.PointAmount)
                        If _currentMember.Account IsNot Nothing Then
                            _currentMember.Account.Balance = remainingPts.ToString(Globalization.CultureInfo.InvariantCulture)
                            _currentMember.Account.IdrBalance = remainingPts
                        End If
                        lblMemberPoints.Text = $"{remainingPts:N0} Pts (Rp {remainingPts:N0})"

                        Await RefreshActiveRedeemBadgeAsync()

                        FormMessageBox.Show(Me, $"Pemotongan poin sebesar Rp {frmBurn.RedeemedResult.PointAmount:N0} berhasil!{Environment.NewLine}{Environment.NewLine}" &
                                                $"Klik tombol '✓ MASUKKAN KE POS' untuk menerapkan ke transaksi POS,{Environment.NewLine}" &
                                                $"atau batalkan pada daftar '📋 Voucher & Poin Terpakai' di samping jika ingin membatalkannya sebelum masuk ke POS.",
                                                "Poin Berhasil Digunakan", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            Catch ex As Exception
                FormMessageBox.Show(Me, "Gagal membuka form gunakan poin: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Async Function RefreshActiveRedeemBadgeAsync() As Task
            Try
                If _client IsNot Nothing AndAlso _client.RedeemRepo IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.TransactionRef) Then
                    Dim activeLogs = Await _client.RedeemRepo.GetActiveLogsByTxRefAsync(Me.TransactionRef)
                    _activeLogs = If(activeLogs, New List(Of GoappRedeemLogItem)())
                    Dim count = _activeLogs.Count

                    grpActiveRedeems.Text = $"📋 Voucher & Poin Terpakai ({count})"
                    lblActiveRedeemsInfo.Text = If(count > 0, $"{count} item siap masuk ke POS. Pilih item untuk membatalkan.", "Belum ada voucher atau poin terpakai pada transaksi ini.")

                    lstActiveRedeems.Items.Clear()
                    If count > 0 Then
                        For Each itm In _activeLogs
                            lstActiveRedeems.Items.Add(itm)
                        Next
                        btnSelect.Text = "✓ MASUKKAN KE POS"
                        btnSelect.Enabled = True

                        ' Jika ada item terpakai (> 0), tombol Cancel tidak bisa digunakan
                        btnCancel.Enabled = False
                        btnCancel.BackColor = Color.FromArgb(200, 205, 210)
                        btnCancel.ForeColor = Color.FromArgb(140, 145, 150)
                    Else
                        lstActiveRedeems.Items.Add("(Belum ada voucher atau poin terpakai)")
                        btnCancel.Enabled = True
                        btnCancel.BackColor = Color.FromArgb(108, 117, 125)
                        btnCancel.ForeColor = Color.White

                        If _currentMember IsNot Nothing Then
                            btnSelect.Text = "✓ PILIH MEMBER"
                            btnSelect.Enabled = True
                        Else
                            btnSelect.Text = "✓ PILIH MEMBER"
                            btnSelect.Enabled = False
                        End If
                    End If
                    btnCancelActiveItem.Enabled = False
                Else
                    grpActiveRedeems.Text = "📋 Voucher & Poin Terpakai (0)"
                    lblActiveRedeemsInfo.Text = "Belum ada voucher atau poin terpakai."
                    lstActiveRedeems.Items.Clear()
                    lstActiveRedeems.Items.Add("(Belum ada voucher atau poin terpakai)")
                    btnCancelActiveItem.Enabled = False
                    btnCancel.Enabled = True
                    btnCancel.BackColor = Color.FromArgb(108, 117, 125)
                    btnCancel.ForeColor = Color.White
                End If
            Catch ex As Exception
                ' Abaikan jika gagal cek log
            End Try
        End Function

        Private Sub lstActiveRedeems_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstActiveRedeems.DrawItem
            If e.Index < 0 Then Return

            Dim isSelected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim g = e.Graphics
            Dim bounds = e.Bounds

            ' Background
            Dim bgBrush As Brush
            If isSelected Then
                bgBrush = New SolidBrush(Color.FromArgb(220, 235, 255))
            Else
                bgBrush = If(e.Index Mod 2 = 0, Brushes.White, New SolidBrush(Color.FromArgb(248, 250, 252)))
            End If
            g.FillRectangle(bgBrush, bounds)

            If _activeLogs IsNot Nothing AndAlso e.Index < _activeLogs.Count Then
                Dim itm = _activeLogs(e.Index)

                ' 1. Badge Jenis (Voucher / Poin)
                Dim isVoucher = String.Equals(itm.Jenis, "RedeemVoucher", StringComparison.OrdinalIgnoreCase)
                Dim badgeColor = If(isVoucher, Color.FromArgb(253, 126, 20), Color.FromArgb(25, 135, 84))
                Dim badgeText = If(isVoucher, "🎟 VOUCHER", "⭐ POIN")
                Dim badgeRect As New Rectangle(bounds.Left + 6, bounds.Top + 8, 80, 44)

                Using bBrush As New SolidBrush(If(isSelected, badgeColor, Color.FromArgb(240, 243, 248)))
                    g.FillRectangle(bBrush, badgeRect)
                End Using
                Using bPen As New Pen(badgeColor, 1.5!)
                    g.DrawRectangle(bPen, badgeRect)
                End Using
                Using bFont As New Font("Segoe UI", 8.0!, FontStyle.Bold)
                    Dim sfB As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    Dim tColor = If(isSelected, Color.White, badgeColor)
                    Using tBrush As New SolidBrush(tColor)
                        g.DrawString(badgeText, bFont, tBrush, badgeRect, sfB)
                    End Using
                End Using

                ' 2. Info Detail (Kode, Nama, Nilai)
                Dim textLeft = bounds.Left + 92
                Dim textWidth = bounds.Width - 98
                Dim titleRect As New Rectangle(textLeft, bounds.Top + 8, textWidth, 22)
                Dim descRect As New Rectangle(textLeft, bounds.Top + 30, textWidth, 22)

                Dim titleText = $"[{itm.Kode}] {itm.NamaVoucher}"
                Dim nilaiDesc = If(itm.NilaiPoint > 0, $"Nilai: Rp {itm.NilaiPoint:N0}", "Reward: Free Item")
                Dim descText = $"{nilaiDesc} | {itm.Waktu:HH:mm:ss}"

                Using titleFont As New Font("Segoe UI", 9.5!, FontStyle.Bold)
                    Using titleBrush As New SolidBrush(Color.FromArgb(30, 40, 55))
                        Dim sfT As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                        g.DrawString(titleText, titleFont, titleBrush, titleRect, sfT)
                    End Using
                End Using

                Using descFont As New Font("Segoe UI", 8.5!, FontStyle.Regular)
                    Using descBrush As New SolidBrush(Color.FromArgb(90, 100, 115))
                        Dim sfD As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                        g.DrawString(descText, descFont, descBrush, descRect, sfD)
                    End Using
                End Using
            Else
                ' Placeholder jika list kosong
                Dim itemText = If(e.Index < lstActiveRedeems.Items.Count, lstActiveRedeems.Items(e.Index).ToString(), "")
                Using phFont As New Font("Segoe UI", 9.0!, FontStyle.Italic)
                    Using phBrush As New SolidBrush(Color.FromArgb(120, 130, 145))
                        Dim sfPh As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                        g.DrawString(itemText, phFont, phBrush, bounds, sfPh)
                    End Using
                End Using
            End If

            ' Garis pemisah bawah
            Using linePen As New Pen(Color.FromArgb(230, 235, 242))
                g.DrawLine(linePen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1)
            End Using

            If Not isSelected AndAlso bgBrush IsNot Nothing AndAlso bgBrush IsNot Brushes.White Then
                bgBrush.Dispose()
            End If
        End Sub

        Private Sub lstActiveRedeems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstActiveRedeems.SelectedIndexChanged
            Dim hasValid = (lstActiveRedeems.SelectedIndex >= 0 AndAlso _activeLogs IsNot Nothing AndAlso lstActiveRedeems.SelectedIndex < _activeLogs.Count)
            btnCancelActiveItem.Enabled = hasValid
        End Sub

        Private Sub lstActiveRedeems_DoubleClick(sender As Object, e As EventArgs) Handles lstActiveRedeems.DoubleClick
            If btnCancelActiveItem.Enabled Then
                btnCancelActiveItem_Click(sender, e)
            End If
        End Sub

        Private Async Sub btnCancelActiveItem_Click(sender As Object, e As EventArgs) Handles btnCancelActiveItem.Click
            If lstActiveRedeems.SelectedIndex < 0 OrElse _activeLogs Is Nothing OrElse lstActiveRedeems.SelectedIndex >= _activeLogs.Count Then
                Return
            End If

            Dim selected = _activeLogs(lstActiveRedeems.SelectedIndex)
            If Not selected.CanCancelFromDll Then
                FormMessageBox.Show(Me, "Item ini sudah masuk ke transaksi POS atau sudah dibatalkan sebelumnya sehingga tidak dapat dibatalkan lagi dari form ini.", "Tidak Dapat Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Konfirmasi pembatalan
            Dim confirmText = $"Apakah Anda yakin ingin membatalkan penggunaan {If(selected.Jenis = "RedeemVoucher", "Voucher", "Poin")} berikut?{Environment.NewLine}{Environment.NewLine}" &
                              $"- Kode: {selected.Kode}{Environment.NewLine}" &
                              $"- Nama: {selected.NamaVoucher}{Environment.NewLine}" &
                              $"- Nilai: {If(selected.NilaiPoint > 0, "Rp " & selected.NilaiPoint.ToString("N0"), "-")}{Environment.NewLine}{Environment.NewLine}" &
                              $"Item akan dibatalkan dari sistem Goapp dan tidak akan masuk ke POS."

            If FormMessageBox.Show(Me, confirmText, "Konfirmasi Pembatalan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            btnCancelActiveItem.Enabled = False
            Cursor = Cursors.WaitCursor

            Try
                Dim isSuccess As Boolean = False
                Dim errMsg As String = ""

                If String.Equals(selected.Jenis, "RedeemVoucher", StringComparison.OrdinalIgnoreCase) Then
                    Dim cancelResp = Await _client.CancelVoucherAsync(selected.Kode, selected.TransactionRef, If(_currentMember IsNot Nothing AndAlso _currentMember.Uid > 0, _currentMember.Uid, CType(Nothing, Long?)))
                    isSuccess = cancelResp.IsSuccess
                    errMsg = cancelResp.Message
                Else
                    Dim cancelResp = Await _client.CancelPointPaymentAsync(selected.Kode, "Dibatalkan kasir di DLL sebelum masuk POS")
                    isSuccess = cancelResp.IsSuccess
                    errMsg = cancelResp.Message
                End If

                If isSuccess Then
                    Await _client.RedeemRepo.UpdateStatusCancelAsync(selected.Kode, selected.Jenis)
                    OnItemCancelledFromActiveRedeem(Me, selected)
                    FormMessageBox.Show(Me, $"Penggunaan {selected.Kode} berhasil dibatalkan dari Goapp CRM!", "Sukses Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Await RefreshActiveRedeemBadgeAsync()
                Else
                    FormMessageBox.Show(Me, $"Gagal membatalkan item di Goapp CRM:{Environment.NewLine}{errMsg}", "Gagal Batal", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                FormMessageBox.Show(Me, $"Terjadi exception saat membatalkan:{Environment.NewLine}{ex.Message}", "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = Cursors.Default
                btnCancelActiveItem.Enabled = (lstActiveRedeems.SelectedIndex >= 0 AndAlso _activeLogs IsNot Nothing AndAlso lstActiveRedeems.SelectedIndex < _activeLogs.Count)
            End Try
        End Sub

        Private Sub OnItemCancelledFromActiveRedeem(sender As Object, item As GoappRedeemLogItem)
            If item Is Nothing Then Return

            ' Jika yang dibatalkan adalah RedeemedVoucher saat ini
            If Me.RedeemedVoucher IsNot Nothing AndAlso String.Equals(Me.RedeemedVoucher.VoucherCode, item.Kode, StringComparison.OrdinalIgnoreCase) Then
                Me.RedeemedVoucher = Nothing
            End If

            ' Jika yang dibatalkan adalah RedeemedPoint saat ini
            If Me.RedeemedPoint IsNot Nothing AndAlso String.Equals(Me.RedeemedPoint.PaymentRef, item.Kode, StringComparison.OrdinalIgnoreCase) Then
                If _currentMember IsNot Nothing Then
                    ' Kembalikan saldo poin tampilan
                    Dim restored = _currentMember.AvailablePoints + Me.RedeemedPoint.PointAmount
                    If _currentMember.Account IsNot Nothing Then
                        _currentMember.Account.Balance = restored.ToString(Globalization.CultureInfo.InvariantCulture)
                        _currentMember.Account.IdrBalance = restored
                    End If
                    lblMemberPoints.Text = $"{restored:N0} Pts (Rp {restored:N0})"
                End If
                Me.RedeemedPoint = Nothing
            End If
        End Sub

        Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
            If _currentMember Is Nothing AndAlso Me.RedeemedVoucher Is Nothing AndAlso Me.RedeemedPoint Is Nothing Then
                FormMessageBox.Show(Me, "Silakan cari dan pilih member atau redeem voucher terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Susun argumen data yang akan dikirimkan ke aplikasi POS
            Dim eventArgs As New MemberDataEventArgs With {
                .FullName = If(_currentMember IsNot Nothing, _currentMember.FullName, "Pelanggan Goapp"),
                .MobileNo = If(_currentMember IsNot Nothing, _currentMember.MobileNo, ""),
                .IsActive = If(_currentMember IsNot Nothing, _isMemberActive, True),
                .StatusDescription = If(_currentMember IsNot Nothing, If(_isMemberActive, "Active", "Inactive"), "Active"),
                .Uid = If(_currentMember IsNot Nothing, _currentMember.Uid, 0L),
                .LevelName = If(_currentMember IsNot Nothing AndAlso _currentMember.Level IsNot Nothing, _currentMember.Level.Name, "-"),
                .SchemeName = If(_currentMember IsNot Nothing AndAlso _currentMember.Scheme IsNot Nothing, _currentMember.Scheme.Name, "-"),
                .AvailablePoints = If(_currentMember IsNot Nothing, _currentMember.AvailablePoints, 0D),
                .PointsInRupiah = If(_currentMember IsNot Nothing, _currentMember.PointsInRupiah, 0D),
                .ReferralCode = If(_currentMember IsNot Nothing AndAlso Not String.IsNullOrEmpty(_currentMember.ReferralCode), _currentMember.ReferralCode, "-"),
                .DealCodes = If(_memberDeals, New List(Of MemberDealCodeResponse)()),
                .RawMember = _currentMember,
                .RedeemedVoucher = Me.RedeemedVoucher,
                .RedeemedPoint = Me.RedeemedPoint
            }

            Try
                btnSelect.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                ' Tembakkan event ke aplikasi POS pemanggil
                RaiseEvent sendMemberData(Me, eventArgs)

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                FormMessageBox.Show(Me, "Gagal memproses data member ke POS: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor.Current = Cursors.Default
                btnSelect.Enabled = True
            End Try
        End Sub

        Private Async Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Try
                If _client IsNot Nothing AndAlso _client.RedeemRepo IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.TransactionRef) Then
                    Dim activeLogs = Await _client.RedeemRepo.GetActiveLogsByTxRefAsync(Me.TransactionRef)
                    If activeLogs IsNot Nothing AndAlso activeLogs.Count > 0 Then
                        FormMessageBox.Show(Me,
                            $"Tombol Batal tidak dapat digunakan karena masih ada {activeLogs.Count} voucher / poin yang sedang di-redeem.{Environment.NewLine}{Environment.NewLine}" &
                            $"Silakan batalkan item terlebih dahulu pada daftar '📋 Voucher & Poin Terpakai' di bawah sebelum membatalkan form, " &
                            $"atau klik '✓ MASUKKAN KE POS' untuk menerapkan item ke transaksi POS.",
                            "Tidak Dapat Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If
            Catch
            End Try

            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub ResetDisplay()
            _currentMember = Nothing
            _memberDeals = Nothing
            _isMemberActive = False
            Me.RedeemedVoucher = Nothing
            Me.RedeemedPoint = Nothing

            lblMemberName.Text = "-"
            lblMemberPhone.Text = "-"
            lblMemberLevel.Text = "-"
            lblMemberPoints.Text = "0 Pts (Rp 0)"

            lblStatusBadge.Text = "BELUM DICARI"
            lblStatusBadge.BackColor = Color.FromArgb(230, 235, 245)
            lblStatusBadge.ForeColor = Color.FromArgb(100, 110, 125)

            lstDeals.Items.Clear()
            btnSelect.Enabled = False
            btnUsePoint.Enabled = False
            btnUseSelectedVoucher.Enabled = False
            lblSearchStatus.Text = "Silakan masukkan nomor handphone atau Member ID"
            lblSearchStatus.ForeColor = Color.FromArgb(100, 110, 125)
        End Sub

        Private Sub SetSearchingState(isSearching As Boolean)
            btnSearch.Enabled = Not isSearching
            btnSearch.Text = If(isSearching, "Mencari...", "🔍 CARI MEMBER")
            btnBackspace.Enabled = Not isSearching
            btnClear.Enabled = Not isSearching
            btnPlus.Enabled = Not isSearching
            Cursor = If(isSearching, Cursors.WaitCursor, Cursors.Default)
        End Sub
    End Class
End Namespace
