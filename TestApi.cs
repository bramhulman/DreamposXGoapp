using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "138350315235400";
        string apiSecret = "cbf5b1c921b36bfb06ca988c6d98f608482c40f1";
        
        HttpClient client = new HttpClient();
        var authContent = new StringContent(JsonConvert.SerializeObject(new { username = apiKey, password = apiSecret }), System.Text.Encoding.UTF8, "application/json");
        var authResponse = await client.PostAsync("https://account.goapp.co.id/auth/token-auth/", authContent);
        var authJson = await authResponse.Content.ReadAsStringAsync();
        var token = JObject.Parse(authJson)["token"].ToString();
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // 081588809090 is Archen. 087890760858 is Riri.
        var r = await client.GetAsync("https://api.goapp.co.id/channel/v1/member/member/081588809090/deal_codes/");
        Console.WriteLine(await r.Content.ReadAsStringAsync());
    }
}
