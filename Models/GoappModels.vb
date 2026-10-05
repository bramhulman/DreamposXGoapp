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

    ''' <summary>
    ''' Model untuk pendaftaran / simpan member baru dari POS
    ''' </summary>
    Public Class CreateMemberRequest
        <JsonProperty("first_name")>
        Public Property FirstName As String

        <JsonProperty("last_name")>
        Public Property LastName As String

        <JsonProperty("mobile_no")>
        Public Property MobileNo As String

        <JsonProperty("email", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Email As String

        <JsonProperty("scheme")>
        Public Property Scheme As MemberScheme
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

        <JsonProperty("reward_sku")>
        Public Property RewardSku As String

        <JsonProperty("price")>
        Public Property Price As Decimal

        <JsonProperty("enabled")>
        Public Property Enabled As Boolean

        <JsonProperty("start_time")>
        Public Property StartTime As String

        <JsonProperty("end_time")>
        Public Property EndTime As String

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

    Public Class VoucherCancelRequest
        <JsonProperty("deal_code")>
        Public Property DealCode As String

        <JsonProperty("transaction_ref")>
        Public Property TransactionRef As String

        <JsonProperty("member", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Member As VoucherMemberRef
    End Class

    Public Class CreateDealRequest
        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("description", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Description As String

        <JsonProperty("redeem_code", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property RedeemCode As String

        <JsonProperty("reward_sku")>
        Public Property RewardSku As String

        <JsonProperty("start_time")>
        Public Property StartTime As String

        <JsonProperty("end_time")>
        Public Property EndTime As String

        <JsonProperty("reward_channel")>
        Public Property RewardChannel As ChannelRef

        <JsonProperty("reward_data")>
        Public Property RewardData As RewardDataInfo
    End Class

    Public Class ChannelRef
        <JsonProperty("uid")>
        Public Property Uid As Long
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

        <JsonProperty("store", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Store As OrderStoreRef
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
        <JsonProperty("order_no", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property OrderNo As String

        <JsonProperty("provider_ref", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property ProviderRef As String

        <JsonProperty("order_date", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property OrderDate As String

        <JsonProperty("member", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Member As OrderMemberRef

        <JsonProperty("store", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Store As OrderStoreRef

        <JsonProperty("lines")>
        Public Property Lines As List(Of OrderLineItem) = New List(Of OrderLineItem)()

        <JsonProperty("lines_total")>
        Public Property LinesTotal As Decimal

        <JsonProperty("lines_tax")>
        Public Property LinesTax As Decimal = 0D

        <JsonProperty("total_incl_tax")>
        Public Property TotalInclTax As Decimal

        <JsonProperty("total_paid")>
        Public Property TotalPaid As Decimal

        <JsonProperty("payments")>
        Public Property Payments As List(Of OrderPaymentItem) = New List(Of OrderPaymentItem)()

        <JsonProperty("completed_at", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CompletedAt As String

        <JsonProperty("paid_at", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property PaidAt As String

        <JsonProperty("canceled_at", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CanceledAt As String

        <JsonProperty("cancel_reason", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CancelReason As String
    End Class

    Public Class OrderMemberRef
        <JsonProperty("uid", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Uid As Object ' bisa String atau Long

        <JsonProperty("mobile_no", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property MobileNo As String
    End Class

    Public Class OrderStoreRef
        <JsonProperty("uid", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Uid As Long?

        <JsonProperty("name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Name As String

        <JsonProperty("store_code", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property StoreCode As String
    End Class

    Public Class ChannelDirectoryInfo
        <JsonProperty("uid")>
        Public Property Uid As Long

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("channel_type")>
        Public Property ChannelType As String

        <JsonProperty("icon_url")>
        Public Property IconUrl As String
    End Class

    Public Class OrderLineItem
        <JsonProperty("product")>
        Public Property Product As OrderProductInfo

        <JsonProperty("quantity")>
        Public Property Quantity As Integer

        <JsonProperty("price_before_discount")>
        Public Property PriceBeforeDiscount As Decimal

        <JsonProperty("price")>
        Public Property Price As Decimal
    End Class

    Public Class OrderProductInfo
        <JsonProperty("sku")>
        Public Property Sku As String

        <JsonProperty("name")>
        Public Property Name As String
    End Class

    Public Class OrderPaymentItem
        <JsonProperty("payment_method_name")>
        Public Property PaymentMethodName As String

        <JsonProperty("payment_type")>
        Public Property PaymentType As String ' CASH, EDC, EWALLET, POINT

        <JsonProperty("amount")>
        Public Property Amount As Decimal
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

        <JsonProperty("reward")>
        Public Property Reward As List(Of OrderRewardItem)
    End Class

    Public Class OrderRewardItem
        <JsonProperty("amount")>
        Public Property Amount As Decimal ' Poin yang diperoleh (Earned Points)

        <JsonProperty("est_new_balance")>
        Public Property EstNewBalance As Decimal ' Estimasi saldo poin baru
    End Class
End Namespace
