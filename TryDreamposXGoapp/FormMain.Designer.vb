<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents grpAuth As System.Windows.Forms.GroupBox
    Friend WithEvents txtApiKey As System.Windows.Forms.TextBox
    Friend WithEvents lblApiKey As System.Windows.Forms.Label
    Friend WithEvents txtApiSecret As System.Windows.Forms.TextBox
    Friend WithEvents lblApiSecret As System.Windows.Forms.Label
    Friend WithEvents btnTestAuth As System.Windows.Forms.Button
    Friend WithEvents lblStatusChannel As System.Windows.Forms.Label

    Friend WithEvents tabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabMember As System.Windows.Forms.TabPage
    Friend WithEvents tabVoucher As System.Windows.Forms.TabPage
    Friend WithEvents tabTransaction As System.Windows.Forms.TabPage
    Friend WithEvents tabReceipt As System.Windows.Forms.TabPage

    ' Member Tab
    Friend WithEvents lblInputMember As System.Windows.Forms.Label
    Friend WithEvents txtInputMember As System.Windows.Forms.TextBox
    Friend WithEvents btnSearchMember As System.Windows.Forms.Button
    Friend WithEvents grpMemberResult As System.Windows.Forms.GroupBox
    Friend WithEvents lblMemberName As System.Windows.Forms.Label
    Friend WithEvents lblMemberPhone As System.Windows.Forms.Label
    Friend WithEvents lblMemberLevel As System.Windows.Forms.Label
    Friend WithEvents lblMemberPoints As System.Windows.Forms.Label
    Friend WithEvents lblMemberRupiah As System.Windows.Forms.Label
    Friend WithEvents lblMemberReferral As System.Windows.Forms.Label
    Friend WithEvents txtRawJson As System.Windows.Forms.TextBox

    ' Voucher Tab
    Friend WithEvents lblVoucherCode As System.Windows.Forms.Label
    Friend WithEvents txtVoucherCode As System.Windows.Forms.TextBox
    Friend WithEvents btnValidateVoucher As System.Windows.Forms.Button
    Friend WithEvents btnUseVoucher As System.Windows.Forms.Button
    Friend WithEvents txtVoucherTxRef As System.Windows.Forms.TextBox
    Friend WithEvents lblVoucherTxRef As System.Windows.Forms.Label
    Friend WithEvents lblVoucherStatus As System.Windows.Forms.Label

    ' Transaction & Point Burn Tab
    Friend WithEvents lblBillTotal As System.Windows.Forms.Label
    Friend WithEvents txtBillTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblBurnPoint As System.Windows.Forms.Label
    Friend WithEvents txtBurnPoint As System.Windows.Forms.TextBox
    Friend WithEvents btnBurnPoint As System.Windows.Forms.Button
    Friend WithEvents btnCancelBurnPoint As System.Windows.Forms.Button
    Friend WithEvents btnPushSalesOrder As System.Windows.Forms.Button
    Friend WithEvents lblLastPaymentRef As System.Windows.Forms.Label

    ' Receipt Tab
    Friend WithEvents btnGenerateReceipt As System.Windows.Forms.Button
    Friend WithEvents txtReceiptPreview As System.Windows.Forms.TextBox

    ' Log Console
    Friend WithEvents grpLogs As System.Windows.Forms.GroupBox
    Friend WithEvents txtLogs As System.Windows.Forms.TextBox
    Friend WithEvents btnClearLogs As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grpAuth = New System.Windows.Forms.GroupBox()
        Me.lblApiKey = New System.Windows.Forms.Label()
        Me.txtApiKey = New System.Windows.Forms.TextBox()
        Me.lblApiSecret = New System.Windows.Forms.Label()
        Me.txtApiSecret = New System.Windows.Forms.TextBox()
        Me.btnTestAuth = New System.Windows.Forms.Button()
        Me.lblStatusChannel = New System.Windows.Forms.Label()

        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabMember = New System.Windows.Forms.TabPage()
        Me.tabVoucher = New System.Windows.Forms.TabPage()
        Me.tabTransaction = New System.Windows.Forms.TabPage()
        Me.tabReceipt = New System.Windows.Forms.TabPage()

        ' Member Controls
        Me.lblInputMember = New System.Windows.Forms.Label()
        Me.txtInputMember = New System.Windows.Forms.TextBox()
        Me.btnSearchMember = New System.Windows.Forms.Button()
        Me.grpMemberResult = New System.Windows.Forms.GroupBox()
        Me.lblMemberName = New System.Windows.Forms.Label()
        Me.lblMemberPhone = New System.Windows.Forms.Label()
        Me.lblMemberLevel = New System.Windows.Forms.Label()
        Me.lblMemberPoints = New System.Windows.Forms.Label()
        Me.lblMemberRupiah = New System.Windows.Forms.Label()
        Me.lblMemberReferral = New System.Windows.Forms.Label()
        Me.txtRawJson = New System.Windows.Forms.TextBox()

        ' Voucher Controls
        Me.lblVoucherCode = New System.Windows.Forms.Label()
        Me.txtVoucherCode = New System.Windows.Forms.TextBox()
        Me.btnValidateVoucher = New System.Windows.Forms.Button()
        Me.lblVoucherTxRef = New System.Windows.Forms.Label()
        Me.txtVoucherTxRef = New System.Windows.Forms.TextBox()
        Me.btnUseVoucher = New System.Windows.Forms.Button()
        Me.lblVoucherStatus = New System.Windows.Forms.Label()

        ' Transaction Controls
        Me.lblBillTotal = New System.Windows.Forms.Label()
        Me.txtBillTotal = New System.Windows.Forms.TextBox()
        Me.lblBurnPoint = New System.Windows.Forms.Label()
        Me.txtBurnPoint = New System.Windows.Forms.TextBox()
        Me.btnBurnPoint = New System.Windows.Forms.Button()
        Me.btnCancelBurnPoint = New System.Windows.Forms.Button()
        Me.btnPushSalesOrder = New System.Windows.Forms.Button()
        Me.lblLastPaymentRef = New System.Windows.Forms.Label()

        ' Receipt Controls
        Me.btnGenerateReceipt = New System.Windows.Forms.Button()
        Me.txtReceiptPreview = New System.Windows.Forms.TextBox()

        ' Logs Controls
        Me.grpLogs = New System.Windows.Forms.GroupBox()
        Me.txtLogs = New System.Windows.Forms.TextBox()
        Me.btnClearLogs = New System.Windows.Forms.Button()

        Me.SuspendLayout()

        ' grpAuth
        Me.grpAuth.Controls.Add(Me.lblApiKey)
        Me.grpAuth.Controls.Add(Me.txtApiKey)
        Me.grpAuth.Controls.Add(Me.lblApiSecret)
        Me.grpAuth.Controls.Add(Me.txtApiSecret)
        Me.grpAuth.Controls.Add(Me.btnTestAuth)
        Me.grpAuth.Controls.Add(Me.lblStatusChannel)
        Me.grpAuth.Location = New System.Drawing.Point(12, 12)
        Me.grpAuth.Size = New System.Drawing.Size(960, 65)
        Me.grpAuth.Text = "Kredensial API Goapp"

        Me.lblApiKey.Text = "API Key:"
        Me.lblApiKey.Location = New System.Drawing.Point(12, 25)
        Me.lblApiKey.AutoSize = True

        Me.txtApiKey.Location = New System.Drawing.Point(65, 22)
        Me.txtApiKey.Size = New System.Drawing.Size(140, 23)
        Me.txtApiKey.Text = "138350315235400"

        Me.lblApiSecret.Text = "API Secret:"
        Me.lblApiSecret.Location = New System.Drawing.Point(215, 25)
        Me.lblApiSecret.AutoSize = True

        Me.txtApiSecret.Location = New System.Drawing.Point(280, 22)
        Me.txtApiSecret.Size = New System.Drawing.Size(260, 23)
        Me.txtApiSecret.Text = "cbf5b1c921b36bfb06ca988c6d98f608482c40f1"

        Me.btnTestAuth.Location = New System.Drawing.Point(550, 20)
        Me.btnTestAuth.Size = New System.Drawing.Size(120, 26)
        Me.btnTestAuth.Text = "Test Koneksi"

        Me.lblStatusChannel.Location = New System.Drawing.Point(680, 25)
        Me.lblStatusChannel.Size = New System.Drawing.Size(260, 20)
        Me.lblStatusChannel.Text = "Status: Belum Terkoneksi"
        Me.lblStatusChannel.ForeColor = System.Drawing.Color.DarkBlue

        ' tabControl
        Me.tabControl.Location = New System.Drawing.Point(12, 85)
        Me.tabControl.Size = New System.Drawing.Size(960, 360)
        Me.tabControl.Controls.Add(Me.tabMember)
        Me.tabControl.Controls.Add(Me.tabVoucher)
        Me.tabControl.Controls.Add(Me.tabTransaction)
        Me.tabControl.Controls.Add(Me.tabReceipt)

        ' --- TAB 1: MEMBER ---
        Me.tabMember.Text = "1. Cek & Validasi Member"
        Me.tabMember.Controls.Add(Me.lblInputMember)
        Me.tabMember.Controls.Add(Me.txtInputMember)
        Me.tabMember.Controls.Add(Me.btnSearchMember)
        Me.tabMember.Controls.Add(Me.grpMemberResult)
        Me.tabMember.Controls.Add(Me.txtRawJson)

        Me.lblInputMember.Text = "No HP / Member ID:"
        Me.lblInputMember.Location = New System.Drawing.Point(15, 20)
        Me.lblInputMember.AutoSize = True

        Me.txtInputMember.Location = New System.Drawing.Point(145, 17)
        Me.txtInputMember.Size = New System.Drawing.Size(200, 23)
        Me.txtInputMember.Text = "08159136224"

        Me.btnSearchMember.Text = "Cek Member (Async)"
        Me.btnSearchMember.Location = New System.Drawing.Point(360, 15)
        Me.btnSearchMember.Size = New System.Drawing.Size(160, 27)
        Me.btnSearchMember.BackColor = System.Drawing.Color.LightSteelBlue

        Me.grpMemberResult.Location = New System.Drawing.Point(15, 55)
        Me.grpMemberResult.Size = New System.Drawing.Size(430, 260)
        Me.grpMemberResult.Text = "Informasi Member Goapp"
        Me.grpMemberResult.Controls.Add(Me.lblMemberName)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPhone)
        Me.grpMemberResult.Controls.Add(Me.lblMemberLevel)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPoints)
        Me.grpMemberResult.Controls.Add(Me.lblMemberRupiah)
        Me.grpMemberResult.Controls.Add(Me.lblMemberReferral)

        Me.lblMemberName.Text = "Nama Member : -"
        Me.lblMemberName.Location = New System.Drawing.Point(15, 30)
        Me.lblMemberName.AutoSize = True
        Me.lblMemberName.Font = New System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Bold)

        Me.lblMemberPhone.Text = "No Handphone : -"
        Me.lblMemberPhone.Location = New System.Drawing.Point(15, 65)
        Me.lblMemberPhone.AutoSize = True

        Me.lblMemberLevel.Text = "Level / Scheme : -"
        Me.lblMemberLevel.Location = New System.Drawing.Point(15, 100)
        Me.lblMemberLevel.AutoSize = True

        Me.lblMemberPoints.Text = "Sisa Poin : 0 Pts"
        Me.lblMemberPoints.Location = New System.Drawing.Point(15, 135)
        Me.lblMemberPoints.AutoSize = True
        Me.lblMemberPoints.Font = New System.Drawing.Font("Segoe UI", 11.0F, System.Drawing.FontStyle.Bold)
        Me.lblMemberPoints.ForeColor = System.Drawing.Color.DarkGreen

        Me.lblMemberRupiah.Text = "Nilai Rupiah Poin : Rp 0"
        Me.lblMemberRupiah.Location = New System.Drawing.Point(15, 175)
        Me.lblMemberRupiah.AutoSize = True
        Me.lblMemberRupiah.Font = New System.Drawing.Font("Segoe UI", 11.0F, System.Drawing.FontStyle.Bold)
        Me.lblMemberRupiah.ForeColor = System.Drawing.Color.DarkBlue

        Me.lblMemberReferral.Text = "Referral Code : -"
        Me.lblMemberReferral.Location = New System.Drawing.Point(15, 215)
        Me.lblMemberReferral.AutoSize = True

        Me.txtRawJson.Location = New System.Drawing.Point(460, 15)
        Me.txtRawJson.Size = New System.Drawing.Size(480, 300)
        Me.txtRawJson.Multiline = True
        Me.txtRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtRawJson.ReadOnly = True
        Me.txtRawJson.Font = New System.Drawing.Font("Consolas", 8.5F)

        ' --- TAB 2: VOUCHER ---
        Me.tabVoucher.Text = "2. Validasi & Gunakan Voucher"
        Me.tabVoucher.Controls.Add(Me.lblVoucherCode)
        Me.tabVoucher.Controls.Add(Me.txtVoucherCode)
        Me.tabVoucher.Controls.Add(Me.btnValidateVoucher)
        Me.tabVoucher.Controls.Add(Me.lblVoucherTxRef)
        Me.tabVoucher.Controls.Add(Me.txtVoucherTxRef)
        Me.tabVoucher.Controls.Add(Me.btnUseVoucher)
        Me.tabVoucher.Controls.Add(Me.lblVoucherStatus)

        Me.lblVoucherCode.Text = "Kode Voucher / Deal:"
        Me.lblVoucherCode.Location = New System.Drawing.Point(20, 25)
        Me.lblVoucherCode.AutoSize = True

        Me.txtVoucherCode.Location = New System.Drawing.Point(160, 22)
        Me.txtVoucherCode.Size = New System.Drawing.Size(220, 23)
        Me.txtVoucherCode.Text = "ARKAIS-100-6DR6"

        Me.btnValidateVoucher.Text = "1. Validasi Voucher"
        Me.btnValidateVoucher.Location = New System.Drawing.Point(395, 20)
        Me.btnValidateVoucher.Size = New System.Drawing.Size(150, 26)

        Me.lblVoucherTxRef.Text = "No Struk / Tx Ref POS:"
        Me.lblVoucherTxRef.Location = New System.Drawing.Point(20, 70)
        Me.lblVoucherTxRef.AutoSize = True

        Me.txtVoucherTxRef.Location = New System.Drawing.Point(160, 67)
        Me.txtVoucherTxRef.Size = New System.Drawing.Size(220, 23)
        Me.txtVoucherTxRef.Text = "TRX-" & DateTime.Now.ToString("yyMMddHHmmss")

        Me.btnUseVoucher.Text = "2. Gunakan Voucher (Auto-Retry)"
        Me.btnUseVoucher.Location = New System.Drawing.Point(395, 65)
        Me.btnUseVoucher.Size = New System.Drawing.Size(220, 27)
        Me.btnUseVoucher.BackColor = System.Drawing.Color.PaleGreen

        Me.lblVoucherStatus.Location = New System.Drawing.Point(20, 115)
        Me.lblVoucherStatus.Size = New System.Drawing.Size(700, 60)
        Me.lblVoucherStatus.Text = "Status Voucher: Menunggu aksi pengujian..."
        Me.lblVoucherStatus.Font = New System.Drawing.Font("Segoe UI", 9.5F)

        ' --- TAB 3: TRANSACTION & POINT BURN ---
        Me.tabTransaction.Text = "3. Transaksi POS (Burn & Earn Point)"
        Me.tabTransaction.Controls.Add(Me.lblBillTotal)
        Me.tabTransaction.Controls.Add(Me.txtBillTotal)
        Me.tabTransaction.Controls.Add(Me.lblBurnPoint)
        Me.tabTransaction.Controls.Add(Me.txtBurnPoint)
        Me.tabTransaction.Controls.Add(Me.btnBurnPoint)
        Me.tabTransaction.Controls.Add(Me.btnCancelBurnPoint)
        Me.tabTransaction.Controls.Add(Me.btnPushSalesOrder)
        Me.tabTransaction.Controls.Add(Me.lblLastPaymentRef)

        Me.lblBillTotal.Text = "Total Belanja (Rp):"
        Me.lblBillTotal.Location = New System.Drawing.Point(20, 30)
        Me.lblBillTotal.AutoSize = True

        Me.txtBillTotal.Location = New System.Drawing.Point(160, 27)
        Me.txtBillTotal.Size = New System.Drawing.Size(150, 23)
        Me.txtBillTotal.Text = "50000"

        Me.lblBurnPoint.Text = "Poin Dibakar (Burn):"
        Me.lblBurnPoint.Location = New System.Drawing.Point(20, 75)
        Me.lblBurnPoint.AutoSize = True

        Me.txtBurnPoint.Location = New System.Drawing.Point(160, 72)
        Me.txtBurnPoint.Size = New System.Drawing.Size(150, 23)
        Me.txtBurnPoint.Text = "5"

        Me.btnBurnPoint.Text = "Burn Point (Potong Poin)"
        Me.btnBurnPoint.Location = New System.Drawing.Point(330, 70)
        Me.btnBurnPoint.Size = New System.Drawing.Size(180, 27)
        Me.btnBurnPoint.BackColor = System.Drawing.Color.SandyBrown

        Me.btnCancelBurnPoint.Text = "Cancel Burn Point (Void)"
        Me.btnCancelBurnPoint.Location = New System.Drawing.Point(520, 70)
        Me.btnCancelBurnPoint.Size = New System.Drawing.Size(180, 27)

        Me.btnPushSalesOrder.Text = "Push Transaksi Selesai (Earning Point)"
        Me.btnPushSalesOrder.Location = New System.Drawing.Point(160, 130)
        Me.btnPushSalesOrder.Size = New System.Drawing.Size(260, 32)
        Me.btnPushSalesOrder.BackColor = System.Drawing.Color.LightSkyBlue

        Me.lblLastPaymentRef.Location = New System.Drawing.Point(20, 180)
        Me.lblLastPaymentRef.Size = New System.Drawing.Size(700, 30)
        Me.lblLastPaymentRef.Text = "Last Payment Ref: -"

        ' --- TAB 4: RECEIPT ---
        Me.tabReceipt.Text = "4. Struk POS & Survey QR"
        Me.tabReceipt.Controls.Add(Me.btnGenerateReceipt)
        Me.tabReceipt.Controls.Add(Me.txtReceiptPreview)

        Me.btnGenerateReceipt.Text = "Generate Preview Struk (Point & Survey QR)"
        Me.btnGenerateReceipt.Location = New System.Drawing.Point(20, 15)
        Me.btnGenerateReceipt.Size = New System.Drawing.Size(300, 30)

        Me.txtReceiptPreview.Location = New System.Drawing.Point(20, 55)
        Me.txtReceiptPreview.Size = New System.Drawing.Size(420, 260)
        Me.txtReceiptPreview.Multiline = True
        Me.txtReceiptPreview.Font = New System.Drawing.Font("Courier New", 9.0F)
        Me.txtReceiptPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical

        ' grpLogs
        Me.grpLogs.Location = New System.Drawing.Point(12, 455)
        Me.grpLogs.Size = New System.Drawing.Size(960, 190)
        Me.grpLogs.Text = "Live Activity & Integration Logs"
        Me.grpLogs.Controls.Add(Me.txtLogs)
        Me.grpLogs.Controls.Add(Me.btnClearLogs)

        Me.txtLogs.Location = New System.Drawing.Point(15, 22)
        Me.txtLogs.Size = New System.Drawing.Size(840, 155)
        Me.txtLogs.Multiline = True
        Me.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLogs.Font = New System.Drawing.Font("Consolas", 8.5F)
        Me.txtLogs.ReadOnly = True

        Me.btnClearLogs.Text = "Clear Log"
        Me.btnClearLogs.Location = New System.Drawing.Point(865, 22)
        Me.btnClearLogs.Size = New System.Drawing.Size(80, 30)

        ' FormMain properties
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 655)
        Me.Controls.Add(Me.grpAuth)
        Me.Controls.Add(Me.tabControl)
        Me.Controls.Add(Me.grpLogs)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DreamPOS x Goapp CRM Integration Test Harness"
        Me.ResumeLayout(False)

    End Sub
End Class
