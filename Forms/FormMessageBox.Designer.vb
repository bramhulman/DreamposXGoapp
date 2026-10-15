Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FormMessageBox
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
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents pnlBody As System.Windows.Forms.Panel
        Friend WithEvents lblIcon As System.Windows.Forms.Label
        Friend WithEvents txtMessage As System.Windows.Forms.TextBox
        Friend WithEvents pnlButtons As System.Windows.Forms.Panel
        Friend WithEvents btnButton1 As System.Windows.Forms.Button
        Friend WithEvents btnButton2 As System.Windows.Forms.Button

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.pnlBody = New System.Windows.Forms.Panel()
            Me.txtMessage = New System.Windows.Forms.TextBox()
            Me.lblIcon = New System.Windows.Forms.Label()
            Me.pnlButtons = New System.Windows.Forms.Panel()
            Me.btnButton1 = New System.Windows.Forms.Button()
            Me.btnButton2 = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.pnlBody.SuspendLayout()
            Me.pnlButtons.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 12, 16, 12)
            Me.pnlHeader.Size = New System.Drawing.Size(480, 48)
            Me.pnlHeader.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblTitle.ForeColor = System.Drawing.Color.White
            Me.lblTitle.Location = New System.Drawing.Point(14, 10)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(102, 28)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Informasi"
            '
            'pnlBody
            '
            Me.pnlBody.BackColor = System.Drawing.Color.White
            Me.pnlBody.Controls.Add(Me.txtMessage)
            Me.pnlBody.Controls.Add(Me.lblIcon)
            Me.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlBody.Location = New System.Drawing.Point(0, 48)
            Me.pnlBody.Name = "pnlBody"
            Me.pnlBody.Padding = New System.Windows.Forms.Padding(16)
            Me.pnlBody.Size = New System.Drawing.Size(480, 154)
            Me.pnlBody.TabIndex = 1
            '
            'txtMessage
            '
            Me.txtMessage.BackColor = System.Drawing.Color.White
            Me.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtMessage.Font = New System.Drawing.Font("Segoe UI", 11.0!)
            Me.txtMessage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.txtMessage.Location = New System.Drawing.Point(82, 20)
            Me.txtMessage.Multiline = True
            Me.txtMessage.Name = "txtMessage"
            Me.txtMessage.ReadOnly = True
            Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtMessage.Size = New System.Drawing.Size(380, 114)
            Me.txtMessage.TabIndex = 1
            Me.txtMessage.TabStop = False
            '
            'lblIcon
            '
            Me.lblIcon.Font = New System.Drawing.Font("Segoe UI", 28.0!)
            Me.lblIcon.Location = New System.Drawing.Point(12, 14)
            Me.lblIcon.Name = "lblIcon"
            Me.lblIcon.Size = New System.Drawing.Size(64, 64)
            Me.lblIcon.TabIndex = 0
            Me.lblIcon.Text = "ℹ️"
            Me.lblIcon.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'pnlButtons
            '
            Me.pnlButtons.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.pnlButtons.Controls.Add(Me.btnButton2)
            Me.pnlButtons.Controls.Add(Me.btnButton1)
            Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlButtons.Location = New System.Drawing.Point(0, 202)
            Me.pnlButtons.Name = "pnlButtons"
            Me.pnlButtons.Padding = New System.Windows.Forms.Padding(16, 10, 16, 10)
            Me.pnlButtons.Size = New System.Drawing.Size(480, 68)
            Me.pnlButtons.TabIndex = 2
            '
            'btnButton1
            '
            Me.btnButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnButton1.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnButton1.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnButton1.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
            Me.btnButton1.ForeColor = System.Drawing.Color.White
            Me.btnButton1.Location = New System.Drawing.Point(310, 11)
            Me.btnButton1.Name = "btnButton1"
            Me.btnButton1.Size = New System.Drawing.Size(154, 46)
            Me.btnButton1.TabIndex = 0
            Me.btnButton1.Text = "OK"
            Me.btnButton1.UseVisualStyleBackColor = False
            '
            'btnButton2
            '
            Me.btnButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnButton2.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnButton2.Cursor = System.Windows.Forms.Cursors.Hand
            Me.btnButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnButton2.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
            Me.btnButton2.ForeColor = System.Drawing.Color.White
            Me.btnButton2.Location = New System.Drawing.Point(146, 11)
            Me.btnButton2.Name = "btnButton2"
            Me.btnButton2.Size = New System.Drawing.Size(154, 46)
            Me.btnButton2.TabIndex = 1
            Me.btnButton2.Text = "BATAL"
            Me.btnButton2.UseVisualStyleBackColor = False
            Me.btnButton2.Visible = False
            '
            'FormMessageBox
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(480, 270)
            Me.Controls.Add(Me.pnlBody)
            Me.Controls.Add(Me.pnlButtons)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FormMessageBox"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Pesan Sistem"
            Me.TopMost = True
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlBody.ResumeLayout(False)
            Me.pnlBody.PerformLayout()
            Me.pnlButtons.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
