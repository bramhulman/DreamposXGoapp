import markdown
import codecs

html_template = '''<!DOCTYPE html>
<html lang="id">
<head>
    <meta charset="UTF-8">
    <title>Dokumentasi Integrasi DreamPOS x Goapp CRM</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; max-width: 900px; margin: 0 auto; padding: 20px; color: #333; }
        h1, h2, h3 { color: #0056b3; }
        pre { background: #f4f4f4; padding: 15px; border-radius: 5px; overflow-x: auto; }
        code { font-family: Consolas, monospace; background: #f4f4f4; padding: 2px 5px; border-radius: 3px; }
        table { border-collapse: collapse; width: 100%; margin: 20px 0; }
        th, td { border: 1px solid #ddd; padding: 10px; text-align: left; }
        th { background-color: #f8f9fa; }
    </style>
</head>
<body>
    {{CONTENT}}
</body>
</html>'''

with codecs.open("README.md", "r", encoding="utf-8") as f:
    text = f.read()

html = markdown.markdown(text, extensions=['tables', 'fenced_code'])
final_html = html_template.replace("{{CONTENT}}", html)

with codecs.open("Dokumentasi_Integrasi_DreamPOS_Goapp.html", "w", encoding="utf-8") as f:
    f.write(final_html)

print("HTML Generated successfully.")
