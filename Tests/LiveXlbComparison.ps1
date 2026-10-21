param([switch]$SendLive, [long]$MemberUid = 157557895826504, [string]$ExpectedMobileNo = '')
$ErrorActionPreference = 'Stop'
if (-not $SendLive) { throw 'Use -SendLive only when live sales submissions are authorized.' }
$root = Split-Path $PSScriptRoot -Parent
Set-Location $root
[void][Reflection.Assembly]::LoadFrom("$root/bin/Release/Newtonsoft.Json.dll")
[void][Reflection.Assembly]::LoadFrom("$root/bin/Release/DreamposXGoapp.dll")
$catalog = Get-Content "$root/TryDreamposXGoapp/TestData/XlbProducts.json" -Raw | ConvertFrom-Json
$stream = [IO.File]::Open('D:\Downloads\XLB_Dintaifung.xlsx', 'Open', 'Read', 'ReadWrite')
$sha = [Security.Cryptography.SHA256]::Create()
try { $hash = [BitConverter]::ToString($sha.ComputeHash($stream)).Replace('-', '').ToLowerInvariant() }
finally { $sha.Dispose(); $stream.Dispose() }
if ($hash -ne $catalog.SourceSha256) { throw 'Excel changed: refresh the SKU catalog before sending.' }
[xml]$xml = Get-Content "$root/TryDreamposXGoapp/App.config"
$settings = @{}
foreach ($entry in $xml.configuration.appSettings.add) { $settings[$entry.key] = $entry.value }
$config = New-Object DreamposXGoapp.Models.GoappConfig
foreach ($key in @('ApiKey','ApiSecret','AuthBaseUrl','ChannelBaseUrl','StoreName','StoreCode')) { $config.$key = $settings["Goapp.$key"] }
$config.ChannelUid = [long]$settings['Goapp.ChannelUid']
$config.StoreUid = [long]$settings['Goapp.StoreUid']
$config.DbConnectionString = ($xml.configuration.connectionStrings.add | Where-Object name -eq 'POSDatabase').connectionString
$client = New-Object DreamposXGoapp.Services.GoappApiClient($config)
$before = $client.GetMemberAsync([string]$MemberUid).GetAwaiter().GetResult()
if (-not $before.IsSuccess -or -not $before.Data.IsMemberActive) { throw 'Test member unavailable/inactive.' }
if ($ExpectedMobileNo -and ($before.Data.MobileNo -replace '\D','') -ne ($ExpectedMobileNo -replace '\D','')) { throw 'Member mobile does not match the requested test member.' }
$cases = @(
    @{name='Tanpa XLB'; skus=@('SKU-CONTROL-01','SKU-CONTROL-02','SKU-CONTROL-03')},
    @{name='SKU contoh Postman + XLB ayam'; skus=@('FDUM0007','FDUM0008','FDUM0009')},
    @{name='XLB kepiting dan mala'; skus=@('FDUM0001','FDUM0004','FDUM0017')},
    @{name='XLB dessert'; skus=@('FHDS0015','FHDS0017','FHDS0031')},
    @{name='XLB ayam + dessert + biasa'; skus=@('FDUM0007','FHDS0031','SKU-CONTROL-03')}
)
foreach ($case in $cases) { foreach ($sku in $case.skus) { if (-not $sku.StartsWith('SKU-') -and -not ($catalog.Products | Where-Object Sku -eq $sku)) { throw "Unknown Excel SKU: $sku" } } }
$runId = Get-Date -Format 'yyMMdd-HHmmss'
$evidencePath = "$root/Tests/live-xlb-$runId.json"
$evidence = [ordered]@{started_at=[DateTimeOffset]::Now.ToString('o');member_uid=$MemberUid;member_mobile=$before.Data.MobileNo;member_name=$before.Data.FullName;excel_sha256=$hash;balance_before=$before.Data.AvailablePoints;total_each=250000;results=@()}
$conn = New-Object System.Data.SqlClient.SqlConnection($config.DbConnectionString)
try {
    $conn.Open()
    foreach ($case in $cases) {
        $ref = 'TRX-' + [Guid]::NewGuid().ToString('N')
        $order = New-Object DreamposXGoapp.Models.PushOrderRequest
        $order.ProviderRef=$ref; $order.OrderNo=$ref
        $order.OrderDate=[DateTimeOffset]::Now.ToString('yyyy-MM-dd HH:mm:sszzz')
        $order.PaidAt=$order.OrderDate; $order.CompletedAt=$order.OrderDate
        $order.Member=New-Object DreamposXGoapp.Models.OrderMemberRef; $order.Member.Uid=$MemberUid
        $order.Store=New-Object DreamposXGoapp.Models.OrderStoreRef
        $order.Store.Uid=$config.StoreUid; $order.Store.Name=$config.StoreName; $order.Store.StoreCode=$config.StoreCode
        $order.TotalInclTax=250000; $order.TotalPaid=250000; $order.LinesTotal=250000
        $order.Lines=New-Object 'System.Collections.Generic.List[DreamposXGoapp.Models.OrderLineItem]'
        for ($i=0; $i -lt $case.skus.Count; $i++) {
            $sku=$case.skus[$i]; $product=$catalog.Products | Where-Object Sku -eq $sku | Select-Object -First 1
            $line=New-Object DreamposXGoapp.Models.OrderLineItem
            $line.Product=New-Object DreamposXGoapp.Models.OrderProductInfo
            $line.Product.Sku=$sku; $line.Product.Name=if ($product) {$product.Name} else {"Produk $($i+1)"}
            $line.Quantity=1; $line.Price=if($i -eq 2){83334}else{83333}; $line.PriceBeforeDiscount=$line.Price
            $order.Lines.Add($line)
        }
        $order.Payments=New-Object 'System.Collections.Generic.List[DreamposXGoapp.Models.OrderPaymentItem]'
        $payment=New-Object DreamposXGoapp.Models.OrderPaymentItem
        $payment.PaymentType='CASH'; $payment.PaymentMethodName='CASH'; $payment.Amount=250000; $order.Payments.Add($payment)
        $result=$client.PushSalesOrderAsync($order,$true).GetAwaiter().GetResult()
        $raw=$result.RawJson | ConvertFrom-Json
        $reward=($raw.reward | Measure-Object amount -Sum).Sum
        $summary=$raw.reward_summary.amount
        $cmd=$conn.CreateCommand()
        $cmd.CommandText='SELECT COUNT(*) FROM dbo.GoappApiLog WHERE HttpMethod=''POST'' AND EndpointUrl LIKE ''%/sales/order/%'' AND RequestPayload LIKE @ref'
        [void]$cmd.Parameters.AddWithValue('@ref',"%$ref%")
        $postCount=[int]$cmd.ExecuteScalar(); $cmd.Dispose()
        $entry=[ordered]@{case=$case.name;provider_ref=$ref;skus=$case.skus;http=$result.StatusCode;success=$result.IsSuccess;reward_amount=$reward;reward_summary_amount=$summary;ratio=if($reward -gt 0 -and $null -ne $summary){[decimal]$summary/[decimal]$reward}else{$null};api_log_saved=$result.ApiLogSaved;redeem_log_saved=$result.TransactionLogSaved;sales_post_count=$postCount;request=($result.RequestJson|ConvertFrom-Json);response=$raw}
        $evidence.results+= $entry
        $evidence | ConvertTo-Json -Depth 30 | Set-Content $evidencePath -Encoding UTF8
        ([pscustomobject]$entry | Select-Object case,provider_ref,http,reward_amount,reward_summary_amount,ratio) | ConvertTo-Json -Compress
        if (-not $result.IsSuccess -or -not $result.ApiLogSaved -or -not $result.TransactionLogSaved -or $postCount -ne 1) { throw "Submission/audit failed; inspect $evidencePath before any further action." }
    }
    $after=$client.GetMemberAsync([string]$MemberUid).GetAwaiter().GetResult()
    if($after.IsSuccess){$evidence.balance_after_immediate=$after.Data.AvailablePoints}
    $evidence.completed_at=[DateTimeOffset]::Now.ToString('o')
    $evidence | ConvertTo-Json -Depth 30 | Set-Content $evidencePath -Encoding UTF8
    Write-Output "Evidence: $evidencePath"
} finally { $conn.Dispose() }
