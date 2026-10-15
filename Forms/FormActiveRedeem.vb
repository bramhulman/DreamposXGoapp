Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services

Namespace Forms
    ''' <summary>
    ''' Form modal touch screen untuk melihat daftar voucher dan poin yang baru saja di-redeem
    ''' pada sesi transaksi saat itu, serta memberikan aksi pembatalan (Cancel) selama belum masuk ke POS (StatusPos = 0).
    ''' </summary>
    Public Class FormActiveRedeem
        Private ReadOnly _client As GoappApiClient
        Private ReadOnly _transactionRef As String
        Private ReadOnly _memberUid As Long?
        Private _items As New List(Of GoappRedeemLogItem)()

        ''' <summary>
        ''' Event yang ditembakkan saat sebuah voucher atau poin berhasil dibatalkan,
        ''' agar form induk dapat mengupdate state lokalnya (misal: membersihkan RedeemedVoucher / RedeemedPoint).
        ''' </summary>
        Public Event ItemCancelled(sender As Object, item As GoappRedeemLogItem)

        ''' <summary>
        ''' Inisialisasi FormActiveRedeem
        ''' </summary>
        ''' <param name="client">Instance GoappApiClient</param>
        ''' <param name="transactionRef">Nomor referensi transaksi POS</param>
        ''' <param name="memberUid">UID member Goapp aktif</param>
        Public Sub New(client As GoappApiClient, transactionRef As String, Optional memberUid As Long? = Nothing)
            If client Is Nothing Then
                Throw New ArgumentNullException(NameOf(client))
            End If

            Me.InitializeComponent()
            _client = client
            _transactionRef = If(transactionRef, String.Empty).Trim()
            _memberUid = memberUid
        End Sub

        Private Async Sub FormActiveRedeem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Await LoadActiveRedeemsAsync()
        End Sub

        ''' <summary>
        ''' Memuat daftar item redeem yang aktif (Status = 0) dan belum masuk ke POS (StatusPos = 0)
        ''' </summary>
        Public Async Function LoadActiveRedeemsAsync() As Task
            lblInfo.Text = "Memuat data voucher/poin terpakai dari database..."
            lstActiveRedeems.Items.Clear()

            Try
                _items = Await _client.RedeemRepo.GetActiveLogsByTxRefAsync(_transactionRef)
                If _items IsNot Nothing AndAlso _items.Count > 0 Then
                    For Each itm In _items
                        lstActiveRedeems.Items.Add(itm)
                    Next
                    lblInfo.Text = $"{_items.Count} item ditemukan. Pilih item untuk membatalkan."
                Else
                    _items = New List(Of GoappRedeemLogItem)()
                    lstActiveRedeems.Items.Add("(Tidak ada voucher atau poin aktif yang belum masuk ke POS)")
                    lblInfo.Text = "Tidak ada voucher/poin aktif yang belum masuk POS."
                End If
            Catch ex As Exception
                lblInfo.Text = "Gagal memuat data: " & ex.Message
            Finally
                btnCancelRedeem.Enabled = False
            End Try
        End Function

        Private Sub lstActiveRedeems_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstActiveRedeems.DrawItem
            If e.Index < 0 Then Return

            Dim isSelected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim g = e.Graphics
            Dim bounds = e.Bounds

            ' Background
            Dim bgBrush As Brush
            If isSelected Then
                bgBrush = New SolidBrush(Color.FromArgb(220, 235, 255))
            Else
                bgBrush = If(e.Index Mod 2 = 0, Brushes.White, New SolidBrush(Color.FromArgb(248, 250, 252)))
            End If
            g.FillRectangle(bgBrush, bounds)

            If _items IsNot Nothing AndAlso e.Index < _items.Count Then
                Dim itm = _items(e.Index)

                ' 1. Badge Jenis (Voucher / Poin)
                Dim isVoucher = String.Equals(itm.Jenis, "RedeemVoucher", StringComparison.OrdinalIgnoreCase)
                Dim badgeColor = If(isVoucher, Color.FromArgb(253, 126, 20), Color.FromArgb(25, 135, 84))
                Dim badgeText = If(isVoucher, "🎟 VOUCHER", "⭐ POIN")
                Dim badgeRect As New Rectangle(bounds.Left + 10, bounds.Top + 10, 100, 42)

                Using bBrush As New SolidBrush(If(isSelected, badgeColor, Color.FromArgb(240, 243, 248)))
                    g.FillRectangle(bBrush, badgeRect)
                End Using
                Using bPen As New Pen(badgeColor, 1.5!)
                    g.DrawRectangle(bPen, badgeRect)
                End Using
                Using bFont As New Font("Segoe UI", 9.0!, FontStyle.Bold)
                    Dim sfB As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    Dim tColor = If(isSelected, Color.White, badgeColor)
                    Using tBrush As New SolidBrush(tColor)
                        g.DrawString(badgeText, bFont, tBrush, badgeRect, sfB)
                    End Using
                End Using

                ' 2. Info Detail (Kode, Nama, Nilai)
                Dim textLeft = bounds.Left + 120
                Dim textWidth = bounds.Width - 130 - 150
                Dim titleRect As New Rectangle(textLeft, bounds.Top + 10, textWidth, 22)
                Dim descRect As New Rectangle(textLeft, bounds.Top + 32, textWidth, 20)

                Dim titleText = $"[{itm.Kode}] {itm.NamaVoucher}"
                Dim nilaiDesc = If(itm.NilaiPoint > 0, $"Nilai: Rp {itm.NilaiPoint:N0}", "Reward: Free Item")
                Dim descText = $"{nilaiDesc} | Jam: {itm.Waktu:HH:mm:ss} | Status: Belum Masuk POS"

                Using titleFont As New Font("Segoe UI", 10.5!, FontStyle.Bold)
                    Using titleBrush As New SolidBrush(Color.FromArgb(30, 40, 55))
                        Dim sfT As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                        g.DrawString(titleText, titleFont, titleBrush, titleRect, sfT)
                    End Using
                End Using

                Using descFont As New Font("Segoe UI", 9.0!, FontStyle.Regular)
                    Using descBrush As New SolidBrush(Color.FromArgb(90, 100, 115))
                        Dim sfD As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                        g.DrawString(descText, descFont, descBrush, descRect, sfD)
                    End Using
                End Using

                ' 3. Badge Status POS (Kanan)
                Dim statusRect As New Rectangle(bounds.Right - 145, bounds.Top + 15, 135, 30)
                Using tagBrush As New SolidBrush(Color.FromArgb(255, 243, 205))
                    g.FillRectangle(tagBrush, statusRect)
                End Using
                Using tagPen As New Pen(Color.FromArgb(255, 193, 7))
                    g.DrawRectangle(tagPen, statusRect)
                End Using
                Using tagFont As New Font("Segoe UI", 8.0!, FontStyle.Bold)
                    Dim sfTag As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    Using tagTextBrush As New SolidBrush(Color.FromArgb(102, 77, 3))
                        g.DrawString("⏳ BELUM MASUK POS", tagFont, tagTextBrush, statusRect, sfTag)
                    End Using
                End Using
            Else
                ' Placeholder
                Dim itemText = If(e.Index < lstActiveRedeems.Items.Count, lstActiveRedeems.Items(e.Index).ToString(), "")
                Using phFont As New Font("Segoe UI", 9.5!, FontStyle.Italic)
                    Using phBrush As New SolidBrush(Color.FromArgb(120, 130, 145))
                        Dim sfPh As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                        g.DrawString(itemText, phFont, phBrush, bounds, sfPh)
                    End Using
                End Using
            End If

            ' Garis pemisah bawah
            Using linePen As New Pen(Color.FromArgb(230, 235, 242))
                g.DrawLine(linePen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1)
            End Using

            If Not isSelected AndAlso bgBrush IsNot Nothing AndAlso bgBrush IsNot Brushes.White Then
                bgBrush.Dispose()
            End If
        End Sub

        Private Sub lstActiveRedeems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstActiveRedeems.SelectedIndexChanged
            Dim hasValidSelection = (lstActiveRedeems.SelectedIndex >= 0 AndAlso _items IsNot Nothing AndAlso lstActiveRedeems.SelectedIndex < _items.Count)
            btnCancelRedeem.Enabled = hasValidSelection
        End Sub

        Private Async Sub btnCancelRedeem_Click(sender As Object, e As EventArgs) Handles btnCancelRedeem.Click
            If lstActiveRedeems.SelectedIndex < 0 OrElse _items Is Nothing OrElse lstActiveRedeems.SelectedIndex >= _items.Count Then
                Return
            End If

            Dim selected = _items(lstActiveRedeems.SelectedIndex)
            If Not selected.CanCancelFromDll Then
                FormMessageBox.Show(Me, "Item ini sudah masuk ke transaksi POS atau sudah dibatalkan sebelumnya sehingga tidak dapat dibatalkan lagi dari form ini.", "Tidak Dapat Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Konfirmasi pembatalan
            Dim confirmText = $"Apakah Anda yakin ingin membatalkan penggunaan {If(selected.Jenis = "RedeemVoucher", "Voucher", "Poin")} berikut?{Environment.NewLine}{Environment.NewLine}" &
                              $"- Kode: {selected.Kode}{Environment.NewLine}" &
                              $"- Nama: {selected.NamaVoucher}{Environment.NewLine}" &
                              $"- Nilai: {If(selected.NilaiPoint > 0, "Rp " & selected.NilaiPoint.ToString("N0"), "-")}{Environment.NewLine}{Environment.NewLine}" &
                              $"Item akan dibatalkan dari sistem Goapp dan tidak akan masuk ke POS."

            If FormMessageBox.Show(Me, confirmText, "Konfirmasi Pembatalan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            SetProcessing(True)
            lblInfo.Text = $"Menghubungi server Goapp untuk membatalkan {selected.Kode}..."

            Try
                Dim isSuccess As Boolean = False
                Dim errMsg As String = ""

                If String.Equals(selected.Jenis, "RedeemVoucher", StringComparison.OrdinalIgnoreCase) Then
                    ' Batalkan Voucher via API Goapp
                    Dim cancelResp = Await _client.CancelVoucherAsync(selected.Kode, selected.TransactionRef, _memberUid)
                    isSuccess = cancelResp.IsSuccess
                    errMsg = cancelResp.Message
                Else
                    ' Batalkan Poin (Burn Point) via API Goapp
                    Dim cancelResp = Await _client.CancelPointPaymentAsync(selected.Kode, "Dibatalkan kasir di DLL sebelum masuk POS")
                    isSuccess = cancelResp.IsSuccess
                    errMsg = cancelResp.Message
                End If

                If isSuccess Then
                    ' Update status di database menjadi 1 (Cancel)
                    Await _client.RedeemRepo.UpdateStatusCancelAsync(selected.Kode, selected.Jenis)

                    FormMessageBox.Show(Me, $"Penggunaan {selected.Kode} berhasil dibatalkan dari Goapp CRM!", "Sukses Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Tembakkan event ke form induk agar state lokal direset
                    RaiseEvent ItemCancelled(Me, selected)

                    ' Refresh daftar
                    Await LoadActiveRedeemsAsync()
                Else
                    FormMessageBox.Show(Me, $"Gagal membatalkan item di Goapp CRM:{Environment.NewLine}{errMsg}", "Gagal Batal", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    lblInfo.Text = "Gagal membatalkan item: " & errMsg
                End If

            Catch ex As Exception
                FormMessageBox.Show(Me, $"Terjadi exception saat membatalkan:{Environment.NewLine}{ex.Message}", "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                lblInfo.Text = "Error: " & ex.Message
            Finally
                SetProcessing(False)
            End Try
        End Sub

        Private Sub SetProcessing(isProcessing As Boolean)
            btnCancelRedeem.Enabled = Not isProcessing
            btnClose.Enabled = Not isProcessing
            lstActiveRedeems.Enabled = Not isProcessing
            Cursor = If(isProcessing, Cursors.WaitCursor, Cursors.Default)
        End Sub
    End Class
End Namespace
