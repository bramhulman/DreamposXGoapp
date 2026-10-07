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
    Friend WithEvents tabRegister As System.Windows.Forms.TabPage
    Friend WithEvents tabMasterVoucher As System.Windows.Forms.TabPage

    ' Member Tab
    Friend WithEvents lblInputMember As System.Windows.Forms.Label
    Friend WithEvents txtInputMember As System.Windows.Forms.TextBox
    Friend WithEvents btnSearchMember As System.Windows.Forms.Button
    Friend WithEvents btnGoToRegister As System.Windows.Forms.Button
    Friend WithEvents grpMemberResult As System.Windows.Forms.GroupBox
    Friend WithEvents lblMemberName As System.Windows.Forms.Label
    Friend WithEvents lblMemberPhone As System.Windows.Forms.Label
    Friend WithEvents lblMemberLevel As System.Windows.Forms.Label
    Friend WithEvents lblMemberPoints As System.Windows.Forms.Label
    Friend WithEvents lblMemberRupiah As System.Windows.Forms.Label
    Friend WithEvents lblMemberReferral As System.Windows.Forms.Label
    Friend WithEvents lblMemberPromo As System.Windows.Forms.Label
    Friend WithEvents txtRawJson As System.Windows.Forms.TextBox

    ' Voucher Tab
    Friend WithEvents lblVoucherCode As System.Windows.Forms.Label
    Friend WithEvents txtVoucherCode As System.Windows.Forms.TextBox
    Friend WithEvents btnValidateVoucher As System.Windows.Forms.Button
    Friend WithEvents btnUseVoucher As System.Windows.Forms.Button
    Friend WithEvents btnCancelVoucher As System.Windows.Forms.Button
    Friend WithEvents txtVoucherTxRef As System.Windows.Forms.TextBox
    Friend WithEvents lblVoucherTxRef As System.Windows.Forms.Label
    Friend WithEvents lblVoucherStatus As System.Windows.Forms.Label
    Friend WithEvents txtVoucherRawJson As System.Windows.Forms.TextBox

    ' Transaction & Point Burn Tab
    Friend WithEvents lblBillTotal As System.Windows.Forms.Label
    Friend WithEvents txtBillTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblBurnPoint As System.Windows.Forms.Label
    Friend WithEvents txtBurnPoint As System.Windows.Forms.TextBox
    Friend WithEvents btnBurnPoint As System.Windows.Forms.Button
    Friend WithEvents btnCancelBurnPoint As System.Windows.Forms.Button
    Friend WithEvents btnPushSalesOrder As System.Windows.Forms.Button
    Friend WithEvents lblLastPaymentRef As System.Windows.Forms.Label
    Friend WithEvents txtOrderRawJson As System.Windows.Forms.TextBox

    ' Receipt Tab
    Friend WithEvents btnGenerateReceipt As System.Windows.Forms.Button
    Friend WithEvents txtReceiptPreview As System.Windows.Forms.TextBox
    Friend WithEvents picQrCode As System.Windows.Forms.PictureBox
    Friend WithEvents lblQrCode As System.Windows.Forms.Label

    ' Register Tab Controls
    Friend WithEvents grpRegForm As System.Windows.Forms.GroupBox
    Friend WithEvents lblRegFirstName As System.Windows.Forms.Label
    Friend WithEvents txtRegFirstName As System.Windows.Forms.TextBox
    Friend WithEvents lblRegLastName As System.Windows.Forms.Label
    Friend WithEvents txtRegLastName As System.Windows.Forms.TextBox
    Friend WithEvents lblRegMobile As System.Windows.Forms.Label
    Friend WithEvents txtRegMobile As System.Windows.Forms.TextBox
    Friend WithEvents lblRegEmail As System.Windows.Forms.Label
    Friend WithEvents txtRegEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblRegScheme As System.Windows.Forms.Label
    Friend WithEvents cmbRegScheme As System.Windows.Forms.ComboBox
    Friend WithEvents btnSubmitRegister As System.Windows.Forms.Button
    Friend WithEvents lblRegStatus As System.Windows.Forms.Label
    Friend WithEvents grpRegInfo As System.Windows.Forms.GroupBox
    Friend WithEvents txtRegNotes As System.Windows.Forms.TextBox

    ' Master Voucher & Void Tab Controls
    Friend WithEvents grpCreateDeal As System.Windows.Forms.GroupBox
    Friend WithEvents lblDealName As System.Windows.Forms.Label
    Friend WithEvents txtDealName As System.Windows.Forms.TextBox
    Friend WithEvents lblDealSku As System.Windows.Forms.Label
    Friend WithEvents txtDealSku As System.Windows.Forms.TextBox
    Friend WithEvents lblDealType As System.Windows.Forms.Label
    Friend WithEvents cmbDealType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDealAmount As System.Windows.Forms.Label
    Friend WithEvents txtDealAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblDealCode As System.Windows.Forms.Label
    Friend WithEvents txtDealCode As System.Windows.Forms.TextBox
    Friend WithEvents btnCreateDeal As System.Windows.Forms.Button
    Friend WithEvents lblCreateDealStatus As System.Windows.Forms.Label

    Friend WithEvents grpManageDeals As System.Windows.Forms.GroupBox
    Friend WithEvents btnLoadDeals As System.Windows.Forms.Button
    Friend WithEvents lstDeals As System.Windows.Forms.ListBox
    Friend WithEvents lblCancelVoucherTitle As System.Windows.Forms.Label
    Friend WithEvents lblCancelVoucherCode As System.Windows.Forms.Label
    Friend WithEvents txtCancelVoucherCode As System.Windows.Forms.TextBox
    Friend WithEvents lblCancelTxRef As System.Windows.Forms.Label
    Friend WithEvents txtCancelTxRef As System.Windows.Forms.TextBox
    Friend WithEvents btnSubmitCancelVoucher As System.Windows.Forms.Button
    Friend WithEvents lblCancelVoucherStatus As System.Windows.Forms.Label

    ' Log Console
    Friend WithEvents grpLogs As System.Windows.Forms.GroupBox
    Friend WithEvents txtLogs As System.Windows.Forms.TextBox
    Friend WithEvents btnClearLogs As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()>
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
        Me.tabRegister = New System.Windows.Forms.TabPage()
        Me.tabMasterVoucher = New System.Windows.Forms.TabPage()

        ' Member Controls
        Me.lblInputMember = New System.Windows.Forms.Label()
        Me.txtInputMember = New System.Windows.Forms.TextBox()
        Me.btnSearchMember = New System.Windows.Forms.Button()
        Me.btnGoToRegister = New System.Windows.Forms.Button()
        Me.grpMemberResult = New System.Windows.Forms.GroupBox()
        Me.lblMemberName = New System.Windows.Forms.Label()
        Me.lblMemberPhone = New System.Windows.Forms.Label()
        Me.lblMemberLevel = New System.Windows.Forms.Label()
        Me.lblMemberPoints = New System.Windows.Forms.Label()
        Me.lblMemberRupiah = New System.Windows.Forms.Label()
        Me.lblMemberReferral = New System.Windows.Forms.Label()
        Me.lblMemberPromo = New System.Windows.Forms.Label()
        Me.txtRawJson = New System.Windows.Forms.TextBox()

        ' Voucher Controls
        Me.lblVoucherCode = New System.Windows.Forms.Label()
        Me.txtVoucherCode = New System.Windows.Forms.TextBox()
        Me.btnValidateVoucher = New System.Windows.Forms.Button()
        Me.lblVoucherTxRef = New System.Windows.Forms.Label()
        Me.txtVoucherTxRef = New System.Windows.Forms.TextBox()
        Me.btnUseVoucher = New System.Windows.Forms.Button()
        Me.btnCancelVoucher = New System.Windows.Forms.Button()
        Me.lblVoucherStatus = New System.Windows.Forms.Label()
        Me.txtVoucherRawJson = New System.Windows.Forms.TextBox()

        ' Transaction Controls
        Me.lblBillTotal = New System.Windows.Forms.Label()
        Me.txtBillTotal = New System.Windows.Forms.TextBox()
        Me.lblBurnPoint = New System.Windows.Forms.Label()
        Me.txtBurnPoint = New System.Windows.Forms.TextBox()
        Me.btnBurnPoint = New System.Windows.Forms.Button()
        Me.btnCancelBurnPoint = New System.Windows.Forms.Button()
        Me.btnPushSalesOrder = New System.Windows.Forms.Button()
        Me.lblLastPaymentRef = New System.Windows.Forms.Label()
        Me.txtOrderRawJson = New System.Windows.Forms.TextBox()

        ' Receipt Controls
        Me.btnGenerateReceipt = New System.Windows.Forms.Button()
        Me.txtReceiptPreview = New System.Windows.Forms.TextBox()

        ' Register Controls
        Me.grpRegForm = New System.Windows.Forms.GroupBox()
        Me.lblRegFirstName = New System.Windows.Forms.Label()
        Me.txtRegFirstName = New System.Windows.Forms.TextBox()
        Me.lblRegLastName = New System.Windows.Forms.Label()
        Me.txtRegLastName = New System.Windows.Forms.TextBox()
        Me.lblRegMobile = New System.Windows.Forms.Label()
        Me.txtRegMobile = New System.Windows.Forms.TextBox()
        Me.lblRegEmail = New System.Windows.Forms.Label()
        Me.txtRegEmail = New System.Windows.Forms.TextBox()
        Me.lblRegScheme = New System.Windows.Forms.Label()
        Me.cmbRegScheme = New System.Windows.Forms.ComboBox()
        Me.btnSubmitRegister = New System.Windows.Forms.Button()
        Me.lblRegStatus = New System.Windows.Forms.Label()
        Me.grpRegInfo = New System.Windows.Forms.GroupBox()
        Me.txtRegNotes = New System.Windows.Forms.TextBox()

        ' Master Voucher & Void Controls
        Me.grpCreateDeal = New System.Windows.Forms.GroupBox()
        Me.lblDealName = New System.Windows.Forms.Label()
        Me.txtDealName = New System.Windows.Forms.TextBox()
        Me.lblDealSku = New System.Windows.Forms.Label()
        Me.txtDealSku = New System.Windows.Forms.TextBox()
        Me.lblDealType = New System.Windows.Forms.Label()
        Me.cmbDealType = New System.Windows.Forms.ComboBox()
        Me.lblDealAmount = New System.Windows.Forms.Label()
        Me.txtDealAmount = New System.Windows.Forms.TextBox()
        Me.lblDealCode = New System.Windows.Forms.Label()
        Me.txtDealCode = New System.Windows.Forms.TextBox()
        Me.btnCreateDeal = New System.Windows.Forms.Button()
        Me.lblCreateDealStatus = New System.Windows.Forms.Label()

        Me.grpManageDeals = New System.Windows.Forms.GroupBox()
        Me.btnLoadDeals = New System.Windows.Forms.Button()
        Me.lstDeals = New System.Windows.Forms.ListBox()
        Me.lblCancelVoucherTitle = New System.Windows.Forms.Label()
        Me.lblCancelVoucherCode = New System.Windows.Forms.Label()
        Me.txtCancelVoucherCode = New System.Windows.Forms.TextBox()
        Me.lblCancelTxRef = New System.Windows.Forms.Label()
        Me.txtCancelTxRef = New System.Windows.Forms.TextBox()
        Me.btnSubmitCancelVoucher = New System.Windows.Forms.Button()
        Me.lblCancelVoucherStatus = New System.Windows.Forms.Label()

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
        Me.txtApiKey.Text = ""

        Me.lblApiSecret.Text = "API Secret:"
        Me.lblApiSecret.Location = New System.Drawing.Point(215, 25)
        Me.lblApiSecret.AutoSize = True

        Me.txtApiSecret.Location = New System.Drawing.Point(280, 22)
        Me.txtApiSecret.Size = New System.Drawing.Size(260, 23)
        Me.txtApiSecret.Text = ""

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
        Me.tabControl.Controls.Add(Me.tabRegister)
        Me.tabControl.Controls.Add(Me.tabMasterVoucher)

        ' --- TAB 1: MEMBER ---
        Me.tabMember.Text = "1. Cek & Validasi Member"
        Me.tabMember.Controls.Add(Me.lblInputMember)
        Me.tabMember.Controls.Add(Me.txtInputMember)
        Me.tabMember.Controls.Add(Me.btnSearchMember)
        Me.tabMember.Controls.Add(Me.btnGoToRegister)
        Me.tabMember.Controls.Add(Me.grpMemberResult)
        Me.tabMember.Controls.Add(Me.txtRawJson)

        Me.lblInputMember.Text = "No HP / Member ID:"
        Me.lblInputMember.Location = New System.Drawing.Point(15, 20)
        Me.lblInputMember.AutoSize = True

        Me.txtInputMember.Location = New System.Drawing.Point(145, 17)
        Me.txtInputMember.Size = New System.Drawing.Size(180, 23)
        Me.txtInputMember.Text = "081588809090"
        'Me.txtInputMember.Text = "087890760858" '081588809090

        Me.btnSearchMember.Text = "Cek Member (Async)"
        Me.btnSearchMember.Location = New System.Drawing.Point(335, 15)
        Me.btnSearchMember.Size = New System.Drawing.Size(150, 27)
        Me.btnSearchMember.BackColor = System.Drawing.Color.LightSteelBlue

        Me.btnGoToRegister.Text = "+ Daftar Member Baru"
        Me.btnGoToRegister.Location = New System.Drawing.Point(495, 15)
        Me.btnGoToRegister.Size = New System.Drawing.Size(160, 27)
        Me.btnGoToRegister.BackColor = System.Drawing.Color.PaleGreen

        Me.grpMemberResult.Location = New System.Drawing.Point(15, 55)
        Me.grpMemberResult.Size = New System.Drawing.Size(430, 260)
        Me.grpMemberResult.Text = "Informasi Member Goapp"
        Me.grpMemberResult.Controls.Add(Me.lblMemberName)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPhone)
        Me.grpMemberResult.Controls.Add(Me.lblMemberLevel)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPoints)
        Me.grpMemberResult.Controls.Add(Me.lblMemberRupiah)
        Me.grpMemberResult.Controls.Add(Me.lblMemberReferral)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPromo)

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

        Me.lblMemberPromo.Text = "Promo/Benefit : -"
        Me.lblMemberPromo.Location = New System.Drawing.Point(15, 250)
        Me.lblMemberPromo.AutoSize = True
        Me.lblMemberPromo.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblMemberPromo.ForeColor = System.Drawing.Color.MediumVioletRed

        Me.txtRawJson.Location = New System.Drawing.Point(460, 55)
        Me.txtRawJson.Size = New System.Drawing.Size(480, 260)
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
        Me.tabVoucher.Controls.Add(Me.btnCancelVoucher)
        Me.tabVoucher.Controls.Add(Me.lblVoucherStatus)
        Me.tabVoucher.Controls.Add(Me.txtVoucherRawJson)

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

        Me.btnCancelVoucher.Text = "3. Batalkan Voucher (Void)"
        Me.btnCancelVoucher.Location = New System.Drawing.Point(625, 65)
        Me.btnCancelVoucher.Size = New System.Drawing.Size(180, 27)
        Me.btnCancelVoucher.BackColor = System.Drawing.Color.LightPink

        Me.lblVoucherStatus.Location = New System.Drawing.Point(20, 115)
        Me.lblVoucherStatus.Size = New System.Drawing.Size(700, 60)
        Me.lblVoucherStatus.Text = "Status Voucher: Menunggu aksi pengujian..."
        Me.lblVoucherStatus.Font = New System.Drawing.Font("Segoe UI", 9.5F)

        Me.txtVoucherRawJson.Location = New System.Drawing.Point(20, 180)
        Me.txtVoucherRawJson.Size = New System.Drawing.Size(920, 200)
        Me.txtVoucherRawJson.Multiline = True
        Me.txtVoucherRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtVoucherRawJson.ReadOnly = True
        Me.txtVoucherRawJson.Font = New System.Drawing.Font("Consolas", 8.5F)

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
        Me.tabTransaction.Controls.Add(Me.txtOrderRawJson)

        Me.lblBillTotal.Text = "Total Belanja (Rp):"
        Me.lblBillTotal.Location = New System.Drawing.Point(20, 30)
        Me.lblBillTotal.AutoSize = True

        Me.txtBillTotal.Location = New System.Drawing.Point(160, 27)
        Me.txtBillTotal.Size = New System.Drawing.Size(150, 23)
        Me.txtBillTotal.Text = "500000"

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
        Me.lblLastPaymentRef.Size = New System.Drawing.Size(700, 20)

        Me.txtOrderRawJson.Location = New System.Drawing.Point(20, 210)
        Me.txtOrderRawJson.Size = New System.Drawing.Size(900, 110)
        Me.txtOrderRawJson.Multiline = True
        Me.txtOrderRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtOrderRawJson.ReadOnly = True
        Me.txtOrderRawJson.Font = New System.Drawing.Font("Consolas", 8.5F)
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

        Me.lblQrCode = New System.Windows.Forms.Label()
        Me.lblQrCode.Text = "QR Code (Live Render):"
        Me.lblQrCode.Location = New System.Drawing.Point(460, 35)
        Me.lblQrCode.AutoSize = True

        Me.picQrCode = New System.Windows.Forms.PictureBox()
        Me.picQrCode.Location = New System.Drawing.Point(460, 55)
        Me.picQrCode.Size = New System.Drawing.Size(200, 200)
        Me.picQrCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picQrCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle

        Me.tabReceipt.Controls.Add(Me.lblQrCode)
        Me.tabReceipt.Controls.Add(Me.picQrCode)


        ' --- TAB 5: REGISTER MEMBER ---
        Me.tabRegister.Text = "5. Daftar Member Baru"
        Me.tabRegister.Controls.Add(Me.grpRegForm)
        Me.tabRegister.Controls.Add(Me.grpRegInfo)

        ' grpRegForm
        Me.grpRegForm.Text = "Formulir Registrasi Member Baru (CRM)"
        Me.grpRegForm.Location = New System.Drawing.Point(20, 15)
        Me.grpRegForm.Size = New System.Drawing.Size(510, 300)
        Me.grpRegForm.Controls.Add(Me.lblRegFirstName)
        Me.grpRegForm.Controls.Add(Me.txtRegFirstName)
        Me.grpRegForm.Controls.Add(Me.lblRegLastName)
        Me.grpRegForm.Controls.Add(Me.txtRegLastName)
        Me.grpRegForm.Controls.Add(Me.lblRegMobile)
        Me.grpRegForm.Controls.Add(Me.txtRegMobile)
        Me.grpRegForm.Controls.Add(Me.lblRegEmail)
        Me.grpRegForm.Controls.Add(Me.txtRegEmail)
        Me.grpRegForm.Controls.Add(Me.lblRegScheme)
        Me.grpRegForm.Controls.Add(Me.cmbRegScheme)
        Me.grpRegForm.Controls.Add(Me.btnSubmitRegister)
        Me.grpRegForm.Controls.Add(Me.lblRegStatus)

        Me.lblRegFirstName.Text = "Nama Depan (*):"
        Me.lblRegFirstName.Location = New System.Drawing.Point(20, 30)
        Me.lblRegFirstName.AutoSize = True

        Me.txtRegFirstName.Location = New System.Drawing.Point(160, 27)
        Me.txtRegFirstName.Size = New System.Drawing.Size(320, 23)

        Me.lblRegLastName.Text = "Nama Belakang:"
        Me.lblRegLastName.Location = New System.Drawing.Point(20, 68)
        Me.lblRegLastName.AutoSize = True

        Me.txtRegLastName.Location = New System.Drawing.Point(160, 65)
        Me.txtRegLastName.Size = New System.Drawing.Size(320, 23)

        Me.lblRegMobile.Text = "Nomor HP (*):"
        Me.lblRegMobile.Location = New System.Drawing.Point(20, 106)
        Me.lblRegMobile.AutoSize = True

        Me.txtRegMobile.Location = New System.Drawing.Point(160, 103)
        Me.txtRegMobile.Size = New System.Drawing.Size(320, 23)

        Me.lblRegEmail.Text = "Email (Opsional):"
        Me.lblRegEmail.Location = New System.Drawing.Point(20, 144)
        Me.lblRegEmail.AutoSize = True

        Me.txtRegEmail.Location = New System.Drawing.Point(160, 141)
        Me.txtRegEmail.Size = New System.Drawing.Size(320, 23)

        Me.lblRegScheme.Text = "Scheme / Level (*):"
        Me.lblRegScheme.Location = New System.Drawing.Point(20, 182)
        Me.lblRegScheme.AutoSize = True

        Me.cmbRegScheme.Location = New System.Drawing.Point(160, 179)
        Me.cmbRegScheme.Size = New System.Drawing.Size(320, 23)
        Me.cmbRegScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList

        Me.btnSubmitRegister.Text = "Simpan / Daftarkan Member (POST)"
        Me.btnSubmitRegister.Location = New System.Drawing.Point(160, 218)
        Me.btnSubmitRegister.Size = New System.Drawing.Size(320, 36)
        Me.btnSubmitRegister.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnSubmitRegister.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold)

        Me.lblRegStatus.Text = "Status: Siap mendaftarkan member..."
        Me.lblRegStatus.Location = New System.Drawing.Point(20, 265)
        Me.lblRegStatus.Size = New System.Drawing.Size(460, 25)
        Me.lblRegStatus.ForeColor = System.Drawing.Color.DarkSlateGray

        ' grpRegInfo
        Me.grpRegInfo.Text = "Petunjuk & Panduan POS"
        Me.grpRegInfo.Location = New System.Drawing.Point(545, 15)
        Me.grpRegInfo.Size = New System.Drawing.Size(395, 300)
        Me.grpRegInfo.Controls.Add(Me.txtRegNotes)

        Me.txtRegNotes.Location = New System.Drawing.Point(15, 25)
        Me.txtRegNotes.Size = New System.Drawing.Size(365, 260)
        Me.txtRegNotes.Multiline = True
        Me.txtRegNotes.ReadOnly = True
        Me.txtRegNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRegNotes.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        Me.txtRegNotes.Text = "CARA PENDAFTARAN MEMBER BARU:" & vbCrLf & vbCrLf &
            "1. Jika kasir mencari nomor HP member di Tab 1 dan tidak ditemukan, klik tombol '+ Daftar Member Baru' atau buka Tab 5 ini." & vbCrLf & vbCrLf &
            "2. Masukkan Nama Depan dan Nomor HP (wajib diisi)." & vbCrLf & vbCrLf &
            "3. Pilih Tier/Scheme membership (Default: Go Member)." & vbCrLf & vbCrLf &
            "4. Klik tombol 'Simpan / Daftarkan Member'." & vbCrLf & vbCrLf &
            "5. Setelah berhasil, data member langsung otomatis tersimpan di Goapp CRM dan aplikasi akan berpindah ke Tab 1 untuk memuat profil & saldo poin member secara instan."

        ' --- TAB 6: MASTER & SIKLUS VOUCHER ---
        Me.tabMasterVoucher.Text = "6. Master & Siklus Voucher"
        Me.tabMasterVoucher.Controls.Add(Me.grpCreateDeal)
        Me.tabMasterVoucher.Controls.Add(Me.grpManageDeals)

        ' grpCreateDeal
        Me.grpCreateDeal.Text = "Insert Master Promo / Voucher (POST /member/deal/)"
        Me.grpCreateDeal.Location = New System.Drawing.Point(15, 15)
        Me.grpCreateDeal.Size = New System.Drawing.Size(460, 300)
        Me.grpCreateDeal.Controls.Add(Me.lblDealName)
        Me.grpCreateDeal.Controls.Add(Me.txtDealName)
        Me.grpCreateDeal.Controls.Add(Me.lblDealSku)
        Me.grpCreateDeal.Controls.Add(Me.txtDealSku)
        Me.grpCreateDeal.Controls.Add(Me.lblDealType)
        Me.grpCreateDeal.Controls.Add(Me.cmbDealType)
        Me.grpCreateDeal.Controls.Add(Me.lblDealAmount)
        Me.grpCreateDeal.Controls.Add(Me.txtDealAmount)
        Me.grpCreateDeal.Controls.Add(Me.lblDealCode)
        Me.grpCreateDeal.Controls.Add(Me.txtDealCode)
        Me.grpCreateDeal.Controls.Add(Me.btnCreateDeal)
        Me.grpCreateDeal.Controls.Add(Me.lblCreateDealStatus)

        Me.lblDealName.Text = "Nama Promo / Voucher:"
        Me.lblDealName.Location = New System.Drawing.Point(15, 30)
        Me.lblDealName.AutoSize = True

        Me.txtDealName.Location = New System.Drawing.Point(170, 27)
        Me.txtDealName.Size = New System.Drawing.Size(265, 23)
        Me.txtDealName.Text = "Voucher Diskon Kasir POS"

        Me.lblDealSku.Text = "Reward SKU (Catalog):"
        Me.lblDealSku.Location = New System.Drawing.Point(15, 68)
        Me.lblDealSku.AutoSize = True

        Me.txtDealSku.Location = New System.Drawing.Point(170, 65)
        Me.txtDealSku.Size = New System.Drawing.Size(265, 23)
        Me.txtDealSku.Text = "NEWITEM100K"

        Me.lblDealType.Text = "Tipe Diskon:"
        Me.lblDealType.Location = New System.Drawing.Point(15, 106)
        Me.lblDealType.AutoSize = True

        Me.cmbDealType.Location = New System.Drawing.Point(170, 103)
        Me.cmbDealType.Size = New System.Drawing.Size(265, 23)
        Me.cmbDealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDealType.Items.AddRange(New Object() {"amount (Potongan Rp)", "percentage (Diskon %)", "free_item (Gratis Barang)"})
        Me.cmbDealType.SelectedIndex = 0

        Me.lblDealAmount.Text = "Nilai Diskon (Rp / %):"
        Me.lblDealAmount.Location = New System.Drawing.Point(15, 144)
        Me.lblDealAmount.AutoSize = True

        Me.txtDealAmount.Location = New System.Drawing.Point(170, 141)
        Me.txtDealAmount.Size = New System.Drawing.Size(265, 23)
        Me.txtDealAmount.Text = "15000"

        Me.lblDealCode.Text = "Kode Promo (Universal):"
        Me.lblDealCode.Location = New System.Drawing.Point(15, 182)
        Me.lblDealCode.AutoSize = True

        Me.txtDealCode.Location = New System.Drawing.Point(170, 179)
        Me.txtDealCode.Size = New System.Drawing.Size(265, 23)
        Me.txtDealCode.Text = "PROMO15K"

        Me.btnCreateDeal.Text = "Insert Master Voucher (POST)"
        Me.btnCreateDeal.Location = New System.Drawing.Point(170, 215)
        Me.btnCreateDeal.Size = New System.Drawing.Size(265, 35)
        Me.btnCreateDeal.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnCreateDeal.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)

        Me.lblCreateDealStatus.Text = "Catatan: Master promo dibuat di CRM Admin. Via API mewajibkan reward_sku valid di catalog."
        Me.lblCreateDealStatus.Location = New System.Drawing.Point(15, 260)
        Me.lblCreateDealStatus.Size = New System.Drawing.Size(425, 35)
        Me.lblCreateDealStatus.ForeColor = System.Drawing.Color.DarkSlateGray

        ' grpManageDeals
        Me.grpManageDeals.Text = "Daftar Promo Aktif & Void Voucher (cancel_code)"
        Me.grpManageDeals.Location = New System.Drawing.Point(490, 15)
        Me.grpManageDeals.Size = New System.Drawing.Size(450, 300)
        Me.grpManageDeals.Controls.Add(Me.btnLoadDeals)
        Me.grpManageDeals.Controls.Add(Me.lstDeals)
        Me.grpManageDeals.Controls.Add(Me.lblCancelVoucherTitle)
        Me.grpManageDeals.Controls.Add(Me.lblCancelVoucherCode)
        Me.grpManageDeals.Controls.Add(Me.txtCancelVoucherCode)
        Me.grpManageDeals.Controls.Add(Me.lblCancelTxRef)
        Me.grpManageDeals.Controls.Add(Me.txtCancelTxRef)
        Me.grpManageDeals.Controls.Add(Me.btnSubmitCancelVoucher)
        Me.grpManageDeals.Controls.Add(Me.lblCancelVoucherStatus)

        Me.btnLoadDeals.Text = "Muat Promo Aktif Toko (GET /deal/)"
        Me.btnLoadDeals.Location = New System.Drawing.Point(15, 22)
        Me.btnLoadDeals.Size = New System.Drawing.Size(420, 27)

        Me.lstDeals.Location = New System.Drawing.Point(15, 55)
        Me.lstDeals.Size = New System.Drawing.Size(420, 95)
        Me.lstDeals.Font = New System.Drawing.Font("Segoe UI", 8.5F)

        Me.lblCancelVoucherTitle.Text = "Batalkan Voucher yang Terpakai (Void Kupon):"
        Me.lblCancelVoucherTitle.Location = New System.Drawing.Point(15, 158)
        Me.lblCancelVoucherTitle.AutoSize = True
        Me.lblCancelVoucherTitle.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)

        Me.lblCancelVoucherCode.Text = "Kode Voucher:"
        Me.lblCancelVoucherCode.Location = New System.Drawing.Point(15, 185)
        Me.lblCancelVoucherCode.AutoSize = True

        Me.txtCancelVoucherCode.Location = New System.Drawing.Point(110, 182)
        Me.txtCancelVoucherCode.Size = New System.Drawing.Size(150, 23)

        Me.lblCancelTxRef.Text = "Tx Ref:"
        Me.lblCancelTxRef.Location = New System.Drawing.Point(265, 185)
        Me.lblCancelTxRef.AutoSize = True

        Me.txtCancelTxRef.Location = New System.Drawing.Point(310, 182)
        Me.txtCancelTxRef.Size = New System.Drawing.Size(125, 23)

        Me.btnSubmitCancelVoucher.Text = "Batalkan / Void Voucher (POST /cancel_code/)"
        Me.btnSubmitCancelVoucher.Location = New System.Drawing.Point(110, 215)
        Me.btnSubmitCancelVoucher.Size = New System.Drawing.Size(325, 30)
        Me.btnSubmitCancelVoucher.BackColor = System.Drawing.Color.LightPink

        Me.lblCancelVoucherStatus.Text = "Status: Siap membatalkan voucher terpakai..."
        Me.lblCancelVoucherStatus.Location = New System.Drawing.Point(15, 255)
        Me.lblCancelVoucherStatus.Size = New System.Drawing.Size(420, 30)

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
