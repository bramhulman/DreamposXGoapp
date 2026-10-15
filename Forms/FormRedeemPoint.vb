Imports System
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services

Namespace Forms
    ''' <summary>
    ''' Form dialog modal visual untuk menginput jumlah poin yang ingin dibakar (Burn Point)
    ''' dengan batasan saldo poin member dan total transaksi POS.
    ''' </summary>
    Public Class FormRedeemPoint
        Private ReadOnly _client As GoappApiClient

        ''' <summary>
        ''' Data hasil redeem point yang berhasil dibakar untuk dipassing ke POS
        ''' </summary>
        Public Property RedeemedResult As RedeemedPointInfo = Nothing

        ''' <summary>
        ''' UID member Goapp yang sedang aktif
        ''' </summary>
        Public Property MemberUid As Long = 0L

        ''' <summary>
        ''' Nama lengkap member aktif
        ''' </summary>
        Public Property MemberNameText As String = String.Empty

        ''' <summary>
        ''' Saldo poin yang dimiliki member
        ''' </summary>
        Public Property AvailablePoints As Decimal = 0D

        ''' <summary>
        ''' Total transaksi POS (m_colSM.Total)
        ''' </summary>
        Public Property CurrentTotalTransaction As Decimal = 0D

        ''' <summary>
        ''' Nomor referensi transaksi dari POS (misal: SM_Number)
        ''' </summary>
        Public Property TransactionRef As String = String.Empty

        ''' <summary>
        ''' Nama toko / outlet POS
        ''' </summary>
        Public Property StoreName As String = String.Empty

        ''' <summary>
        ''' Kode toko / outlet POS
        ''' </summary>
        Public Property StoreCode As String = String.Empty

        ''' <summary>
        ''' Batas maksimal poin yang boleh digunakan: Nilai minimum antara Saldo Poin dan Total Transaksi
        ''' </summary>
        Public ReadOnly Property MaxAllowedPoints As Decimal
            Get
                Dim maxBurn = Math.Min(AvailablePoints, CurrentTotalTransaction)
                Return If(maxBurn > 0D, maxBurn, 0D)
            End Get
        End Property

        ''' <summary>
        ''' Inisialisasi FormRedeemPoint
        ''' </summary>
        ''' <param name="client">Instance GoappApiClient</param>
        ''' <param name="memberUid">UID member aktif</param>
        ''' <param name="memberName">Nama member</param>
        ''' <param name="availablePoints">Saldo poin yang dimiliki member</param>
        ''' <param name="totalTx">Total transaksi POS (m_colSM.Total)</param>
        ''' <param name="txRef">Nomor transaksi POS</param>
        ''' <param name="storeName">Nama toko</param>
        ''' <param name="storeCode">Kode toko</param>
        Public Sub New(client As GoappApiClient,
                       memberUid As Long,
                       memberName As String,
                       availablePoints As Decimal,
                       totalTx As Decimal,
                       Optional txRef As String = "",
                       Optional storeName As String = "",
                       Optional storeCode As String = "")
            If client Is Nothing Then
                Throw New ArgumentNullException(NameOf(client), "GoappApiClient tidak boleh Nothing.")
            End If

            Me.InitializeComponent()
            _client = client
            Me.MemberUid = memberUid
            Me.MemberNameText = If(memberName, "Member Goapp")
            Me.AvailablePoints = availablePoints
            Me.CurrentTotalTransaction = totalTx
            Me.TransactionRef = If(txRef, String.Empty).Trim()
            Me.StoreName = If(Not String.IsNullOrEmpty(storeName), storeName, If(_client.Config IsNot Nothing, _client.Config.StoreName, String.Empty))
            Me.StoreCode = If(Not String.IsNullOrEmpty(storeCode), storeCode, If(_client.Config IsNot Nothing, _client.Config.StoreCode, String.Empty))

            AttachNumpadEvents()
        End Sub

        Private Sub AttachNumpadEvents()
            Dim numButtons As Button() = {btnNum0, btnNum1, btnNum2, btnNum3, btnNum4, btnNum5, btnNum6, btnNum7, btnNum8, btnNum9}
            For Each btn In numButtons
                AddHandler btn.Click, AddressOf OnNumberButtonClick
            Next
        End Sub

        Private Sub FormRedeemPoint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            lblMemberName.Text = Me.MemberNameText
            lblAvailablePoints.Text = $"{Me.AvailablePoints:N0} Pts (Rp {Me.AvailablePoints:N0})"
            lblTotalTx.Text = $"Rp {Me.CurrentTotalTransaction:N0}"
            lblMaxBurn.Text = $"{Me.MaxAllowedPoints:N0} Pts (Rp {Me.MaxAllowedPoints:N0})"

            txtBurnPoints.Text = "0"
            txtBurnPoints.SelectAll()
            txtBurnPoints.Focus()
        End Sub

        Private Sub OnNumberButtonClick(sender As Object, e As EventArgs)
            Dim btn = TryCast(sender, Button)
            If btn IsNot Nothing Then
                Dim digit = btn.Text
                Dim current = txtBurnPoints.Text.Trim()
                If current = "0" Then
                    txtBurnPoints.Text = digit
                Else
                    txtBurnPoints.Text = current & digit
                End If
                txtBurnPoints.SelectionStart = txtBurnPoints.TextLength
                txtBurnPoints.Focus()
            End If
        End Sub

        Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
            txtBurnPoints.Text = "0"
            txtBurnPoints.SelectionStart = txtBurnPoints.TextLength
            txtBurnPoints.Focus()
        End Sub

        Private Sub btnBackspace_Click(sender As Object, e As EventArgs) Handles btnBackspace.Click
            Dim current = txtBurnPoints.Text.Trim()
            If current.Length > 1 Then
                txtBurnPoints.Text = current.Substring(0, current.Length - 1)
            Else
                txtBurnPoints.Text = "0"
            End If
            txtBurnPoints.SelectionStart = txtBurnPoints.TextLength
            txtBurnPoints.Focus()
        End Sub

        Private Sub btnMaxPoint_Click(sender As Object, e As EventArgs) Handles btnMaxPoint.Click
            Dim maxBurn = Me.MaxAllowedPoints
            txtBurnPoints.Text = Math.Floor(maxBurn).ToString("0")
            txtBurnPoints.SelectionStart = txtBurnPoints.TextLength
            txtBurnPoints.Focus()
        End Sub

        Private Sub txtBurnPoints_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBurnPoints.KeyPress
            ' Hanya izinkan angka dan tombol kontrol (backspace)
            If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub txtBurnPoints_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBurnPoints.KeyDown
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                btnBurnPoint.PerformClick()
            End If
        End Sub

        Private Async Sub btnBurnPoint_Click(sender As Object, e As EventArgs) Handles btnBurnPoint.Click
            Dim burnPt As Decimal = 0D
            If Not Decimal.TryParse(txtBurnPoints.Text.Trim(), burnPt) OrElse burnPt <= 0D Then
                FormMessageBox.Show(Me, "Silakan masukkan jumlah poin yang ingin digunakan (harus lebih besar dari 0)!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBurnPoints.Focus()
                Return
            End If

            ' Validasi 1: Tidak boleh melebihi saldo poin member
            If burnPt > Me.AvailablePoints Then
                FormMessageBox.Show(Me, $"Poin yang dimasukkan ({burnPt:N0} Pts) melebihi saldo poin member ({Me.AvailablePoints:N0} Pts)!", "Saldo Poin Tidak Cukup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBurnPoints.Focus()
                Return
            End If

            ' Validasi 2: Nilai poin yang digunakan tidak boleh lebih besar dari total transaksinya
            If burnPt > Me.CurrentTotalTransaction Then
                FormMessageBox.Show(Me, $"Nilai poin yang digunakan ({burnPt:N0} Pts) tidak boleh lebih besar dari total transaksi (Rp {Me.CurrentTotalTransaction:N0})!", "Poin Melebihi Total Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBurnPoints.Focus()
                Return
            End If

            ' Konfirmasi kasir
            Dim confirmMsg = $"Konfirmasi penggunaan poin:{Environment.NewLine}" &
                             $"- Member: {Me.MemberNameText}{Environment.NewLine}" &
                             $"- Poin Digunakan: {burnPt:N0} Pts{Environment.NewLine}" &
                             $"- Nilai Diskon: Rp {burnPt:N0}{Environment.NewLine}" &
                             $"- Total Transaksi: Rp {Me.CurrentTotalTransaction:N0}{Environment.NewLine}{Environment.NewLine}" &
                             $"Lanjutkan pemotongan poin?"
            If FormMessageBox.Show(Me, confirmMsg, "Konfirmasi Burn Point", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            SetProcessingState(True)
            lblStatus.Text = "Memproses burn point ke Goapp CRM..."
            lblStatus.ForeColor = Color.FromArgb(13, 110, 253)

            Try
                Dim providerRef = If(Not String.IsNullOrEmpty(Me.TransactionRef), Me.TransactionRef, "TRX-" & DateTime.Now.ToString("yyMMddHHmmss"))
                Dim orderAmountIdr = burnPt ' 1 Poin = Rp 1
                Dim authCode = txtAuthCode.Text.Trim()

                ' Panggil API Burn Point Goapp
                Dim resp = Await _client.CreatePointPaymentAsync(Me.MemberUid, burnPt, orderAmountIdr, providerRef, authorizationCode:=authCode, storeName:=Me.StoreName, storeCode:=Me.StoreCode)

                If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
                    Dim payRef = resp.Data.PaymentRef
                    Dim statusStr = resp.Data.Status

                    Me.RedeemedResult = New RedeemedPointInfo With {
                        .PaymentRef = payRef,
                        .ProviderRef = providerRef,
                        .PointAmount = burnPt,
                        .OrderAmountIdr = orderAmountIdr,
                        .Status = statusStr
                    }

                    ' Catat ke tabel database [dbo].[GoappRedeemLog] dengan Status = 0 (Digunakan/Burn) dan StatusPos = 0 (Belum Masuk POS)
                    Try
                        Dim logItem As New GoappRedeemLogItem With {
                            .Waktu = DateTime.Now,
                            .Jenis = "BurnPoint",
                            .Kode = payRef,
                            .NamaVoucher = "GoappPoint",
                            .NilaiPoint = burnPt,
                            .Status = 0,
                            .StatusPos = 0,
                            .TransactionRef = providerRef,
                            .MemberUid = Me.MemberUid.ToString(),
                            .MemberName = Me.MemberNameText
                        }
                        Dim logTask = _client.RedeemRepo.InsertRedeemLogAsync(logItem)
                    Catch exLog As Exception
                        Console.WriteLine("Gagal menyimpan log burn point: " & exLog.Message)
                    End Try

                    lblStatus.Text = $"Burn point berhasil! Ref: {payRef}"
                    lblStatus.ForeColor = Color.FromArgb(25, 135, 84)

                    FormMessageBox.Show(Me, $"Penggunaan {burnPt:N0} poin berhasil!{Environment.NewLine}Payment Ref: {payRef}{Environment.NewLine}Diskon Rp {burnPt:N0} akan diterapkan ke transaksi.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    lblStatus.Text = "Gagal memproses burn point."
                    lblStatus.ForeColor = Color.FromArgb(220, 53, 69)
                    FormMessageBox.Show(Me, $"Gagal menggunakan poin member:{Environment.NewLine}{resp.Message}", "Gagal Burn Point", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            Catch ex As Exception
                lblStatus.Text = "Terjadi kesalahan sistem saat burn point."
                lblStatus.ForeColor = Color.Red
                FormMessageBox.Show(Me, $"Terjadi kesalahan sistem saat memanggil API Goapp:{Environment.NewLine}{ex.Message}", "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                SetProcessingState(False)
            End Try
        End Sub

        Private Sub SetProcessingState(isProcessing As Boolean)
            btnBurnPoint.Enabled = Not isProcessing
            btnBurnPoint.Text = If(isProcessing, "Memproses...", "⚡ GUNAKAN POIN")
            btnCancel.Enabled = Not isProcessing
            btnMaxPoint.Enabled = Not isProcessing
            pnlNumpad.Enabled = Not isProcessing
            txtBurnPoints.Enabled = Not isProcessing
            txtAuthCode.Enabled = Not isProcessing
            Cursor = If(isProcessing, Cursors.WaitCursor, Cursors.Default)
        End Sub
    End Class
End Namespace
