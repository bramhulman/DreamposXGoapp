# Analisis poin dan transaksi tanpa member — 16 Oktober 2026

## Kesimpulan berdasarkan bukti

Untuk member UID `157557895826504`, tiga transaksi yang diminta menunjukkan satu POST `/sales/order/` per referensi pada log SQL, reward respons 5.000, dan kenaikan saldo GET member 12.500. Selisih 7.500 sudah terdapat dalam respons server GET member; bukan hasil penjumlahan tampilan aplikasi.

| provider_ref | Log POST / GET member | Nominal | Saldo sebelum | Reward respons | est_new_balance | Saldo GET sesudah | Kenaikan aktual | Selisih dari reward |
|---|---|---:|---:|---:|---:|---:|---:|---:|
| TRX-261016-113743 | 270 / 271 | 190.000 | 27.300 | 5.000 | 32.300 | 39.800 | 12.500 | 7.500 |
| TRX-261016-114146 | 273 / 274 | 150.000 | 39.800 | 5.000 | 44.800 | 52.300 | 12.500 | 7.500 |
| TRX-261016-114836 | 278 / 279 | 150.000 | 52.300 | 5.000 | 57.300 | 64.800 | 12.500 | 7.500 |

Kasus pendukung sebelumnya: `TRX-261016-113513`, log 267/268, nominal 190.000, saldo 14.800 → 27.300, reward 5.000, estimasi 19.800. Polanya sama.

`reward.account.uid` dan akun pada GET member sama, yaitu `157557895813704`. Setiap respons order pada tiga kasus tersebut berisi satu item reward sebesar 5.000. Tidak ada perbedaan akun atau penjumlahan beberapa reward yang menjelaskan selisih pada kasus ini.

## Pemeriksaan penyebab

1. **Pengiriman sales order ganda:** tidak ditemukan `provider_ref` berulang di log sales order tanggal 16 Oktober untuk kasus yang diperiksa. Handler tombol hanya memiliki satu pemanggilan `PushSalesOrderAsync`; pencarian member sesudahnya adalah GET. Tidak ada POST payment di antara tiga POST order tersebut dan GET member berikutnya.
2. **Retry otomatis:** jalur push sales tidak memakai loop retry voucher. Helper HTTP hanya mengulang satu kali setelah respons eksplisit 401. Ketiga request kasus ini tercatat HTTP 201, tanpa 401 pada log tersebut.
3. **Kesalahan parsing lokal:** `AvailablePoints` membaca `account.balance` dengan invariant culture. Nilai 64.800 ada di JSON GET server. Aplikasi tidak mengubah saldo server lewat properti lokal tersebut.
4. **Reward tambahan di Goapp:** kenaikan yang konsisten 12.500 = 5.000 + 7.500 mengarah pada aturan bonus/campaign/tier/referral tambahan atau proses pemberian reward lain di server. Ini hipotesis yang kuat dari pola, belum bukti aturan spesifik. Pengiriman ganda sederhana reward 5.000 akan menghasilkan 10.000, bukan 12.500.
5. **Pemetaan scheme/store:** respons menunjukkan member pada scheme Din Tai Fung, sedangkan store yang dikirim Go DimSum Bogor. Periksa apakah konfigurasi akun/reward lintas scheme/store ini memang dimaksudkan. Ini temuan konfigurasi, bukan kesimpulan penyebab pasti.
6. **Batas bukti:** log lama ditulis fire-and-forget sehingga request yang auditnya gagal dapat tidak tercatat. Tidak dapat menyingkirkan proses integrasi lain di luar aplikasi atau pekerjaan asynchronous di Goapp hanya dari database lokal.

Untuk menentukan penyebab pasti, admin Goapp perlu memeriksa ledger akun `157557895813704` dan sumber event/campaign pada order UID `158168018556488`, `158168143265352`, dan `158168352737352`, khususnya kredit tambahan 7.500 di setiap order. Jangan mengurangi saldo atau mengirim ulang order sebagai percobaan perbaikan.

