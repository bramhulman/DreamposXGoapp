import os
from fpdf import FPDF

class PDFReport(FPDF):
    def header(self):
        if self.page_no() > 1:
            self.set_font("helvetica", "I", 8)
            self.set_text_color(100, 116, 139)
            self.cell(0, 7, "Dokumentasi Integrasi DreamPOS x Goapp CRM - Modul Teknis & Panduan", 0, 0, "L")
            self.cell(0, 7, "Versi 1.0", 0, 1, "R")
            self.set_draw_color(226, 232, 240)
            self.line(15, 17, 195, 17)
            self.ln(4)

    def footer(self):
        self.set_y(-15)
        self.set_font("helvetica", "I", 8)
        self.set_text_color(148, 163, 184)
        self.set_draw_color(226, 232, 240)
        self.line(15, self.get_y(), 195, self.get_y())
        self.cell(0, 10, f"Halaman {self.page_no()}/{{nb}}", 0, 0, "C")

    def chapter_title(self, num, label):
        self.set_font("helvetica", "B", 13)
        self.set_text_color(30, 58, 138) # Dark blue
        self.set_fill_color(239, 246, 255)
        self.cell(0, 9, f"{num}. {label}", 0, 1, "L", fill=True)
        self.ln(3)

    def section_title(self, label):
        self.set_font("helvetica", "B", 11)
        self.set_text_color(15, 118, 110) # Teal
        self.cell(0, 7, label, 0, 1, "L")
        self.ln(1)

    def body_p(self, text):
        self.set_font("helvetica", "", 9.5)
        self.set_text_color(36, 41, 46)
        self.multi_cell(0, 5, text)
        self.ln(2)

    def code_block(self, code_text):
        self.set_font("courier", "", 8)
        self.set_fill_color(15, 23, 42) # Dark background
        self.set_text_color(248, 250, 252) # Light text
        lines = code_text.strip().split("\n")
        max_w = 180
        # Calculate height
        h = len(lines) * 4.2 + 4
        self.set_x(15)
        # Background rect
        y_start = self.get_y()
        if y_start + h > 275:
            self.add_page()
            y_start = self.get_y()
        self.rect(15, y_start, max_w, h, "F")
        self.set_y(y_start + 2)
        for line in lines:
            self.set_x(17)
            self.cell(max_w - 4, 4.2, line[:95], 0, 1, "L")
        self.set_y(y_start + h + 3)
        self.set_text_color(36, 41, 46)

    def callout(self, title, text, bg_r=239, bg_g=246, bg_b=255, border_r=59, border_g=130, border_b=246):
        y_start = self.get_y()
        self.set_font("helvetica", "B", 9)
        self.set_text_color(border_r, border_g, border_b)
        self.set_fill_color(bg_r, bg_g, bg_b)
        
        # Draw background and left border
        self.set_x(15)
        lines_text = text.split("\n")
        h = 8 + len(lines_text) * 4.5 + 4
        if y_start + h > 275:
            self.add_page()
            y_start = self.get_y()
            
        self.rect(15, y_start, 180, h, "F")
        self.set_draw_color(border_r, border_g, border_b)
        self.set_line_width(1.0)
        self.line(15, y_start, 15, y_start + h)
        self.set_line_width(0.2)
        
        self.set_y(y_start + 2)
        self.set_x(18)
        self.cell(170, 5, title, 0, 1, "L")
        self.set_font("helvetica", "", 8.5)
        self.set_text_color(36, 41, 46)
        for l in lines_text:
            self.set_x(18)
            self.multi_cell(172, 4.2, l)
        self.set_y(y_start + h + 3)

