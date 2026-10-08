# Dokumentasi Integrasi API Goapp CRM x DreamPOS

Dokumen ini berisi panduan teknis, contoh kode (cara pemanggilan `.dll`), dan *raw* JSON (Request & Response) berdasarkan hasil pengujian integrasi antara DreamPOS dan backend Goapp CRM.

---

## 1. Persiapan Konfigurasi (App.config)
Agar *hardcoding* dapat dihindari, nilai-nilai kredensial dan preferensi toko disimpan di dalam file `App.config` aplikasi pemanggil (POS).

```xml
<appSettings>
    <!-- Kredensial Channel Goapp CRM -->
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
```

---

## 2. Instansiasi DLL (`GoappApiClient`)
Di dalam project POS Anda, pastikan Anda menambahkan *Reference* ke `DreamposXGoapp.dll`. Lalu inisialisasi *client* seperti berikut (Contoh dalam VB.NET):

```vb
Imports DreamposXGoapp.Models
Imports DreamposXGoapp.Services

' 1. Baca konfigurasi dari App.config
Dim config As New GoappConfig With {
    .ApiKey = ConfigurationManager.AppSettings("Goapp.ApiKey"),
    .ApiSecret = ConfigurationManager.AppSettings("Goapp.ApiSecret"),
    .ChannelUid = Long.Parse(ConfigurationManager.AppSettings("Goapp.ChannelUid")),
    .AuthBaseUrl = ConfigurationManager.AppSettings("Goapp.AuthBaseUrl"),
    .ChannelBaseUrl = ConfigurationManager.AppSettings("Goapp.ChannelBaseUrl"),
    .StoreUid = Long.Parse(ConfigurationManager.AppSettings("Goapp.StoreUid")),
    .StoreName = ConfigurationManager.AppSettings("Goapp.StoreName"),
    .StoreCode = ConfigurationManager.AppSettings("Goapp.StoreCode")
}

' 2. Instansiasi Client
Dim goappClient As New GoappApiClient(config)
```

---

## 3. Fitur Utama & Contoh JSON

### A. Validasi Voucher (Cek Tipe & Nominal Diskon)
Digunakan sebelum transaksi dibayar. Berfungsi untuk mengetahui tipe potongan (`amount` / `percentage`) dan nilainya.

**Pemanggilan DLL:**
```vb
Dim code = "0M9NO"
Dim memberUid As Long? = 155999749384776
Dim resp = Await goappClient.ValidateVoucherAsync(code, memberUid)

If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
    Console.WriteLine($"Diskon: {resp.Data.DiscountAmount} Tipe: {resp.Data.DiscountType}")
End If
```

**Request JSON (`POST /member/deal/validate/`):**
```json
{
  "deal_code": "0M9NO",
  "member": {
    "uid": 155999749384776
  }
}
```

**Response JSON (Sukses):**
```json
{
  "uid": 138501234567,
  "deal_code": "0M9NO",
  "name": "Diskon Spesial DimSum",
  "discount_type": "amount",
  "discount_amount": 50000.0,
  "status": "valid"
}
```

---

### B. Batalkan Voucher (Void / Cancel)
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

### C. Burn Point (Potong Poin Member)
**Catatan Penting:** 
1. Endpoint ini **menolak** penggunaan `Channel UID`. Harus mengirimkan parameter `store` lengkap dengan `Store UID` fisik dan `store_code`.
2. Jika admin belum mengaktifkan metode Point Payment di Dashboard, akan muncul Error `400 Payment not configured`.
3. Rasio perhitungan pemotongan di DLL sudah dikunci `1 Poin = 1 Rupiah` (Bukan dikali 1000).

**Pemanggilan DLL:**
```vb
Dim pointDibakar As Decimal = 5000
Dim txRef As String = "TRX-261006090001"
Dim resp = Await goappClient.CreatePointPaymentAsync(155999749384776, pointDibakar, pointDibakar, txRef)
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

### D. Push Transaksi (Earning Point)
Mencatat total transaksi ke Goapp. Perhitungan berapa poin yang didapat pelanggan **DIHITUNG OLEH SERVER GOAPP**, bukan aplikasi kasir (Misal: Batas maksimal per hari, tidak berlaku kelipatan belanja, dsb diatur di Dashboard CRM Goapp).

**Pemanggilan DLL:**
```vb
Dim lines As New List(Of OrderLineItem)
lines.Add(New OrderLineItem With {.Sku = "ITEM-1", .Name = "Baju A", .Quantity = 1, .Price = 100000})

