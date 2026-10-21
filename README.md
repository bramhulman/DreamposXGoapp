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
3. Rasio perhitungan pemotongan di DLL sudah dikunci `1 Poin = 1 Rupiah`.
4. Menyertakan field `total_order_amount` untuk mencatat total nilai belanja keseluruhan pada transaksi POS saat sebagian dibayar menggunakan poin (`order_amount`).

**Pemanggilan DLL:**
```vb
Dim pointDibakar As Decimal = 20000      ' Poin yang dipotong
Dim orderAmountIdr As Decimal = 20000    ' Nilai Rupiah dari pemotongan poin
Dim totalBelanjaPos As Decimal = 100000  ' Total nilai tagihan transaksi POS
Dim txRef As String = "TRX/23/232233"
Dim authCode As String = "asd123"        ' Opsional: isi jika butuh validasi authorization_code

Dim resp = Await goappClient.CreatePointPaymentAsync(110270830028872, pointDibakar, orderAmountIdr, txRef, authorizationCode:=authCode, totalOrderAmount:=totalBelanjaPos)

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
    "uid": "110270830028872"
  },
  "store": {
    "name": "JOBB Jakarta",
    "store_code": "JOBBJKT"
  },
  "provider_ref": "TRX/23/232233",
  "order_amount": "20000",
  "order_currency": "idr",
  "total_order_amount": "100000",
  "authorization_code": "asd123"
}
```

**Contoh Response Error (400) Jika Dashboard Belum Dikonfigurasi:**
```json
{
  "detail": "Payment for transaction type 'payment' not configured"
}
```

---

### E. Alur Push Transaksi POS (Sales Order & Burn Point)
Alur pengiriman transaksi penjualan dari kasir DreamPOS ke Goapp diatur dengan ketentuan:

1. **Seluruh Transaksi Penjualan Dikirim ke `POST {{CHANNEL_API_BASE_URL}}/sales/order/?calculate_estimate_point=1` (default terbaru):**
   - Berfungsi untuk mencatat pesanan penjualan dan penambahan poin (**Earning Points**).
   - Seluruh nilai pembayaran disatukan menjadi **1 baris payment saja** (`payment_method_name: "Total Paid"`, `amount: billTotal`) tanpa memecah rincian metode pembayaran lain.

   **Contoh Request JSON (`POST /sales/order/?calculate_estimate_point=1`):**
   ```json
   {
     "member": {
       "uid": 155999749384776,
       "mobile_no": "08123456789"
     },
     "provider_ref": "TRX-261014-142500",
     "order_no": "TRX-261014-142500",
     "order_date": "2026-10-14 14:25:00+07:00",
     "lines_total": 200000.0,
     "lines_tax": 0.0,
     "total_incl_tax": 200000.0,
     "total_paid": 200000.0,
     "paid_at": "2026-10-14 14:25:00+07:00",
     "completed_at": "2026-10-14 14:25:00+07:00",
     "lines": [
       {
         "product": {
           "sku": "POS-ITEM-01",
           "name": "Penjualan DreamPOS"
         },
         "quantity": 1,
         "price_before_discount": 200000.0,
         "price": 200000.0
       }
     ],
     "payments": [
       {
         "payment_method_name": "Total Paid",
         "payment_type": "CASH",
         "amount": 200000.0
       }
     ]
   }
   ```