Temuan lain di form lama: menekan Burn Point secara terpisah lalu Push Transaksi dengan nilai burn masih terisi dapat membuat dua POST payment dengan referensi berbeda. Ini menyebabkan pemotongan ganda, bukan kenaikan 7.500 pada tiga kasus di atas. Untuk pengujian earning saja, gunakan nilai burn 0. Perhitungan saldo pada preview struk juga bersifat estimasi lokal; gunakan respons GET member sebagai sumber saldo aktual.

## Perubahan implementasi

- Metode DLL baru `GoappApiClient.PushGuestSalesOrderAsync(PushOrderRequest)` mengirim ke endpoint sales order yang sama tanpa properti `member` dan tanpa memanggil API payment/burn. Input yang berisi member atau payment type POINT ditolak.
- Semua push order memakai nomor `provider_ref` tetap. Default form tidak membuat nomor baru pada setiap penekanan tombol; tersedia tombol Transaksi baru.
- Saat SQL dikonfigurasi, reservasi di `GoappRedeemLog` menggunakan key SHA-256 untuk endpoint/channel/store/provider_ref dan unique filtered index. Ini mencegah dua client/proses yang memakai database/jurnal sama mengirim order yang sama bersamaan.
- Respons sukses yang sudah tersimpan dikembalikan saat dipanggil ulang tanpa POST lagi. Payload berubah dengan nomor yang sama ditolak. Pending/Unknown/Failed tidak dikirim ulang otomatis; periksa hasil di Goapp dan lakukan rekonsiliasi oleh operator sebelum tindakan berikutnya.
- Tanpa SQL, transaksi member memiliki penjagaan dalam proses aplikasi saja. Transaksi guest mewajibkan SQL agar ada kedua log. Perlindungan berbasis jurnal berlaku untuk pengiriman versi baru dan selama jurnal masih tersimpan; retensi tiga bulan tetap berlaku. Referensi berbeda diperlakukan sebagai transaksi berbeda.
- `GoappApiLog` ditunggu selesai sebelum method HTTP mengembalikan respons, termasuk audit attempt 401 sebelum retry. HTTP status asli dipertahankan bila parsing JSON gagal. Tidak ada retry otomatis untuk timeout/5xx.
- `GoappRedeemLog` mendapatkan kolom tambahan `OrderSubmissionKey`, `OrderSubmissionState`, `OrderRequestJson`, `OrderResponseJson`, `OrderHttpStatus`, `OrderApiLogSaved`, dan unique filtered index `UX_OrderSubmissionKey`. Migrasi idempoten berjalan pada penggunaan pertama. Akun SQL perlu izin ALTER/CREATE INDEX serta akses tabel; jika reservasi SQL gagal, POST tidak dilakukan.
- Transaksi tanpa member dicatat sebagai `Jenis=SalesOrder`, `MemberUid=NULL`, `NilaiPoint=0` pada hasil normal, dan `StatusPos=1`, sehingga tidak muncul sebagai redeem aktif yang dapat dibatalkan. Sales member dicatat sebagai `Jenis=SalesOrder`.
- `TotalEarnedPoints` menjumlahkan seluruh item reward respons. Form transaksi member menampilkan saldo sebelum/sesudah, selisih aktual, dan reward server. Nilai reward dari server tidak diubah atau dipaksakan oleh DLL.
- Jika server mengembalikan reward pada transaksi guest, respons diberi warning dan `RequiresReconciliation=True`; DLL tidak mengirim ulang dan tidak berpura-pura menghapus reward server.

## Bukti uji langsung Goapp

Pengujian produksi dilakukan atas izin pengguna dengan satu transaksi khusus uji:

| Bukti | Hasil |
|---|---|
| provider_ref | TEST-OD-261016-123514-f0a699 |
| Nilai transaksi | Rp150.000 |
| Request member | Tidak dikirim |
| HTTP | 201 Created |
| Goapp order_no | SO-000969 |
| Goapp order UID | 158169786030664 |
| reward pada JSON respons | null |
| Poin diperoleh | 0 |
| GoappApiLog.Id | 281 |
| GoappRedeemLog.Id | 1 |
| Jurnal | OnlineDelivery / Succeeded / StatusPos=1 / NilaiPoint=0 / MemberUid=NULL |
| Pemanggilan kedua, payload sama | Hasil tersimpan, IsDuplicate=True |
| Jumlah POST di GoappApiLog untuk referensi ini | 1 |

