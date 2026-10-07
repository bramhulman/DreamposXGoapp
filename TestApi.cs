using System;
using System.IO;
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
        
        // 1. Get Token
        var authContent = new StringContent(JsonConvert.SerializeObject(new { username = apiKey, password = apiSecret }), System.Text.Encoding.UTF8, "application/json");
        var authResponse = await client.PostAsync("https://account.goapp.co.id/auth/token-auth/", authContent);
        var authJson = await authResponse.Content.ReadAsStringAsync();
        var token = JObject.Parse(authJson)["token"].ToString();
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // 2. Get Member
        string phone = "081588809090";
        var memberResponse = await client.GetAsync($"https://api.goapp.co.id/channel/v1/member/member/{phone}/");
        var memberJson = await memberResponse.Content.ReadAsStringAsync();
        
        Console.WriteLine("--- RESPONSE ---");
        Console.WriteLine(JToken.Parse(memberJson).ToString(Formatting.Indented));
    }
}
