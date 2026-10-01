Imports System.Collections.Generic
Imports Newtonsoft.Json

Namespace Models
    ''' <summary>
    ''' Generic API Response Wrapper untuk DreamPOS
    ''' </summary>
    Public Class ApiResponse(Of T)
        Public Property IsSuccess As Boolean
        Public Property StatusCode As Integer
        Public Property Message As String
        Public Property Data As T
        Public Property RawJson As String
    End Class

    ''' <summary>
    ''' Response autentikasi token Goapp
    ''' </summary>
    Public Class AuthTokenResponse
        <JsonProperty("token")>
        Public Property Token As String

        <JsonProperty("refresh_token")>
        Public Property RefreshToken As String

        <JsonProperty("expired_at")>
        Public Property ExpiredAt As Double

        <JsonProperty("has_password")>
        Public Property HasPassword As Boolean
    End Class

    ''' <summary>
    ''' Detail channel toko / POS
    ''' </summary>
    Public Class ChannelInfoResponse
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("channel_type")>
        Public Property ChannelType As String

        <JsonProperty("icon_url")>
        Public Property IconUrl As String
    End Class

    ''' <summary>
    ''' Detail informasi member CRM Goapp
    ''' </summary>
    Public Class MemberResponse
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("first_name")>
        Public Property FirstName As String

        <JsonProperty("last_name")>
        Public Property LastName As String

        <JsonProperty("mobile_no")>
        Public Property MobileNo As String

        <JsonProperty("email")>
        Public Property Email As String

        <JsonProperty("referral_code")>
        Public Property ReferralCode As String

        <JsonProperty("scheme")>
        Public Property Scheme As MemberScheme

        <JsonProperty("level")>
        Public Property Level As MemberLevel

        <JsonProperty("account")>
        Public Property Account As MemberAccount

        <JsonProperty("direct_deal")>
        Public Property DirectDeal As DirectDealInfo

        Public ReadOnly Property FullName As String
            Get
                Dim full = $"{FirstName} {LastName}".Trim()
                Return If(String.IsNullOrEmpty(full), "Member Goapp", full)
            End Get
        End Property

        Public ReadOnly Property AvailablePoints As Decimal
            Get
                If Account IsNot Nothing AndAlso Not String.IsNullOrEmpty(Account.Balance) Then
                    Dim val As Decimal
                    If Decimal.TryParse(Account.Balance, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, val) Then
                        Return val
                    End If
                End If
                Return 0D
            End Get
        End Property

        Public ReadOnly Property PointsInRupiah As Decimal
            Get
                If Account IsNot Nothing Then
                    If Account.IdrBalance.HasValue Then
                        Return Account.IdrBalance.Value
                    End If
                    ' Fallback jika idr_balance tidak ada atau string
                    Dim val As Decimal
                    If Decimal.TryParse(Account.Balance, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, val) Then
                        Return val * 1000D ' Standard rasio estimasi jika tidak tertera
                    End If
                End If
                Return 0D
            End Get
        End Property
    End Class

    Public Class MemberScheme
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("name")>
        Public Property Name As String
    End Class

    Public Class MemberLevel
        <JsonProperty("id")>
        Public Property Id As Integer

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("level")>
        Public Property Level As Integer
    End Class

    Public Class MemberAccount
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("balance")>
        Public Property Balance As String

        <JsonProperty("idr_balance")>
        Public Property IdrBalance As Decimal?

        <JsonProperty("account_type")>
        Public Property AccountType As AccountTypeInfo
    End Class

    Public Class AccountTypeInfo
        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("currency")>
        Public Property Currency As String

        <JsonProperty("currency_label")>
        Public Property CurrencyLabel As String
    End Class

    Public Class DirectDealInfo
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("description")>
        Public Property Description As String

        <JsonProperty("redeem_code")>
        Public Property RedeemCode As String

        <JsonProperty("reward_data")>
        Public Property RewardData As RewardDataInfo
    End Class

    Public Class RewardDataInfo
        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("discount_type")>
        Public Property DiscountType As String ' percentage / amount / free_item

        <JsonProperty("discount_amount")>
        Public Property DiscountAmount As Decimal

        <JsonProperty("min_purchase")>
        Public Property MinPurchase As Decimal

        <JsonProperty("products")>
        Public Property Products As List(Of String)
    End Class

    ''' <summary>
    ''' Voucher Validation and Use Models
    ''' </summary>
    Public Class VoucherValidateRequest
        <JsonProperty("deal_code")>
        Public Property DealCode As String

        <JsonProperty("member", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Member As VoucherMemberRef
    End Class

    Public Class VoucherMemberRef
        <JsonProperty("uid", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Uid As Long?

        <JsonProperty("external_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property ExternalId As String
    End Class

    Public Class VoucherUseRequest
        <JsonProperty("deal_code")>
        Public Property DealCode As String

        <JsonProperty("transaction_ref")>
        Public Property TransactionRef As String

        <JsonProperty("member", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Member As VoucherMemberRef
    End Class

    Public Class VoucherResult
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("deal_code")>
        Public Property DealCode As String

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("discount_type")>
        Public Property DiscountType As String

        <JsonProperty("discount_amount")>
        Public Property DiscountAmount As Decimal

        <JsonProperty("status")>
        Public Property Status As String

        <JsonProperty("detail")>
        Public Property Detail As String
    End Class

    ''' <summary>
    ''' Point Payment (Burn Point) Models
    ''' </summary>
    Public Class PaymentTransactionRequest
        <JsonProperty("member")>
        Public Property Member As PaymentMemberRef

        <JsonProperty("amount")>
        Public Property Amount As Decimal ' Point amount

        <JsonProperty("order_amount")>
        Public Property OrderAmount As Decimal ' IDR amount

        <JsonProperty("order_currency")>
        Public Property OrderCurrency As String = "idr"

        <JsonProperty("provider_ref")>
        Public Property ProviderRef As String ' POS Bill/Transaction No
    End Class

    Public Class PaymentMemberRef
        <JsonProperty("uid")>
        Public Property Uid As Long
    End Class

    Public Class PaymentTransactionResponse
        <JsonProperty("payment_ref")>
        Public Property PaymentRef As String

        <JsonProperty("provider_ref")>
        Public Property ProviderRef As String

        <JsonProperty("amount")>
        Public Property Amount As Decimal

        <JsonProperty("order_amount")>
        Public Property OrderAmount As Decimal

        <JsonProperty("status")>
        Public Property Status As String ' pending, settlement, cancel

        <JsonProperty("account")>
        Public Property Account As MemberAccount

        <JsonProperty("detail")>
        Public Property Detail As String
    End Class

    ''' <summary>
    ''' Sales Order Push (Earning Point) Models
    ''' </summary>
    Public Class PushOrderRequest
        <JsonProperty("order_no")>
        Public Property OrderNo As String

        <JsonProperty("store")>
        Public Property Store As StoreRef

        <JsonProperty("contact", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Contact As ContactRef

        <JsonProperty("lines")>
        Public Property Lines As List(Of OrderLineItem)

        <JsonProperty("payment_method")>
        Public Property PaymentMethod As String = "Cash"

        <JsonProperty("grand_total")>
        Public Property GrandTotal As Decimal
    End Class

    Public Class StoreRef
        <JsonProperty("uid")>
        Public Property Uid As Long
    End Class

    Public Class ContactRef
        <JsonProperty("uid", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Uid As Long?

        <JsonProperty("mobile_no", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property MobileNo As String
    End Class

    Public Class OrderLineItem
        <JsonProperty("sku")>
        Public Property Sku As String

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("quantity")>
        Public Property Quantity As Integer

        <JsonProperty("price")>
        Public Property Price As Decimal

        <JsonProperty("discount")>
        Public Property Discount As Decimal = 0D
    End Class

    Public Class PushOrderResponse
        <JsonProperty("id")>
        Public Property Id As Long

        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("order_no")>
        Public Property OrderNo As String

        <JsonProperty("status")>
        Public Property Status As String

        <JsonProperty("detail")>
        Public Property Detail As String
    End Class
End Namespace