' Pembayaran bisa berisi banyak metode (Gopay, EDC, Tunai, dsb)
Dim payments As New List(Of Object)
payments.Add(New With { .payment_method_name = "Cash", .payment_type = "CASH", .amount = 100000 })

Dim resp = Await goappClient.PushSalesOrderAsync(
    orderNo:="POS-261005",
    providerRef:="POS-261005",
    memberUid:=155999749384776,
    mobileNo:="087890760858",
    lines:=lines,
    payments:=payments
)

' Baca poin yang didapat (dihitung oleh Goapp)
Dim pointDidapat = If(resp.Data.Reward.Count > 0, resp.Data.Reward(0).Amount, 0)
```

**Request JSON (`POST /sales/order/`):**
*(Bisa menggunakan base channel (tanpa parameter `store`) jika TIDAK ADA metode pembayaran point `POINT` di dalamnya. Server Goapp akan otomatis me-mapping ke Store Default.)*
```json
{
  "order_no": "POS-261005-165553",
  "provider_ref": "POS-261005-165553",
  "order_date": "2026-10-05 16:55:53+07:00",
  "member": {
    "uid": 155999749384776,
    "mobile_no": "081588809090"
  },
  "lines": [
    {
      "product": {
        "sku": "ITEM-1",
        "name": "Baju A"
      },
      "quantity": 1,
      "price": 100000.0,
      "price_before_discount": 100000.0
    }
  ],
  "lines_total": 100000.0,
  "lines_tax": 0.0,
  "total_incl_tax": 100000.0,
  "total_paid": 100000.0,
  "payments": [
    {
      "payment_method_name": "Cash",
      "payment_type": "CASH",
      "amount": 100000.0
    }
  ],
  "completed_at": "2026-10-05 16:55:53+07:00",
  "paid_at": "2026-10-05 16:55:53+07:00"
}
```

**Response JSON (Menunjukkan Member tidak dapat poin karena terkena limit dashboard / bukan kelipatan):**
```json
{
  "order_no": "SO-000902",
  "status": "Invalid",
  "reward": [
    {
      "amount": 0,
      "est_new_balance": 109410.0
    }
  ]
}
```

## 5. Fitur Baru & Improvement Terbaru (Oktober 2026)
Beberapa pembaruan fungsional dan teknis yang telah ditambahkan ke dalam aplikasi POS:

1. **Penambahan Data Promo/Benefit di Tab Cek Member:**
   - Menampilkan promo otomatis yang dimiliki member langsung dari objek \direct_deal\ (API Get Member).
   - Kasir kini dapat langsung melihat nama promo dan kode redeem aktif tanpa perlu melakukan pencarian manual.

2. **Penyempurnaan UI Validasi Voucher (Tab 2):**
   - Menambahkan kotak **Raw JSON Textbox** di Tab 2 (Validasi & Gunakan Voucher) untuk mempermudah debugging dan pelaporan *request/response* API tanpa perlu berpindah tab.
   - Status Validasi Voucher yang tadinya hanya satu baris, kini diperbesar menjadi multiline untuk menampilkan rincian: **Nama Deal**, **Deskripsi**, **Tipe Diskon**, dan **Nominal Potongan**.

3. **Modern Database Logging (SQL Server):**
   - Mengimplementasikan sistem logging *Asynchronous* ke dalam database **POS_Restaurant**.
   - Setiap kali POS melakukan *request* ke API Goapp, sistem otomatis akan menyimpan riwayat interaksi di tabel \[dbo].[GoappApiLog]\ secara *fire-and-forget* (tidak membuat UI freeze/lag).
   - Atribut yang dilog meliputi:
     - \LogDate\ & \LogDateTime\ (Tanggal & Waktu presisi).
     - \HttpMethod\ (GET/POST) & \EndpointUrl\.
     - \RequestPayload\ & \ResponsePayload\ (Raw JSON lengkap).
     - \StatusCode\ & \IsSuccess\.
     - \ErrorMessage\ (Pesan spesifik dari API / Exception aplikasi saat crash).
     - \DurationMs\ (Waktu respons / Latency API dalam milidetik).


## 6. Update Ekstra (List Benefit Member)
1. **Fitur Cek Benefit/Promo Member (Tab 1)**
   - Mengimplementasikan pemanggilan API baru yaitu GET /member/member/{id}/deal_codes/?status=not_used.
   - API ini dipanggil secara transparan dan asinkron tepat setelah data utama member berhasil diambil.
   - Semua benefit dan promo (seperti voucher diskon atau free item) yang dimiliki oleh member tersebut akan langsung dirender ke dalam ListBox, mempermudah kasir menginformasikan penawaran yang tersedia kepada pelanggan.
