Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FormActiveRedeem
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
        Friend WithEvents lstActiveRedeems As System.Windows.Forms.ListBox
        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents btnCancelRedeem As System.Windows.Forms.Button
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents lblInfo As System.Windows.Forms.Label

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblHeaderTitle = New System.Windows.Forms.Label()
            Me.lblHeaderSubtitle = New System.Windows.Forms.Label()
            Me.lstActiveRedeems = New System.Windows.Forms.ListBox()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.lblInfo = New System.Windows.Forms.Label()
            Me.btnCancelRedeem = New System.Windows.Forms.Button()
            Me.btnClose = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblHeaderTitle)
            Me.pnlHeader.Controls.Add(Me.lblHeaderSubtitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 12, 16, 12)
            Me.pnlHeader.Size = New System.Drawing.Size(684, 65)
            Me.pnlHeader.TabIndex = 0
            '
            'lblHeaderTitle
            '
            Me.lblHeaderTitle.AutoSize = True
            Me.lblHeaderTitle.Font = New System.Drawing.Font("Segoe UI", 13.0!, System.Drawing.FontStyle.Bold)
            Me.lblHeaderTitle.ForeColor = System.Drawing.Color.White
            Me.lblHeaderTitle.Location = New System.Drawing.Point(14, 9)
            Me.lblHeaderTitle.Name = "lblHeaderTitle"
            Me.lblHeaderTitle.Size = New System.Drawing.Size(374, 30)
            Me.lblHeaderTitle.TabIndex = 0
            Me.lblHeaderTitle.Text = "📋 Daftar Voucher & Poin Terpakai"
            '
            'lblHeaderSubtitle
            '
            Me.lblHeaderSubtitle.AutoSize = True
            Me.lblHeaderSubtitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblHeaderSubtitle.Location = New System.Drawing.Point(16, 38)
            Me.lblHeaderSubtitle.Name = "lblHeaderSubtitle"
            Me.lblHeaderSubtitle.Size = New System.Drawing.Size(438, 20)
            Me.lblHeaderSubtitle.TabIndex = 1
            Me.lblHeaderSubtitle.Text = "Voucher/Poin yang berstatus Belum Masuk POS dapat dibatalkan kasir"
            '
            'lstActiveRedeems
            '
            Me.lstActiveRedeems.BackColor = System.Drawing.Color.White
            Me.lstActiveRedeems.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.lstActiveRedeems.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lstActiveRedeems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.lstActiveRedeems.Font = New System.Drawing.Font("Segoe UI", 10.0!)
            Me.lstActiveRedeems.FormattingEnabled = True
            Me.lstActiveRedeems.ItemHeight = 62
            Me.lstActiveRedeems.Location = New System.Drawing.Point(0, 65)
            Me.lstActiveRedeems.Name = "lstActiveRedeems"
            Me.lstActiveRedeems.Size = New System.Drawing.Size(684, 287)
            Me.lstActiveRedeems.TabIndex = 1
            '
            'pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(248, Byte), Integer))
            Me.pnlBottom.Controls.Add(Me.lblInfo)
            Me.pnlBottom.Controls.Add(Me.btnCancelRedeem)
            Me.pnlBottom.Controls.Add(Me.btnClose)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 352)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Padding = New System.Windows.Forms.Padding(12)
            Me.pnlBottom.Size = New System.Drawing.Size(684, 68)
            Me.pnlBottom.TabIndex = 2
            '
            'lblInfo
            '
            Me.lblInfo.AutoSize = True
            Me.lblInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
            Me.lblInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblInfo.Location = New System.Drawing.Point(12, 24)
            Me.lblInfo.Name = "lblInfo"
            Me.lblInfo.Size = New System.Drawing.Size(268, 20)
            Me.lblInfo.TabIndex = 2
            Me.lblInfo.Text = "Pilih item di atas lalu klik tombol Batalkan"
            '
            'btnCancelRedeem
            '
            Me.btnCancelRedeem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancelRedeem.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.btnCancelRedeem.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnCancelRedeem.Enabled = False
            Me.btnCancelRedeem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCancelRedeem.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnCancelRedeem.ForeColor = System.Drawing.Color.White
            Me.btnCancelRedeem.Location = New System.Drawing.Point(344, 12)
            Me.btnCancelRedeem.Name = "btnCancelRedeem"
            Me.btnCancelRedeem.Size = New System.Drawing.Size(200, 44)
            Me.btnCancelRedeem.TabIndex = 1
            Me.btnCancelRedeem.Text = "❌ BATALKAN ITEM"
            Me.btnCancelRedeem.UseVisualStyleBackColor = False
            '
            'btnClose
            '
            Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnClose.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnClose.ForeColor = System.Drawing.Color.White
            Me.btnClose.Location = New System.Drawing.Point(554, 12)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(118, 44)
            Me.btnClose.TabIndex = 0
            Me.btnClose.Text = "TUTUP"
            Me.btnClose.UseVisualStyleBackColor = False
            '
            'FormActiveRedeem
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.CancelButton = Me.btnClose
            Me.ClientSize = New System.Drawing.Size(684, 420)
            Me.Controls.Add(Me.lstActiveRedeems)
            Me.Controls.Add(Me.pnlBottom)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FormActiveRedeem"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Daftar Voucher & Poin Terpakai"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlBottom.ResumeLayout(False)
            Me.pnlBottom.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
