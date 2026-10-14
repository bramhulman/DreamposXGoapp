# Dokumentasi Integrasi API Goapp CRM x DreamPOS

Dokumen ini berisi panduan teknis, contoh kode (cara pemanggilan `.dll`), dan *raw* JSON (Request & Response) berdasarkan hasil pengujian integrasi antara DreamPOS dan backend Goapp CRM.

---

## 1. Persiapan Konfigurasi (App.config Aplikasi Pemanggil)
Agar tidak terjadi *hardcoding* di dalam `.dll`, seluruh kredensial API dan koneksi database SQL Server untuk logging didefinisikan secara terpusat di dalam file `App.config` aplikasi POS pemanggil (`TryDreamposXGoapp\App.config`):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <!-- 1. Connection String Database SQL Server untuk Logging API -->
    <connectionStrings>
        <add name="POSDatabase" 
             connectionString="Server=SASL16\SQLEXPRESS03;Database=POS_Restaurant;User Id=dreampos;Password=sophialatjuba;TrustServerCertificate=True;MultipleActiveResultSets=True;" 
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
```

---

## 3. Fitur Utama & Contoh JSON

### A. Validasi Voucher & Ekstraksi Objek `reward_data` (Lengkap)
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
```

---

### B. Gunakan Voucher (Use Voucher)
Menggunakan voucher saat transaksi pembayaran dilakukan. Jika voucher memerlukan otorisasi (misal OTP/kode otorisasi dari aplikasi), sertakan parameter `authorizationCode`.

**Pemanggilan DLL:**
```vb
Dim code As String = "ARKAIS-100-6DR6"
Dim txRef As String = "TRX-261006095015"
Dim memberUid As Long? = 155999749384776
Dim authCode As String = "abc123" ' Opsional: isi jika sistem voucher memerlukan authorization_code

' Panggil fungsi Auto-Retry (otomatis mencoba ulang jika server busy / timeout):
Dim resp = Await goappClient.UseVoucherWithRetryAsync(code, txRef, memberUid, customMaxRetry:=3, authorizationCode:=authCode)

If resp.IsSuccess Then
    MessageBox.Show("Voucher berhasil digunakan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
Else
    MessageBox.Show($"Gagal menggunakan voucher: {resp.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
End If
```

**Request JSON (`POST /member/deal/use/`):**
```json
{
  "deal_code": "ARKAIS-100-6DR6",
  "transaction_ref": "TRX-261006095015",
  "member": {
    "uid": 155999749384776
  },
  "authorization_code": "abc123"
}
```

---

### C. Batalkan Voucher (Void / Cancel)
Membatalkan pemakaian voucher dan mengembalikannya ke HP pelanggan. **Catatan Penting:** Pemanggilan fungsi ini HANYA membatalkan status voucher, **tidak** membatalkan transaksi / struk kasir Anda. 

**Pemanggilan DLL:**
```vb
' Wajib menyertakan TxRef asal dan UID Member!
Dim resp = Await goappClient.CancelVoucherAsync("0M9NO", "TRX-261006095015", 155999749384776)
```

**Request JSON (`POST /member/deal/cancel_code/`):**
```json
{
  "deal_code": "0M9NO",
  "transaction_ref": "TRX-261006095015",
  "member": {
    "uid": 155999749384776
  }
}
```

**Contoh Response Error (400) Jika Lupa Menyisipkan UID Member:**
```json
{
  "detail": "Missing member information"
}
```

---

### D. Burn Point (Potong Poin Member)
**Catatan Penting:** 
1. Endpoint ini **menolak** penggunaan `Channel UID`. Harus mengirimkan parameter `store` lengkap dengan `Store UID` fisik dan `store_code`.
2. Jika alur auto-confirm memerlukan kode otorisasi dari member (misal OTP/PIN/Authorization Code dari app Goapp), kirimkan parameter `authorizationCode`.
3. Rasio perhitungan pemotongan di DLL sudah dikunci `1 Poin = 1 Rupiah` (Bukan dikali 1000).

**Pemanggilan DLL:**
```vb
Dim pointDibakar As Decimal = 5000
Dim orderAmountIdr As Decimal = 5000
Dim txRef As String = "TRX-261006090001"
Dim authCode As String = "asd123" ' Opsional: isi jika butuh validasi authorization_code

Dim resp = Await goappClient.CreatePointPaymentAsync(155999749384776, pointDibakar, orderAmountIdr, txRef, authorizationCode:=authCode)

If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
    Dim paymentRef = resp.Data.PaymentRef
    Dim status = resp.Data.Status
    MessageBox.Show($"Burn Point Berhasil! Ref: {paymentRef}, Status: {status}")
End If
```

**Request JSON (`POST /member/payment/`):**
```json
{
  "member": {
    "uid": 155999749384776
  },
  "amount": 5000,
  "order_amount": 5000,
  "order_currency": "idr",
  "provider_ref": "TRX-261006090001",
  "authorization_code": "asd123",
  "store": {
    "uid": 138533231320136,
    "name": "Go DimSum Bogor",
    "store_code": "DS"
  }
}
```

