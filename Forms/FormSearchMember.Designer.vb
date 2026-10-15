Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FormSearchMember
        Inherits System.Windows.Forms.Form

        ' Form overrides dispose to clean up the component list.
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

        Friend WithEvents pnlLeft As System.Windows.Forms.Panel
        Friend WithEvents lblPhonePrompt As System.Windows.Forms.Label
        Friend WithEvents txtPhoneNo As System.Windows.Forms.TextBox

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
        Friend WithEvents btnPlus As System.Windows.Forms.Button
        Friend WithEvents btnSearch As System.Windows.Forms.Button

        Friend WithEvents pnlMiddle As System.Windows.Forms.Panel
        Friend WithEvents grpMemberInfo As System.Windows.Forms.GroupBox
        Friend WithEvents lblStatusBadge As System.Windows.Forms.Label
        Friend WithEvents lblNameTitle As System.Windows.Forms.Label
        Friend WithEvents lblMemberName As System.Windows.Forms.Label
        Friend WithEvents lblPhoneTitle As System.Windows.Forms.Label
        Friend WithEvents lblMemberPhone As System.Windows.Forms.Label
        Friend WithEvents lblLevelTitle As System.Windows.Forms.Label
        Friend WithEvents lblMemberLevel As System.Windows.Forms.Label
        Friend WithEvents lblPointsTitle As System.Windows.Forms.Label
        Friend WithEvents lblMemberPoints As System.Windows.Forms.Label
        Friend WithEvents btnUsePoint As System.Windows.Forms.Button

        Friend WithEvents grpBenefits As System.Windows.Forms.GroupBox
        Friend WithEvents lstDeals As System.Windows.Forms.ListBox
        Friend WithEvents btnUseSelectedVoucher As System.Windows.Forms.Button

        Friend WithEvents grpActiveRedeems As System.Windows.Forms.GroupBox
        Friend WithEvents lblActiveRedeemsInfo As System.Windows.Forms.Label
        Friend WithEvents lstActiveRedeems As System.Windows.Forms.ListBox
        Friend WithEvents pnlActiveAction As System.Windows.Forms.Panel
        Friend WithEvents btnCancelActiveItem As System.Windows.Forms.Button

        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents btnScanQrVoucher As System.Windows.Forms.Button
        Friend WithEvents btnSelect As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents lblSearchStatus As System.Windows.Forms.Label

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblHeaderTitle = New System.Windows.Forms.Label()
            Me.lblHeaderSubtitle = New System.Windows.Forms.Label()
            Me.pnlLeft = New System.Windows.Forms.Panel()
            Me.lblPhonePrompt = New System.Windows.Forms.Label()
            Me.txtPhoneNo = New System.Windows.Forms.TextBox()
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
            Me.btnPlus = New System.Windows.Forms.Button()
            Me.btnSearch = New System.Windows.Forms.Button()
            Me.pnlMiddle = New System.Windows.Forms.Panel()
            Me.grpBenefits = New System.Windows.Forms.GroupBox()
            Me.lstDeals = New System.Windows.Forms.ListBox()
            Me.btnUseSelectedVoucher = New System.Windows.Forms.Button()
            Me.grpMemberInfo = New System.Windows.Forms.GroupBox()
            Me.lblStatusBadge = New System.Windows.Forms.Label()
            Me.lblNameTitle = New System.Windows.Forms.Label()
            Me.lblMemberName = New System.Windows.Forms.Label()
            Me.lblPhoneTitle = New System.Windows.Forms.Label()
            Me.lblMemberPhone = New System.Windows.Forms.Label()
            Me.lblLevelTitle = New System.Windows.Forms.Label()
            Me.lblMemberLevel = New System.Windows.Forms.Label()
            Me.lblPointsTitle = New System.Windows.Forms.Label()
            Me.lblMemberPoints = New System.Windows.Forms.Label()
            Me.btnUsePoint = New System.Windows.Forms.Button()
            Me.grpActiveRedeems = New System.Windows.Forms.GroupBox()
            Me.lblActiveRedeemsInfo = New System.Windows.Forms.Label()
            Me.lstActiveRedeems = New System.Windows.Forms.ListBox()
            Me.pnlActiveAction = New System.Windows.Forms.Panel()
            Me.btnCancelActiveItem = New System.Windows.Forms.Button()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.lblSearchStatus = New System.Windows.Forms.Label()
            Me.btnScanQrVoucher = New System.Windows.Forms.Button()
            Me.btnSelect = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.pnlLeft.SuspendLayout()
            Me.pnlMiddle.SuspendLayout()
            Me.grpBenefits.SuspendLayout()
            Me.grpMemberInfo.SuspendLayout()
            Me.grpActiveRedeems.SuspendLayout()
            Me.pnlActiveAction.SuspendLayout()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblHeaderTitle)
            Me.pnlHeader.Controls.Add(Me.lblHeaderSubtitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New System.Drawing.Size(1000, 60)
            Me.pnlHeader.TabIndex = 0
            '
            'lblHeaderTitle
            '
            Me.lblHeaderTitle.AutoSize = True
            Me.lblHeaderTitle.Font = New System.Drawing.Font("Segoe UI", 13.0!, System.Drawing.FontStyle.Bold)
            Me.lblHeaderTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
            Me.lblHeaderTitle.Location = New System.Drawing.Point(14, 9)
            Me.lblHeaderTitle.Name = "lblHeaderTitle"
            Me.lblHeaderTitle.Size = New System.Drawing.Size(338, 30)
            Me.lblHeaderTitle.TabIndex = 0
            Me.lblHeaderTitle.Text = "Pencarian Member Goapp CRM"
            '
            'lblHeaderSubtitle
            '
            Me.lblHeaderSubtitle.AutoSize = True
            Me.lblHeaderSubtitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblHeaderSubtitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
            Me.lblHeaderSubtitle.Location = New System.Drawing.Point(15, 34)
            Me.lblHeaderSubtitle.Name = "lblHeaderSubtitle"
            Me.lblHeaderSubtitle.Size = New System.Drawing.Size(394, 20)
            Me.lblHeaderSubtitle.TabIndex = 1
            Me.lblHeaderSubtitle.Text = "Masukkan Nomor Handphone atau Member ID pelanggan"
            '
            'pnlLeft
            '
            Me.pnlLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlLeft.Controls.Add(Me.lblPhonePrompt)
            Me.pnlLeft.Controls.Add(Me.txtPhoneNo)
            Me.pnlLeft.Controls.Add(Me.btnNum1)
            Me.pnlLeft.Controls.Add(Me.btnNum2)
            Me.pnlLeft.Controls.Add(Me.btnNum3)
            Me.pnlLeft.Controls.Add(Me.btnNum4)
            Me.pnlLeft.Controls.Add(Me.btnNum5)
            Me.pnlLeft.Controls.Add(Me.btnNum6)
            Me.pnlLeft.Controls.Add(Me.btnNum7)
            Me.pnlLeft.Controls.Add(Me.btnNum8)
            Me.pnlLeft.Controls.Add(Me.btnNum9)
            Me.pnlLeft.Controls.Add(Me.btnClear)
            Me.pnlLeft.Controls.Add(Me.btnNum0)
            Me.pnlLeft.Controls.Add(Me.btnBackspace)
            Me.pnlLeft.Controls.Add(Me.btnPlus)
            Me.pnlLeft.Controls.Add(Me.btnSearch)
            Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlLeft.Location = New System.Drawing.Point(0, 60)
            Me.pnlLeft.Name = "pnlLeft"
            Me.pnlLeft.Padding = New System.Windows.Forms.Padding(12)
            Me.pnlLeft.Size = New System.Drawing.Size(320, 500)
            Me.pnlLeft.TabIndex = 1
            '
            'lblPhonePrompt
            '
            Me.lblPhonePrompt.AutoSize = True
            Me.lblPhonePrompt.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblPhonePrompt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(95, Byte), Integer))
            Me.lblPhonePrompt.Location = New System.Drawing.Point(12, 10)
            Me.lblPhonePrompt.Name = "lblPhonePrompt"
            Me.lblPhonePrompt.Size = New System.Drawing.Size(163, 20)
            Me.lblPhonePrompt.TabIndex = 0
            Me.lblPhonePrompt.Text = "NO HP / MEMBER ID :"
            '
            'txtPhoneNo
            '
            Me.txtPhoneNo.BackColor = System.Drawing.Color.White
            Me.txtPhoneNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtPhoneNo.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
            Me.txtPhoneNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(45, Byte), Integer))
            Me.txtPhoneNo.Location = New System.Drawing.Point(12, 30)
            Me.txtPhoneNo.MaxLength = 20
            Me.txtPhoneNo.Name = "txtPhoneNo"
            Me.txtPhoneNo.Size = New System.Drawing.Size(296, 43)
            Me.txtPhoneNo.TabIndex = 1
            Me.txtPhoneNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnNum1
            '
            Me.btnNum1.BackColor = System.Drawing.Color.White
            Me.btnNum1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum1.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum1.Location = New System.Drawing.Point(12, 78)
            Me.btnNum1.Name = "btnNum1"
            Me.btnNum1.Size = New System.Drawing.Size(92, 56)
            Me.btnNum1.TabIndex = 2
            Me.btnNum1.Text = "1"
            Me.btnNum1.UseVisualStyleBackColor = False
            '
            'btnNum2
            '
            Me.btnNum2.BackColor = System.Drawing.Color.White
            Me.btnNum2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum2.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum2.Location = New System.Drawing.Point(114, 78)
            Me.btnNum2.Name = "btnNum2"
            Me.btnNum2.Size = New System.Drawing.Size(92, 56)
            Me.btnNum2.TabIndex = 3
            Me.btnNum2.Text = "2"
            Me.btnNum2.UseVisualStyleBackColor = False
            '
            'btnNum3
            '
            Me.btnNum3.BackColor = System.Drawing.Color.White
            Me.btnNum3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum3.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum3.Location = New System.Drawing.Point(216, 78)
            Me.btnNum3.Name = "btnNum3"
            Me.btnNum3.Size = New System.Drawing.Size(92, 56)
            Me.btnNum3.TabIndex = 4
            Me.btnNum3.Text = "3"
            Me.btnNum3.UseVisualStyleBackColor = False
            '
            'btnNum4
            '
            Me.btnNum4.BackColor = System.Drawing.Color.White
            Me.btnNum4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum4.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum4.Location = New System.Drawing.Point(12, 142)
            Me.btnNum4.Name = "btnNum4"
            Me.btnNum4.Size = New System.Drawing.Size(92, 56)
            Me.btnNum4.TabIndex = 5
            Me.btnNum4.Text = "4"
            Me.btnNum4.UseVisualStyleBackColor = False
            '
            'btnNum5
            '
            Me.btnNum5.BackColor = System.Drawing.Color.White
            Me.btnNum5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum5.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum5.Location = New System.Drawing.Point(114, 142)
            Me.btnNum5.Name = "btnNum5"
            Me.btnNum5.Size = New System.Drawing.Size(92, 56)
            Me.btnNum5.TabIndex = 6
            Me.btnNum5.Text = "5"
            Me.btnNum5.UseVisualStyleBackColor = False
            '
            'btnNum6
            '
            Me.btnNum6.BackColor = System.Drawing.Color.White
            Me.btnNum6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum6.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum6.Location = New System.Drawing.Point(216, 142)
            Me.btnNum6.Name = "btnNum6"
            Me.btnNum6.Size = New System.Drawing.Size(92, 56)
            Me.btnNum6.TabIndex = 7
            Me.btnNum6.Text = "6"
            Me.btnNum6.UseVisualStyleBackColor = False
            '
            'btnNum7
            '
            Me.btnNum7.BackColor = System.Drawing.Color.White
            Me.btnNum7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum7.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum7.Location = New System.Drawing.Point(12, 206)
            Me.btnNum7.Name = "btnNum7"
            Me.btnNum7.Size = New System.Drawing.Size(92, 56)
            Me.btnNum7.TabIndex = 8
            Me.btnNum7.Text = "7"
            Me.btnNum7.UseVisualStyleBackColor = False
            '
            'btnNum8
            '
            Me.btnNum8.BackColor = System.Drawing.Color.White
            Me.btnNum8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum8.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum8.Location = New System.Drawing.Point(114, 206)
            Me.btnNum8.Name = "btnNum8"
            Me.btnNum8.Size = New System.Drawing.Size(92, 56)
            Me.btnNum8.TabIndex = 9
            Me.btnNum8.Text = "8"
            Me.btnNum8.UseVisualStyleBackColor = False
            '
            'btnNum9
            '
            Me.btnNum9.BackColor = System.Drawing.Color.White
            Me.btnNum9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum9.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum9.Location = New System.Drawing.Point(216, 206)
            Me.btnNum9.Name = "btnNum9"
            Me.btnNum9.Size = New System.Drawing.Size(92, 56)
            Me.btnNum9.TabIndex = 10
            Me.btnNum9.Text = "9"
            Me.btnNum9.UseVisualStyleBackColor = False
            '
            'btnPlus
            '
            Me.btnPlus.BackColor = System.Drawing.Color.White
            Me.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPlus.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
            Me.btnPlus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnPlus.Location = New System.Drawing.Point(12, 270)
            Me.btnPlus.Name = "btnPlus"
            Me.btnPlus.Size = New System.Drawing.Size(92, 56)
            Me.btnPlus.TabIndex = 11
            Me.btnPlus.Text = "+"
            Me.btnPlus.UseVisualStyleBackColor = False
            '
            'btnNum0
            '
            Me.btnNum0.BackColor = System.Drawing.Color.White
            Me.btnNum0.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnNum0.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
            Me.btnNum0.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.btnNum0.Location = New System.Drawing.Point(114, 270)
            Me.btnNum0.Name = "btnNum0"
            Me.btnNum0.Size = New System.Drawing.Size(92, 56)
            Me.btnNum0.TabIndex = 12
            Me.btnNum0.Text = "0"
            Me.btnNum0.UseVisualStyleBackColor = False
            '
            'btnBackspace
            '
            Me.btnBackspace.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
            Me.btnBackspace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnBackspace.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
            Me.btnBackspace.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.btnBackspace.Location = New System.Drawing.Point(216, 270)
            Me.btnBackspace.Name = "btnBackspace"
            Me.btnBackspace.Size = New System.Drawing.Size(92, 56)
            Me.btnBackspace.TabIndex = 13
            Me.btnBackspace.Text = "⌫ HAPUS"
            Me.btnBackspace.UseVisualStyleBackColor = False
            '
            'btnClear
            '
            Me.btnClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(199, Byte), Integer))
            Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnClear.ForeColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(9, Byte), Integer))
            Me.btnClear.Location = New System.Drawing.Point(12, 334)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(92, 48)
            Me.btnClear.TabIndex = 14
            Me.btnClear.Text = "CLEAR"
            Me.btnClear.UseVisualStyleBackColor = False
            '
            'btnSearch
            '
            Me.btnSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSearch.Font = New System.Drawing.Font("Segoe UI", 11.5!, System.Drawing.FontStyle.Bold)
            Me.btnSearch.ForeColor = System.Drawing.Color.White
            Me.btnSearch.Location = New System.Drawing.Point(114, 334)
            Me.btnSearch.Name = "btnSearch"
            Me.btnSearch.Size = New System.Drawing.Size(194, 48)
            Me.btnSearch.TabIndex = 15
            Me.btnSearch.Text = "🔍 CARI MEMBER"
            Me.btnSearch.UseVisualStyleBackColor = False
            '
            'pnlMiddle
            '
            Me.pnlMiddle.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlMiddle.Controls.Add(Me.grpActiveRedeems)
            Me.pnlMiddle.Controls.Add(Me.grpBenefits)
            Me.pnlMiddle.Controls.Add(Me.grpMemberInfo)
            Me.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlMiddle.Location = New System.Drawing.Point(320, 60)
            Me.pnlMiddle.Name = "pnlMiddle"
            Me.pnlMiddle.Padding = New System.Windows.Forms.Padding(6, 6, 8, 6)
            Me.pnlMiddle.Size = New System.Drawing.Size(500, 524)
            Me.pnlMiddle.TabIndex = 2
            '
            'grpMemberInfo
            '
            Me.grpMemberInfo.Controls.Add(Me.lblStatusBadge)
            Me.grpMemberInfo.Controls.Add(Me.lblNameTitle)
            Me.grpMemberInfo.Controls.Add(Me.lblMemberName)
            Me.grpMemberInfo.Controls.Add(Me.lblPhoneTitle)
            Me.grpMemberInfo.Controls.Add(Me.lblMemberPhone)
            Me.grpMemberInfo.Controls.Add(Me.lblLevelTitle)
            Me.grpMemberInfo.Controls.Add(Me.lblMemberLevel)
            Me.grpMemberInfo.Controls.Add(Me.lblPointsTitle)
            Me.grpMemberInfo.Controls.Add(Me.lblMemberPoints)
            Me.grpMemberInfo.Controls.Add(Me.btnUsePoint)
            Me.grpMemberInfo.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpMemberInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpMemberInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.grpMemberInfo.Location = New System.Drawing.Point(6, 6)
            Me.grpMemberInfo.Name = "grpMemberInfo"
            Me.grpMemberInfo.Size = New System.Drawing.Size(486, 165)
            Me.grpMemberInfo.TabIndex = 0
            Me.grpMemberInfo.TabStop = False
            Me.grpMemberInfo.Text = "Informasi Profil Member"
            '
            'lblStatusBadge
            '
            Me.lblStatusBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.lblStatusBadge.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
            Me.lblStatusBadge.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblStatusBadge.Location = New System.Drawing.Point(210, 18)
            Me.lblStatusBadge.Name = "lblStatusBadge"
            Me.lblStatusBadge.Size = New System.Drawing.Size(115, 24)
            Me.lblStatusBadge.TabIndex = 8
            Me.lblStatusBadge.Text = "BELUM DICARI"
            Me.lblStatusBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblNameTitle
            '
            Me.lblNameTitle.AutoSize = True
            Me.lblNameTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblNameTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblNameTitle.Location = New System.Drawing.Point(10, 22)
            Me.lblNameTitle.Name = "lblNameTitle"
            Me.lblNameTitle.Size = New System.Drawing.Size(116, 20)
            Me.lblNameTitle.TabIndex = 0
            Me.lblNameTitle.Text = "Nama Lengkap :"
            '
            'lblMemberName
            '
            Me.lblMemberName.AutoSize = True
            Me.lblMemberName.Font = New System.Drawing.Font("Segoe UI", 11.5!, System.Drawing.FontStyle.Bold)
            Me.lblMemberName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(45, Byte), Integer))
            Me.lblMemberName.Location = New System.Drawing.Point(10, 40)
            Me.lblMemberName.Name = "lblMemberName"
            Me.lblMemberName.Size = New System.Drawing.Size(20, 28)
            Me.lblMemberName.TabIndex = 1
            Me.lblMemberName.Text = "-"
            '
            'lblPhoneTitle
            '
            Me.lblPhoneTitle.AutoSize = True
            Me.lblPhoneTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblPhoneTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblPhoneTitle.Location = New System.Drawing.Point(10, 68)
            Me.lblPhoneTitle.Name = "lblPhoneTitle"
            Me.lblPhoneTitle.Size = New System.Drawing.Size(118, 20)
            Me.lblPhoneTitle.TabIndex = 2
            Me.lblPhoneTitle.Text = "No Handphone :"
            '
            'lblMemberPhone
            '
            Me.lblMemberPhone.AutoSize = True
            Me.lblMemberPhone.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblMemberPhone.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.lblMemberPhone.Location = New System.Drawing.Point(10, 85)
            Me.lblMemberPhone.Name = "lblMemberPhone"
            Me.lblMemberPhone.Size = New System.Drawing.Size(16, 21)
            Me.lblMemberPhone.TabIndex = 3
            Me.lblMemberPhone.Text = "-"
            '
            'lblLevelTitle
            '
            Me.lblLevelTitle.AutoSize = True
            Me.lblLevelTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblLevelTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblLevelTitle.Location = New System.Drawing.Point(165, 68)
            Me.lblLevelTitle.Name = "lblLevelTitle"
            Me.lblLevelTitle.Size = New System.Drawing.Size(116, 20)
            Me.lblLevelTitle.TabIndex = 4
            Me.lblLevelTitle.Text = "Level / Scheme :"
            '
            'lblMemberLevel
            '
            Me.lblMemberLevel.AutoSize = True
            Me.lblMemberLevel.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblMemberLevel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.lblMemberLevel.Location = New System.Drawing.Point(165, 85)
            Me.lblMemberLevel.Name = "lblMemberLevel"
            Me.lblMemberLevel.Size = New System.Drawing.Size(16, 21)
            Me.lblMemberLevel.TabIndex = 5
            Me.lblMemberLevel.Text = "-"
            '
            'lblPointsTitle
            '
            Me.lblPointsTitle.AutoSize = True
            Me.lblPointsTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblPointsTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblPointsTitle.Location = New System.Drawing.Point(10, 112)
            Me.lblPointsTitle.Name = "lblPointsTitle"
            Me.lblPointsTitle.Size = New System.Drawing.Size(150, 20)
            Me.lblPointsTitle.TabIndex = 6
            Me.lblPointsTitle.Text = "Saldo Poin (Reward) :"
            '
            'lblMemberPoints
            '
            Me.lblMemberPoints.AutoSize = True
            Me.lblMemberPoints.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblMemberPoints.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblMemberPoints.Location = New System.Drawing.Point(10, 130)
            Me.lblMemberPoints.Name = "lblMemberPoints"
            Me.lblMemberPoints.Size = New System.Drawing.Size(122, 28)
            Me.lblMemberPoints.TabIndex = 7
            Me.lblMemberPoints.Text = "0 Pts (Rp 0)"
            '
            'btnUsePoint
            '
            Me.btnUsePoint.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnUsePoint.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnUsePoint.Enabled = False
            Me.btnUsePoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnUsePoint.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnUsePoint.ForeColor = System.Drawing.Color.White
            Me.btnUsePoint.Location = New System.Drawing.Point(165, 122)
            Me.btnUsePoint.Name = "btnUsePoint"
            Me.btnUsePoint.Size = New System.Drawing.Size(160, 40)
            Me.btnUsePoint.TabIndex = 9
            Me.btnUsePoint.Text = "⭐ Gunakan Point"
            Me.btnUsePoint.UseVisualStyleBackColor = False
            '
            'grpBenefits
            '
            Me.grpBenefits.Controls.Add(Me.lstDeals)
            Me.grpBenefits.Controls.Add(Me.btnUseSelectedVoucher)
            Me.grpBenefits.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpBenefits.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpBenefits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.grpBenefits.Location = New System.Drawing.Point(6, 171)
            Me.grpBenefits.Name = "grpBenefits"
            Me.grpBenefits.Padding = New System.Windows.Forms.Padding(8)
            Me.grpBenefits.Size = New System.Drawing.Size(486, 205)
            Me.grpBenefits.TabIndex = 1
            Me.grpBenefits.TabStop = False
            Me.grpBenefits.Text = "Daftar Benefit / Voucher Member"
            '
            'lstDeals
            '
            Me.lstDeals.BackColor = System.Drawing.Color.White
            Me.lstDeals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lstDeals.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lstDeals.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.lstDeals.Font = New System.Drawing.Font("Segoe UI", 10.0!)
            Me.lstDeals.FormattingEnabled = True
            Me.lstDeals.ItemHeight = 46
            Me.lstDeals.Location = New System.Drawing.Point(8, 28)
            Me.lstDeals.Name = "lstDeals"
            Me.lstDeals.Size = New System.Drawing.Size(470, 131)
            Me.lstDeals.TabIndex = 0
            '
            'btnUseSelectedVoucher
            '
            Me.btnUseSelectedVoucher.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnUseSelectedVoucher.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnUseSelectedVoucher.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.btnUseSelectedVoucher.Enabled = False
            Me.btnUseSelectedVoucher.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnUseSelectedVoucher.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnUseSelectedVoucher.ForeColor = System.Drawing.Color.White
            Me.btnUseSelectedVoucher.Location = New System.Drawing.Point(8, 159)
            Me.btnUseSelectedVoucher.Name = "btnUseSelectedVoucher"
            Me.btnUseSelectedVoucher.Size = New System.Drawing.Size(470, 38)
            Me.btnUseSelectedVoucher.TabIndex = 1
            Me.btnUseSelectedVoucher.Text = "🎫 GUNAKAN VOUCHER"
            Me.btnUseSelectedVoucher.UseVisualStyleBackColor = False
            '
            'grpActiveRedeems
            '
            Me.grpActiveRedeems.Controls.Add(Me.lstActiveRedeems)
            Me.grpActiveRedeems.Controls.Add(Me.lblActiveRedeemsInfo)
            Me.grpActiveRedeems.Controls.Add(Me.pnlActiveAction)
            Me.grpActiveRedeems.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpActiveRedeems.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpActiveRedeems.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.grpActiveRedeems.Location = New System.Drawing.Point(6, 376)
            Me.grpActiveRedeems.Name = "grpActiveRedeems"
            Me.grpActiveRedeems.Padding = New System.Windows.Forms.Padding(8)
            Me.grpActiveRedeems.Size = New System.Drawing.Size(486, 142)
            Me.grpActiveRedeems.TabIndex = 2
            Me.grpActiveRedeems.TabStop = False
            Me.grpActiveRedeems.Text = "📋 Voucher & Poin Terpakai (0)"
            '
            'lblActiveRedeemsInfo
            '
            Me.lblActiveRedeemsInfo.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblActiveRedeemsInfo.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Italic)
            Me.lblActiveRedeemsInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblActiveRedeemsInfo.Location = New System.Drawing.Point(8, 22)
            Me.lblActiveRedeemsInfo.Name = "lblActiveRedeemsInfo"
            Me.lblActiveRedeemsInfo.Size = New System.Drawing.Size(470, 20)
            Me.lblActiveRedeemsInfo.TabIndex = 0
            Me.lblActiveRedeemsInfo.Text = "Belum ada voucher atau poin terpakai."
            '
            'lstActiveRedeems
            '
            Me.lstActiveRedeems.BackColor = System.Drawing.Color.White
            Me.lstActiveRedeems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lstActiveRedeems.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lstActiveRedeems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.lstActiveRedeems.Font = New System.Drawing.Font("Segoe UI", 9.5!)
            Me.lstActiveRedeems.FormattingEnabled = True
            Me.lstActiveRedeems.ItemHeight = 44
            Me.lstActiveRedeems.Location = New System.Drawing.Point(8, 42)
            Me.lstActiveRedeems.Name = "lstActiveRedeems"
            Me.lstActiveRedeems.Size = New System.Drawing.Size(470, 58)
            Me.lstActiveRedeems.TabIndex = 1
            '
            'pnlActiveAction
            '
            Me.pnlActiveAction.Controls.Add(Me.btnCancelActiveItem)
            Me.pnlActiveAction.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlActiveAction.Location = New System.Drawing.Point(8, 100)
            Me.pnlActiveAction.Name = "pnlActiveAction"
            Me.pnlActiveAction.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
            Me.pnlActiveAction.Size = New System.Drawing.Size(470, 34)
            Me.pnlActiveAction.TabIndex = 2
            '
            'btnCancelActiveItem
            '
            Me.btnCancelActiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.btnCancelActiveItem.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnCancelActiveItem.Dock = System.Windows.Forms.DockStyle.Fill
            Me.btnCancelActiveItem.Enabled = False
            Me.btnCancelActiveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancelActiveItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnCancelActiveItem.ForeColor = System.Drawing.Color.White
            Me.btnCancelActiveItem.Location = New System.Drawing.Point(0, 4)
            Me.btnCancelActiveItem.Name = "btnCancelActiveItem"
            Me.btnCancelActiveItem.Size = New System.Drawing.Size(470, 30)
            Me.btnCancelActiveItem.TabIndex = 0
            Me.btnCancelActiveItem.Text = "❌ BATALKAN ITEM TERPILIH"
            Me.btnCancelActiveItem.UseVisualStyleBackColor = False
            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(242, Byte), Integer))
            Me.pnlBottom.Controls.Add(Me.lblSearchStatus)
            Me.pnlBottom.Controls.Add(Me.btnScanQrVoucher)
            Me.pnlBottom.Controls.Add(Me.btnSelect)
            Me.pnlBottom.Controls.Add(Me.btnCancel)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 584)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Size = New System.Drawing.Size(820, 66)
            Me.pnlBottom.TabIndex = 3
            '
            'lblSearchStatus
            '
            Me.lblSearchStatus.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Italic)
            Me.lblSearchStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSearchStatus.Location = New System.Drawing.Point(12, 10)
            Me.lblSearchStatus.Name = "lblSearchStatus"
            Me.lblSearchStatus.Size = New System.Drawing.Size(210, 44)
            Me.lblSearchStatus.TabIndex = 3
            Me.lblSearchStatus.Text = "Silakan masukkan nomor handphone"
            Me.lblSearchStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnScanQrVoucher
            '
            Me.btnScanQrVoucher.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnScanQrVoucher.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(20, Byte), Integer))
            Me.btnScanQrVoucher.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnScanQrVoucher.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.btnScanQrVoucher.ForeColor = System.Drawing.Color.White
            Me.btnScanQrVoucher.Location = New System.Drawing.Point(230, 12)
            Me.btnScanQrVoucher.Name = "btnScanQrVoucher"
            Me.btnScanQrVoucher.Size = New System.Drawing.Size(175, 42)
            Me.btnScanQrVoucher.TabIndex = 2
            Me.btnScanQrVoucher.Text = "🎟️ REDEEM KODE / QR"
            Me.btnScanQrVoucher.UseVisualStyleBackColor = False
            '
            'btnSelect
            '
            Me.btnSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSelect.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnSelect.Enabled = False
            Me.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSelect.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.btnSelect.ForeColor = System.Drawing.Color.White
            Me.btnSelect.Location = New System.Drawing.Point(415, 12)
            Me.btnSelect.Name = "btnSelect"
            Me.btnSelect.Size = New System.Drawing.Size(240, 42)
            Me.btnSelect.TabIndex = 1
            Me.btnSelect.Text = "✓ PILIH MEMBER"
            Me.btnSelect.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.5!)
            Me.btnCancel.ForeColor = System.Drawing.Color.White
            Me.btnCancel.Location = New System.Drawing.Point(665, 12)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(145, 42)
            Me.btnCancel.TabIndex = 0
            Me.btnCancel.Text = "BATAL"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'FormSearchMember
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(820, 650)
            Me.Controls.Add(Me.pnlMiddle)
            Me.Controls.Add(Me.pnlLeft)
            Me.Controls.Add(Me.pnlBottom)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FormSearchMember"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Goapp CRM - Pencarian Member"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlLeft.ResumeLayout(False)
            Me.pnlLeft.PerformLayout()
            Me.pnlMiddle.ResumeLayout(False)
            Me.grpBenefits.ResumeLayout(False)
            Me.grpMemberInfo.ResumeLayout(False)
            Me.grpMemberInfo.PerformLayout()
            Me.grpActiveRedeems.ResumeLayout(False)
            Me.pnlActiveAction.ResumeLayout(False)
            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
