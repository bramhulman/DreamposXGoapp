Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FormRedeemVoucher
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
        Friend WithEvents lblCodePrompt As System.Windows.Forms.Label
        Friend WithEvents txtVoucherCode As System.Windows.Forms.TextBox
        Friend WithEvents lblAuthCodePrompt As System.Windows.Forms.Label
        Friend WithEvents txtAuthCode As System.Windows.Forms.TextBox
        Friend WithEvents grpVoucherInfo As System.Windows.Forms.GroupBox
        Friend WithEvents lblDealNameTitle As System.Windows.Forms.Label
        Friend WithEvents lblDealName As System.Windows.Forms.Label
        Friend WithEvents lblRewardTitle As System.Windows.Forms.Label
        Friend WithEvents lblRewardInfo As System.Windows.Forms.Label
        Friend WithEvents lblStatus As System.Windows.Forms.Label
        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents btnUseVoucher As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblHeaderTitle = New System.Windows.Forms.Label()
            Me.lblHeaderSubtitle = New System.Windows.Forms.Label()
            Me.lblCodePrompt = New System.Windows.Forms.Label()
            Me.txtVoucherCode = New System.Windows.Forms.TextBox()
            Me.lblAuthCodePrompt = New System.Windows.Forms.Label()
            Me.txtAuthCode = New System.Windows.Forms.TextBox()
            Me.grpVoucherInfo = New System.Windows.Forms.GroupBox()
            Me.lblDealNameTitle = New System.Windows.Forms.Label()
            Me.lblDealName = New System.Windows.Forms.Label()
            Me.lblRewardTitle = New System.Windows.Forms.Label()
            Me.lblRewardInfo = New System.Windows.Forms.Label()
            Me.lblStatus = New System.Windows.Forms.Label()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.btnUseVoucher = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.grpVoucherInfo.SuspendLayout()
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
            Me.pnlHeader.Size = New System.Drawing.Size(540, 58)
            Me.pnlHeader.TabIndex = 0
            '
            'lblHeaderTitle
            '
            Me.lblHeaderTitle.AutoSize = True
            Me.lblHeaderTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblHeaderTitle.ForeColor = System.Drawing.Color.White
            Me.lblHeaderTitle.Location = New System.Drawing.Point(14, 7)
            Me.lblHeaderTitle.Name = "lblHeaderTitle"
            Me.lblHeaderTitle.Size = New System.Drawing.Size(252, 28)
            Me.lblHeaderTitle.TabIndex = 0
            Me.lblHeaderTitle.Text = "REDEEM VOUCHER GOAPP"
            '
            'lblHeaderSubtitle
            '
            Me.lblHeaderSubtitle.AutoSize = True
            Me.lblHeaderSubtitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblHeaderSubtitle.Location = New System.Drawing.Point(15, 33)
            Me.lblHeaderSubtitle.Name = "lblHeaderSubtitle"
            Me.lblHeaderSubtitle.Size = New System.Drawing.Size(325, 20)
            Me.lblHeaderSubtitle.TabIndex = 1
            Me.lblHeaderSubtitle.Text = "Validasi & Gunakan Voucher Promo / Free Items"
            '
            'lblCodePrompt
            '
            Me.lblCodePrompt.AutoSize = True
            Me.lblCodePrompt.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblCodePrompt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.lblCodePrompt.Location = New System.Drawing.Point(20, 68)
            Me.lblCodePrompt.Name = "lblCodePrompt"
            Me.lblCodePrompt.Size = New System.Drawing.Size(286, 20)
            Me.lblCodePrompt.TabIndex = 1
            Me.lblCodePrompt.Text = "KODE VOUCHER / SCAN BARCODE QR:"
            '
            'txtVoucherCode
            '
            Me.txtVoucherCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.txtVoucherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtVoucherCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtVoucherCode.Font = New System.Drawing.Font("Segoe UI", 13.0!, System.Drawing.FontStyle.Bold)
            Me.txtVoucherCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.txtVoucherCode.Location = New System.Drawing.Point(20, 90)
            Me.txtVoucherCode.Name = "txtVoucherCode"
            Me.txtVoucherCode.Size = New System.Drawing.Size(500, 36)
            Me.txtVoucherCode.TabIndex = 2
            Me.txtVoucherCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'lblAuthCodePrompt
            '
            Me.lblAuthCodePrompt.AutoSize = True
            Me.lblAuthCodePrompt.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblAuthCodePrompt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.lblAuthCodePrompt.Location = New System.Drawing.Point(20, 134)
            Me.lblAuthCodePrompt.Name = "lblAuthCodePrompt"
            Me.lblAuthCodePrompt.Size = New System.Drawing.Size(325, 20)
            Me.lblAuthCodePrompt.TabIndex = 3
            Me.lblAuthCodePrompt.Text = "KODE OTORISASI / AUTHENTICATION CODE:"
            '
            'txtAuthCode
            '
            Me.txtAuthCode.BackColor = System.Drawing.Color.White
            Me.txtAuthCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtAuthCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtAuthCode.Font = New System.Drawing.Font("Segoe UI", 11.5!, System.Drawing.FontStyle.Bold)
            Me.txtAuthCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.txtAuthCode.Location = New System.Drawing.Point(20, 156)
            Me.txtAuthCode.Name = "txtAuthCode"
            Me.txtAuthCode.Size = New System.Drawing.Size(500, 33)
            Me.txtAuthCode.TabIndex = 4
            Me.txtAuthCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'grpVoucherInfo
            '
            Me.grpVoucherInfo.Controls.Add(Me.lblRewardInfo)
            Me.grpVoucherInfo.Controls.Add(Me.lblRewardTitle)
            Me.grpVoucherInfo.Controls.Add(Me.lblDealName)
            Me.grpVoucherInfo.Controls.Add(Me.lblDealNameTitle)
            Me.grpVoucherInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.grpVoucherInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.grpVoucherInfo.Location = New System.Drawing.Point(20, 198)
            Me.grpVoucherInfo.Name = "grpVoucherInfo"
            Me.grpVoucherInfo.Padding = New System.Windows.Forms.Padding(12)
            Me.grpVoucherInfo.Size = New System.Drawing.Size(500, 125)
            Me.grpVoucherInfo.TabIndex = 5
            Me.grpVoucherInfo.TabStop = False
            Me.grpVoucherInfo.Text = "Informasi Voucher & Hadiah"
            '
            'lblDealNameTitle
            '
            Me.lblDealNameTitle.AutoSize = True
            Me.lblDealNameTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblDealNameTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblDealNameTitle.Location = New System.Drawing.Point(12, 24)
            Me.lblDealNameTitle.Name = "lblDealNameTitle"
            Me.lblDealNameTitle.Size = New System.Drawing.Size(107, 20)
            Me.lblDealNameTitle.TabIndex = 0
            Me.lblDealNameTitle.Text = "Nama Voucher:"
            '
            'lblDealName
            '
            Me.lblDealName.AutoSize = True
            Me.lblDealName.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
            Me.lblDealName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblDealName.Location = New System.Drawing.Point(12, 44)
            Me.lblDealName.Name = "lblDealName"
            Me.lblDealName.Size = New System.Drawing.Size(16, 21)
            Me.lblDealName.TabIndex = 1
            Me.lblDealName.Text = "-"
            '
            'lblRewardTitle
            '
            Me.lblRewardTitle.AutoSize = True
            Me.lblRewardTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!)
            Me.lblRewardTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(135, Byte), Integer))
            Me.lblRewardTitle.Location = New System.Drawing.Point(12, 70)
            Me.lblRewardTitle.Name = "lblRewardTitle"
            Me.lblRewardTitle.Size = New System.Drawing.Size(155, 20)
            Me.lblRewardTitle.TabIndex = 2
            Me.lblRewardTitle.Text = "Reward / Barang Gratis:"
            '
            'lblRewardInfo
            '
            Me.lblRewardInfo.AutoSize = True
            Me.lblRewardInfo.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.lblRewardInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblRewardInfo.Location = New System.Drawing.Point(12, 90)
            Me.lblRewardInfo.Name = "lblRewardInfo"
            Me.lblRewardInfo.Size = New System.Drawing.Size(17, 23)
            Me.lblRewardInfo.TabIndex = 3
            Me.lblRewardInfo.Text = "-"
            '
            'lblStatus
            '
            Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
            Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblStatus.Location = New System.Drawing.Point(20, 330)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(500, 36)
            Me.lblStatus.TabIndex = 6
            Me.lblStatus.Text = "Tekan 'Gunakan Voucher' untuk validasi & menerapkan voucher ke transaksi."
            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(242, Byte), Integer))
            Me.pnlBottom.Controls.Add(Me.btnCancel)
            Me.pnlBottom.Controls.Add(Me.btnUseVoucher)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 375)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Padding = New System.Windows.Forms.Padding(16, 10, 16, 10)
            Me.pnlBottom.Size = New System.Drawing.Size(540, 65)
            Me.pnlBottom.TabIndex = 7
            '
            'btnUseVoucher
            '
            Me.btnUseVoucher.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnUseVoucher.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnUseVoucher.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnUseVoucher.ForeColor = System.Drawing.Color.White
            Me.btnUseVoucher.Location = New System.Drawing.Point(260, 10)
            Me.btnUseVoucher.Name = "btnUseVoucher"
            Me.btnUseVoucher.Size = New System.Drawing.Size(260, 45)
            Me.btnUseVoucher.TabIndex = 0
            Me.btnUseVoucher.Text = "✔ GUNAKAN VOUCHER"
            Me.btnUseVoucher.UseVisualStyleBackColor = False
            '
            'btnCancel
            '
            Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnCancel.ForeColor = System.Drawing.Color.White
            Me.btnCancel.Location = New System.Drawing.Point(20, 10)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(120, 45)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "BATAL"
            Me.btnCancel.UseVisualStyleBackColor = False
            '
            'FormRedeemVoucher
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(540, 440)
            Me.Controls.Add(Me.pnlBottom)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.grpVoucherInfo)
            Me.Controls.Add(Me.txtAuthCode)
            Me.Controls.Add(Me.lblAuthCodePrompt)
            Me.Controls.Add(Me.txtVoucherCode)
            Me.Controls.Add(Me.lblCodePrompt)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FormRedeemVoucher"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Redeem Voucher Goapp CRM"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.grpVoucherInfo.ResumeLayout(False)
            Me.grpVoucherInfo.PerformLayout()
            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
    End Class
End Namespace