2. **Jika Terdapat Burn Point (`burn_point > 0`), Sistem JUGA Memanggil `POST {{CHANNEL_API_BASE_URL}}/member/payment/`:**
   - Setelah / bersamaan dengan sales order, request pemotongan poin dikirim ke endpoint payment.
   - Mengirimkan nilai `order_amount` sebesar nilai poin yang dibakar (misal Rp 20.000), `total_order_amount` sebesar total belanja kasir (misal Rp 100.000), dan menyertakan `authorization_code` jika ada proteksi OTP.

   **Contoh Request JSON Tambahan (`POST /member/payment/`):**
   ```json
   {
     "member": {
       "uid": "110270830028872"
     },
     "store": {
       "name": "JOBB Jakarta",
       "store_code": "JOBBJKT"
     },
     "provider_ref": "TRX/23/232233",
     "order_amount": "20000",
     "order_currency": "idr",
     "total_order_amount": "100000",
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



## Retensi log SQL (3 bulan)

Setelah insert berhasil, DLL membersihkan log yang lebih tua dari `DATEADD(MONTH, -3, GETDATE())` menurut waktu SQL Server:

- `dbo.GoappApiLog`: berdasarkan `LogDateTime`.
- `dbo.GoappRedeemLog`: berdasarkan `Waktu`, pada insert sinkron maupun asinkron.

Data tepat pada batas 3 bulan tetap disimpan. Berlaku juga untuk nama tabel kustom. Pembersihan pertama berjalan pada insert pertama, lalu maksimal sekali setiap 24 jam per koneksi/tabel selama proses aplikasi hidup. Saat aplikasi tidak digunakan, pembersihan menunggu insert berikutnya. Delete dilakukan per batch 1.000 baris. Jika gagal, insert tetap berhasil dan pembersihan dicoba kembali pada insert setelah 5 menit. Akun SQL aplikasi memerlukan izin DELETE pada kedua tabel.

Penghapusan membebaskan ruang untuk dipakai ulang SQL Server; ukuran fisik file database tidak otomatis mengecil.
## Transaksi tanpa member dan analisis poin

Tersedia `GoappApiClient.PushGuestSalesOrderAsync(order)` untuk transaksi biasa tanpa member. Gunakan `Member = Nothing`, pembayaran selain POINT, dan nomor `ProviderRef` tetap. Method mewajibkan koneksi SQL, menyimpan audit ke `GoappApiLog` serta jurnal `SalesOrder` ke `GoappRedeemLog`, dan mencegah pengiriman ulang referensi sama. Migrasi kolom jurnal/index otomatis memerlukan izin ALTER tabel/CREATE INDEX. Hasil tidak pasti harus direkonsiliasi sebelum dikirim ulang.

Gunakan aplikasi TryDreamposXGoapp → tab 3. Transaksi POS → centang Ijinkan tanpa member → Kirim Transaksi Selesai. Form terpisah telah dihapus. Analisis kasus poin berlebih, bukti uji langsung HTTP 201 dengan reward 0, detail migrasi, batas anti-duplikat, dan cara penggunaan ada di [ANALISIS_POIN_DAN_ONLINE_DELIVERY.md](ANALISIS_POIN_DAN_ONLINE_DELIVERY.md).
Proses transaksi pada form utama menggunakan referensi umum TRX dan pembayaran CASH. Data uji otomatis berisi tiga produk dengan SKU berbeda serta pembagian nilai yang tepat sesuai total transaksi (satu produk untuk nominal di bawah Rp3). SKU tetap sama saat transaksi tersebut dikirim ulang. Pencatatan memakai Jenis=SalesOrder tanpa penanda online/delivery.


## Update 21 Oktober 2026: ringkasan estimasi poin sales order

Mengikuti `Channel API.postman_collection21Okt2026.json`, kedua metode push transaksi sekarang menggunakan:

```http
POST {{CHANNEL_API_BASE_URL}}/sales/order/?calculate_estimate_point=1
```

Parameter query `calculate_estimate_point` mendukung `1` (aktif) dan `0` (nonaktif). Parameter ini meminta tambahan `reward_summary` pada respons. **Ini tetap POST transaksi nyata yang menyimpan order dan dapat menghasilkan poin, bukan simulasi/dry-run.** Body JSON transaksi mengikuti pola sebelumnya; parameter tidak dimasukkan ke body.

Contoh tambahan respons dari koleksi terbaru:

```json
{
  "reward": [{ "amount": 5000, "est_new_balance": 213600 }],
  "reward_summary": { "amount": 7500, "est_new_balance": 216100 }
}
```

- `PushOrderResponse.RewardSummary.Amount`: total estimasi poin dari server, pada contoh 7.500.
- `PushOrderResponse.RewardSummary.EstNewBalance`: estimasi saldo baru dari server, pada contoh 216.100.
- `TotalEarnedPoints`: memakai `reward_summary.amount` jika nilainya tersedia; fallback ke jumlah rincian `reward[].amount` untuk respons lama. Nilai ringkasan 0 tetap 0.
- `TotalRewardDetailPoints`: jumlah rincian reward, pada contoh 5.000. **Jangan menjumlahkan reward 5.000 dengan ringkasan 7.500.**
- `EstimatedNewBalance`: memakai saldo ringkasan jika tersedia; fallback ke `est_new_balance` jika respons lama berisi tepat satu item reward. Saldo aktual tetap dapat diperiksa melalui GET member.

### Pemanggilan DLL

Method lama satu parameter tetap tersedia. Default `GoappConfig.CalculateEstimatePoint = True`; aplikasi pemanggil dapat mengubah default melalui konfigurasi yang dioper ke DLL, atau memilih per request melalui overload baru:

```vb
' Default terbaru, sesuai GoappConfig.CalculateEstimatePoint (True):
Dim result = Await client.PushSalesOrderAsync(order)