def create_pdf(output_path):
    pdf = PDFReport(orientation="P", unit="mm", format="A4")
    pdf.set_margins(15, 15, 15)
    pdf.set_auto_page_break(auto=True, margin=18)
    pdf.alias_nb_pages()

    # COVER / HEADER
    pdf.add_page()
    pdf.set_font("helvetica", "B", 18)
    pdf.set_text_color(30, 58, 138)
    pdf.cell(0, 10, "DOKUMENTASI INTEGRASI POS X GOAPP CRM", 0, 1, "C")
    
    pdf.set_font("helvetica", "B", 10.5)
    pdf.set_text_color(75, 85, 99)
    pdf.cell(0, 6, "Panduan Arsitektur Multi-Project VB.NET, Spesifikasi API & Skenario Pengujian", 0, 1, "C")
    
    pdf.set_draw_color(30, 58, 138)
    pdf.set_line_width(0.8)
    pdf.line(15, 30, 195, 30)
    pdf.set_line_width(0.2)
    pdf.ln(6)

    # META INFO BOX
    pdf.set_fill_color(248, 250, 252)
    pdf.set_draw_color(203, 213, 225)
    pdf.rect(15, 33, 180, 20, "DF")
    pdf.set_y(34)
    pdf.set_font("helvetica", "B", 8.5)
    pdf.set_text_color(51, 65, 85)
    pdf.set_x(18)
    pdf.cell(85, 5, "Aplikasi POS   : DreamPOS (Visual Basic .NET)", 0, 0, "L")
    pdf.cell(85, 5, "Target Platform: VS 2022 / .NET 4.7.2", 0, 1, "L")
    pdf.set_x(18)
    pdf.cell(85, 5, "Sistem CRM     : Goapp Cloud CRM (Channel API)", 0, 0, "L")
    pdf.cell(85, 5, "Tanggal Terbit : Oktober 2026 | Versi 1.0", 0, 1, "L")
    pdf.set_x(18)
    pdf.cell(85, 5, "Channel Toko   : Dream POS (UID: 138350315235400)", 0, 0, "L")
    pdf.cell(85, 5, "Status Test    : Terverifikasi Aktif (HTTP 200)", 0, 1, "L")
    pdf.set_y(57)

    # 1. RINGKASAN EKSEKUTIF
    pdf.chapter_title("1", "Ringkasan Eksekutif & Arsitektur Solusi")
    pdf.body_p(
        "Dokumentasi ini disusun untuk memandu tim engineering dalam membangun, menguji, dan memelihara "
        "jembatan integrasi antara sistem kasir Point of Sale (DreamPOS) dengan sistem CRM Goapp. "
        "Arsitektur solusi dibangun menggunakan Multi-Project dalam 1 Solution pada Visual Studio 2022:"
    )
    pdf.body_p(
        "1. DreamposXGoapp (.dll) - Class Library independen berisi seluruh logika protokol HTTP, token authentication "
        "caching, pencarian & validasi member, kalkulasi point balance, auto-retry voucher, dan generator struk thermal.\n"
        "2. TryDreamposXGoapp (.exe) - Windows Forms App berfungsi sebagai Test Harness visual-manual untuk "
        "menguji seluruh fungsionalitas secara terisolasi sebelum dimasukkan ke aplikasi kasir produksi."
    )
    pdf.callout(
        "Verifikasi Kredensial Langsung ke Server Goapp (Sukses 100%):",
        "Auth Endpoint: https://account.goapp.co.id/auth/token-auth/ -> Status 200 OK (Token Aktif)\n"
        "Channel Info : https://api.goapp.co.id/channel/v1/directory/channel/0/ -> Nama Channel: 'Dream POS'",
        bg_r=236, bg_g=253, bg_b=245, border_r=16, border_g=185, border_b=129
    )

    # 2. STRUKTUR PROYEK
    pdf.chapter_title("2", "Struktur Solusi & Konfigurasi Visual Studio 2022")
    pdf.section_title("2.1 Struktur File Workspace")
    pdf.code_block(
"""D:\\PCL\\DreamposXGoapp\\
|-- DreamposXGoapp.sln                  <-- Solution Multi-Project
|-- DreamposXGoapp.vbproj               <-- [PROYEK 1] Class Library (.dll)
|   |-- Models\\GoappConfig.vb          (Konfigurasi Key, Secret, Endpoint, Retry)
|   |-- Models\\GoappModels.vb          (DTO: Member, Account, Voucher, Order, Payment)
|   |-- Services\\GoappApiClient.vb     (HTTP Client, Token Cache, Member, Voucher, Burn/Earn)
|   \\-- Services\\ReceiptGenerator.vb   (Struk 40-Kolom, Saldo Poin & Survey QR Link)
\\-- TryDreamposXGoapp\\                  <-- [PROYEK 2] Windows Forms (.exe)
    |-- TryDreamposXGoapp.vbproj        (Project Reference ke DreamposXGoapp)
    |-- Program.vb                      (Entry Point STAThread Sub Main)
    |-- FormMain.vb                     (Event Handlers Async/Await)
    \\-- FormMain.Designer.vb            (Desain UI Tabbed Form)"""
    )
    pdf.section_title("2.2 Konfigurasi Project Reference")
    pdf.body_p(
        "Agar proyek UI dapat memanggil fungsi DLL secara langsung tanpa konflik dependensi:\n"
        "1. Di Solution Explorer, klik kanan node References pada proyek TryDreamposXGoapp.\n"
        "2. Pilih Add Reference... -> Projects -> Solution -> centang DreamposXGoapp -> klik OK.\n"
        "3. Dalam file TryDreamposXGoapp.vbproj, referensi terdaftar sebagai <ProjectReference Include=\"..\\DreamposXGoapp.vbproj\" />."
    )
    pdf.callout(
        "Catatan Senior Programmer: Target Framework .NET 4.7.2",
        "Target framework dinaikkan dari .NET 4.0 ke .NET Framework 4.7.2 untuk mendukung:\n"
        "- Native TLS 1.2 / HTTPS modern tanpa perlu hack ServicePointManager tambahan.\n"
        "- Dukungan native Async / Await dan System.Net.Http.HttpClient tanpa risiko DLL hell.",
        bg_r=255, bg_g=251, bg_b=235, border_r=245, border_g=158, border_b=11
    )

    # 3. PEMETAAN KOLEKSI API
    pdf.add_page()
    pdf.chapter_title("3", "Pemetaan Koleksi API Goapp (D:\\Downloads\\APIGoapp)")
    pdf.body_p("Folder koleksi Postman yang Anda sediakan berisi 8 file JSON. Berikut adalah matriks fungsinya:")

    # Table of API Collections
    pdf.set_font("helvetica", "B", 8)
    pdf.set_fill_color(241, 245, 249)
    pdf.set_text_color(30, 41, 59)
    pdf.cell(50, 6, "Nama File Postman", 1, 0, "L", True)
    pdf.cell(40, 6, "Domain Goapp", 1, 0, "L", True)
    pdf.cell(90, 6, "Relevansi & Peruntukan Bagi POS DreamPOS", 1, 1, "L", True)

    pdf.set_font("helvetica", "", 7.5)
    pdf.set_text_color(36, 41, 46)
    rows = [
        ("Channel API.postman_collection.json", "Offline POS & Store", "CORE ENGINE POS: Auth, Member Search, Push Order, Burn Point, Voucher."),
        ("ERP Integration API.postman_collection.json", "Catalog & Inventory", "Sinkronisasi Master SKU dan update stok gudang/toko ke Goapp."),
        ("Commerce Client API.postman_collection.json", "Customer Mobile App", "Front-end pelanggan berbelanja mandiri via app (bukan kasir)."),
        ("CDP API.postman_collection.json", "Data Platform", "Batch update kontak dan rekam riwayat aktivitas customer CRM."),
        ("Core API & Operator API", "Omnichannel Ticketing", "CS Chatbot, Live Agent inbox, dan routing tiket antrian pelanggan."),
        ("WhatsApp API.postman_collection.json", "WhatsApp Business", "Kirim notifikasi nota belanja atau e-receipt otomatis via WA.")
    ]
    for r in rows:
        pdf.cell(50, 5, r[0][:28], 1, 0, "L")
        pdf.cell(40, 5, r[1], 1, 0, "L")
        pdf.cell(90, 5, r[2][:62], 1, 1, "L")
    pdf.ln(4)

    # 4. SPESIFIKASI ENDPOINT & TIPE DATA
    pdf.chapter_title("4", "Spesifikasi Endpoint, Payload & Tipe Data Integrasi")

    pdf.section_title("4.1 Autentikasi & Manajemen Token")
    pdf.body_p(
        "Method: POST | Endpoint: https://account.goapp.co.id/auth/token-auth/\n"
        "Payload Input: username (String: API Key) dan password (String: API Secret).\n"
        "Output Response: token (String Bearer), refresh_token (String), expired_at (Double Epoch)."
    )

    pdf.section_title("4.2 Pencarian & Validasi Member (No HP atau Member ID)")
    pdf.body_p(
        "Method: GET | Endpoint: https://api.goapp.co.id/channel/v1/member/member/{mobile_no_or_member_id}/\n"
        "Path Variable: mobile_no_or_member_id (String: Nomor HP misal '08159136224' atau UID '110270830028872').\n"
        "Output Respon:\n"
        "- uid (Long): ID unik member CRM.\n"
        "- first_name, last_name (String): Nama lengkap konsumen.\n"
        "- account.balance (String/Decimal): Saldo poin aktif member.\n"
        "- account.idr_balance (Decimal): Nilai rupiah dari poin (misal 633 Pts = Rp 633.000).\n"
        "- level (Object): Level/tier member (contoh: Gold, Silver).\n"
        "- direct_deal (Object): Voucher aktif yang otomatis terpasang pada member (Show Available Voucher)."
    )

    pdf.section_title("4.3 Push Transaksi Selesai (Earning Point)")
    pdf.body_p(
        "Method: POST | Endpoint: https://api.goapp.co.id/channel/v1/sales/order/\n"
        "Catatan: Perhitungan poin yang didapat 100% dikendalikan oleh Server Goapp (rule kelipatan, limit harian, dsb), bukan oleh aplikasi POS."
    )
    pdf.code_block(
"""// Payload Request Body:
{
  "order_no": "POS-261001-0001",           // String: No Faktur Kasir POS (Unik)
  "provider_ref": "POS-261001-0001",       // String: Referensi transaksi POS
  "order_date": "2026-10-01 12:30:00+07:00", // String ISO 8601
  "member": { "uid": 110270830028872 },    // Long: Member UID penerima poin
  "store": { "uid": 138350315235400 },     // Long: UID Channel Dream POS Store
  "lines_total": 100000.00,                // Decimal: Subtotal belanja
  "total_paid": 100000.00,                 // Decimal: Total yang dibayar
  "lines": [
    {
      "product": { "sku": "SKU-01", "name": "Baju Kemeja" },
      "quantity": 1,                       // Integer: Qty beli
      "price": 100000.00                   // Decimal: Harga satuan netto
    }
  ],
  "payments": [
    { "payment_method_name": "Cash", "payment_type": "CASH", "amount": 100000.00 }
  ]
}
// Respon Sukses: reward[0].amount (Poin reward baru yang diterima member)."""
    )

    # 4.4 BURN POINT & VOUCHER
    pdf.add_page()
    pdf.section_title("4.4 Burn Point (Pembayaran Transaksi Menggunakan Poin)")
    pdf.body_p(
        "Method: POST | Endpoint: https://api.goapp.co.id/channel/v1/member/payment/\n"
        "Endpoint ini WAJIB menggunakan Physical Store UID (bukan Channel UID) dan store_code.\n"
        "Payload Input:\n"
        "- member.uid (Long): UID member pemilik poin.\n"
        "- amount (Decimal): Jumlah poin yang dipotong (misal: 5000.00).\n"
        "- order_amount (Decimal): Nilai rupiah dari poin (Rasio 1:1, misal: 5000.00).\n"
        "- provider_ref (String): No transaksi POS (mencegah double-deduction).\n"
        "- store (Object): Harus berisi uid fisik dan store_code (contoh: 'DS')."
    )

    pdf.section_title("4.5 Validasi & Penggunaan Voucher (Persen, Nominal, Free Item, Specific SKU)")
    pdf.body_p(
        "Validasi: POST /member/deal/validate/ | Gunakan: POST /member/deal/use/\n"
        "Payload Input: deal_code (String), transaction_ref (String), member.uid (Long).\n"
        "Tipe-tipe Voucher CRM Goapp:\n"
        "1. Diskon Persen (%): discount_type='percentage', discount_amount=0.20 (Diskon 20%).\n"
        "2. Diskon Nominal (Rp): discount_type='amount', discount_amount=25000 (Potongan Rp 25.000).\n"
        "3. Free Item: discount_type='free_item', reward_sku='SKU-PROMO' (Gratis 1 pcs item tersebut).\n"
        "4. Diskon Item Tertentu: products=['SKU-A', 'SKU-B'] (Hanya memotong harga item SKU terkait)."
    )

    pdf.section_title("4.6 Mekanisme Auto-Retry Voucher")
    pdf.body_p(
        "Fungsi UseVoucherWithRetryAsync pada DLL menerapkan algoritma resiliensi:\n"
        "- Error 400 (Bad Request/Expired): Berhenti seketika dan tidak melakukan retry sia-sia.\n"
        "- Error Jaringan/Timeout/5xx Server Busy: Otomatis mencoba ulang hingga 3x dengan jeda waktu "
        "Exponential Backoff (1.5 detik -> 3.0 detik) sehingga kasir tidak panik saat koneksi drop sekejap."
    )

    pdf.section_title("4.7 Saldo Poin & QR Survey pada Struk POS (ReceiptGenerator.vb)")
    pdf.body_p(
        "ReceiptGenerator.vb menghasilkan format teks 40 karakter per baris untuk thermal printer:\n"
        "- Sisa Saldo Poin Akhir = Saldo Awal + Poin Diperoleh - Poin Digunakan.\n"
        "- Nilai Rupiah Poin = Saldo Poin Akhir x Rasio Rupiah (misal Rp 1.000/poin).\n"
        "- Survey QR Link: https://survey.goapp.co.id/?ref={orderNo}&member_uid={memberUid}&mobile={mobileNo}."
    )

    pdf.section_title("4.8 Pendaftaran Member Baru (Save / Register Member CRM)")
    pdf.body_p(
        "Method: POST | Endpoint: https://api.goapp.co.id/channel/v1/member/member/\n"
        "Payload Input:\n"
        "- first_name (String, Wajib): Nama depan calon member.\n"
        "- last_name (String, Opsional): Nama belakang (default '-' jika kosong).\n"
        "- mobile_no (String, Wajib): Nomor HP unik format 08xx/628xx.\n"
        "- email (String, Opsional): Alamat email pelanggan.\n"
        "- scheme.uid (Long) & scheme.name (String): Tier membership (Default Go Member: 138348545946696).\n"
        "Catatan UU PDP / Privasi: Channel API sengaja tidak menyediakan GET /member/member/ (Bulk List) demi "
        "keamanan privasi data konsumen, hanya pencarian per nomor HP dan registrasi member baru."
    )

    # 5. PANDUAN PENGUJIAN FORM VISUAL
    pdf.add_page()
    pdf.chapter_title("5", "Panduan Form Pengujian Visual & Cara Memanggil DLL")
    pdf.body_p(
        "Proyek TryDreamposXGoapp.exe menyediakan antarmuka pengujian komprehensif bagi tim QA & Kasir:\n"
        "- Top Bar: Input API Key, Secret, dan tombol 'Test Koneksi' (Verifikasi channel toko).\n"
        "- Tab 1: Cek & Validasi Member (Input No HP/ID, tampilkan Nama, Sisa Poin, Nilai Rp, tombol '+ Daftar Member Baru').\n"
        "- Tab 2: Validasi & Gunakan Voucher (Uji coba validasi dan eksekusi voucher dengan Auto-Retry).\n"
        "- Tab 3: Transaksi POS (Simulasi Burn Point, Void Poin, dan Push Order Earning Point).\n"
        "- Tab 4: Struk POS (Preview teks struk thermal 40-kolom dan URL Survey QR).\n"
        "- Tab 5: Daftar Member Baru (Form registrasi member CRM langsung dari kasir POS).\n"
        "- Live Logs: Jendela pemantauan respon HTTP dan status payload secara real-time."
    )

    pdf.section_title("5.1 Contoh Kode VB.NET Memanggil DLL untuk Daftar Member Baru")
    pdf.code_block(
"""' Memanggil method RegisterMemberAsync dari Class Library DreamposXGoapp.dll:
Dim resp As ApiResponse(Of MemberResponse) = Await _client.RegisterMemberAsync(
    firstName:="Budi",
    lastName:="Santoso",
    mobileNo:="081298765432",
    email:="budi@example.com",
    schemeUid:=138348545946696,
    schemeName:="Go Member"
)

If resp.IsSuccess AndAlso resp.Data IsNot Nothing Then
    Dim member = resp.Data
    MessageBox.Show("Member Berhasil Didaftarkan! UID: " & member.Uid.ToString())
Else
    MessageBox.Show("Gagal Mendaftar: " & resp.Message)
End If"""
    )

    pdf.section_title("5.2 Panduan Langkah Pengujian Registrasi Member dari Form TryDreamposXGoapp")
    pdf.body_p(
        "1. Buka TryDreamposXGoapp.exe dan pastikan tombol 'Test Koneksi' menunjukkan status hijau (Terkoneksi).\n"
        "2. Masukkan nomor HP baru pada Tab 1 (misal 08999999992), klik 'Cek Member'. Sistem akan info tidak ditemukan.\n"
        "3. Klik tombol '+ Daftar Member Baru' di Tab 1 (atau klik Tab 5: '5. Daftar Member Baru'). "
        "Nomor HP otomatis disalin ke kolom form pendaftaran.\n"
        "4. Masukkan Nama Depan dan pilih Scheme membership (dropdown otomatis memuat list skema CRM Goapp).\n"
        "5. Klik tombol 'Simpan / Daftarkan Member (POST)'. Sistem akan mengirim data ke Goapp CRM.\n"
        "6. Dialog konfirmasi sukses muncul dengan UID resmi. Sistem otomatis kembali ke Tab 1 dan memuat profil serta saldo poin member secara instan."
    )

    pdf.section_title("5.4  Membatalkan Kode Voucher yang Sudah Dipakai (Tab 2 - Cancel Voucher)")
    pdf.body_p(
        "Tombol '3. Batalkan Voucher (Void)' (warna merah muda) di Tab 2 digunakan saat kasir membatalkan\n"
        "struk yang sebelumnya sudah meredeem voucher. Kode voucher dikembalikan ke status available.\n\n"
        "Prasyarat: kode voucher sudah pernah di-redeem, nomor nota sama persis saat penggunaan,\n"
        "UID member sudah tersimpan dari pencarian Tab 1.\n\n"
        "Contoh pemanggilan DLL:\n"
        "    Dim resp = Await _client.CancelVoucherAsync(dealCode, txRef, memberUid)\n"
        "    ' Endpoint : POST /member/deal/cancel_code/\n"
        "    ' Request  : {deal_code, transaction_ref, member:{uid}}\n"
        "    ' Response : {\"detail\": \"Deal code cancelled successfully.\"}"
    )

    pdf.section_title("5.5  Panduan Tab 6 - Master & Siklus Voucher")
    pdf.body_p(
        "Tab 6 menyediakan antarmuka manajemen master promo/voucher Goapp CRM, terbagi dua panel:\n\n"
        "PANEL KIRI - Insert Master Voucher Baru (POST /member/deal/):\n"
        "  - Nama Promo    : Nama deskriptif program promo (contoh: Diskon Hemat 50K)\n"
        "  - Reward SKU    : SKU produk reward di katalog channel - WAJIB valid (contoh: VOUCHER50K)\n"
        "  - Tipe Diskon   : Persentase (%), Nominal (Rp), Item Gratis, Diskon Item Spesifik\n"
        "  - Nilai Diskon  : Besaran diskon sesuai tipe yang dipilih (contoh: 50000)\n"
        "  - Tanggal Mulai : Format yyyy-MM-dd (contoh: 2024-01-01)\n"
        "  - Tanggal Akhir : Format yyyy-MM-dd (contoh: 2024-12-31)\n"
        "  CATATAN: reward_sku WAJIB terdaftar di /catalog/product/. SKU tidak valid = 'Invalid reward_sku'.\n\n"
        "PANEL KANAN - Load Daftar Promo & Void Kode:\n"
        "  - Klik 'Load Daftar Promo Aktif' -> listbox terisi dari GET /member/deal/\n"
        "  - Pilih deal dari list, isi Kode Voucher + Nomor Nota + UID Member\n"
        "  - Klik 'Submit Void Kode Voucher' -> POST /member/deal/cancel_code/ dijalankan\n"
        "  DLL: Await _client.GetAvailableDealsAsync()  -> List(Of DirectDealInfo)\n"
        "  DLL: Await _client.CreateDealAsync(name, rewardSku, startTime, endTime, price, enabled)"
    )

    pdf.section_title("5.6  Referensi Endpoint API Baru (4.9 / 4.10 / 4.11)")
    pdf.body_p(
        "4.9  GET /member/deal/\n"
        "     Deskripsi : Mengambil daftar seluruh program promo/voucher aktif di channel.\n"
        "     Response  : Array DirectDealInfo (uid, name, reward_sku, price, enabled, start_time, end_time)\n"
        "     DLL       : Await _client.GetAvailableDealsAsync()\n\n"
        "4.10 POST /member/deal/cancel_code/\n"
        "     Deskripsi : Membatalkan kode voucher yang sudah digunakan, kembali ke status available.\n"
        "     Request   : WAJIB menyertakan {deal_code, transaction_ref, member:{uid}} (Gagal jika member uid null)\n"
        "     Response  : {\"detail\": \"Deal code cancelled successfully.\"}\n"
        "     DLL       : Await _client.CancelVoucherAsync(dealCode, txRef, memberUid)\n\n"
        "4.11 POST /member/deal/\n"
        "     Deskripsi : Membuat program promo/voucher baru di channel.\n"
        "     Limitasi  : PUT/PATCH tidak didukung (HTTP 405). reward_sku WAJIB ada di /catalog/product/.\n"
        "     Request   : {name, reward_sku, price, enabled, start_time, end_time}\n"
        "     DLL       : Await _client.CreateDealAsync(name, rewardSku, startTime, endTime, price, enabled)"
    )

    # 6. SKENARIO PENGUJIAN (QA CHECKLIST)
    pdf.add_page()
    pdf.chapter_title("6", "Matriks & Skenario Pengujian (QA Checklist)")

    pdf.set_font("helvetica", "B", 8)
    pdf.set_fill_color(241, 245, 249)
    pdf.set_text_color(30, 41, 59)
    pdf.cell(32, 6, "Skenario", 1, 0, "L", True)
    pdf.cell(70, 6, "Langkah Uji Coba Kasir", 1, 0, "L", True)
    pdf.cell(58, 6, "Ekspektasi Hasil Sistem", 1, 0, "L", True)
    pdf.cell(20, 6, "Status", 1, 1, "C", True)

    checklist = [
        ("1. Test Auth", "Klik tombol 'Test Koneksi' pada form utama.", "Label hijau: Terkoneksi: Dream POS (offline).", "PASSED"),
        ("2. Cari No HP", "Tab 1: Input '08159136224', klik 'Cek Member'.", "Nama, Sisa Poin, dan Nilai Rp terisi otomatis.", "READY"),
        ("3. Cari Member ID", "Tab 1: Input UID '110270830028872', klik Cek.", "Hasil sama persis dengan pencarian No HP.", "READY"),
        ("4. Validasi Voucher", "Tab 2: Masukkan kode deal, klik Validasi.", "Tampil status valid dan popup detail potongan voucher.", "READY"),
        ("5. Pakai Voucher", "Tab 2: Masukkan No Struk, klik Gunakan.", "Voucher terkunci, Auto-Retry berjalan jika lag.", "READY"),
        ("6. Burn Point", "Tab 3: Input belanja & 50 poin, klik Burn Point.", "Poin terpotong, muncul PaymentRef transaksi.", "READY"),
        ("7. Void Burn Point", "Tab 3: Klik tombol 'Cancel Burn Point (Void)'.", "Status Payment menjadi CANCEL, poin kembali.", "READY"),
        ("8. Earning Point", "Tab 3: Klik 'Push Transaksi Selesai'.", "Transaksi terkirim, tampil Poin Reward baru.", "READY"),
        ("9. Cetak Struk", "Tab 4: Klik 'Generate Preview Struk'.", "Teks 40 kolom tercetak rapi dengan link survey.", "READY"),
        ("10. Daftar Member", "Tab 5: Input Nama & HP baru, klik Simpan.", "Tersimpan (HTTP 201), redirect Tab 1 & poin tampil.", "PASSED"),
        ("11. Cancel Voucher", "Tab 2: Isi kode & nota yg dipakai, klik 'Batalkan'.", "Voucher kembali available. API: code cancelled.", "READY"),
        ("12. Load Promo List", "Tab 6 Kanan: Klik 'Load Daftar Promo Aktif'.", "Listbox terisi daftar deal dari GET /member/deal/.", "READY"),
        ("13. Void Deal Code", "Tab 6: Pilih deal, isi Kode+Nota+UID, klik Void.", "Kode di-void via POST /member/deal/cancel_code/.", "READY"),
        ("14. Insert Voucher", "Tab 6 Kiri: Isi Nama,SKU,Tipe,Nilai,Tgl, klik Insert.", "Promo baru terbuat (201) atau pesan SKU invalid.", "READY"),
    ]

    pdf.set_font("helvetica", "", 7.5)
    for c in checklist:
        pdf.cell(32, 5.5, c[0], 1, 0, "L")
        pdf.cell(70, 5.5, c[1], 1, 0, "L")
        pdf.cell(58, 5.5, c[2], 1, 0, "L")
        if c[3] == "PASSED":
            pdf.set_text_color(16, 185, 129)
            pdf.set_font("helvetica", "B", 7.5)
            pdf.cell(20, 5.5, c[3], 1, 1, "C")
            pdf.set_font("helvetica", "", 7.5)
            pdf.set_text_color(36, 41, 46)
        else:
            pdf.cell(20, 5.5, c[3], 1, 1, "C")

    pdf.ln(5)
    pdf.chapter_title("7", "Rekomendasi Lanjutan untuk Tim Engineering POS")
    pdf.body_p(
        "1. Database Offline Fallback: Simpan seluruh antrian push order yang gagal ke tabel lokal (MySQL/SQL Server). "
        "Gunakan Background Worker untuk mengirim ulang otomatis saat koneksi internet toko kembali stabil.\n"
        "2. Idempotency Key: Selalu gunakan nomor transaksi kasir yang unik pada field 'provider_ref' agar tidak "
        "terjadi pemotongan poin atau earning ganda.\n"
        "3. Thermal Printer QR Code: Untuk mencetak gambar QR survey pada printer thermal 58mm/80mm, gunakan perintah "
        "ESC/POS (GS ( k) atau generate barcode bitmap menggunakan library ZXing.NET sebelum dikirim ke port printer."
    )

    pdf.output(output_path)
    print("PDF Generated successfully at:", output_path)

if __name__ == "__main__":
    out_file = r"D:\PCL\DreamposXGoapp\Dokumentasi_Integrasi_DreamPOS_Goapp.pdf"
    create_pdf(out_file)

