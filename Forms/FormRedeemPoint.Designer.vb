Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FormRedeemPoint
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

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblHeaderTitle As System.Windows.Forms.Label
        Friend WithEvents lblHeaderSubtitle As System.Windows.Forms.Label
        Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
        Friend WithEvents lblMemberNameTitle As System.Windows.Forms.Label
        Friend WithEvents lblMemberName As System.Windows.Forms.Label
        Friend WithEvents lblAvailablePointsTitle As System.Windows.Forms.Label
        Friend WithEvents lblAvailablePoints As System.Windows.Forms.Label
        Friend WithEvents lblTotalTxTitle As System.Windows.Forms.Label
        Friend WithEvents lblTotalTx As System.Windows.Forms.Label
        Friend WithEvents lblMaxBurnTitle As System.Windows.Forms.Label
        Friend WithEvents lblMaxBurn As System.Windows.Forms.Label
        Friend WithEvents lblPointPrompt As System.Windows.Forms.Label
        Friend WithEvents txtBurnPoints As System.Windows.Forms.TextBox
        Friend WithEvents btnMaxPoint As System.Windows.Forms.Button
        Friend WithEvents lblAuthPrompt As System.Windows.Forms.Label
        Friend WithEvents txtAuthCode As System.Windows.Forms.TextBox
        Friend WithEvents pnlNumpad As System.Windows.Forms.Panel
        Friend WithEvents btnNum1 As System.Windows.Forms.Button
        Friend WithEvents btnNum2 As System.Windows.Forms.Button
        Friend WithEvents btnNum3 As System.Windows.Forms.Button
        Friend WithEvents btnNum4 As System.Windows.Forms.Button
        Friend WithEvents btnNum5 As System.Windows.Forms.Button
        Friend WithEvents btnNum6 As System.Windows.Forms.Button
        Friend WithEvents btnNum7 As System.Windows.Forms.Button
        Friend WithEvents btnNum8 As System.Windows.Forms.Button
        Friend WithEvents btnNum9 As System.Windows.Forms.Button
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents btnNum0 As System.Windows.Forms.Button
        Friend WithEvents btnBackspace As System.Windows.Forms.Button
        Friend WithEvents lblStatus As System.Windows.Forms.Label
        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents btnBurnPoint As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblHeaderSubtitle = New System.Windows.Forms.Label()
            Me.lblHeaderTitle = New System.Windows.Forms.Label()
            Me.grpInfo = New System.Windows.Forms.GroupBox()
            Me.lblMaxBurn = New System.Windows.Forms.Label()
            Me.lblMaxBurnTitle = New System.Windows.Forms.Label()
            Me.lblTotalTx = New System.Windows.Forms.Label()
            Me.lblTotalTxTitle = New System.Windows.Forms.Label()
            Me.lblAvailablePoints = New System.Windows.Forms.Label()
            Me.lblAvailablePointsTitle = New System.Windows.Forms.Label()
            Me.lblMemberName = New System.Windows.Forms.Label()
            Me.lblMemberNameTitle = New System.Windows.Forms.Label()
            Me.lblPointPrompt = New System.Windows.Forms.Label()
            Me.txtBurnPoints = New System.Windows.Forms.TextBox()
            Me.btnMaxPoint = New System.Windows.Forms.Button()
            Me.lblAuthPrompt = New System.Windows.Forms.Label()
            Me.txtAuthCode = New System.Windows.Forms.TextBox()
            Me.pnlNumpad = New System.Windows.Forms.Panel()
            Me.btnNum1 = New System.Windows.Forms.Button()
            Me.btnNum2 = New System.Windows.Forms.Button()
            Me.btnNum3 = New System.Windows.Forms.Button()
            Me.btnNum4 = New System.Windows.Forms.Button()
            Me.btnNum5 = New System.Windows.Forms.Button()
            Me.btnNum6 = New System.Windows.Forms.Button()
            Me.btnNum7 = New System.Windows.Forms.Button()
            Me.btnNum8 = New System.Windows.Forms.Button()
            Me.btnNum9 = New System.Windows.Forms.Button()
            Me.btnClear = New System.Windows.Forms.Button()
            Me.btnNum0 = New System.Windows.Forms.Button()
            Me.btnBackspace = New System.Windows.Forms.Button()
            Me.lblStatus = New System.Windows.Forms.Label()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.btnBurnPoint = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.grpInfo.SuspendLayout()
            Me.pnlNumpad.SuspendLayout()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblHeaderSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblHeaderTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 10, 16, 10)
            Me.pnlHeader.Size = New System.Drawing.Size(520, 58)
            Me.pnlHeader.TabIndex = 0
            '
            'lblHeaderSubtitle
            '
            Me.lblHeaderSubtitle.AutoSize = True
            Me.lblHeaderSubtitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblHeaderSubtitle.Location = New System.Drawing.Point(15, 33)
            Me.lblHeaderSubtitle.Name = "lblHeaderSubtitle"
            Me.lblHeaderSubtitle.Size = New System.Drawing.Size(301, 20)
            Me.lblHeaderSubtitle.TabIndex = 1
            Me.lblHeaderSubtitle.Text = "Tukarkan Poin Menjadi Diskon Transaksi POS"
            '
            'lblHeaderTitle
            '
            Me.lblHeaderTitle.AutoSize = True
            Me.lblHeaderTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblHeaderTitle.ForeColor = System.Drawing.Color.White
            Me.lblHeaderTitle.Location = New System.Drawing.Point(14, 7)
            Me.lblHeaderTitle.Name = "lblHeaderTitle"
            Me.lblHeaderTitle.Size = New System.Drawing.Size(311, 28)
            Me.lblHeaderTitle.TabIndex = 0
            Me.lblHeaderTitle.Text = "GUNAKAN POIN (BURN POINT)"
            '
            'grpInfo
            '
            Me.grpInfo.Controls.Add(Me.lblMaxBurn)
            Me.grpInfo.Controls.Add(Me.lblMaxBurnTitle)
            Me.grpInfo.Controls.Add(Me.lblTotalTx)
            Me.grpInfo.Controls.Add(Me.lblTotalTxTitle)
            Me.grpInfo.Controls.Add(Me.lblAvailablePoints)
            Me.grpInfo.Controls.Add(Me.lblAvailablePointsTitle)
            Me.grpInfo.Controls.Add(Me.lblMemberName)
            Me.grpInfo.Controls.Add(Me.lblMemberNameTitle)
            Me.grpInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.grpInfo.Location = New System.Drawing.Point(16, 66)
            Me.grpInfo.Name = "grpInfo"
            Me.grpInfo.Size = New System.Drawing.Size(488, 126)
            Me.grpInfo.TabIndex = 1
            Me.grpInfo.TabStop = False
            Me.grpInfo.Text = "Informasi Poin & Transaksi"
            '
            'lblMaxBurn
            '
            Me.lblMaxBurn.AutoSize = True
            Me.lblMaxBurn.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.lblMaxBurn.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblMaxBurn.Location = New System.Drawing.Point(260, 92)
            Me.lblMaxBurn.Name = "lblMaxBurn"
            Me.lblMaxBurn.Size = New System.Drawing.Size(53, 25)
            Me.lblMaxBurn.TabIndex = 7
            Me.lblMaxBurn.Text = "0 Pts"
            '
            'lblMaxBurnTitle
            '
            Me.lblMaxBurnTitle.AutoSize = True
            Me.lblMaxBurnTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblMaxBurnTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblMaxBurnTitle.Location = New System.Drawing.Point(260, 72)
            Me.lblMaxBurnTitle.Name = "lblMaxBurnTitle"
            Me.lblMaxBurnTitle.Size = New System.Drawing.Size(140, 20)
            Me.lblMaxBurnTitle.TabIndex = 6
            Me.lblMaxBurnTitle.Text = "Maks. Poin Dipakai :"
            '
            'lblTotalTx
            '
            Me.lblTotalTx.AutoSize = True
            Me.lblTotalTx.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.lblTotalTx.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(55, Byte), Integer))
            Me.lblTotalTx.Location = New System.Drawing.Point(12, 92)
            Me.lblTotalTx.Name = "lblTotalTx"
            Me.lblTotalTx.Size = New System.Drawing.Size(47, 23)
            Me.lblTotalTx.TabIndex = 5
            Me.lblTotalTx.Text = "Rp 0"
            '
            'lblTotalTxTitle
            '
            Me.lblTotalTxTitle.AutoSize = True
            Me.lblTotalTxTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblTotalTxTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblTotalTxTitle.Location = New System.Drawing.Point(12, 72)
            Me.lblTotalTxTitle.Name = "lblTotalTxTitle"
            Me.lblTotalTxTitle.Size = New System.Drawing.Size(143, 20)
            Me.lblTotalTxTitle.TabIndex = 4
            Me.lblTotalTxTitle.Text = "Total Transaksi POS :"
            '
            'lblAvailablePoints
            '
            Me.lblAvailablePoints.AutoSize = True
            Me.lblAvailablePoints.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.lblAvailablePoints.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblAvailablePoints.Location = New System.Drawing.Point(260, 42)
            Me.lblAvailablePoints.Name = "lblAvailablePoints"
            Me.lblAvailablePoints.Size = New System.Drawing.Size(110, 25)
            Me.lblAvailablePoints.TabIndex = 3
            Me.lblAvailablePoints.Text = "0 Pts (Rp 0)"
            '
            'lblAvailablePointsTitle
            '
            Me.lblAvailablePointsTitle.AutoSize = True
            Me.lblAvailablePointsTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblAvailablePointsTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblAvailablePointsTitle.Location = New System.Drawing.Point(260, 22)
            Me.lblAvailablePointsTitle.Name = "lblAvailablePointsTitle"
            Me.lblAvailablePointsTitle.Size = New System.Drawing.Size(86, 20)
            Me.lblAvailablePointsTitle.TabIndex = 2
            Me.lblAvailablePointsTitle.Text = "Saldo Poin :"
            '
            'lblMemberName
            '
            Me.lblMemberName.AutoSize = True
            Me.lblMemberName.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.lblMemberName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(55, Byte), Integer))
            Me.lblMemberName.Location = New System.Drawing.Point(12, 42)
            Me.lblMemberName.Name = "lblMemberName"
            Me.lblMemberName.Size = New System.Drawing.Size(17, 23)
            Me.lblMemberName.TabIndex = 1
            Me.lblMemberName.Text = "-"
            '
            'lblMemberNameTitle
            '
            Me.lblMemberNameTitle.AutoSize = True
            Me.lblMemberNameTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblMemberNameTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblMemberNameTitle.Location = New System.Drawing.Point(12, 22)
            Me.lblMemberNameTitle.Name = "lblMemberNameTitle"
            Me.lblMemberNameTitle.Size = New System.Drawing.Size(72, 20)
            Me.lblMemberNameTitle.TabIndex = 0
            Me.lblMemberNameTitle.Text = "Member :"
            '
            'lblPointPrompt
            '
            Me.lblPointPrompt.AutoSize = True
            Me.lblPointPrompt.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblPointPrompt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.lblPointPrompt.Location = New System.Drawing.Point(16, 200)
            Me.lblPointPrompt.Name = "lblPointPrompt"
            Me.lblPointPrompt.Size = New System.Drawing.Size(264, 20)
            Me.lblPointPrompt.TabIndex = 2
            Me.lblPointPrompt.Text = "JUMLAH POIN YANG DIGUNAKAN :"
            '
            'txtBurnPoints
            '
            Me.txtBurnPoints.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.txtBurnPoints.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtBurnPoints.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
            Me.txtBurnPoints.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.txtBurnPoints.Location = New System.Drawing.Point(16, 224)
            Me.txtBurnPoints.Name = "txtBurnPoints"
            Me.txtBurnPoints.Size = New System.Drawing.Size(360, 43)
            Me.txtBurnPoints.TabIndex = 3
            Me.txtBurnPoints.Text = "0"
            Me.txtBurnPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'btnMaxPoint
            '
            Me.btnMaxPoint.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer), CType(CType(7, Byte), Integer))
            Me.btnMaxPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnMaxPoint.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnMaxPoint.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnMaxPoint.Location = New System.Drawing.Point(384, 224)
            Me.btnMaxPoint.Name = "btnMaxPoint"
            Me.btnMaxPoint.Size = New System.Drawing.Size(120, 43)
            Me.btnMaxPoint.TabIndex = 4
            Me.btnMaxPoint.Text = "MAKSIMAL"
            Me.btnMaxPoint.UseVisualStyleBackColor = False
            '
            'lblAuthPrompt
            '
            Me.lblAuthPrompt.AutoSize = True
            Me.lblAuthPrompt.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblAuthPrompt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(95, Byte), Integer))
            Me.lblAuthPrompt.Location = New System.Drawing.Point(16, 274)
            Me.lblAuthPrompt.Name = "lblAuthPrompt"
            Me.lblAuthPrompt.Size = New System.Drawing.Size(324, 20)
            Me.lblAuthPrompt.TabIndex = 5
            Me.lblAuthPrompt.Text = "KODE OTORISASI / AUTH CODE (OPSIONAL):"
            '
            'txtAuthCode
            '
            Me.txtAuthCode.BackColor = System.Drawing.Color.White
            Me.txtAuthCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtAuthCode.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
            Me.txtAuthCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.txtAuthCode.Location = New System.Drawing.Point(16, 296)
            Me.txtAuthCode.Name = "txtAuthCode"
            Me.txtAuthCode.Size = New System.Drawing.Size(488, 32)
            Me.txtAuthCode.TabIndex = 6
            Me.txtAuthCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'pnlNumpad
            '
            Me.pnlNumpad.Controls.Add(Me.btnNum1)
            Me.pnlNumpad.Controls.Add(Me.btnNum2)
            Me.pnlNumpad.Controls.Add(Me.btnNum3)
            Me.pnlNumpad.Controls.Add(Me.btnNum4)
            Me.pnlNumpad.Controls.Add(Me.btnNum5)
            Me.pnlNumpad.Controls.Add(Me.btnNum6)
            Me.pnlNumpad.Controls.Add(Me.btnNum7)
            Me.pnlNumpad.Controls.Add(Me.btnNum8)
            Me.pnlNumpad.Controls.Add(Me.btnNum9)
            Me.pnlNumpad.Controls.Add(Me.btnClear)
            Me.pnlNumpad.Controls.Add(Me.btnNum0)
            Me.pnlNumpad.Controls.Add(Me.btnBackspace)
            Me.pnlNumpad.Location = New System.Drawing.Point(16, 336)
            Me.pnlNumpad.Name = "pnlNumpad"
            Me.pnlNumpad.Size = New System.Drawing.Size(488, 200)
            Me.pnlNumpad.TabIndex = 7
            '
            'btnNum1
            '
            Me.btnNum1.BackColor = System.Drawing.Color.White
            Me.btnNum1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum1.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum1.Location = New System.Drawing.Point(0, 0)
            Me.btnNum1.Name = "btnNum1"
            Me.btnNum1.Size = New System.Drawing.Size(156, 45)
            Me.btnNum1.TabIndex = 0
            Me.btnNum1.Text = "1"
            Me.btnNum1.UseVisualStyleBackColor = False
            '
            'btnNum2
            '
            Me.btnNum2.BackColor = System.Drawing.Color.White
            Me.btnNum2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum2.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum2.Location = New System.Drawing.Point(166, 0)
            Me.btnNum2.Name = "btnNum2"
            Me.btnNum2.Size = New System.Drawing.Size(156, 45)
            Me.btnNum2.TabIndex = 1
            Me.btnNum2.Text = "2"
            Me.btnNum2.UseVisualStyleBackColor = False
            '
            'btnNum3
            '
            Me.btnNum3.BackColor = System.Drawing.Color.White
            Me.btnNum3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum3.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum3.Location = New System.Drawing.Point(332, 0)
            Me.btnNum3.Name = "btnNum3"
            Me.btnNum3.Size = New System.Drawing.Size(156, 45)
            Me.btnNum3.TabIndex = 2
            Me.btnNum3.Text = "3"
            Me.btnNum3.UseVisualStyleBackColor = False
            '
            'btnNum4
            '
            Me.btnNum4.BackColor = System.Drawing.Color.White
            Me.btnNum4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum4.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum4.Location = New System.Drawing.Point(0, 51)
            Me.btnNum4.Name = "btnNum4"
            Me.btnNum4.Size = New System.Drawing.Size(156, 45)
            Me.btnNum4.TabIndex = 3
            Me.btnNum4.Text = "4"
            Me.btnNum4.UseVisualStyleBackColor = False
            '
            'btnNum5
            '
            Me.btnNum5.BackColor = System.Drawing.Color.White
            Me.btnNum5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum5.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum5.Location = New System.Drawing.Point(166, 51)
            Me.btnNum5.Name = "btnNum5"
            Me.btnNum5.Size = New System.Drawing.Size(156, 45)
            Me.btnNum5.TabIndex = 4
            Me.btnNum5.Text = "5"
            Me.btnNum5.UseVisualStyleBackColor = False
            '
            'btnNum6
            '
            Me.btnNum6.BackColor = System.Drawing.Color.White
            Me.btnNum6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum6.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum6.Location = New System.Drawing.Point(332, 51)
            Me.btnNum6.Name = "btnNum6"
            Me.btnNum6.Size = New System.Drawing.Size(156, 45)
            Me.btnNum6.TabIndex = 5
            Me.btnNum6.Text = "6"
            Me.btnNum6.UseVisualStyleBackColor = False
            '
            'btnNum7
            '
            Me.btnNum7.BackColor = System.Drawing.Color.White
            Me.btnNum7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum7.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum7.Location = New System.Drawing.Point(0, 102)
            Me.btnNum7.Name = "btnNum7"
            Me.btnNum7.Size = New System.Drawing.Size(156, 45)
            Me.btnNum7.TabIndex = 6
            Me.btnNum7.Text = "7"
            Me.btnNum7.UseVisualStyleBackColor = False
            '
            'btnNum8
            '
            Me.btnNum8.BackColor = System.Drawing.Color.White
            Me.btnNum8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum8.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum8.Location = New System.Drawing.Point(166, 102)
            Me.btnNum8.Name = "btnNum8"
            Me.btnNum8.Size = New System.Drawing.Size(156, 45)
            Me.btnNum8.TabIndex = 7
            Me.btnNum8.Text = "8"
            Me.btnNum8.UseVisualStyleBackColor = False
            '
            'btnNum9
            '
            Me.btnNum9.BackColor = System.Drawing.Color.White
            Me.btnNum9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum9.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum9.Location = New System.Drawing.Point(332, 102)
            Me.btnNum9.Name = "btnNum9"
            Me.btnNum9.Size = New System.Drawing.Size(156, 45)
            Me.btnNum9.TabIndex = 8
            Me.btnNum9.Text = "9"
            Me.btnNum9.UseVisualStyleBackColor = False
            '
            'btnClear
            '
            Me.btnClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(205, Byte), Integer))
            Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.btnClear.ForeColor = System.Drawing.Color.FromArgb(CType(CType(133, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(4, Byte), Integer))
            Me.btnClear.Location = New System.Drawing.Point(0, 153)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(156, 45)
            Me.btnClear.TabIndex = 9
            Me.btnClear.Text = "CLEAR"
            Me.btnClear.UseVisualStyleBackColor = False
            '
            'btnNum0
            '
            Me.btnNum0.BackColor = System.Drawing.Color.White
            Me.btnNum0.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum0.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum0.Location = New System.Drawing.Point(166, 153)
            Me.btnNum0.Name = "btnNum0"
            Me.btnNum0.Size = New System.Drawing.Size(156, 45)
            Me.btnNum0.TabIndex = 10
            Me.btnNum0.Text = "0"
            Me.btnNum0.UseVisualStyleBackColor = False
            '
            'btnBackspace
            '
            Me.btnBackspace.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
            Me.btnBackspace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnBackspace.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.btnBackspace.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.btnBackspace.Location = New System.Drawing.Point(332, 153)
            Me.btnBackspace.Name = "btnBackspace"
            Me.btnBackspace.Size = New System.Drawing.Size(156, 45)
            Me.btnBackspace.TabIndex = 11
            Me.btnBackspace.Text = "⌫ HAPUS"
            Me.btnBackspace.UseVisualStyleBackColor = False
            '
            'lblStatus
            '
            Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
            Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblStatus.Location = New System.Drawing.Point(16, 542)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(488, 22)
            Me.lblStatus.TabIndex = 8
            Me.lblStatus.Text = "Masukkan jumlah poin yang ingin dibakar (1 Poin = Rp 1)."
            Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(242, Byte), Integer))
            Me.pnlBottom.Controls.Add(Me.btnBurnPoint)
            Me.pnlBottom.Controls.Add(Me.btnCancel)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 574)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Size = New System.Drawing.Size(520, 60)
            Me.pnlBottom.TabIndex = 9
            '
            'btnBurnPoint
            '
            Me.btnBurnPoint.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnBurnPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnBurnPoint.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.btnBurnPoint.ForeColor = System.Drawing.Color.White
            Me.btnBurnPoint.Location = New System.Drawing.Point(260, 10)
            Me.btnBurnPoint.Name = "btnBurnPoint"
            Me.btnBurnPoint.Size = New System.Drawing.Size(244, 40)
            Me.btnBurnPoint.TabIndex = 1
            Me.btnBurnPoint.Text = "⚡ GUNAKAN POIN"
            Me.btnBurnPoint.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnCancel.ForeColor = System.Drawing.Color.White
            Me.btnCancel.Location = New System.Drawing.Point(16, 10)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(120, 40)
            Me.btnCancel.TabIndex = 0
            Me.btnCancel.Text = "BATAL"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'FormRedeemPoint
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(520, 634)
            Me.Controls.Add(Me.pnlBottom)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.pnlNumpad)
            Me.Controls.Add(Me.txtAuthCode)
            Me.Controls.Add(Me.lblAuthPrompt)
            Me.Controls.Add(Me.btnMaxPoint)
            Me.Controls.Add(Me.txtBurnPoints)
            Me.Controls.Add(Me.lblPointPrompt)
            Me.Controls.Add(Me.grpInfo)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FormRedeemPoint"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Gunakan Poin Member - Goapp"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.grpInfo.ResumeLayout(False)
            Me.grpInfo.PerformLayout()
            Me.pnlNumpad.ResumeLayout(False)
            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
    End Class
End Namespace
