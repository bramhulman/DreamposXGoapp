Imports System
Imports System.Collections.Generic
Imports DreamposXGoapp.Models

Namespace Forms
    ''' <summary>
    ''' Data event yang dikirimkan ke aplikasi POS saat kasir memilih member di FormSearchMember.
    ''' </summary>
    Public Class MemberDataEventArgs
        Inherits EventArgs

        Public Property FullName As String = String.Empty
        Public Property MobileNo As String = String.Empty
        Public Property IsActive As Boolean = True
        Public Property StatusDescription As String = "Active"
        Public Property Uid As Long = 0
        Public Property LevelName As String = "-"
        Public Property SchemeName As String = "-"
        Public Property AvailablePoints As Decimal = 0
        Public Property PointsInRupiah As Decimal = 0
        Public Property ReferralCode As String = String.Empty
        Public Property DealCodes As List(Of MemberDealCodeResponse)
        Public Property RawMember As MemberResponse
        Public Property RedeemedVoucher As RedeemedVoucherInfo
        Public Property RedeemedPoint As RedeemedPointInfo

        Public Sub New()
            DealCodes = New List(Of MemberDealCodeResponse)()
        End Sub
    End Class
End Namespace
