Imports System
Imports System.Text
Imports DreamposXGoapp.Models

Namespace Services
    ''' <summary>
    ''' Generator format teks struk POS yang memuat informasi CRM Goapp
    ''' (Saldo poin dan Link/QR Survey kepuasan pelanggan)
    ''' </summary>
    Public Class ReceiptGenerator
        ''' <summary>
        ''' Menghasilkan teks struk POS standar 40-kolom siap cetak thermal printer
        ''' </summary>
        Public Shared Function GenerateReceiptText(
            storeName As String,
            orderNo As String,
            cashierName As String,
            member As MemberResponse,
            earnedPoints As Decimal,
            burnedPoints As Decimal,
            orderTotal As Decimal,
            paidTotal As Decimal,
            surveyUrlBase As String) As String

            Dim balance As Decimal? = If(member Is Nothing, CType(Nothing, Decimal?), member.AvailablePoints + earnedPoints - burnedPoints)
            Return GenerateReceiptCore(storeName, orderNo, cashierName, member, earnedPoints, burnedPoints, orderTotal, paidTotal, surveyUrlBase, balance, False)
        End Function

        ''' <summary>Preview struk memakai amount dan est_new_balance hanya dari reward_summary.</summary>
        Public Shared Function GenerateReceiptText(
            storeName As String, orderNo As String, cashierName As String, member As MemberResponse,
            orderResponse As PushOrderResponse, burnedPoints As Decimal, orderTotal As Decimal,
            paidTotal As Decimal, surveyUrlBase As String) As String
            Dim earned As Decimal? = Nothing
            Dim balance As Decimal? = Nothing
            If orderResponse IsNot Nothing Then
                If orderResponse.HasEarningPointSummary Then earned = orderResponse.RewardSummary.Amount
                balance = orderResponse.EstimatedNewBalance
            End If
            Return GenerateReceiptCore(storeName, orderNo, cashierName, member, earned, burnedPoints, orderTotal, paidTotal, surveyUrlBase, balance, True)
        End Function

        Private Shared Function GenerateReceiptCore(
            storeName As String, orderNo As String, cashierName As String, member As MemberResponse,
            earnedPoints As Decimal?, burnedPoints As Decimal, orderTotal As Decimal, paidTotal As Decimal,
            surveyUrlBase As String, finalBalance As Decimal?, fromSummary As Boolean) As String
            Dim sb As New StringBuilder()
            Dim lineSeparator = New String("-"c, 40)
            Dim doubleSeparator = New String("="c, 40)

            sb.AppendLine(CenterText(storeName, 40))
            sb.AppendLine(CenterText("INTEGRASI DREAMPOS X GOAPP", 40))
            sb.AppendLine(doubleSeparator)
            sb.AppendLine($"No Struk : {orderNo}")
            sb.AppendLine($"Tanggal  : {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
            sb.AppendLine($"Kasir    : {cashierName}")
            sb.AppendLine(lineSeparator)

            ' Ringkasan Pembayaran
            sb.AppendLine(PadRow("Total Belanja", $"{orderTotal:N0}"))
            If burnedPoints > 0 Then
                sb.AppendLine(PadRow($"Bayar Poin ({burnedPoints} Pts)", $"-{burnedPoints * 1000:N0}"))
            End If
            sb.AppendLine(PadRow("Total Bayar", $"{paidTotal:N0}"))
            sb.AppendLine(lineSeparator)

            ' Bagian Loyalty / CRM Goapp
            If member IsNot Nothing Then
                sb.AppendLine(CenterText("*** INFO MEMBER GOAPP ***", 40))
                sb.AppendLine($"Member   : {member.FullName}")
                sb.AppendLine($"No HP    : {member.MobileNo}")
                sb.AppendLine($"Level    : {If(member.Level IsNot Nothing, member.Level.Name, "Standard")}")
                sb.AppendLine(lineSeparator)
                sb.AppendLine(PadRow("Poin Diperoleh (Earn)", If(earnedPoints.HasValue, $"+{earnedPoints.GetValueOrDefault():N0} Pts", "Tidak tersedia")))
                If burnedPoints > 0 Then
                    sb.AppendLine(PadRow("Poin Digunakan (Burn)", $"-{burnedPoints:N0} Pts"))
                End If
                sb.AppendLine(PadRow(If(fromSummary, "ESTIMASI SALDO POIN", "SISA SALDO POIN"), If(finalBalance.HasValue, $"{finalBalance.GetValueOrDefault():N0} Pts", "Tidak tersedia")))
                If Not fromSummary AndAlso finalBalance.HasValue Then
                    sb.AppendLine(PadRow("NILAI RUPIAH POIN", $"Rp {finalBalance.Value * 1000:N0}"))
                End If
                sb.AppendLine(lineSeparator)
            End If

            ' Bagian Survey QR / Link
            Dim surveyUrl = $"{surveyUrlBase.TrimEnd("/"c)}/?ref={orderNo}"
            If member IsNot Nothing Then
                surveyUrl &= $"&member_uid={member.Uid}&mobile={member.MobileNo}"
            End If

            sb.AppendLine(CenterText("IKUTI SURVEY KEPUASAN PELANGGAN", 40))
            sb.AppendLine(CenterText("SCAN QR CODE ATAU KUNJUNGI:", 40))
            sb.AppendLine(CenterText(surveyUrl, 40))
            sb.AppendLine(doubleSeparator)
            sb.AppendLine(CenterText("TERIMA KASIH ATAS KUNJUNGAN ANDA", 40))
            sb.AppendLine(Environment.NewLine)

            Return sb.ToString()
        End Function

        Private Shared Function CenterText(text As String, width As Integer) As String
            If text.Length >= width Then Return text
            Dim leftPad = (width - text.Length) \ 2
            Return text.PadLeft(leftPad + text.Length).PadRight(width)
        End Function

        Private Shared Function PadRow(left As String, right As String, Optional width As Integer = 40) As String
            Dim spaceLen = width - left.Length - right.Length
            If spaceLen < 1 Then spaceLen = 1
            Return left & New String(" "c, spaceLen) & right
        End Function
    End Class
End Namespace
