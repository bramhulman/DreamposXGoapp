<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
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

    Friend WithEvents txtRawJson As System.Windows.Forms.TextBox
    Friend WithEvents lstMemberPromos As System.Windows.Forms.ListBox

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.grpAuth = New System.Windows.Forms.GroupBox()
        Me.lblApiKey = New System.Windows.Forms.Label()
        Me.txtApiKey = New System.Windows.Forms.TextBox()
        Me.lblApiSecret = New System.Windows.Forms.Label()
        Me.txtApiSecret = New System.Windows.Forms.TextBox()
        Me.btnTestAuth = New System.Windows.Forms.Button()
        Me.lblStatusChannel = New System.Windows.Forms.Label()
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabMember = New System.Windows.Forms.TabPage()
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
        Me.lstMemberPromos = New System.Windows.Forms.ListBox()
        Me.txtRawJson = New System.Windows.Forms.TextBox()
        Me.tabVoucher = New System.Windows.Forms.TabPage()
        Me.lblVoucherCode = New System.Windows.Forms.Label()
        Me.txtVoucherCode = New System.Windows.Forms.TextBox()
        Me.btnValidateVoucher = New System.Windows.Forms.Button()
        Me.lblVoucherTxRef = New System.Windows.Forms.Label()
        Me.txtVoucherTxRef = New System.Windows.Forms.TextBox()
        Me.btnUseVoucher = New System.Windows.Forms.Button()
        Me.btnCancelVoucher = New System.Windows.Forms.Button()
        Me.lblVoucherStatus = New System.Windows.Forms.Label()
        Me.txtVoucherRawJson = New System.Windows.Forms.TextBox()
        Me.tabTransaction = New System.Windows.Forms.TabPage()
        Me.lblBillTotal = New System.Windows.Forms.Label()
        Me.txtBillTotal = New System.Windows.Forms.TextBox()
        Me.lblBurnPoint = New System.Windows.Forms.Label()
        Me.txtBurnPoint = New System.Windows.Forms.TextBox()
        Me.btnBurnPoint = New System.Windows.Forms.Button()
        Me.btnCancelBurnPoint = New System.Windows.Forms.Button()
        Me.btnPushSalesOrder = New System.Windows.Forms.Button()
        Me.lblLastPaymentRef = New System.Windows.Forms.Label()
        Me.txtOrderRawJson = New System.Windows.Forms.TextBox()
        Me.tabReceipt = New System.Windows.Forms.TabPage()
        Me.btnGenerateReceipt = New System.Windows.Forms.Button()
        Me.txtReceiptPreview = New System.Windows.Forms.TextBox()
        Me.lblQrCode = New System.Windows.Forms.Label()
        Me.picQrCode = New System.Windows.Forms.PictureBox()
        Me.tabRegister = New System.Windows.Forms.TabPage()
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
        Me.tabMasterVoucher = New System.Windows.Forms.TabPage()
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
        Me.grpLogs = New System.Windows.Forms.GroupBox()
        Me.txtLogs = New System.Windows.Forms.TextBox()
        Me.btnClearLogs = New System.Windows.Forms.Button()
        Me.grpAuth.SuspendLayout()
        Me.tabControl.SuspendLayout()
        Me.tabMember.SuspendLayout()
        Me.grpMemberResult.SuspendLayout()
        Me.tabVoucher.SuspendLayout()
        Me.tabTransaction.SuspendLayout()
        Me.tabReceipt.SuspendLayout()
        CType(Me.picQrCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabRegister.SuspendLayout()
        Me.grpRegForm.SuspendLayout()
        Me.grpRegInfo.SuspendLayout()
        Me.tabMasterVoucher.SuspendLayout()
        Me.grpCreateDeal.SuspendLayout()
        Me.grpManageDeals.SuspendLayout()
        Me.grpLogs.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAuth
        '
        Me.grpAuth.Controls.Add(Me.lblApiKey)
        Me.grpAuth.Controls.Add(Me.txtApiKey)
        Me.grpAuth.Controls.Add(Me.lblApiSecret)
        Me.grpAuth.Controls.Add(Me.txtApiSecret)
        Me.grpAuth.Controls.Add(Me.btnTestAuth)
        Me.grpAuth.Controls.Add(Me.lblStatusChannel)
        Me.grpAuth.Location = New System.Drawing.Point(12, 12)
        Me.grpAuth.Name = "grpAuth"
        Me.grpAuth.Size = New System.Drawing.Size(960, 65)
        Me.grpAuth.TabIndex = 0
        Me.grpAuth.TabStop = False
        Me.grpAuth.Text = "Kredensial API Goapp"
        '
        'lblApiKey
        '
        Me.lblApiKey.AutoSize = True
        Me.lblApiKey.Location = New System.Drawing.Point(12, 25)
        Me.lblApiKey.Name = "lblApiKey"
        Me.lblApiKey.Size = New System.Drawing.Size(62, 20)
        Me.lblApiKey.TabIndex = 0
        Me.lblApiKey.Text = "API Key:"
        '
        'txtApiKey
        '
        Me.txtApiKey.Location = New System.Drawing.Point(65, 22)
        Me.txtApiKey.Name = "txtApiKey"
        Me.txtApiKey.Size = New System.Drawing.Size(140, 27)
        Me.txtApiKey.TabIndex = 1
        '
        'lblApiSecret
        '
        Me.lblApiSecret.AutoSize = True
        Me.lblApiSecret.Location = New System.Drawing.Point(215, 25)
        Me.lblApiSecret.Name = "lblApiSecret"
        Me.lblApiSecret.Size = New System.Drawing.Size(79, 20)
        Me.lblApiSecret.TabIndex = 2
        Me.lblApiSecret.Text = "API Secret:"
        '
        'txtApiSecret
        '
        Me.txtApiSecret.Location = New System.Drawing.Point(280, 22)
        Me.txtApiSecret.Name = "txtApiSecret"
        Me.txtApiSecret.Size = New System.Drawing.Size(260, 27)
        Me.txtApiSecret.TabIndex = 3
        '
        'btnTestAuth
        '
        Me.btnTestAuth.Location = New System.Drawing.Point(550, 20)
        Me.btnTestAuth.Name = "btnTestAuth"
        Me.btnTestAuth.Size = New System.Drawing.Size(120, 26)
        Me.btnTestAuth.TabIndex = 4
        Me.btnTestAuth.Text = "Test Koneksi"
        '
        'lblStatusChannel
        '
        Me.lblStatusChannel.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblStatusChannel.Location = New System.Drawing.Point(680, 25)
        Me.lblStatusChannel.Name = "lblStatusChannel"
        Me.lblStatusChannel.Size = New System.Drawing.Size(260, 20)
        Me.lblStatusChannel.TabIndex = 5
        Me.lblStatusChannel.Text = "Status: Belum Terkoneksi"
        '
        'tabControl
        '
        Me.tabControl.Controls.Add(Me.tabMember)
        Me.tabControl.Controls.Add(Me.tabVoucher)
        Me.tabControl.Controls.Add(Me.tabTransaction)
        Me.tabControl.Controls.Add(Me.tabReceipt)
        Me.tabControl.Controls.Add(Me.tabRegister)
        Me.tabControl.Controls.Add(Me.tabMasterVoucher)
        Me.tabControl.Location = New System.Drawing.Point(12, 85)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(960, 480)
        Me.tabControl.TabIndex = 1
        '
        'tabMember
        '
        Me.tabMember.Controls.Add(Me.lblInputMember)
        Me.tabMember.Controls.Add(Me.txtInputMember)
        Me.tabMember.Controls.Add(Me.btnSearchMember)
        Me.tabMember.Controls.Add(Me.btnGoToRegister)
        Me.tabMember.Controls.Add(Me.grpMemberResult)
        Me.tabMember.Controls.Add(Me.txtRawJson)
        Me.tabMember.Location = New System.Drawing.Point(4, 29)
        Me.tabMember.Name = "tabMember"
        Me.tabMember.Size = New System.Drawing.Size(952, 447)
        Me.tabMember.TabIndex = 0
        Me.tabMember.Text = "1. Cek & Validasi Member"
        '
        'lblInputMember
        '
        Me.lblInputMember.AutoSize = True
        Me.lblInputMember.Location = New System.Drawing.Point(15, 20)
        Me.lblInputMember.Name = "lblInputMember"
        Me.lblInputMember.Size = New System.Drawing.Size(144, 20)
        Me.lblInputMember.TabIndex = 0
        Me.lblInputMember.Text = "No HP / Member ID:"
        '
        'txtInputMember
        '
        Me.txtInputMember.Location = New System.Drawing.Point(145, 17)
        Me.txtInputMember.Name = "txtInputMember"
        Me.txtInputMember.Size = New System.Drawing.Size(180, 27)
        Me.txtInputMember.TabIndex = 1
        Me.txtInputMember.Text = "081588809090"
        '
        'btnSearchMember
        '
        Me.btnSearchMember.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnSearchMember.Location = New System.Drawing.Point(335, 15)
        Me.btnSearchMember.Name = "btnSearchMember"
        Me.btnSearchMember.Size = New System.Drawing.Size(150, 27)
        Me.btnSearchMember.TabIndex = 2
        Me.btnSearchMember.Text = "Cek Member (Async)"
        Me.btnSearchMember.UseVisualStyleBackColor = False
        '
        'btnGoToRegister
        '
        Me.btnGoToRegister.BackColor = System.Drawing.Color.PaleGreen
        Me.btnGoToRegister.Location = New System.Drawing.Point(495, 15)
        Me.btnGoToRegister.Name = "btnGoToRegister"
        Me.btnGoToRegister.Size = New System.Drawing.Size(160, 27)
        Me.btnGoToRegister.TabIndex = 3
        Me.btnGoToRegister.Text = "+ Daftar Member Baru"
        Me.btnGoToRegister.UseVisualStyleBackColor = False
        '
        'grpMemberResult
        '
        Me.grpMemberResult.Controls.Add(Me.lblMemberName)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPhone)
        Me.grpMemberResult.Controls.Add(Me.lblMemberLevel)
        Me.grpMemberResult.Controls.Add(Me.lblMemberPoints)
        Me.grpMemberResult.Controls.Add(Me.lblMemberRupiah)
        Me.grpMemberResult.Controls.Add(Me.lblMemberReferral)
        Me.grpMemberResult.Controls.Add(Me.lstMemberPromos)
        Me.grpMemberResult.Location = New System.Drawing.Point(15, 55)
        Me.grpMemberResult.Name = "grpMemberResult"
        Me.grpMemberResult.Size = New System.Drawing.Size(430, 389)
        Me.grpMemberResult.TabIndex = 4
        Me.grpMemberResult.TabStop = False
        Me.grpMemberResult.Text = "Informasi Member Goapp"
        '
        'lblMemberName
        '
        Me.lblMemberName.AutoSize = True
        Me.lblMemberName.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMemberName.Location = New System.Drawing.Point(15, 30)
        Me.lblMemberName.Name = "lblMemberName"
        Me.lblMemberName.Size = New System.Drawing.Size(152, 23)
        Me.lblMemberName.TabIndex = 0
        Me.lblMemberName.Text = "Nama Member : -"
        '
        'lblMemberPhone
        '
        Me.lblMemberPhone.AutoSize = True
        Me.lblMemberPhone.Location = New System.Drawing.Point(15, 65)
        Me.lblMemberPhone.Name = "lblMemberPhone"
        Me.lblMemberPhone.Size = New System.Drawing.Size(128, 20)
        Me.lblMemberPhone.TabIndex = 1
        Me.lblMemberPhone.Text = "No Handphone : -"
        '
        'lblMemberLevel
        '
        Me.lblMemberLevel.AutoSize = True
        Me.lblMemberLevel.Location = New System.Drawing.Point(15, 92)
        Me.lblMemberLevel.Name = "lblMemberLevel"
        Me.lblMemberLevel.Size = New System.Drawing.Size(126, 20)
        Me.lblMemberLevel.TabIndex = 2
        Me.lblMemberLevel.Text = "Level / Scheme : -"
        '
        'lblMemberPoints
        '
        Me.lblMemberPoints.AutoSize = True
        Me.lblMemberPoints.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblMemberPoints.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblMemberPoints.Location = New System.Drawing.Point(15, 118)
        Me.lblMemberPoints.Name = "lblMemberPoints"
        Me.lblMemberPoints.Size = New System.Drawing.Size(149, 25)
        Me.lblMemberPoints.TabIndex = 3
        Me.lblMemberPoints.Text = "Sisa Poin : 0 Pts"
        '
        'lblMemberRupiah
        '
        Me.lblMemberRupiah.AutoSize = True
        Me.lblMemberRupiah.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblMemberRupiah.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblMemberRupiah.Location = New System.Drawing.Point(15, 150)
        Me.lblMemberRupiah.Name = "lblMemberRupiah"
        Me.lblMemberRupiah.Size = New System.Drawing.Size(219, 25)
        Me.lblMemberRupiah.TabIndex = 4
        Me.lblMemberRupiah.Text = "Nilai Rupiah Poin : Rp 0"
        '
        'lblMemberReferral
        '
        Me.lblMemberReferral.AutoSize = True
        Me.lblMemberReferral.Location = New System.Drawing.Point(15, 181)
        Me.lblMemberReferral.Name = "lblMemberReferral"
        Me.lblMemberReferral.Size = New System.Drawing.Size(117, 20)
        Me.lblMemberReferral.TabIndex = 5
        Me.lblMemberReferral.Text = "Referral Code : -"
        '
        'lstMemberPromos
        '
        Me.lstMemberPromos.FormattingEnabled = True
        Me.lstMemberPromos.ItemHeight = 20
        Me.lstMemberPromos.Location = New System.Drawing.Point(15, 204)
        Me.lstMemberPromos.Name = "lstMemberPromos"
        Me.lstMemberPromos.Size = New System.Drawing.Size(400, 184)
        Me.lstMemberPromos.TabIndex = 6
        '
        'txtRawJson
        '
        Me.txtRawJson.Font = New System.Drawing.Font("Consolas", 8.5!)
        Me.txtRawJson.Location = New System.Drawing.Point(460, 55)
        Me.txtRawJson.Multiline = True
        Me.txtRawJson.Name = "txtRawJson"
        Me.txtRawJson.ReadOnly = True
        Me.txtRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtRawJson.Size = New System.Drawing.Size(480, 389)
        Me.txtRawJson.TabIndex = 5
        '
        'tabVoucher
        '
        Me.tabVoucher.Controls.Add(Me.lblVoucherCode)
        Me.tabVoucher.Controls.Add(Me.txtVoucherCode)
        Me.tabVoucher.Controls.Add(Me.btnValidateVoucher)
        Me.tabVoucher.Controls.Add(Me.lblVoucherTxRef)
        Me.tabVoucher.Controls.Add(Me.txtVoucherTxRef)
        Me.tabVoucher.Controls.Add(Me.btnUseVoucher)
        Me.tabVoucher.Controls.Add(Me.btnCancelVoucher)
        Me.tabVoucher.Controls.Add(Me.lblVoucherStatus)
        Me.tabVoucher.Controls.Add(Me.txtVoucherRawJson)
        Me.tabVoucher.Location = New System.Drawing.Point(4, 29)
        Me.tabVoucher.Name = "tabVoucher"
        Me.tabVoucher.Size = New System.Drawing.Size(952, 447)
        Me.tabVoucher.TabIndex = 1
        Me.tabVoucher.Text = "2. Validasi & Gunakan Voucher"
        '
        'lblVoucherCode
        '
        Me.lblVoucherCode.AutoSize = True
        Me.lblVoucherCode.Location = New System.Drawing.Point(20, 25)
        Me.lblVoucherCode.Name = "lblVoucherCode"
        Me.lblVoucherCode.Size = New System.Drawing.Size(149, 20)
        Me.lblVoucherCode.TabIndex = 0
        Me.lblVoucherCode.Text = "Kode Voucher / Deal:"
        '
        'txtVoucherCode
        '
        Me.txtVoucherCode.Location = New System.Drawing.Point(160, 22)
        Me.txtVoucherCode.Name = "txtVoucherCode"
        Me.txtVoucherCode.Size = New System.Drawing.Size(220, 27)
        Me.txtVoucherCode.TabIndex = 1
        Me.txtVoucherCode.Text = "ARKAIS-100-6DR6"
        '
        'btnValidateVoucher
        '
        Me.btnValidateVoucher.Location = New System.Drawing.Point(395, 20)
        Me.btnValidateVoucher.Name = "btnValidateVoucher"
        Me.btnValidateVoucher.Size = New System.Drawing.Size(150, 26)
        Me.btnValidateVoucher.TabIndex = 2
        Me.btnValidateVoucher.Text = "1. Validasi Voucher"
        '
        'lblVoucherTxRef
        '
        Me.lblVoucherTxRef.AutoSize = True
        Me.lblVoucherTxRef.Location = New System.Drawing.Point(20, 70)
        Me.lblVoucherTxRef.Name = "lblVoucherTxRef"
        Me.lblVoucherTxRef.Size = New System.Drawing.Size(154, 20)
        Me.lblVoucherTxRef.TabIndex = 3
        Me.lblVoucherTxRef.Text = "No Struk / Tx Ref POS:"
        '
        'txtVoucherTxRef
        '
        Me.txtVoucherTxRef.Location = New System.Drawing.Point(160, 67)
        Me.txtVoucherTxRef.Name = "txtVoucherTxRef"
        Me.txtVoucherTxRef.Size = New System.Drawing.Size(220, 27)
        Me.txtVoucherTxRef.TabIndex = 4
        Me.txtVoucherTxRef.Text = "TRX-261008142730"
        '
        'btnUseVoucher
        '
        Me.btnUseVoucher.BackColor = System.Drawing.Color.PaleGreen
        Me.btnUseVoucher.Location = New System.Drawing.Point(395, 65)
        Me.btnUseVoucher.Name = "btnUseVoucher"
        Me.btnUseVoucher.Size = New System.Drawing.Size(220, 27)
        Me.btnUseVoucher.TabIndex = 5
        Me.btnUseVoucher.Text = "2. Gunakan Voucher (Auto-Retry)"
        Me.btnUseVoucher.UseVisualStyleBackColor = False
        '
        'btnCancelVoucher
        '
        Me.btnCancelVoucher.BackColor = System.Drawing.Color.LightPink
        Me.btnCancelVoucher.Location = New System.Drawing.Point(625, 65)
        Me.btnCancelVoucher.Name = "btnCancelVoucher"
        Me.btnCancelVoucher.Size = New System.Drawing.Size(180, 27)
        Me.btnCancelVoucher.TabIndex = 6
        Me.btnCancelVoucher.Text = "3. Batalkan Voucher (Void)"
        Me.btnCancelVoucher.UseVisualStyleBackColor = False
        '
        'lblVoucherStatus
        '
        Me.lblVoucherStatus.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblVoucherStatus.Location = New System.Drawing.Point(20, 110)
        Me.lblVoucherStatus.Name = "lblVoucherStatus"
        Me.lblVoucherStatus.Size = New System.Drawing.Size(700, 105)
        Me.lblVoucherStatus.TabIndex = 7
        Me.lblVoucherStatus.Text = "Status Voucher: Menunggu aksi pengujian..."
        '
        'txtVoucherRawJson
        '
        Me.txtVoucherRawJson.Font = New System.Drawing.Font("Consolas", 8.5!)
        Me.txtVoucherRawJson.Location = New System.Drawing.Point(20, 220)
        Me.txtVoucherRawJson.Multiline = True
        Me.txtVoucherRawJson.Name = "txtVoucherRawJson"
        Me.txtVoucherRawJson.ReadOnly = True
        Me.txtVoucherRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtVoucherRawJson.Size = New System.Drawing.Size(920, 160)
        Me.txtVoucherRawJson.TabIndex = 8
        '
        'tabTransaction
        '
        Me.tabTransaction.Controls.Add(Me.lblBillTotal)
        Me.tabTransaction.Controls.Add(Me.txtBillTotal)
        Me.tabTransaction.Controls.Add(Me.lblBurnPoint)
        Me.tabTransaction.Controls.Add(Me.txtBurnPoint)
        Me.tabTransaction.Controls.Add(Me.btnBurnPoint)
        Me.tabTransaction.Controls.Add(Me.btnCancelBurnPoint)
        Me.tabTransaction.Controls.Add(Me.btnPushSalesOrder)
        Me.tabTransaction.Controls.Add(Me.lblLastPaymentRef)
        Me.tabTransaction.Controls.Add(Me.txtOrderRawJson)
        Me.tabTransaction.Location = New System.Drawing.Point(4, 29)
        Me.tabTransaction.Name = "tabTransaction"
        Me.tabTransaction.Size = New System.Drawing.Size(952, 447)
        Me.tabTransaction.TabIndex = 2
        Me.tabTransaction.Text = "3. Transaksi POS (Burn & Earn Point)"
        '
        'lblBillTotal
        '
        Me.lblBillTotal.AutoSize = True
        Me.lblBillTotal.Location = New System.Drawing.Point(20, 30)
        Me.lblBillTotal.Name = "lblBillTotal"
        Me.lblBillTotal.Size = New System.Drawing.Size(130, 20)
        Me.lblBillTotal.TabIndex = 0
        Me.lblBillTotal.Text = "Total Belanja (Rp):"
        '
        'txtBillTotal
        '
        Me.txtBillTotal.Location = New System.Drawing.Point(160, 27)
        Me.txtBillTotal.Name = "txtBillTotal"
        Me.txtBillTotal.Size = New System.Drawing.Size(150, 27)
        Me.txtBillTotal.TabIndex = 1
        Me.txtBillTotal.Text = "500000"
        '
        'lblBurnPoint
        '
        Me.lblBurnPoint.AutoSize = True
        Me.lblBurnPoint.Location = New System.Drawing.Point(20, 75)
        Me.lblBurnPoint.Name = "lblBurnPoint"
        Me.lblBurnPoint.Size = New System.Drawing.Size(140, 20)
        Me.lblBurnPoint.TabIndex = 2
        Me.lblBurnPoint.Text = "Poin Dibakar (Burn):"
        '
        'txtBurnPoint
        '
        Me.txtBurnPoint.Location = New System.Drawing.Point(160, 72)
        Me.txtBurnPoint.Name = "txtBurnPoint"
        Me.txtBurnPoint.Size = New System.Drawing.Size(150, 27)
        Me.txtBurnPoint.TabIndex = 3
        Me.txtBurnPoint.Text = "5"
        '
        'btnBurnPoint
        '
        Me.btnBurnPoint.BackColor = System.Drawing.Color.SandyBrown
        Me.btnBurnPoint.Location = New System.Drawing.Point(330, 70)
        Me.btnBurnPoint.Name = "btnBurnPoint"
        Me.btnBurnPoint.Size = New System.Drawing.Size(180, 27)
        Me.btnBurnPoint.TabIndex = 4
        Me.btnBurnPoint.Text = "Burn Point (Potong Poin)"
        Me.btnBurnPoint.UseVisualStyleBackColor = False
        '
        'btnCancelBurnPoint
        '
        Me.btnCancelBurnPoint.Location = New System.Drawing.Point(520, 70)
        Me.btnCancelBurnPoint.Name = "btnCancelBurnPoint"
        Me.btnCancelBurnPoint.Size = New System.Drawing.Size(180, 27)
        Me.btnCancelBurnPoint.TabIndex = 5
        Me.btnCancelBurnPoint.Text = "Cancel Burn Point (Void)"
        '
        'btnPushSalesOrder
        '
        Me.btnPushSalesOrder.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnPushSalesOrder.Location = New System.Drawing.Point(160, 130)
        Me.btnPushSalesOrder.Name = "btnPushSalesOrder"
        Me.btnPushSalesOrder.Size = New System.Drawing.Size(260, 32)
        Me.btnPushSalesOrder.TabIndex = 6
        Me.btnPushSalesOrder.Text = "Push Transaksi Selesai (Earning Point)"
        Me.btnPushSalesOrder.UseVisualStyleBackColor = False
        '
        'lblLastPaymentRef
        '
        Me.lblLastPaymentRef.Location = New System.Drawing.Point(20, 180)
        Me.lblLastPaymentRef.Name = "lblLastPaymentRef"
        Me.lblLastPaymentRef.Size = New System.Drawing.Size(700, 20)
        Me.lblLastPaymentRef.TabIndex = 7
        Me.lblLastPaymentRef.Text = "Last Payment Ref: -"
        '
        'txtOrderRawJson
        '
        Me.txtOrderRawJson.Font = New System.Drawing.Font("Consolas", 8.5!)
        Me.txtOrderRawJson.Location = New System.Drawing.Point(20, 210)
        Me.txtOrderRawJson.Multiline = True
        Me.txtOrderRawJson.Name = "txtOrderRawJson"
        Me.txtOrderRawJson.ReadOnly = True
        Me.txtOrderRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtOrderRawJson.Size = New System.Drawing.Size(900, 110)
        Me.txtOrderRawJson.TabIndex = 8
        '
        'tabReceipt
        '
        Me.tabReceipt.Controls.Add(Me.btnGenerateReceipt)
        Me.tabReceipt.Controls.Add(Me.txtReceiptPreview)
        Me.tabReceipt.Controls.Add(Me.lblQrCode)
        Me.tabReceipt.Controls.Add(Me.picQrCode)
        Me.tabReceipt.Location = New System.Drawing.Point(4, 29)
        Me.tabReceipt.Name = "tabReceipt"
        Me.tabReceipt.Size = New System.Drawing.Size(952, 447)
        Me.tabReceipt.TabIndex = 3
        Me.tabReceipt.Text = "4. Struk POS & Survey QR"
        '
        'btnGenerateReceipt
        '
        Me.btnGenerateReceipt.Location = New System.Drawing.Point(20, 15)
        Me.btnGenerateReceipt.Name = "btnGenerateReceipt"
        Me.btnGenerateReceipt.Size = New System.Drawing.Size(300, 30)
        Me.btnGenerateReceipt.TabIndex = 0
        Me.btnGenerateReceipt.Text = "Generate Preview Struk (Point & Survey QR)"
        '
        'txtReceiptPreview
        '
        Me.txtReceiptPreview.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.txtReceiptPreview.Location = New System.Drawing.Point(20, 55)
        Me.txtReceiptPreview.Multiline = True
        Me.txtReceiptPreview.Name = "txtReceiptPreview"
        Me.txtReceiptPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtReceiptPreview.Size = New System.Drawing.Size(420, 260)
        Me.txtReceiptPreview.TabIndex = 1
        '
        'lblQrCode
        '
        Me.lblQrCode.AutoSize = True
        Me.lblQrCode.Location = New System.Drawing.Point(460, 35)
        Me.lblQrCode.Name = "lblQrCode"
        Me.lblQrCode.Size = New System.Drawing.Size(162, 20)
        Me.lblQrCode.TabIndex = 2
        Me.lblQrCode.Text = "QR Code (Live Render):"
        '
        'picQrCode
        '
        Me.picQrCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picQrCode.Location = New System.Drawing.Point(460, 55)
        Me.picQrCode.Name = "picQrCode"
        Me.picQrCode.Size = New System.Drawing.Size(200, 200)
        Me.picQrCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picQrCode.TabIndex = 3
        Me.picQrCode.TabStop = False
        '
        'tabRegister
        '
        Me.tabRegister.Controls.Add(Me.grpRegForm)
        Me.tabRegister.Controls.Add(Me.grpRegInfo)
        Me.tabRegister.Location = New System.Drawing.Point(4, 29)
        Me.tabRegister.Name = "tabRegister"
        Me.tabRegister.Size = New System.Drawing.Size(952, 447)
        Me.tabRegister.TabIndex = 4
        Me.tabRegister.Text = "5. Daftar Member Baru"
        '
        'grpRegForm
        '
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
        Me.grpRegForm.Location = New System.Drawing.Point(20, 15)
        Me.grpRegForm.Name = "grpRegForm"
        Me.grpRegForm.Size = New System.Drawing.Size(510, 300)
        Me.grpRegForm.TabIndex = 0
        Me.grpRegForm.TabStop = False
        Me.grpRegForm.Text = "Formulir Registrasi Member Baru (CRM)"
        '
        'lblRegFirstName
        '
        Me.lblRegFirstName.AutoSize = True
        Me.lblRegFirstName.Location = New System.Drawing.Point(20, 30)
        Me.lblRegFirstName.Name = "lblRegFirstName"
        Me.lblRegFirstName.Size = New System.Drawing.Size(120, 20)
        Me.lblRegFirstName.TabIndex = 0
        Me.lblRegFirstName.Text = "Nama Depan (*):"
        '
        'txtRegFirstName
        '
        Me.txtRegFirstName.Location = New System.Drawing.Point(160, 27)
        Me.txtRegFirstName.Name = "txtRegFirstName"
        Me.txtRegFirstName.Size = New System.Drawing.Size(320, 27)
        Me.txtRegFirstName.TabIndex = 1
        '
        'lblRegLastName
        '
        Me.lblRegLastName.AutoSize = True
        Me.lblRegLastName.Location = New System.Drawing.Point(20, 68)
        Me.lblRegLastName.Name = "lblRegLastName"
        Me.lblRegLastName.Size = New System.Drawing.Size(117, 20)
        Me.lblRegLastName.TabIndex = 2
        Me.lblRegLastName.Text = "Nama Belakang:"
        '
        'txtRegLastName
        '
        Me.txtRegLastName.Location = New System.Drawing.Point(160, 65)
        Me.txtRegLastName.Name = "txtRegLastName"
        Me.txtRegLastName.Size = New System.Drawing.Size(320, 27)
        Me.txtRegLastName.TabIndex = 3
        '
        'lblRegMobile
        '
        Me.lblRegMobile.AutoSize = True
        Me.lblRegMobile.Location = New System.Drawing.Point(20, 106)
        Me.lblRegMobile.Name = "lblRegMobile"
        Me.lblRegMobile.Size = New System.Drawing.Size(102, 20)
        Me.lblRegMobile.TabIndex = 4
        Me.lblRegMobile.Text = "Nomor HP (*):"
        '
        'txtRegMobile
        '
        Me.txtRegMobile.Location = New System.Drawing.Point(160, 103)
        Me.txtRegMobile.Name = "txtRegMobile"
        Me.txtRegMobile.Size = New System.Drawing.Size(320, 27)
        Me.txtRegMobile.TabIndex = 5
        '
        'lblRegEmail
        '
        Me.lblRegEmail.AutoSize = True
        Me.lblRegEmail.Location = New System.Drawing.Point(20, 144)
        Me.lblRegEmail.Name = "lblRegEmail"
        Me.lblRegEmail.Size = New System.Drawing.Size(122, 20)
        Me.lblRegEmail.TabIndex = 6
        Me.lblRegEmail.Text = "Email (Opsional):"
        '
        'txtRegEmail
        '
        Me.txtRegEmail.Location = New System.Drawing.Point(160, 141)
        Me.txtRegEmail.Name = "txtRegEmail"
        Me.txtRegEmail.Size = New System.Drawing.Size(320, 27)
        Me.txtRegEmail.TabIndex = 7
        '
        'lblRegScheme
        '
        Me.lblRegScheme.AutoSize = True
        Me.lblRegScheme.Location = New System.Drawing.Point(20, 182)
        Me.lblRegScheme.Name = "lblRegScheme"
        Me.lblRegScheme.Size = New System.Drawing.Size(132, 20)
        Me.lblRegScheme.TabIndex = 8
        Me.lblRegScheme.Text = "Scheme / Level (*):"
        '
        'cmbRegScheme
        '
        Me.cmbRegScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRegScheme.Location = New System.Drawing.Point(160, 179)
        Me.cmbRegScheme.Name = "cmbRegScheme"
        Me.cmbRegScheme.Size = New System.Drawing.Size(320, 28)
        Me.cmbRegScheme.TabIndex = 9
        '
        'btnSubmitRegister
        '
        Me.btnSubmitRegister.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnSubmitRegister.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.btnSubmitRegister.Location = New System.Drawing.Point(160, 218)
        Me.btnSubmitRegister.Name = "btnSubmitRegister"
        Me.btnSubmitRegister.Size = New System.Drawing.Size(320, 36)
        Me.btnSubmitRegister.TabIndex = 10
        Me.btnSubmitRegister.Text = "Simpan / Daftarkan Member (POST)"
        Me.btnSubmitRegister.UseVisualStyleBackColor = False
        '
        'lblRegStatus
        '
        Me.lblRegStatus.ForeColor = System.Drawing.Color.DarkSlateGray
        Me.lblRegStatus.Location = New System.Drawing.Point(20, 265)
        Me.lblRegStatus.Name = "lblRegStatus"
        Me.lblRegStatus.Size = New System.Drawing.Size(460, 25)
        Me.lblRegStatus.TabIndex = 11
        Me.lblRegStatus.Text = "Status: Siap mendaftarkan member..."
        '
        'grpRegInfo
        '
        Me.grpRegInfo.Controls.Add(Me.txtRegNotes)
        Me.grpRegInfo.Location = New System.Drawing.Point(545, 15)
        Me.grpRegInfo.Name = "grpRegInfo"
        Me.grpRegInfo.Size = New System.Drawing.Size(395, 300)
        Me.grpRegInfo.TabIndex = 1
        Me.grpRegInfo.TabStop = False
        Me.grpRegInfo.Text = "Petunjuk & Panduan POS"
        '
        'txtRegNotes
        '
        Me.txtRegNotes.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtRegNotes.Location = New System.Drawing.Point(15, 25)
        Me.txtRegNotes.Multiline = True
        Me.txtRegNotes.Name = "txtRegNotes"
        Me.txtRegNotes.ReadOnly = True
        Me.txtRegNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRegNotes.Size = New System.Drawing.Size(365, 260)
        Me.txtRegNotes.TabIndex = 0
        Me.txtRegNotes.Text = resources.GetString("txtRegNotes.Text")
        '
        'tabMasterVoucher
        '
        Me.tabMasterVoucher.Controls.Add(Me.grpCreateDeal)
        Me.tabMasterVoucher.Controls.Add(Me.grpManageDeals)
        Me.tabMasterVoucher.Location = New System.Drawing.Point(4, 29)
        Me.tabMasterVoucher.Name = "tabMasterVoucher"
        Me.tabMasterVoucher.Size = New System.Drawing.Size(952, 447)
        Me.tabMasterVoucher.TabIndex = 5
        Me.tabMasterVoucher.Text = "6. Master & Siklus Voucher"
        '
        'grpCreateDeal
        '
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
        Me.grpCreateDeal.Location = New System.Drawing.Point(15, 15)
        Me.grpCreateDeal.Name = "grpCreateDeal"
        Me.grpCreateDeal.Size = New System.Drawing.Size(460, 300)
        Me.grpCreateDeal.TabIndex = 0
        Me.grpCreateDeal.TabStop = False
        Me.grpCreateDeal.Text = "Insert Master Promo / Voucher (POST /member/deal/)"
        '
        'lblDealName
        '
        Me.lblDealName.AutoSize = True
        Me.lblDealName.Location = New System.Drawing.Point(15, 30)
        Me.lblDealName.Name = "lblDealName"
        Me.lblDealName.Size = New System.Drawing.Size(167, 20)
        Me.lblDealName.TabIndex = 0
        Me.lblDealName.Text = "Nama Promo / Voucher:"
        '
        'txtDealName
        '
        Me.txtDealName.Location = New System.Drawing.Point(170, 27)
        Me.txtDealName.Name = "txtDealName"
        Me.txtDealName.Size = New System.Drawing.Size(265, 27)
        Me.txtDealName.TabIndex = 1
        Me.txtDealName.Text = "Voucher Diskon Kasir POS"
        '
        'lblDealSku
        '
        Me.lblDealSku.AutoSize = True
        Me.lblDealSku.Location = New System.Drawing.Point(15, 68)
        Me.lblDealSku.Name = "lblDealSku"
        Me.lblDealSku.Size = New System.Drawing.Size(159, 20)
        Me.lblDealSku.TabIndex = 2
        Me.lblDealSku.Text = "Reward SKU (Catalog):"
        '
        'txtDealSku
        '
        Me.txtDealSku.Location = New System.Drawing.Point(170, 65)
        Me.txtDealSku.Name = "txtDealSku"
        Me.txtDealSku.Size = New System.Drawing.Size(265, 27)
        Me.txtDealSku.TabIndex = 3
        Me.txtDealSku.Text = "NEWITEM100K"
        '
        'lblDealType
        '
        Me.lblDealType.AutoSize = True
        Me.lblDealType.Location = New System.Drawing.Point(15, 106)
        Me.lblDealType.Name = "lblDealType"
        Me.lblDealType.Size = New System.Drawing.Size(90, 20)
        Me.lblDealType.TabIndex = 4
        Me.lblDealType.Text = "Tipe Diskon:"
        '
        'cmbDealType
        '
        Me.cmbDealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDealType.Items.AddRange(New Object() {"amount (Potongan Rp)", "percentage (Diskon %)", "free_item (Gratis Barang)"})
        Me.cmbDealType.Location = New System.Drawing.Point(170, 103)
        Me.cmbDealType.Name = "cmbDealType"
        Me.cmbDealType.Size = New System.Drawing.Size(265, 28)
        Me.cmbDealType.TabIndex = 5
        '
        'lblDealAmount
        '
        Me.lblDealAmount.AutoSize = True
        Me.lblDealAmount.Location = New System.Drawing.Point(15, 144)
        Me.lblDealAmount.Name = "lblDealAmount"
        Me.lblDealAmount.Size = New System.Drawing.Size(150, 20)
        Me.lblDealAmount.TabIndex = 6
        Me.lblDealAmount.Text = "Nilai Diskon (Rp / %):"
        '
        'txtDealAmount
        '
        Me.txtDealAmount.Location = New System.Drawing.Point(170, 141)
        Me.txtDealAmount.Name = "txtDealAmount"
        Me.txtDealAmount.Size = New System.Drawing.Size(265, 27)
        Me.txtDealAmount.TabIndex = 7
        Me.txtDealAmount.Text = "15000"
        '
        'lblDealCode
        '
        Me.lblDealCode.AutoSize = True
        Me.lblDealCode.Location = New System.Drawing.Point(15, 182)
        Me.lblDealCode.Name = "lblDealCode"
        Me.lblDealCode.Size = New System.Drawing.Size(169, 20)
        Me.lblDealCode.TabIndex = 8
        Me.lblDealCode.Text = "Kode Promo (Universal):"
        '
        'txtDealCode
        '
        Me.txtDealCode.Location = New System.Drawing.Point(170, 179)
        Me.txtDealCode.Name = "txtDealCode"
        Me.txtDealCode.Size = New System.Drawing.Size(265, 27)
        Me.txtDealCode.TabIndex = 9
        Me.txtDealCode.Text = "PROMO15K"
        '
        'btnCreateDeal
        '
        Me.btnCreateDeal.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnCreateDeal.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCreateDeal.Location = New System.Drawing.Point(170, 215)
        Me.btnCreateDeal.Name = "btnCreateDeal"
        Me.btnCreateDeal.Size = New System.Drawing.Size(265, 35)
        Me.btnCreateDeal.TabIndex = 10
        Me.btnCreateDeal.Text = "Insert Master Voucher (POST)"
        Me.btnCreateDeal.UseVisualStyleBackColor = False
        '
        'lblCreateDealStatus
        '
        Me.lblCreateDealStatus.ForeColor = System.Drawing.Color.DarkSlateGray
        Me.lblCreateDealStatus.Location = New System.Drawing.Point(15, 260)
        Me.lblCreateDealStatus.Name = "lblCreateDealStatus"
        Me.lblCreateDealStatus.Size = New System.Drawing.Size(425, 35)
        Me.lblCreateDealStatus.TabIndex = 11
        Me.lblCreateDealStatus.Text = "Catatan: Master promo dibuat di CRM Admin. Via API mewajibkan reward_sku valid di" &
    " catalog."
        '
        'grpManageDeals
        '
        Me.grpManageDeals.Controls.Add(Me.btnLoadDeals)
        Me.grpManageDeals.Controls.Add(Me.lstDeals)
        Me.grpManageDeals.Controls.Add(Me.lblCancelVoucherTitle)
        Me.grpManageDeals.Controls.Add(Me.lblCancelVoucherCode)
        Me.grpManageDeals.Controls.Add(Me.txtCancelVoucherCode)
        Me.grpManageDeals.Controls.Add(Me.lblCancelTxRef)
        Me.grpManageDeals.Controls.Add(Me.txtCancelTxRef)
        Me.grpManageDeals.Controls.Add(Me.btnSubmitCancelVoucher)
        Me.grpManageDeals.Controls.Add(Me.lblCancelVoucherStatus)
        Me.grpManageDeals.Location = New System.Drawing.Point(490, 15)
        Me.grpManageDeals.Name = "grpManageDeals"
        Me.grpManageDeals.Size = New System.Drawing.Size(450, 300)
        Me.grpManageDeals.TabIndex = 1
        Me.grpManageDeals.TabStop = False
        Me.grpManageDeals.Text = "Daftar Promo Aktif & Void Voucher (cancel_code)"
        '
        'btnLoadDeals
        '
        Me.btnLoadDeals.Location = New System.Drawing.Point(15, 22)
        Me.btnLoadDeals.Name = "btnLoadDeals"
        Me.btnLoadDeals.Size = New System.Drawing.Size(420, 27)
        Me.btnLoadDeals.TabIndex = 0
        Me.btnLoadDeals.Text = "Muat Promo Aktif Toko (GET /deal/)"
        '
        'lstDeals
        '
        Me.lstDeals.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.lstDeals.ItemHeight = 19
        Me.lstDeals.Location = New System.Drawing.Point(15, 55)
        Me.lstDeals.Name = "lstDeals"
        Me.lstDeals.Size = New System.Drawing.Size(420, 80)
        Me.lstDeals.TabIndex = 1
        '
        'lblCancelVoucherTitle
        '
        Me.lblCancelVoucherTitle.AutoSize = True
        Me.lblCancelVoucherTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblCancelVoucherTitle.Location = New System.Drawing.Point(15, 158)
        Me.lblCancelVoucherTitle.Name = "lblCancelVoucherTitle"
        Me.lblCancelVoucherTitle.Size = New System.Drawing.Size(333, 20)
        Me.lblCancelVoucherTitle.TabIndex = 2
        Me.lblCancelVoucherTitle.Text = "Batalkan Voucher yang Terpakai (Void Kupon):"
        '
        'lblCancelVoucherCode
        '
        Me.lblCancelVoucherCode.AutoSize = True
        Me.lblCancelVoucherCode.Location = New System.Drawing.Point(15, 185)
        Me.lblCancelVoucherCode.Name = "lblCancelVoucherCode"
        Me.lblCancelVoucherCode.Size = New System.Drawing.Size(104, 20)
        Me.lblCancelVoucherCode.TabIndex = 3
        Me.lblCancelVoucherCode.Text = "Kode Voucher:"
        '
        'txtCancelVoucherCode
        '
        Me.txtCancelVoucherCode.Location = New System.Drawing.Point(110, 182)
        Me.txtCancelVoucherCode.Name = "txtCancelVoucherCode"
        Me.txtCancelVoucherCode.Size = New System.Drawing.Size(150, 27)
        Me.txtCancelVoucherCode.TabIndex = 4
        '
        'lblCancelTxRef
        '
        Me.lblCancelTxRef.AutoSize = True
        Me.lblCancelTxRef.Location = New System.Drawing.Point(265, 185)
        Me.lblCancelTxRef.Name = "lblCancelTxRef"
        Me.lblCancelTxRef.Size = New System.Drawing.Size(52, 20)
        Me.lblCancelTxRef.TabIndex = 5
        Me.lblCancelTxRef.Text = "Tx Ref:"
        '
        'txtCancelTxRef
        '
        Me.txtCancelTxRef.Location = New System.Drawing.Point(310, 182)
        Me.txtCancelTxRef.Name = "txtCancelTxRef"
        Me.txtCancelTxRef.Size = New System.Drawing.Size(125, 27)
        Me.txtCancelTxRef.TabIndex = 6
        '
        'btnSubmitCancelVoucher
        '
        Me.btnSubmitCancelVoucher.BackColor = System.Drawing.Color.LightPink
        Me.btnSubmitCancelVoucher.Location = New System.Drawing.Point(110, 215)
        Me.btnSubmitCancelVoucher.Name = "btnSubmitCancelVoucher"
        Me.btnSubmitCancelVoucher.Size = New System.Drawing.Size(325, 30)
        Me.btnSubmitCancelVoucher.TabIndex = 7
        Me.btnSubmitCancelVoucher.Text = "Batalkan / Void Voucher (POST /cancel_code/)"
        Me.btnSubmitCancelVoucher.UseVisualStyleBackColor = False
        '
        'lblCancelVoucherStatus
        '
        Me.lblCancelVoucherStatus.Location = New System.Drawing.Point(15, 255)
        Me.lblCancelVoucherStatus.Name = "lblCancelVoucherStatus"
        Me.lblCancelVoucherStatus.Size = New System.Drawing.Size(420, 30)
        Me.lblCancelVoucherStatus.TabIndex = 8
        Me.lblCancelVoucherStatus.Text = "Status: Siap membatalkan voucher terpakai..."
        '
        'grpLogs
        '
        Me.grpLogs.Controls.Add(Me.txtLogs)
        Me.grpLogs.Controls.Add(Me.btnClearLogs)
        Me.grpLogs.Location = New System.Drawing.Point(12, 575)
        Me.grpLogs.Name = "grpLogs"
        Me.grpLogs.Size = New System.Drawing.Size(960, 190)
        Me.grpLogs.TabIndex = 2
        Me.grpLogs.TabStop = False
        Me.grpLogs.Text = "Live Activity & Integration Logs"
        '
        'txtLogs
        '
        Me.txtLogs.Font = New System.Drawing.Font("Consolas", 8.5!)
        Me.txtLogs.Location = New System.Drawing.Point(15, 22)
        Me.txtLogs.Multiline = True
        Me.txtLogs.Name = "txtLogs"
        Me.txtLogs.ReadOnly = True
        Me.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLogs.Size = New System.Drawing.Size(840, 155)
        Me.txtLogs.TabIndex = 0
        '
        'btnClearLogs
        '
        Me.btnClearLogs.Location = New System.Drawing.Point(865, 22)
        Me.btnClearLogs.Name = "btnClearLogs"
        Me.btnClearLogs.Size = New System.Drawing.Size(80, 30)
        Me.btnClearLogs.TabIndex = 1
        Me.btnClearLogs.Text = "Clear Log"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 775)
        Me.Controls.Add(Me.grpAuth)
        Me.Controls.Add(Me.tabControl)
        Me.Controls.Add(Me.grpLogs)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DreamPOS x Goapp CRM Integration Test Harness"
        Me.grpAuth.ResumeLayout(False)
        Me.grpAuth.PerformLayout()
        Me.tabControl.ResumeLayout(False)
        Me.tabMember.ResumeLayout(False)
        Me.tabMember.PerformLayout()
        Me.grpMemberResult.ResumeLayout(False)
        Me.grpMemberResult.PerformLayout()
        Me.tabVoucher.ResumeLayout(False)
        Me.tabVoucher.PerformLayout()
        Me.tabTransaction.ResumeLayout(False)
        Me.tabTransaction.PerformLayout()
        Me.tabReceipt.ResumeLayout(False)
        Me.tabReceipt.PerformLayout()
        CType(Me.picQrCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabRegister.ResumeLayout(False)
        Me.grpRegForm.ResumeLayout(False)
        Me.grpRegForm.PerformLayout()
        Me.grpRegInfo.ResumeLayout(False)
        Me.grpRegInfo.PerformLayout()
        Me.tabMasterVoucher.ResumeLayout(False)
        Me.grpCreateDeal.ResumeLayout(False)
        Me.grpCreateDeal.PerformLayout()
        Me.grpManageDeals.ResumeLayout(False)
        Me.grpManageDeals.PerformLayout()
        Me.grpLogs.ResumeLayout(False)
        Me.grpLogs.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
End Class