' Pilihan eksplisit per transaksi:
Dim withSummary = Await client.PushSalesOrderAsync(order, calculateEstimatePoint:=True)
Dim withoutSummary = Await client.PushSalesOrderAsync(orderLain, calculateEstimatePoint:=False)

' Tanpa member: tetap tidak mengirim identitas atau melakukan burn point.
orderTanpaMember.Member = Nothing
Dim guest = Await client.PushGuestSalesOrderAsync(orderTanpaMember, calculateEstimatePoint:=True)

If result.IsSuccess AndAlso result.Data IsNot Nothing Then
    Dim totalPoin = result.Data.TotalEarnedPoints
    Dim saldoEstimasi As Decimal? = result.Data.EstimatedNewBalance
End If
```

Gunakan data/nomor struk berbeda hanya untuk transaksi yang memang berbeda. Mengubah flag estimasi pada nomor transaksi yang sudah terkirim **tidak** membuat POST ulang: hasil sebelumnya dikembalikan dari jurnal/cache. Jika respons tersimpan belum berisi ringkasan, DLL memberi warning dan memakai fallback, bukan mengirim ulang demi mendapatkan ringkasan baru.

### Form Try dan konfigurasi

Di tab **3. Transaksi POS**, pilihan **Hitung estimasi poin** ditambahkan pada form yang sama, berdampingan dengan **Ijinkan tanpa member**. Default aktif; lepas centang untuk mengirim query `0`. Request/response panel menampilkan URL lengkap termasuk query. Pesan hasil menampilkan rincian reward, total dari ringkasan atau fallback, serta estimasi saldo baru.

Aplikasi Try membaca pengaturan berikut dari App.config, lalu meneruskannya ke GoappConfig. DLL tetap tidak membaca App.config sendiri:

```xml
<add key="Goapp.CalculateEstimatePoint" value="true" />
```

URL lengkap dan raw respons termasuk `reward_summary` disimpan pada `GoappApiLog`. `GoappRedeemLog.OrderResponseJson` menyimpan respons lengkap dan `NilaiPoint` memakai `TotalEarnedPoints`, sehingga mengikuti ringkasan bila tersedia. Tidak ada tabel atau migrasi SQL baru untuk pembaruan ini; retensi 3 bulan serta perlindungan kirim ganda tetap berlaku.

### Pengujian

Test fixture berasal dari contoh **Calculate estimated points earned** di koleksi 21 Oktober, termasuk angka 5.000/7.500/216.100. Pengujian memeriksa query 1/0, kedua overload, null/0/string decimal, fallback respons lama, flag saat retry 401, penyimpanan SQL, dan replay transaksi dengan perubahan flag tanpa POST tambahan. Jalankan `Tests/OrderSubmissionTests.exe` setelah build proyek test. Pengujian langsung Goapp dilakukan terpisah dengan referensi uji baru dan dicatat pada laporan analisis.

### Bukti uji langsung 21 Oktober 2026

- Tanpa member: `TEST-TRX-261021-100341-13fc3e`, tiga SKU, Rp150.000, HTTP 201, order UID `158386291108424`, poin 0. Kedua log SQL tersimpan dan pengulangan dicegah.
- Dengan member uji: `TEST-EST-261021-100352-414314`, tiga SKU, Rp150.000, HTTP 201, order UID `158386296021064`. Rincian reward 5.000; reward_summary.amount 5.000; reward_summary.est_new_balance 29.800. Saldo sebelum 24.800, GET segera sesudah POST masih 24.800, lalu GET ulang menunjukkan 29.800 (+5.000), sesuai ringkasan. Jangan POST ulang hanya karena saldo aktual belum langsung berubah.
- Masing-masing memakai query `calculate_estimate_point=1`, tersimpan di kedua tabel, dan hanya satu POST per referensi, termasuk ketika pemanggilan berikutnya mengubah flag.
- Sebanyak 43 pemeriksaan otomatis (HTTP tiruan dan SQL terpisah) serta pemeriksaan render form lulus. Bukti JSON ada di `Tests/live-estimate-result.json` dan `Tests/live-guest-result.json`. Halaman PDF pembaruan dan form Try telah diperiksa secara visual.