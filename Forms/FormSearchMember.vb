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
        ''' Inisialisasi FormSearchMember dengan passing instance GoappApiClient dari POS
        ''' </summary>
        Public Sub New(client As GoappApiClient)
            If client Is Nothing Then
                Throw New ArgumentNullException(NameOf(client), "GoappApiClient tidak boleh Nothing. Pastikan sudah diinisialisasi dari aplikasi POS.")
            End If
            Me.InitializeComponent()
            _client = client
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
            AttachNumpadEvents()
        End Sub

        Private Sub AttachNumpadEvents()
            ' Pasang event handler klik tombol angka 0-9
            Dim numButtons As Button() = {btnNum0, btnNum1, btnNum2, btnNum3, btnNum4, btnNum5, btnNum6, btnNum7, btnNum8, btnNum9, btnPlus}
            For Each btn In numButtons
                AddHandler btn.Click, AddressOf OnNumberButtonClick
            Next
        End Sub

        Private Sub FormSearchMember_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ResetDisplay()
            txtPhoneNo.Focus()
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
                MessageBox.Show("Silakan masukkan Nomor HP atau Member ID!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
                    Else
                        lblStatusBadge.Text = "🔴 " & statusDesc
                        lblStatusBadge.BackColor = Color.FromArgb(248, 215, 218)
                        lblStatusBadge.ForeColor = Color.FromArgb(114, 28, 36)
                        btnSelect.Enabled = True ' Tetap diizinkan agar POS bisa memutuskan apakah boleh atau tidak
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
                    MessageBox.Show($"Member tidak ditemukan:{Environment.NewLine}{memberResp.Message}", "Hasil Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Catch ex As Exception
                ResetDisplay()
                lblSearchStatus.Text = "Terjadi kesalahan sistem."
                lblSearchStatus.ForeColor = Color.Red
                MessageBox.Show($"Terjadi kesalahan saat memanggil API:{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                SetSearchingState(False)
            End Try
        End Sub

        Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
            If _currentMember Is Nothing Then
                MessageBox.Show("Silakan cari dan pilih member terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Susun argumen data yang akan dikirimkan ke aplikasi POS
            Dim eventArgs As New MemberDataEventArgs With {
                .FullName = _currentMember.FullName,
                .MobileNo = _currentMember.MobileNo,
                .IsActive = _isMemberActive,
                .StatusDescription = If(_isMemberActive, "Active", "Inactive"),
                .Uid = _currentMember.Uid,
                .LevelName = If(_currentMember.Level IsNot Nothing, _currentMember.Level.Name, "-"),
                .SchemeName = If(_currentMember.Scheme IsNot Nothing, _currentMember.Scheme.Name, "-"),
                .AvailablePoints = _currentMember.AvailablePoints,
                .PointsInRupiah = _currentMember.PointsInRupiah,
                .ReferralCode = If(String.IsNullOrEmpty(_currentMember.ReferralCode), "-", _currentMember.ReferralCode),
                .DealCodes = If(_memberDeals, New List(Of MemberDealCodeResponse)()),
                .RawMember = _currentMember
            }

            ' Tembakkan event ke aplikasi POS pemanggil
            RaiseEvent sendMemberData(Me, eventArgs)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub ResetDisplay()
            _currentMember = Nothing
            _memberDeals = Nothing
            _isMemberActive = False

            lblMemberName.Text = "-"
            lblMemberPhone.Text = "-"
            lblMemberLevel.Text = "-"
            lblMemberPoints.Text = "0 Pts (Rp 0)"

            lblStatusBadge.Text = "BELUM DICARI"
            lblStatusBadge.BackColor = Color.FromArgb(230, 235, 245)
            lblStatusBadge.ForeColor = Color.FromArgb(100, 110, 125)

            lstDeals.Items.Clear()
            btnSelect.Enabled = False
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
