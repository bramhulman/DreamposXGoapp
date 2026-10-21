using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DreamposXGoapp.Models;
using DreamposXGoapp.Services;
using Newtonsoft.Json.Linq;

class OrderSubmissionTests {
    static int passed;
    static void Check(bool condition, string name) { if (!condition) throw new Exception("FAIL: " + name); Console.WriteLine("PASS: " + name); passed++; }
    class Fake : HttpMessageHandler {
        public int Posts, Auths; public string Body, Url, ResponseOverride; public readonly List<string> Urls=new List<string>(); public int Status=201; public bool NetworkFailure, Malformed, UnauthorizedOnce, RewardOnGuest; public int Delay;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct) {
            if (request.RequestUri.AbsolutePath.EndsWith("token-auth/")) { Interlocked.Increment(ref Auths); return Json(200,"{\"token\":\"test-token\",\"expired_at\":9999999999}"); }
            if (request.Method != HttpMethod.Post || !request.RequestUri.AbsolutePath.EndsWith("sales/order/")) throw new Exception("Unexpected endpoint");
            int n=Interlocked.Increment(ref Posts); Url=request.RequestUri.AbsoluteUri;Urls.Add(Url); Body=await request.Content.ReadAsStringAsync();
            if (Delay>0) await Task.Delay(Delay);
            if (NetworkFailure) throw new HttpRequestException("Simulated lost response");
            if (UnauthorizedOnce && n==1) return Json(401,"{\"detail\":\"expired\"}");
            if (ResponseOverride!=null) return Json(Status,ResponseOverride);
            bool guest=JObject.Parse(Body)["member"]==null;
            if(Malformed) return Json(Status,"{broken");
            if(Status>=400) return Json(Status,"{\"detail\":\"test failure\"}");
            var response=new JObject { ["uid"]=123,["order_no"]="TEST",["reward"]=JArray.Parse(guest && !RewardOnGuest ? "[]" : "[{\"amount\":5000},{\"amount\":7500}]") };
            if(request.RequestUri.Query.Contains("calculate_estimate_point=1")) response["reward_summary"]=new JObject { ["amount"]=guest && !RewardOnGuest ? 0 : 12500,["est_new_balance"]=22500 };
            return Json(Status,response.ToString());
        }
        static HttpResponseMessage Json(int status,string json) { return new HttpResponseMessage((HttpStatusCode)status) { Content=new StringContent(json) }; }
    }
    static GoappConfig Config(string db="") { return new GoappConfig { ApiKey="test",ApiSecret="test",ChannelUid=1,AuthBaseUrl="https://auth.invalid",ChannelBaseUrl="https://api.invalid",StoreCode="TEST",DbConnectionString=db }; }
    static PushOrderRequest Order(bool guest=false) {
        return new PushOrderRequest { ProviderRef="TEST-"+Guid.NewGuid().ToString("N"),OrderDate="2026-10-16 12:00:00+07:00",TotalPaid=150000,TotalInclTax=150000,LinesTotal=150000,Member=guest ? null : new OrderMemberRef { Uid=123L },
            Lines=new List<OrderLineItem>{new OrderLineItem{Product=new OrderProductInfo{Sku="TEST",Name="Test"},Quantity=1,Price=150000,PriceBeforeDiscount=150000}},
            Payments=new List<OrderPaymentItem>{new OrderPaymentItem{PaymentType="CASH",PaymentMethodName="CASH",Amount=150000}} };
    }
    static async Task Run() {
        var h=new Fake();var c=new GoappApiClient(Config(),h);var o=Order();var r=await c.PushSalesOrderAsync(o);
        Check(r.IsSuccess && r.Data.TotalEarnedPoints==12500,"earning points come from returned reward_summary");
        var repeated=await c.PushSalesOrderAsync(o);Check(repeated.IsSuccess && repeated.IsDuplicate && h.Posts==1,"same order is replayed locally without another POST");
        o.TotalPaid=160000;var changed=await c.PushSalesOrderAsync(o);Check(!changed.IsSuccess && h.Posts==1,"same reference with altered payload is blocked");
        Check(o.Store==null && o.OrderNo==null,"caller request remains unchanged");
        h=new Fake{Delay=100};c=new GoappApiClient(Config(),h);o=Order();var concurrent=await Task.WhenAll(c.PushSalesOrderAsync(o),c.PushSalesOrderAsync(o));Check(h.Posts==1 && concurrent.Count(x=>x.IsSuccess)==1,"concurrent submission sends exactly one POST");
        foreach(int status in new[]{0,500,201}) {
            h=new Fake{NetworkFailure=status==0,Status=status==0 ? 201 : status,Malformed=status==201};c=new GoappApiClient(Config(),h);o=Order();
            r=await c.PushSalesOrderAsync(o);var next=await c.PushSalesOrderAsync(o);
            Check(!r.IsSuccess && r.RequiresReconciliation && !next.IsSuccess && h.Posts==1,"ambiguous result blocked without retry: "+status);
            if(status==201) Check(r.StatusCode==201,"parse failure preserves real HTTP status");
        }
        h=new Fake{UnauthorizedOnce=true};c=new GoappApiClient(Config(),h);r=await c.PushSalesOrderAsync(Order());Check(r.IsSuccess && h.Posts==2 && h.Auths==2,"only explicit 401 is retried once after token refresh");
        h=new Fake();c=new GoappApiClient(Config(),h);r=await c.PushGuestSalesOrderAsync(Order(true));Check(!r.IsSuccess && h.Posts==0,"guest order requires SQL for both logs");
        r=await c.PushGuestSalesOrderAsync(Order());Check(!r.IsSuccess && h.Posts==0,"guest method rejects member identity");
        var buildLines=typeof(TryDreamposXGoapp.FormMain).GetMethod("CreateTestOrderLines",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic);
        var lines=(List<OrderLineItem>)buildLines.Invoke(null,new object[]{133333m,"TEST-SKU"});
        Check(lines.Count==3 && lines.Select(x=>x.Product.Sku).Distinct().Count()==3,"main form generates three distinct SKUs");
        Check(lines.Sum(x=>x.Price*x.Quantity)==133333m,"generated SKU amounts equal transaction total including remainder");
        var sameLines=(List<OrderLineItem>)buildLines.Invoke(null,new object[]{133333m,"TEST-SKU"});
        Check(lines.Select(x=>x.Product.Sku).SequenceEqual(sameLines.Select(x=>x.Product.Sku)),"same transaction preserves generated SKUs for duplicate protection");
        await XlbTests();
        await EstimateTests();
        await SqlTests();
    }
    static List<OrderLineItem> XlbLines(decimal total,string reference,bool include) {
        return (List<OrderLineItem>)typeof(TryDreamposXGoapp.FormMain).GetMethod("CreateTestOrderLinesWithXlb",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[]{total,reference,include});
    }
    static Dictionary<string,string> XlbCatalog() {
        return JObject.Parse(File.ReadAllText("TryDreamposXGoapp/TestData/XlbProducts.json"))["Products"].ToDictionary(p=>(string)p["Sku"],p=>(string)p["Name"],StringComparer.OrdinalIgnoreCase);
    }
    static async Task XlbTests() {
        var catalog=XlbCatalog();Check(catalog.Count==46,"Excel XLB snapshot contains all 46 unique SKUs");
        using(var stream=typeof(TryDreamposXGoapp.FormMain).Assembly.GetManifestResourceStream("TryDreamposXGoapp.TestData.XlbProducts.json"))
        using(var reader=new StreamReader(stream)) {
            var embedded=new HashSet<string>(JObject.Parse(reader.ReadToEnd())["Products"].Select(p=>(string)p["Sku"]),StringComparer.OrdinalIgnoreCase);
            Check(embedded.SetEquals(catalog.Keys),"Try executable embeds the complete Excel-derived SKU set without requiring Downloads at runtime");
        }
        bool excludes=true,includes=true,totals=true,stable=true,canonicalNames=true,distinct=true;var seen=new HashSet<string>(StringComparer.OrdinalIgnoreCase);var counts=new HashSet<int>();
        foreach(decimal total in new[]{150000m,133333m,0.01m,2.5m,3m,100.01m}) for(int i=0;i<512;i++) {
            string reference="XLB-CASE-"+i;var off=XlbLines(total,reference,false);var on=XlbLines(total,reference,true);int xlb=on.Count(x=>catalog.ContainsKey(x.Product.Sku));
            excludes &= !off.Any(x=>catalog.ContainsKey(x.Product.Sku));includes &= xlb>=1 && xlb<=2 && (on.Count==1 || xlb<on.Count);
            totals &= on.Sum(x=>x.Price*x.Quantity)==total && off.Sum(x=>x.Price*x.Quantity)==total && on.All(x=>x.Price>0);
            stable &= Newtonsoft.Json.JsonConvert.SerializeObject(on)==Newtonsoft.Json.JsonConvert.SerializeObject(XlbLines(total,reference,true));
            distinct &= on.Select(x=>x.Product.Sku).Distinct(StringComparer.OrdinalIgnoreCase).Count()==on.Count;
            foreach(var line in on.Where(x=>catalog.ContainsKey(x.Product.Sku))) {canonicalNames &= catalog[line.Product.Sku]==line.Product.Name;seen.Add(line.Product.Sku);}
            if(on.Count==3) counts.Add(xlb);
        }
        Check(excludes,"unchecked XLB never includes any spreadsheet SKU across 3072 total/reference cases");
        Check(includes,"checked XLB always includes one or two source SKUs and mixes ordinary items where possible");
        Check(totals,"XLB and non-XLB item totals remain exact including fractions and small amounts");
        Check(stable && distinct,"XLB selection is stable for retries and never repeats a SKU in the same order");
        Check(canonicalNames,"XLB product names match the first Excel row for each SKU");
        Check(seen.SetEquals(catalog.Keys) && counts.SetEquals(new[]{1,2}),"reference variation covers all 46 Excel SKUs and both one/two XLB combinations");
        var handler=new Fake();var client=new GoappApiClient(Config(),handler);var order=Order();order.Lines=XlbLines(order.TotalPaid,order.ProviderRef,true);
        var result=await client.PushSalesOrderAsync(order,true);var body=JObject.Parse(handler.Body);
        Check(result.IsSuccess && body["lines"].Any(x=>catalog.ContainsKey((string)x["product"]["sku"])),"HTTP sales payload contains actual XLB SKUs when checked");
        Check(body["with_xlb"]==null && body["dengan_xlb"]==null,"XLB checkbox changes products only, without an invented API parameter");
        order.Lines=XlbLines(order.TotalPaid,order.ProviderRef,false);var changed=await client.PushSalesOrderAsync(order,true);
        Check(!changed.IsSuccess && changed.StatusCode==409 && handler.Posts==1,"changing XLB mode on a submitted reference never resends the transaction");
    }
    static string EstimateExample() {
        var collection=JObject.Parse(File.ReadAllText("Channel API.postman_collection21Okt2026.json"));
        return collection["item"].First(x=>(string)x["name"]=="Post sales order")["response"].First(x=>(string)x["name"]=="Calculate estimated points earned")["body"].ToString();
    }
    static string UserEarningExample() {
        return "{\"reward\":[{\"amount\":10000,\"est_new_balance\":241100.0}],\"reward_summary\":{\"amount\":15000.0,\"est_new_balance\":246100.0}}";
    }
    static async Task EstimateTests() {
        var h=new Fake {ResponseOverride=EstimateExample()};var c=new GoappApiClient(Config(),h);var o=Order();var r=await c.PushSalesOrderAsync(o);
        Check(h.Url.EndsWith("/sales/order/?calculate_estimate_point=1"),"default DLL method requests calculate_estimate_point=1");
        Check(JObject.Parse(h.Body)["calculate_estimate_point"]==null,"estimate option is a query parameter, not JSON body data");
        Check(r.Data.RewardSummary.Amount==7500m && r.Data.TotalRewardDetailPoints==5000m && r.Data.TotalEarnedPoints==7500m,"real collection fixture uses reward_summary total without double counting reward");
        Check(r.Data.EstimatedNewBalance==216100m,"real collection fixture exposes summary estimated balance");
        var repeat=await c.PushSalesOrderAsync(o,false);Check(repeat.IsSuccess && repeat.IsDuplicate && h.Posts==1 && repeat.Data.TotalEarnedPoints==7500m,"toggling estimate flag never sends same order again");
        h=new Fake();var config=Config();config.CalculateEstimatePoint=false;c=new GoappApiClient(config,h);r=await c.PushSalesOrderAsync(Order());
        Check(h.Url.EndsWith("calculate_estimate_point=0") && r.Data.TotalEarnedPoints==0m && !r.Data.HasEarningPointSummary && !r.Data.EstimatedNewBalance.HasValue,"disabling summary never substitutes legacy reward values as earning or estimated balance");
        h=new Fake();c=new GoappApiClient(Config(),h);o=Order();r=await c.PushSalesOrderAsync(o,false);var cached=await c.PushSalesOrderAsync(o,true);
        Check(cached.IsDuplicate && h.Posts==1 && !string.IsNullOrEmpty(cached.WarningMessage),"enabling estimate for a saved legacy result does not create another transaction");
        h=new Fake {UnauthorizedOnce=true};c=new GoappApiClient(Config(),h);r=await c.PushSalesOrderAsync(Order(),true);
        Check(h.Posts==2 && h.Urls.All(url=>url.EndsWith("calculate_estimate_point=1")),"401 token refresh preserves estimate query on both attempts");
        var zero=Newtonsoft.Json.JsonConvert.DeserializeObject<PushOrderResponse>("{\"reward_summary\":{\"amount\":0,\"est_new_balance\":0},\"reward\":[{\"amount\":5000}]}");
        Check(zero.TotalEarnedPoints==0 && zero.EstimatedNewBalance==0,"explicit zero summary values are preserved rather than replaced by detail rewards");
        foreach(string value in new[]{"null","{}","{\"amount\":null}"}) {
            var fallback=Newtonsoft.Json.JsonConvert.DeserializeObject<PushOrderResponse>("{\"reward_summary\":"+value+",\"reward\":[{\"amount\":5000,\"est_new_balance\":6000}]}");
            Check(fallback.TotalEarnedPoints==0m && !fallback.HasEarningPointSummary && !fallback.EstimatedNewBalance.HasValue,"missing/null summary never falls back to reward fields: "+value);
        }
        var strings=Newtonsoft.Json.JsonConvert.DeserializeObject<PushOrderResponse>("{\"reward_summary\":{\"amount\":\"7500.00\",\"est_new_balance\":\"216100.00\"}}");
        Check(strings.TotalEarnedPoints==7500m && strings.EstimatedNewBalance==216100m,"summary decimal values also parse when encoded as JSON strings");
        h=new Fake {ResponseOverride=UserEarningExample()};c=new GoappApiClient(Config(),h);var summary=await c.PushSalesOrderAsync(Order(),true);
        Check(summary.IsSuccess && summary.Data.TotalEarnedPoints==15000m && summary.Data.EstimatedNewBalance==246100m,"user example reads 15000 earning and 246100 balance from summary rather than 10000/241100 reward");
        var member=new MemberResponse {Account=new MemberAccount {Balance="246100"}};
        var receipt=ReceiptGenerator.GenerateReceiptText("Test","TEST-SUMMARY","Test",member,summary.Data,0,150000,150000,"https://survey.invalid");
        Check(receipt.Contains("+"+15000m.ToString("N0")+" Pts") && receipt.Contains(246100m.ToString("N0")+" Pts"),"receipt uses summary earning and summary ending balance");
        Check(!receipt.Contains(261100m.ToString("N0")) && !receipt.Contains(241100m.ToString("N0")),"receipt never adds earning again to refreshed member balance or uses reward estimate");
        var unavailable=Newtonsoft.Json.JsonConvert.DeserializeObject<PushOrderResponse>("{\"reward\":[{\"amount\":10000,\"est_new_balance\":241100}]}" );
        var emptyReceipt=ReceiptGenerator.GenerateReceiptText("Test","TEST-NONE","Test",member,unavailable,0,150000,150000,"https://survey.invalid");
        Check(emptyReceipt.Contains("Tidak tersedia") && !emptyReceipt.Contains("+"+10000m.ToString("N0")),"receipt displays unavailable for missing summary instead of legacy reward");

    }
    static async Task SqlTests() {
        var xml=XDocument.Load("TryDreamposXGoapp/App.config");string db=xml.Descendants("connectionStrings").Elements("add").First(x=>(string)x.Attribute("name")=="POSDatabase").Attribute("connectionString").Value;
        string suffix=Guid.NewGuid().ToString("N").Substring(0,10);string api="CodexApi_"+suffix,redeem="CodexRedeem_"+suffix;
        using(var conn=new SqlConnection(db)) {
            await conn.OpenAsync();
            try {
                foreach(var spec in new[]{new[]{"create_log_table.sql","GoappApiLog",api},new[]{"create_redeem_table.sql","GoappRedeemLog",redeem}}) {
                    string sql=File.ReadAllText(spec[0]).Replace("USE [POS_Restaurant];","").Replace("\r\nGO", "").Replace("\nGO", "").Replace(spec[1],spec[2]);
                    using(var cmd=new SqlCommand(sql,conn)) await cmd.ExecuteNonQueryAsync();
                }
                using(var cmd=new SqlCommand("INSERT INTO dbo."+api+" (LogDate,LogDateTime,HttpMethod,EndpointUrl,IsSuccess,DurationMs) VALUES (DATEADD(month,-4,GETDATE()),DATEADD(month,-4,GETDATE()),'TEST','old',1,0),(DATEADD(month,-2,GETDATE()),DATEADD(month,-2,GETDATE()),'TEST','recent',1,0); INSERT INTO dbo."+redeem+" (Waktu,Jenis,Kode,NamaVoucher) VALUES (DATEADD(month,-4,GETDATE()),'TEST','old','old'),(DATEADD(month,-2,GETDATE()),'TEST','recent','recent');",conn)) await cmd.ExecuteNonQueryAsync();
                var h=new Fake{Delay=50};var c=new GoappApiClient(Config(db),h);c.LogTableName="[dbo].["+api+"]";c.RedeemRepo.TableName="[dbo].["+redeem+"]";
                var o=Order(true);var r=await c.PushGuestSalesOrderAsync(o);
                Check(r.IsSuccess && r.ApiLogSaved && r.TransactionLogSaved,"guest success is saved to both SQL tables");
                Check(h.Url.EndsWith("calculate_estimate_point=1"),"guest submission also requests estimate query");
                using(var cmd=new SqlCommand("SELECT COUNT(*) FROM dbo."+api+" WHERE EndpointUrl LIKE '%/sales/order/?calculate_estimate_point=1'",conn)) Check((int)await cmd.ExecuteScalarAsync()==1,"SQL API log preserves complete endpoint including estimate query");
                using(var cmd=new SqlCommand("SELECT (SELECT COUNT(*) FROM dbo."+api+" WHERE HttpMethod='TEST' AND EndpointUrl='recent')+(SELECT COUNT(*) FROM dbo."+redeem+" WHERE Jenis='TEST' AND Kode='recent')",conn)) Check((int)await cmd.ExecuteScalarAsync()==2,"three-month retention keeps recent rows in both tables");
                using(var cmd=new SqlCommand("SELECT (SELECT COUNT(*) FROM dbo."+api+" WHERE EndpointUrl='old')+(SELECT COUNT(*) FROM dbo."+redeem+" WHERE Kode='old')",conn)) Check((int)await cmd.ExecuteScalarAsync()==0,"three-month retention deletes expired rows in both tables");
                Check(JObject.Parse(h.Body)["member"]==null && r.Data.TotalEarnedPoints==0,"guest HTTP payload omits member; mock returns zero reward");
                using(var cmd=new SqlCommand("SELECT COUNT(*) FROM dbo."+redeem+" WHERE Jenis='SalesOrder' AND NilaiPoint=0 AND StatusPos=1 AND OrderSubmissionState='Succeeded' AND MemberUid IS NULL",conn)) Check((int)await cmd.ExecuteScalarAsync()==1,"guest row cannot appear as cancellable active redeem");
                var h2=new Fake();var c2=new GoappApiClient(Config(db),h2);c2.LogTableName=c.LogTableName;c2.RedeemRepo.TableName=c.RedeemRepo.TableName;
                var replay=await c2.PushGuestSalesOrderAsync(o);Check(replay.IsSuccess && replay.IsDuplicate && h2.Posts==0,"SQL journal prevents resend across client instances");
                var simultaneous=Order(true);var results=await Task.WhenAll(c.PushGuestSalesOrderAsync(simultaneous),c2.PushGuestSalesOrderAsync(simultaneous));Check(h.Posts+h2.Posts==2,"SQL unique key allows only one new simultaneous POST");
                // Failed SQL persistence blocks API transmission, rather than losing audit records.
                var bad=new GoappApiClient(Config(db),h2);bad.RedeemRepo.TableName="[dbo].[NoSuchTable_"+suffix+"]";
                int before=h2.Posts;var fail=await bad.PushGuestSalesOrderAsync(Order(true));Check(!fail.IsSuccess && h2.Posts==before,"SQL journal failure prevents HTTP send");
                var points=Order(true);points.Payments[0].PaymentType=" POINT ";before=h.Posts;var rejected=await c.PushGuestSalesOrderAsync(points);Check(!rejected.IsSuccess && h.Posts==before,"guest rejects POINT payments including whitespace");
                var unusual=new Fake{RewardOnGuest=true};var guestClient=new GoappApiClient(Config(db),unusual);guestClient.LogTableName=c.LogTableName;guestClient.RedeemRepo.TableName=c.RedeemRepo.TableName;
                var unexpected=await guestClient.PushGuestSalesOrderAsync(Order(true));Check(unexpected.IsSuccess && unexpected.RequiresReconciliation && !string.IsNullOrEmpty(unexpected.WarningMessage),"unexpected backend guest reward raises reconciliation warning");
                var summaryHandler=new Fake{ResponseOverride=EstimateExample()};var summaryClient=new GoappApiClient(Config(db),summaryHandler);summaryClient.LogTableName=c.LogTableName;summaryClient.RedeemRepo.TableName=c.RedeemRepo.TableName;
                var summaryOrder=Order();var summaryResult=await summaryClient.PushSalesOrderAsync(summaryOrder,true);
                Check(summaryResult.IsSuccess && summaryResult.TransactionLogSaved && summaryResult.Data.TotalEarnedPoints==7500m,"member order with summary is stored successfully");
                using(var cmd=new SqlCommand("SELECT COUNT(*) FROM dbo."+redeem+" WHERE TransactionRef=@Ref AND NilaiPoint=7500 AND OrderResponseJson LIKE '%reward_summary%'",conn)) {cmd.Parameters.AddWithValue("@Ref",summaryOrder.ProviderRef);Check((int)await cmd.ExecuteScalarAsync()==1,"SQL journal stores raw reward_summary and its total rather than sum of summary plus reward");}
                var replaySummaryClient=new GoappApiClient(Config(db),new Fake());replaySummaryClient.LogTableName=c.LogTableName;replaySummaryClient.RedeemRepo.TableName=c.RedeemRepo.TableName;
                var summaryReplay=await replaySummaryClient.PushSalesOrderAsync(summaryOrder,false);Check(summaryReplay.IsDuplicate && summaryReplay.Data.TotalEarnedPoints==7500m,"summary survives SQL replay even when the flag is changed");
                var userHandler=new Fake{ResponseOverride=UserEarningExample()};var userClient=new GoappApiClient(Config(db),userHandler);userClient.LogTableName=c.LogTableName;userClient.RedeemRepo.TableName=c.RedeemRepo.TableName;
                var userOrder=Order();var userResult=await userClient.PushSalesOrderAsync(userOrder,true);
                Check(userResult.IsSuccess && userResult.ApiLogSaved && userResult.TransactionLogSaved && userResult.Data.TotalEarnedPoints==15000m && userResult.Data.EstimatedNewBalance==246100m,"DLL and both SQL logs accept user sample as 15000/246100");
                using(var cmd=new SqlCommand("SELECT NilaiPoint FROM dbo."+redeem+" WHERE TransactionRef=@Ref",conn)) {cmd.Parameters.AddWithValue("@Ref",userOrder.ProviderRef);Check(Convert.ToDecimal(await cmd.ExecuteScalarAsync())==15000m,"SQL earning records summary 15000 rather than reward 10000");}
                var userReplay=await replaySummaryClient.PushSalesOrderAsync(userOrder,true);Check(userReplay.IsDuplicate && userReplay.Data.TotalEarnedPoints==15000m && userReplay.Data.EstimatedNewBalance==246100m,"SQL replay preserves both user summary fields");

                foreach(bool include in new[]{false,true}) {
                    var mixedHandler=new Fake();var mixedClient=new GoappApiClient(Config(db),mixedHandler);mixedClient.LogTableName=c.LogTableName;mixedClient.RedeemRepo.TableName=c.RedeemRepo.TableName;
                    var mixedOrder=Order();mixedOrder.Lines=XlbLines(mixedOrder.TotalPaid,mixedOrder.ProviderRef,include);var mixed=await mixedClient.PushSalesOrderAsync(mixedOrder,true);
                    Check(mixed.IsSuccess && mixed.ApiLogSaved && mixed.TransactionLogSaved,"both SQL logs succeed for XLB mode "+include);
                    using(var cmd=new SqlCommand("SELECT OrderRequestJson FROM dbo."+redeem+" WHERE TransactionRef=@Ref",conn)) {cmd.Parameters.AddWithValue("@Ref",mixedOrder.ProviderRef);var saved=JObject.Parse((string)await cmd.ExecuteScalarAsync());bool hasXlb=saved["lines"].Any(x=>XlbCatalog().ContainsKey((string)x["product"]["sku"]));Check(hasXlb==include,"SQL request journal preserves exact SKU mode "+include);}
                }



            } finally {
                foreach(string table in new[]{api,redeem}) using(var cmd=new SqlCommand("IF OBJECT_ID('dbo."+table+"') IS NOT NULL DROP TABLE dbo."+table,conn)) await cmd.ExecuteNonQueryAsync();
            }
        }
    }
    static async Task LiveGuest() {
        var xml=XDocument.Load("TryDreamposXGoapp/App.config");var settings=xml.Descendants("appSettings").Elements("add").ToDictionary(x=>(string)x.Attribute("key"),x=>(string)x.Attribute("value"));
        var db=xml.Descendants("connectionStrings").Elements("add").First(x=>(string)x.Attribute("name")=="POSDatabase").Attribute("connectionString").Value;
        var config=new GoappConfig { ApiKey=settings["Goapp.ApiKey"],ApiSecret=settings["Goapp.ApiSecret"],ChannelUid=long.Parse(settings["Goapp.ChannelUid"]),AuthBaseUrl=settings["Goapp.AuthBaseUrl"],ChannelBaseUrl=settings["Goapp.ChannelBaseUrl"],DbConnectionString=db,StoreUid=long.Parse(settings["Goapp.StoreUid"]),StoreName=settings["Goapp.StoreName"],StoreCode=settings["Goapp.StoreCode"] };
        var client=new GoappApiClient(config);var order=Order(true);order.ProviderRef="TEST-TRX-"+DateTime.Now.ToString("yyMMdd-HHmmss")+"-"+Guid.NewGuid().ToString("N").Substring(0,6);order.OrderNo=order.ProviderRef;order.OrderDate=DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:sszzz");order.CompletedAt=order.OrderDate;order.PaidAt=order.OrderDate;order.Lines[0].Product.Sku="POS-ITEM-01";order.Lines=(List<OrderLineItem>)typeof(TryDreamposXGoapp.FormMain).GetMethod("CreateTestOrderLines",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[]{order.TotalPaid,order.ProviderRef});
        var result=await client.PushGuestSalesOrderAsync(order);
        var evidence=new JObject { ["provider_ref"]=order.ProviderRef,["lines_count"]=order.Lines.Count,["calculate_estimate_point"]=1,["http"]=result.StatusCode,["success"]=result.IsSuccess,["message"]=result.Message,["warning"]=result.WarningMessage,["api_log_saved"]=result.ApiLogSaved,["transaction_log_saved"]=result.TransactionLogSaved,["requires_reconciliation"]=result.RequiresReconciliation,["earned_points"]=result.Data==null ? 0 : result.Data.TotalEarnedPoints,["order_uid"]=result.Data==null ? 0 : result.Data.Uid };
        if(!result.IsSuccess) evidence["response"]=result.RawJson;
        Console.WriteLine(evidence.ToString());File.WriteAllText("Tests/live-guest-result.json",evidence.ToString());
        Check(result.IsSuccess && result.Data!=null,"Goapp accepts live guest sales order");Check(result.Data.TotalEarnedPoints==0,"live Goapp guest order returns zero reward");Check(result.ApiLogSaved && result.TransactionLogSaved,"live guest order persisted in both requested tables");
        var duplicate=await client.PushGuestSalesOrderAsync(order);Check(duplicate.IsSuccess && duplicate.IsDuplicate,"live duplicate is replayed without a second POST");
        using(var conn=new SqlConnection(db)) {await conn.OpenAsync();using(var cmd=new SqlCommand("SELECT COUNT(*) FROM dbo.GoappApiLog WHERE EndpointUrl LIKE '%/sales/order/%' AND RequestPayload LIKE @Ref",conn)) {cmd.Parameters.AddWithValue("@Ref","%"+order.ProviderRef+"%");int count=(int)await cmd.ExecuteScalarAsync();Check(count==1,"SQL confirms exactly one live sales POST for test reference");evidence["sales_post_log_count"]=count;}}
        evidence["duplicate_prevented"]=duplicate.IsDuplicate;File.WriteAllText("Tests/live-guest-result.json",evidence.ToString());
    }
    static async Task LiveEstimateMember(long memberUid) {
        var xml=XDocument.Load("TryDreamposXGoapp/App.config");var settings=xml.Descendants("appSettings").Elements("add").ToDictionary(x=>(string)x.Attribute("key"),x=>(string)x.Attribute("value"));
        string db=xml.Descendants("connectionStrings").Elements("add").First(x=>(string)x.Attribute("name")=="POSDatabase").Attribute("connectionString").Value;
        var config=new GoappConfig {ApiKey=settings["Goapp.ApiKey"],ApiSecret=settings["Goapp.ApiSecret"],ChannelUid=long.Parse(settings["Goapp.ChannelUid"]),AuthBaseUrl=settings["Goapp.AuthBaseUrl"],ChannelBaseUrl=settings["Goapp.ChannelBaseUrl"],DbConnectionString=db,StoreUid=long.Parse(settings["Goapp.StoreUid"]),StoreName=settings["Goapp.StoreName"],StoreCode=settings["Goapp.StoreCode"],CalculateEstimatePoint=true};
        var client=new GoappApiClient(config);var before=await client.GetMemberAsync(memberUid.ToString());
        Check(before.IsSuccess && before.Data!=null && before.Data.IsMemberActive,"live estimate test member exists and is active before POST");
        var order=Order();order.Member=new OrderMemberRef{Uid=memberUid};order.ProviderRef="TEST-EST-"+DateTime.Now.ToString("yyMMdd-HHmmss")+"-"+Guid.NewGuid().ToString("N").Substring(0,6);order.OrderNo=order.ProviderRef;
        order.OrderDate=DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:sszzz");order.PaidAt=order.OrderDate;order.CompletedAt=order.OrderDate;
        order.Lines=(List<OrderLineItem>)typeof(TryDreamposXGoapp.FormMain).GetMethod("CreateTestOrderLines",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[]{order.TotalPaid,order.ProviderRef});
        var result=await client.PushSalesOrderAsync(order,true);var after=await client.GetMemberAsync(memberUid.ToString());
        var raw=string.IsNullOrWhiteSpace(result.RawJson) ? new JObject() : JObject.Parse(result.RawJson);
        var evidence=new JObject { ["provider_ref"]=order.ProviderRef,["calculate_estimate_point"]=1,["http"]=result.StatusCode,["success"]=result.IsSuccess,["order_uid"]=result.Data==null ? 0 : result.Data.Uid,["reward_detail_points"]=result.Data==null ? 0 : result.Data.TotalRewardDetailPoints,["total_points"]=result.Data==null ? 0 : result.Data.TotalEarnedPoints,["reward_summary"]=raw["reward_summary"],["balance_before"]=before.Data.AvailablePoints,["balance_after"]=after.Data==null ? JValue.CreateNull() : new JValue(after.Data.AvailablePoints),["api_log_saved"]=result.ApiLogSaved,["transaction_log_saved"]=result.TransactionLogSaved,["warning"]=result.WarningMessage };
        Console.WriteLine(evidence.ToString());File.WriteAllText("Tests/live-estimate-result.json",evidence.ToString());
        Check(result.IsSuccess && result.StatusCode==201,"live member sales order with estimate query is accepted");
        Check(result.Data!=null && result.Data.RewardSummary!=null && result.Data.RewardSummary.Amount.HasValue,"live API returns reward_summary for member request");
        Check(result.Data.TotalEarnedPoints==(decimal)raw["reward_summary"]["amount"],"live model total matches summary without adding reward details");
        Check(result.ApiLogSaved && result.TransactionLogSaved,"live estimate request and response persisted in both SQL logs");
        var repeated=await client.PushSalesOrderAsync(order,false);Check(repeated.IsSuccess && repeated.IsDuplicate,"live flag change replays saved response instead of sending another order");
        using(var conn=new SqlConnection(db)) {await conn.OpenAsync();
            using(var cmd=new SqlCommand("SELECT COUNT(*) FROM dbo.GoappApiLog WHERE EndpointUrl LIKE '%/sales/order/?calculate_estimate_point=1' AND RequestPayload LIKE @Ref",conn)) {cmd.Parameters.AddWithValue("@Ref","%"+order.ProviderRef+"%");int count=(int)await cmd.ExecuteScalarAsync();Check(count==1,"live SQL audit confirms one POST including query=1");evidence["sales_post_log_count"]=count;}
            using(var cmd=new SqlCommand("SELECT NilaiPoint FROM dbo.GoappRedeemLog WHERE TransactionRef=@Ref",conn)) {cmd.Parameters.AddWithValue("@Ref",order.ProviderRef);Check(Convert.ToDecimal(await cmd.ExecuteScalarAsync())==result.Data.TotalEarnedPoints,"live journal stores total from reward_summary");}
        }
        evidence["duplicate_prevented"]=repeated.IsDuplicate;File.WriteAllText("Tests/live-estimate-result.json",evidence.ToString());
    }
    static async Task RefreshEstimateEvidence(long memberUid) {
        var xml=XDocument.Load("TryDreamposXGoapp/App.config");var settings=xml.Descendants("appSettings").Elements("add").ToDictionary(x=>(string)x.Attribute("key"),x=>(string)x.Attribute("value"));
        var config=new GoappConfig {ApiKey=settings["Goapp.ApiKey"],ApiSecret=settings["Goapp.ApiSecret"],AuthBaseUrl=settings["Goapp.AuthBaseUrl"],ChannelBaseUrl=settings["Goapp.ChannelBaseUrl"],DbConnectionString=xml.Descendants("connectionStrings").Elements("add").First(x=>(string)x.Attribute("name")=="POSDatabase").Attribute("connectionString").Value};
        var response=await new GoappApiClient(config).GetMemberAsync(memberUid.ToString());Check(response.IsSuccess && response.Data!=null,"read-only member balance recheck succeeds");
        var evidence=JObject.Parse(File.ReadAllText("Tests/live-estimate-result.json"));evidence["balance_after_later"]=response.Data.AvailablePoints;evidence["observed_balance_change"]=response.Data.AvailablePoints-(decimal)evidence["balance_before"];
        File.WriteAllText("Tests/live-estimate-result.json",evidence.ToString());Console.WriteLine(evidence.ToString());
    }
    class RenderMain : TryDreamposXGoapp.FormMain {
        protected override void OnLoad(EventArgs e) { /* Render-only: skip authentication and network events. */ }
    }
    static void RenderForm() {
        var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;
        using(var form=new RenderMain()) {
            var type=typeof(TryDreamposXGoapp.FormMain);
            type.GetField("_config",flags).SetValue(form,Config());
            type.GetMethod("InitializeSalesControls",flags).Invoke(form,null);
            var tabs=(System.Windows.Forms.TabControl)type.GetProperty("tabControl",flags).GetValue(form,null);
            tabs.SelectedTab=(System.Windows.Forms.TabPage)type.GetProperty("tabTransaction",flags).GetValue(form,null);
            form.ShowInTaskbar=false;form.Opacity=0;form.Show();form.PerformLayout();form.Refresh();
            var estimate=(System.Windows.Forms.CheckBox)type.GetField("_calculateEstimatePoint",flags).GetValue(form);
            Check(estimate.Checked && estimate.Right<=tabs.SelectedTab.ClientSize.Width,"main form estimate checkbox defaults on and fits existing transaction tab");
            var xlb=(System.Windows.Forms.CheckBox)type.GetField("_withXlb",flags).GetValue(form);
            Check(!xlb.Checked && xlb.Text=="dengan xlb" && xlb.Right<=tabs.SelectedTab.ClientSize.Width,"XLB checkbox is on the existing form and defaults off");
            xlb.Checked=true;
            using(var image=new System.Drawing.Bitmap(form.Width,form.Height)) {form.DrawToBitmap(image,new System.Drawing.Rectangle(0,0,form.Width,form.Height));image.Save("Tests/xlb-form-preview.png");}
        }
    }
    static async Task Preflight() {
        var settings=XDocument.Load("TryDreamposXGoapp/App.config").Descendants("appSettings").Elements("add").ToDictionary(x=>(string)x.Attribute("key"),x=>(string)x.Attribute("value"));
        var config=new GoappConfig { ApiKey=settings["Goapp.ApiKey"],ApiSecret=settings["Goapp.ApiSecret"],AuthBaseUrl=settings["Goapp.AuthBaseUrl"],ChannelBaseUrl=settings["Goapp.ChannelBaseUrl"] };
        var client=new GoappApiClient(config);string token=await client.GetValidTokenAsync();
        using(var http=new HttpClient()) { http.DefaultRequestHeaders.Authorization=new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
            using(var request=new HttpRequestMessage(HttpMethod.Options,config.ChannelBaseUrl.TrimEnd('/')+"/sales/order/"))
            using(var response=await http.SendAsync(request)) { Console.WriteLine("OPTIONS HTTP: "+(int)response.StatusCode);Console.WriteLine(await response.Content.ReadAsStringAsync()); }
        }
    }
    [STAThread] static int Main(string[] args) { try { if(args.Length==2 && args[0]=="--refresh-estimate-evidence") {RefreshEstimateEvidence(long.Parse(args[1])).GetAwaiter().GetResult();return 0;} if(args.Length==2 && args[0]=="--live-estimate-member") {LiveEstimateMember(long.Parse(args[1])).GetAwaiter().GetResult();return 0;} if(args.Contains("--render-form")) {RenderForm();return 0;} if(args.Contains("--live-guest")) {LiveGuest().GetAwaiter().GetResult();return 0;} if(args.Contains("--preflight")) {Preflight().GetAwaiter().GetResult();return 0;} Run().GetAwaiter().GetResult();Console.WriteLine("Passed: "+passed);return 0;}catch(Exception ex){Console.Error.WriteLine(ex.ToString());return 1;} }
}