Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace Forms
    ''' <summary>
    ''' Form modal dialog pengganti MessageBox standar Windows yang dirancang khusus untuk layar sentuh POS (Touch Screen).
    ''' Dilengkapi tombol-tombol berukuran besar, warna header yang jelas, dan font yang mudah dibaca.
    ''' </summary>
    Public Class FormMessageBox
        Private Sub New()
            Me.InitializeComponent()
        End Sub

        ''' <summary>
        ''' Menampilkan pesan modal dialog layar sentuh dengan parent window tertentu.
        ''' </summary>
        Public Overloads Shared Function Show(owner As IWin32Window,
                                              message As String,
                                              Optional title As String = "",
                                              Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                              Optional icon As MessageBoxIcon = MessageBoxIcon.Information) As DialogResult
            Using frm As New FormMessageBox()
                frm.SetupDialog(message, title, buttons, icon)
                If owner IsNot Nothing Then
                    Return frm.ShowDialog(owner)
                Else
                    Return frm.ShowDialog()
                End If
            End Using
        End Function

        ''' <summary>
        ''' Menampilkan pesan modal dialog layar sentuh.
        ''' </summary>
        Public Overloads Shared Function Show(message As String,
                                              Optional title As String = "",
                                              Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                              Optional icon As MessageBoxIcon = MessageBoxIcon.Information) As DialogResult
            Return Show(Nothing, message, title, buttons, icon)
        End Function

        Private Sub SetupDialog(message As String, title As String, buttons As MessageBoxButtons, icon As MessageBoxIcon)
            ' 1. Pesan & Judul
            txtMessage.Text = message
            If String.IsNullOrWhiteSpace(title) Then
                Select Case icon
                    Case MessageBoxIcon.Error, MessageBoxIcon.Hand, MessageBoxIcon.Stop
                        title = "Error"
                    Case MessageBoxIcon.Warning, MessageBoxIcon.Exclamation
                        title = "Peringatan"
                    Case MessageBoxIcon.Question
                        title = "Konfirmasi"
                    Case Else
                        title = "Informasi"
                End Select
            End If
            lblTitle.Text = title
            Me.Text = title

            ' 2. Ikon & Warna Tema Header
            Dim isSuccess = title.ToLower().Contains("sukses") OrElse title.ToLower().Contains("berhasil")

            Select Case icon
                Case MessageBoxIcon.Error, MessageBoxIcon.Hand, MessageBoxIcon.Stop
                    pnlHeader.BackColor = Color.FromArgb(220, 53, 69) ' Merah
                    lblIcon.Text = "❌"
                    btnButton1.BackColor = Color.FromArgb(220, 53, 69)

                Case MessageBoxIcon.Warning, MessageBoxIcon.Exclamation
                    pnlHeader.BackColor = Color.FromArgb(255, 140, 0) ' Dark Orange / Amber
                    lblIcon.Text = "⚠️"
                    btnButton1.BackColor = Color.FromArgb(255, 140, 0)

                Case MessageBoxIcon.Question
                    pnlHeader.BackColor = Color.FromArgb(13, 110, 253) ' Biru
                    lblIcon.Text = "❓"
                    btnButton1.BackColor = Color.FromArgb(25, 135, 84) ' Tombol YA Hijau

                Case Else
                    If isSuccess Then
                        pnlHeader.BackColor = Color.FromArgb(25, 135, 84) ' Hijau
                        lblIcon.Text = "✅"
                        btnButton1.BackColor = Color.FromArgb(25, 135, 84)
                    Else
                        pnlHeader.BackColor = Color.FromArgb(13, 110, 253) ' Biru
                        lblIcon.Text = "ℹ️"
                        btnButton1.BackColor = Color.FromArgb(13, 110, 253)
                    End If
            End Select

            ' 3. Konfigurasi Tombol Touch Screen
            Select Case buttons
                Case MessageBoxButtons.YesNo
                    btnButton1.Text = "YA"
                    btnButton1.DialogResult = DialogResult.Yes
                    btnButton2.Text = "TIDAK"
                    btnButton2.DialogResult = DialogResult.No
                    btnButton2.BackColor = Color.FromArgb(108, 117, 125)
                    btnButton2.Visible = True
                    Me.AcceptButton = btnButton1
                    Me.CancelButton = btnButton2

                Case MessageBoxButtons.OKCancel
                    btnButton1.Text = "OK"
                    btnButton1.DialogResult = DialogResult.OK
                    btnButton2.Text = "BATAL"
                    btnButton2.DialogResult = DialogResult.Cancel
                    btnButton2.BackColor = Color.FromArgb(108, 117, 125)
                    btnButton2.Visible = True
                    Me.AcceptButton = btnButton1
                    Me.CancelButton = btnButton2

                Case Else ' MessageBoxButtons.OK
                    btnButton1.Text = "OK"
                    btnButton1.DialogResult = DialogResult.OK
                    btnButton2.Visible = False
                    ' Posisikan tombol OK di tengah atau kanan
                    btnButton1.Location = New Point(pnlButtons.ClientSize.Width - btnButton1.Width - 16, btnButton1.Top)
                    Me.AcceptButton = btnButton1
                    Me.CancelButton = btnButton1
            End Select
        End Sub

        Private Sub btnButton1_Click(sender As Object, e As EventArgs) Handles btnButton1.Click
            Me.Close()
        End Sub

        Private Sub btnButton2_Click(sender As Object, e As EventArgs) Handles btnButton2.Click
            Me.Close()
        End Sub
    End Class
End Namespace
