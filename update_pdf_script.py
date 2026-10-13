import re

with open("generate_pdf.py", "r", encoding="utf-8") as f:
    code = f.read()

# 1. Update Section 4.2 output respon: replace direct_deal with deal_codes API
old_4_2 = '''        "- level (Object): Level/tier member (contoh: Gold, Silver).\\n"
        "- direct_deal (Object): Voucher aktif yang otomatis terpasang pada member (Show Available Voucher)."'''

new_4_2 = '''        "- level (Object): Level/tier member (contoh: Gold, Silver).\\n"
        "- GET deal_codes: CRM memisahkan voucher member via /member/member/{id}/deal_codes/?status=not_used."'''

code = code.replace(old_4_2, new_4_2)

# 2. Update Section 4.5: detail reward_data
old_4_5 = '''    pdf.section_title("4.5 Validasi & Penggunaan Voucher (Persen, Nominal, Free Item, Specific SKU)")
    pdf.body_p(
        "Validasi: POST /member/deal/validate/ | Gunakan: POST /member/deal/use/\\n"
        "Payload Input: deal_code (String), transaction_ref (String), member.uid (Long).\\n"
        "Tipe-tipe Voucher CRM Goapp:\\n"
        "1. Diskon Persen (%): discount_type='percentage', discount_amount=0.20 (Diskon 20%).\\n"
        "2. Diskon Nominal (Rp): discount_type='amount', discount_amount=25000 (Potongan Rp 25.000).\\n"
        "3. Free Item: discount_type='free_item', reward_sku='SKU-PROMO' (Gratis 1 pcs item tersebut).\\n"
        "4. Diskon Item Tertentu: products=['SKU-A', 'SKU-B'] (Hanya memotong harga item SKU terkait)."
    )'''

new_4_5 = '''    pdf.section_title("4.5 Validasi & Penggunaan Voucher serta Ekstraksi 'reward_data'")
    pdf.body_p(
        "Validasi: POST /member/deal/validate/ | Gunakan: POST /member/deal/use/\\n"
        "Payload Input: deal_code (String), transaction_ref (String), member.uid (Long).\\n"
        "Objek reward_data dipetakan otomatis ke properti RewardDataInfo pada DLL:\\n"
        "- discount_type: percentage, amount, free_items\\n"
        "- EffectiveDiscountAmount: Nilai diskon (persen atau rupiah)\\n"
        "- EffectiveMinPurchase: Batas minimal belanja dari sub-objek rule.min_purchase\\n"
        "- EffectiveDiscountMaxAmount: Batas maksimal potongan rupiah dari sub-objek reward.discount_max_amount\\n"
        "- GetDetailLines(): Method DLL untuk menghasilkan list string rincian setiap variabel reward_data."
    )'''

code = code.replace(old_4_5, new_4_5)

# 3. Add Section 5.0 on DLL Initialization & Single-source config in generate_pdf.py
old_sec5_header = '''    # 5. PANDUAN PENGUJIAN FORM VISUAL
    pdf.add_page()
    pdf.chapter_title("5", "Panduan Form Pengujian Visual & Cara Memanggil DLL")
    pdf.body_p(
        "Proyek TryDreamposXGoapp.exe menyediakan antarmuka pengujian komprehensif bagi tim QA & Kasir:\\n"'''

new_sec5_header = '''    # 5. PANDUAN PENGUJIAN FORM VISUAL
    pdf.add_page()
    pdf.chapter_title("5", "Panduan Integrasi & Cara Memanggil DLL (1 Jalur Config)")
    pdf.section_title("5.0 Inisialisasi GoappApiClient (Single-Source Config)")
    pdf.body_p(
        "Seluruh parameter dibaca oleh aplikasi pemanggil dari App.config (1 pintu) lalu dioper ke DLL:\\n"
        "- DLL tidak membaca ConfigurationManager sendiri untuk menjaga fleksibilitas dan portabilitas.\\n"
        "- Connection string SQL Server diteruskan via GoappConfig.DbConnectionString untuk audit log API."
    )
    pdf.code_block(
"""' Membaca App.config pemanggil dan inisialisasi DLL:
Dim connSetting = ConfigurationManager.ConnectionStrings("POSDatabase")
Dim config As New GoappConfig With {
    .ApiKey = ConfigurationManager.AppSettings("Goapp.ApiKey"),
    .ApiSecret = ConfigurationManager.AppSettings("Goapp.ApiSecret"),
    .ChannelUid = Long.Parse(ConfigurationManager.AppSettings("Goapp.ChannelUid")),
    .AuthBaseUrl = ConfigurationManager.AppSettings("Goapp.AuthBaseUrl"),
    .ChannelBaseUrl = ConfigurationManager.AppSettings("Goapp.ChannelBaseUrl"),
    .StoreUid = Long.Parse(ConfigurationManager.AppSettings("Goapp.StoreUid")),
    .StoreName = ConfigurationManager.AppSettings("Goapp.StoreName"),
    .StoreCode = ConfigurationManager.AppSettings("Goapp.StoreCode"),
    .DbConnectionString = If(connSetting IsNot Nothing, connSetting.ConnectionString, ""),
    .LogTableName = "[dbo].[GoappApiLog]",
    .TimeoutSeconds = 30
}
Dim _client As New GoappApiClient(config)"""
    )
    pdf.body_p(
        "Proyek TryDreamposXGoapp.exe menyediakan antarmuka pengujian komprehensif bagi tim QA & Kasir:\\n"'''

code = code.replace(old_sec5_header, new_sec5_header)

with open("generate_pdf.py", "w", encoding="utf-8") as f:
    f.write(code)

print("generate_pdf.py updated successfully")
