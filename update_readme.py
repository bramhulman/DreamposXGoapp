import re

with open("README.md", "r", encoding="utf-8") as f:
    text = f.read()

new_sec1_2 = """## 1. Persiapan Konfigurasi (App.config Aplikasi Pemanggil)
Agar tidak terjadi *hardcoding* di dalam `.dll`, seluruh kredensial API dan koneksi database SQL Server untuk logging didefinisikan secara terpusat di dalam file `App.config` aplikasi POS pemanggil (`TryDreamposXGoapp\\App.config`):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <!-- 1. Connection String Database SQL Server untuk Logging API -->
    <connectionStrings>
        <add name="POSDatabase" 
             connectionString="Server=SASL16\\SQLEXPRESS03;Database=POS_Restaurant;User Id=dreampos;Password=sophialatjuba;TrustServerCertificate=True;MultipleActiveResultSets=True;" 
             providerName="System.Data.SqlClient" />
    </connectionStrings>

    <!-- 2. Parameter Kredensial & Endpoint Goapp CRM -->
    <appSettings>
        <add key="Goapp.ApiKey" value="138350315235400" />
        <add key="Goapp.ApiSecret" value="cbf5b1c921b36bfb06ca988c6d98f608482c40f1" />
        <add key="Goapp.ChannelUid" value="138350315235400" />
        <add key="Goapp.AuthBaseUrl" value="https://account.goapp.co.id/auth" />
        <add key="Goapp.ChannelBaseUrl" value="https://api.goapp.co.id/channel/v1" />
        
        <!-- Konfigurasi Physical Store (Wajib untuk Burn Point) -->
        <add key="Goapp.StoreUid" value="138533231320136" />
        <add key="Goapp.StoreName" value="Go DimSum Bogor" />
        <add key="Goapp.StoreCode" value="DS" />
    </appSettings>
</configuration>
```

---

## 2. Cara Pemanggilan & Inisialisasi DLL (`GoappApiClient`)
Di dalam project POS Anda, tambahkan referensi ke `DreamposXGoapp.dll`. 

Sesuai arsitektur terbaru **Single-Source Configuration (1 Jalur Konfigurasi)**, library DLL tidak membaca `ConfigurationManager` sendiri. Aplikasi pemanggil membaca parameter dari `App.config` lalu mengopernya secara eksplisit ke objek `GoappConfig`:

```vb
Imports System.Configuration
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services

' 1. Baca connection string database log dari App.config
Dim dbConnStr As String = String.Empty
Dim connSetting = ConfigurationManager.ConnectionStrings("POSDatabase")
If connSetting IsNot Nothing Then
    dbConnStr = connSetting.ConnectionString
End If

' 2. Susun objek GoappConfig (Passing parameter lengkap 1 pintu)
Dim config As New GoappConfig With {
    .ApiKey = ConfigurationManager.AppSettings("Goapp.ApiKey"),
    .ApiSecret = ConfigurationManager.AppSettings("Goapp.ApiSecret"),
    .ChannelUid = Long.Parse(ConfigurationManager.AppSettings("Goapp.ChannelUid")),
    .AuthBaseUrl = ConfigurationManager.AppSettings("Goapp.AuthBaseUrl"),
    .ChannelBaseUrl = ConfigurationManager.AppSettings("Goapp.ChannelBaseUrl"),
    .StoreUid = Long.Parse(ConfigurationManager.AppSettings("Goapp.StoreUid")),
    .StoreName = ConfigurationManager.AppSettings("Goapp.StoreName"),
    .StoreCode = ConfigurationManager.AppSettings("Goapp.StoreCode"),
    .DbConnectionString = dbConnStr,           ' <-- Connection string SQL Server untuk tabel GoappApiLog
    .LogTableName = "[dbo].[GoappApiLog]",       ' <-- Opsional (Default: [dbo].[GoappApiLog])
    .TimeoutSeconds = 30,
    .MaxRetryAttempts = 3,
    .RetryDelayMilliseconds = 1500
}

' 3. Instansiasi Client
Dim goappClient As New GoappApiClient(config)

' Opsional: Tangkap log aktivitas HTTP secara real-time
AddHandler goappClient.OnLog, Sub(msg As String)
                                  Console.WriteLine(msg)
                              End Sub
```"""

new_voucher = """### A. Validasi Voucher & Ekstraksi Objek `reward_data` (Lengkap)
Digunakan saat kasir memasukkan kode voucher pelanggan. DLL memetakan seluruh detail diskon, termasuk objek bersarang `reward_data` (tipe diskon, batas maksimal diskon, minimal belanja, dsb):

**Pemanggilan DLL:**
```vb
Dim code = "0XJPU"
Dim memberUid As Long? = 157689623681608 ' Opsional: sertakan UID member jika ada

Dim resp = Await goappClient.ValidateVoucherAsync(code, memberUid)

If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
    Dim v = resp.Data
    Dim rd = v.RewardData ' Objek RewardDataInfo lengkap
    
    ' Tampilkan pada Label / Form
    lblVoucherStatus.Text = $"Voucher Valid: {v.Name} ({v.DealCode})"
    
    ' Ambil setiap variabel reward_data
    If rd IsNot Nothing Then
        For Each line In rd.GetDetailLines()
            ' Contoh output baris:
            ' • discount_type : percentage
            ' • discount_amount : 20% (nilai asli: 0.2)
            ' • min_purchase : Rp 200,000
            ' • discount_max_amount : Rp 50,000
            Console.WriteLine(line)
        Next
        
        ' Atau akses variabel properti langsung:
        Dim tipeDiskon = rd.EffectiveDiscountType
        Dim nominal = rd.EffectiveDiscountAmount
        Dim minBelanja = rd.EffectiveMinPurchase
        Dim maksDiskon = rd.EffectiveDiscountMaxAmount
    End If
    
    ' Tampilkan ringkasan lengkap di MessageBox
    MessageBox.Show(String.Join(Environment.NewLine, rd.GetDetailLines()), "Detail reward_data")
End If
```

**Contoh Response JSON (`POST /member/deal/validate/`):**
```json
{
  "id": 2342233,
  "code": "0XJPU",
  "deal": {
    "uid": 157809151046728,
    "name": "Diskon 20%",
    "description": "Potongan belanja 20% min purchase 200rb",
    "reward_data": {
      "discount_type": "percentage",
      "min_purchase": 200000,
      "discount_amount": 0.2,
      "reward": {
        "apply_to": "lines_total",
        "discount_amount_type": "percentage",
        "discount_amount": 0.2,
        "discount_max_amount": 50000
      }
    }
  },
  "status_code": "active"
}
```"""

text = re.sub(r'(?s)## 1\. Persiapan Konfigurasi \(App\.config\).*?Dim goappClient As New GoappApiClient\(config\)\s+```', lambda m: new_sec1_2, text)
text = re.sub(r'(?s)### A\. Validasi Voucher \(Cek Tipe & Nominal Diskon\).*?\"status\": \"valid\"\s+\}\s+```', lambda m: new_voucher, text)

with open("README.md", "w", encoding="utf-8") as f:
    f.write(text)

print("Update README.md complete")
