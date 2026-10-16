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
        public int Posts, Auths; public string Body; public int Status=201; public bool NetworkFailure, Malformed, UnauthorizedOnce, RewardOnGuest; public int Delay;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct) {
            if (request.RequestUri.AbsolutePath.EndsWith("token-auth/")) { Interlocked.Increment(ref Auths); return Json(200,"{\"token\":\"test-token\",\"expired_at\":9999999999}"); }
            if (request.Method != HttpMethod.Post || !request.RequestUri.AbsolutePath.EndsWith("sales/order/")) throw new Exception("Unexpected endpoint");
            int n=Interlocked.Increment(ref Posts); Body=await request.Content.ReadAsStringAsync();
            if (Delay>0) await Task.Delay(Delay);
            if (NetworkFailure) throw new HttpRequestException("Simulated lost response");
            if (UnauthorizedOnce && n==1) return Json(401,"{\"detail\":\"expired\"}");
            bool guest=JObject.Parse(Body)["member"]==null;
            return Json(Status,Malformed ? "{broken" : Status>=400 ? "{\"detail\":\"test failure\"}" : "{\"uid\":123,\"order_no\":\"TEST\",\"reward\":"+(guest && !RewardOnGuest ? "[]" : "[{\"amount\":5000},{\"amount\":7500}]")+"}");
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
        Check(r.IsSuccess && r.Data.TotalEarnedPoints==12500,"sum every returned reward, not only first item");
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
        await SqlTests();
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

            } finally {
                foreach(string table in new[]{api,redeem}) using(var cmd=new SqlCommand("IF OBJECT_ID('dbo."+table+"') IS NOT NULL DROP TABLE dbo."+table,conn)) await cmd.ExecuteNonQueryAsync();
            }
        }
    }
    static async Task LiveGuest() {
        var xml=XDocument.Load("TryDreamposXGoapp/App.config");var settings=xml.Descendants("appSettings").Elements("add").ToDictionary(x=>(string)x.Attribute("key"),x=>(string)x.Attribute("value"));
        var db=xml.Descendants("connectionStrings").Elements("add").First(x=>(string)x.Attribute("name")=="POSDatabase").Attribute("connectionString").Value;
        var config=new GoappConfig { ApiKey=settings["Goapp.ApiKey"],ApiSecret=settings["Goapp.ApiSecret"],ChannelUid=long.Parse(settings["Goapp.ChannelUid"]),AuthBaseUrl=settings["Goapp.AuthBaseUrl"],ChannelBaseUrl=settings["Goapp.ChannelBaseUrl"],DbConnectionString=db,StoreUid=long.Parse(settings["Goapp.StoreUid"]),StoreName=settings["Goapp.StoreName"],StoreCode=settings["Goapp.StoreCode"] };
        var client=new GoappApiClient(config);var order=Order(true);order.ProviderRef="TEST-TRX-"+DateTime.Now.ToString("yyMMdd-HHmmss")+"-"+Guid.NewGuid().ToString("N").Substring(0,6);order.OrderNo=order.ProviderRef;order.OrderDate=DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:sszzz");order.CompletedAt=order.OrderDate;order.PaidAt=order.OrderDate;order.Lines[0].Product.Sku="POS-ITEM-01";order.Lines[0].Product.Name="UJI TRANSAKSI TANPA MEMBER";
        var result=await client.PushGuestSalesOrderAsync(order);
        var evidence=new JObject { ["provider_ref"]=order.ProviderRef,["http"]=result.StatusCode,["success"]=result.IsSuccess,["message"]=result.Message,["warning"]=result.WarningMessage,["api_log_saved"]=result.ApiLogSaved,["transaction_log_saved"]=result.TransactionLogSaved,["requires_reconciliation"]=result.RequiresReconciliation,["earned_points"]=result.Data==null ? 0 : result.Data.TotalEarnedPoints,["order_uid"]=result.Data==null ? 0 : result.Data.Uid };
        if(!result.IsSuccess) evidence["response"]=result.RawJson;
        Console.WriteLine(evidence.ToString());File.WriteAllText("Tests/live-guest-result.json",evidence.ToString());
        Check(result.IsSuccess && result.Data!=null,"Goapp accepts live guest sales order");Check(result.Data.TotalEarnedPoints==0,"live Goapp guest order returns zero reward");Check(result.ApiLogSaved && result.TransactionLogSaved,"live guest order persisted in both requested tables");
        var duplicate=await client.PushGuestSalesOrderAsync(order);Check(duplicate.IsSuccess && duplicate.IsDuplicate,"live duplicate is replayed without a second POST");
        using(var conn=new SqlConnection(db)) {await conn.OpenAsync();using(var cmd=new SqlCommand("SELECT COUNT(*) FROM dbo.GoappApiLog WHERE EndpointUrl LIKE '%/sales/order/' AND RequestPayload LIKE @Ref",conn)) {cmd.Parameters.AddWithValue("@Ref","%"+order.ProviderRef+"%");int count=(int)await cmd.ExecuteScalarAsync();Check(count==1,"SQL confirms exactly one live sales POST for test reference");evidence["sales_post_log_count"]=count;}}
        evidence["duplicate_prevented"]=duplicate.IsDuplicate;File.WriteAllText("Tests/live-guest-result.json",evidence.ToString());
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
    [STAThread] static int Main(string[] args) { try { if(args.Contains("--live-guest")) {LiveGuest().GetAwaiter().GetResult();return 0;} if(args.Contains("--preflight")) {Preflight().GetAwaiter().GetResult();return 0;} Run().GetAwaiter().GetResult();Console.WriteLine("Passed: "+passed);return 0;}catch(Exception ex){Console.Error.WriteLine(ex.ToString());return 1;} }
}