**Contoh Response Error (400) Jika Dashboard Belum Dikonfigurasi:**
```json
{
  "detail": "Payment for transaction type 'payment' not configured"
}
```

---

### E. Transaksi Pembayaran / Push Transaksi (Gabung Semua Metode Pembayaran via `member/payment/`)
Transaksi pembayaran diproses langsung melalui endpoint `{{CHANNEL_API_BASE_URL}}/member/payment/` dengan menggabungkan total nilai belanja (`order_amount`) tanpa perlu memisahkan rincian metode pembayaran (`payment_type`).

**Pemanggilan DLL:**
```vb
Dim memberUid As Long = 155999749384776
Dim totalBelanja As Decimal = 200000 ' Total nominal belanja (IDR)
Dim poinDibakar As Decimal = 0       ' Opsional: isi jika ada pemotongan poin member
Dim txRef As String = "TRX-" & DateTime.Now.ToString("yyMMdd-HHmmss")
Dim authCode As String = "asd123"    ' Opsional: kirim authorization_code jika diperlukan

Dim resp = Await goappClient.CreatePointPaymentAsync(memberUid, poinDibakar, totalBelanja, txRef, authorizationCode:=authCode)

If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
    Dim paymentRef = resp.Data.PaymentRef
    Dim status = resp.Data.Status
    MessageBox.Show($"Transaksi Berhasil! Ref: {paymentRef}, Status: {status}")
Else
    MessageBox.Show($"Transaksi Gagal: {resp.Message}")
End If
```

**Request JSON (`POST /member/payment/`):**
```json
{
  "member": {
    "uid": 155999749384776
  },
  "store": {
    "name": "Go DimSum Bogor",
    "store_code": "DS"
  },
  "provider_ref": "TRX-261014-142500",
  "order_amount": 200000.0,
  "order_currency": "idr",
  "authorization_code": "asd123"
}
```

---

## 5. Fitur Baru & Improvement Terbaru (Oktober 2026)
Beberapa pembaruan fungsional dan teknis yang telah ditambahkan ke dalam aplikasi POS:

1. **Penambahan Data Promo/Benefit di Tab Cek Member:**
   - Menampilkan promo otomatis yang dimiliki member langsung dari objek \direct_deal\ (API Get Member).
   - Kasir kini dapat langsung melihat nama promo dan kode redeem aktif tanpa perlu melakukan pencarian manual.

2. **Penyempurnaan UI Validasi Voucher (Tab 2):**
   - Menambahkan kotak **Raw JSON Textbox** di Tab 2 (Validasi & Gunakan Voucher) untuk mempermudah debugging dan pelaporan *request/response* API tanpa perlu berpindah tab.
   - Status Validasi Voucher yang tadinya hanya satu baris, kini diperbesar menjadi multiline untuk menampilkan rincian: **Nama Deal**, **Deskripsi**, **Tipe Diskon**, dan **Nominal Potongan**.

3. **Modern Database Logging (SQL Server - Arsitektur 1 Jalur Konfigurasi):**
   - Mengimplementasikan sistem logging *Asynchronous* ke dalam database SQL Server.
   - **Arsitektur 1 Jalur (Single-Source Configuration):**
     - Seluruh konfigurasi dan kredensial database murni dibaca dari file konfigurasi aplikasi pemanggil (yaitu TryDreamposXGoapp\App.config).
     - File .dll (DreamposXGoapp.dll) tidak membaca ConfigurationManager sendiri, tidak memiliki App.config internal, dan tidak menyimpan kredensial/connection string secara statis.
     - Semua parameter di-pass secara eksplisit melalui objek GoappConfig.DbConnectionString saat inisialisasi di Form pemanggil.
   - **Safety & Resiliency:** Jika koneksi DB tidak diisi atau database offline, logging akan di-skip secara graceful tanpa mengganggu atau membatalkan transaksi POS.
   - Atribut yang dilog meliputi:
     - LogDate & LogDateTime (Tanggal & Waktu presisi).
     - HttpMethod (GET/POST) & EndpointUrl.
     - RequestPayload & ResponsePayload (Raw JSON lengkap).
     - StatusCode & IsSuccess.
     - ErrorMessage (Pesan spesifik dari API / Exception aplikasi saat crash).
     - DurationMs (Waktu respons / Latency API dalam milidetik).


## 6. Update Ekstra (List Benefit Member)
1. **Fitur Cek Benefit/Promo Member (Tab 1)**
   - Mengimplementasikan pemanggilan API baru yaitu GET /member/member/{id}/deal_codes/?status=not_used.
   - API ini dipanggil secara transparan dan asinkron tepat setelah data utama member berhasil diambil.
   - Semua benefit dan promo (seperti voucher diskon atau free item) yang dimiliki oleh member tersebut akan langsung dirender ke dalam ListBox, mempermudah kasir menginformasikan penawaran yang tersedia kepada pelanggan.