Transaksi uji ini tetap ada di Goapp agar bukti penerimaan dapat diperiksa. Pengujian ini membuktikan penerimaan data tanpa member dan tanpa reward langsung. Alur claim poin oleh pelanggan sesudahnya adalah fitur Goapp dan tidak termasuk implementasi ini.

## Cara menggunakan

Buka `TryDreamposXGoapp/bin/Release/TryDreamposXGoapp.exe`, pilih tab **3. Transaksi POS**, centang **Ijinkan tanpa member**, lalu gunakan tombol **Kirim Transaksi Selesai** yang sama. Tidak ada form terpisah. Member tidak dikirim dan burn point diabaikan saat opsi dicentang, termasuk bila sebelumnya sudah memilih member. Tiga SKU data uji dibuat otomatis; total harga sesuai nominal transaksi dan SKU tetap sama untuk referensi yang sama. Gunakan Transaksi baru hanya untuk transaksi baru.

Contoh pemanggilan DLL setelah order berisi detail produk, pembayaran, serta total:

```vb
' order adalah PushOrderRequest dengan data transaksi POS yang sebenarnya.
order.Member = Nothing
order.ProviderRef = nomorStrukTetap
order.OrderNo = nomorStrukTetap
Dim result = Await client.PushGuestSalesOrderAsync(order)
' Periksa IsSuccess, ApiLogSaved, TransactionLogSaved, WarningMessage,
' RequiresReconciliation, dan result.Data.TotalEarnedPoints.
' Jangan membuat referensi baru untuk mengulang transaksi yang hasilnya belum pasti.
```

## Validasi

Build Release DLL dan aplikasi test berhasil. Ada lima warning XML documentation lama saat kompilasi penuh; tidak ada error kompilasi. Sebanyak 25 assertion otomatis berhasil: jumlah seluruh reward, duplicate/concurrent order, payload berubah, timeout/500/parsing, retry 401, validasi guest, dua log SQL, status redeem, serta retensi tiga bulan. Pengujian SQL otomatis memakai tabel sementara bernama khusus uji dan membersihkannya, bukan menghapus log produksi.

Lima assertion uji langsung berhasil: penerimaan HTTP 201, reward 0, kedua log tersimpan, pengulangan dicegah, dan satu POST pada SQL. Metadata endpoint juga berhasil dibaca dengan OPTIONS 200.

Jalankan pengujian dari root repository menggunakan MSBuild Visual Studio:

```powershell
& 'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe' Tests/OrderSubmissionTests.csproj /t:Build /p:Configuration=Release /nr:false /m:1
& './Tests/bin/OrderSubmissionTests.exe'
```

Opsi `--preflight` hanya membaca metadata API. Opsi `--live-guest` membuat satu transaksi nyata di Goapp dan hanya digunakan bila pengiriman uji memang dikehendaki. Pengujian SQL memakai konfigurasi `POSDatabase` dari App.config; kredensial tidak disalin ke source test.
## Pembaruan tampilan dan identitas transaksi

Proses pada form utama menggunakan identitas transaksi umum: referensi TRX, beberapa SKU otomatis dengan nama Produk 1–3, dan pembayaran CASH. Form transaksi tanpa member terpisah telah dihapus. GoappRedeemLog menggunakan Jenis=SalesOrder untuk transaksi dengan maupun tanpa member. Bukti transaksi uji lama pada tabel di atas dipertahankan sebagai catatan historis; baris lama di SQL/Goapp tidak diubah.

## Uji ulang proses terpadu pada form utama

Uji langsung berikutnya menggunakan generator tiga SKU yang sama dengan form utama, tanpa member: `TEST-TRX-261016-135217-acc127`, nominal Rp150.000, Goapp order UID `158172153091144`. Hasil HTTP 201, reward 0, kedua log tersimpan, serta satu POST saja saat referensi yang sama dipanggil ulang. Sebanyak 25 assertion otomatis lulus, termasuk jumlah SKU, ketepatan total harga, dan konsistensi SKU untuk kirim ulang.