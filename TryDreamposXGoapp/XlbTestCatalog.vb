Imports System.Collections.Generic
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports DreamposXGoapp.Models
Imports Newtonsoft.Json

' Snapshot daftar item Excel disertakan pada aplikasi Try, tanpa ketergantungan Excel saat runtime.
Friend NotInheritable Class XlbTestCatalog
    Private Class CatalogData
        Public Property Products As List(Of OrderProductInfo)
    End Class

    Private Shared ReadOnly Products As List(Of OrderProductInfo) = LoadProducts()
    Private Shared ReadOnly SkuSet As New HashSet(Of String)(Products.Select(Function(p) p.Sku.Trim()), StringComparer.OrdinalIgnoreCase)

    Private Sub New()
    End Sub

    Private Shared Function LoadProducts() As List(Of OrderProductInfo)
        Using stream = GetType(XlbTestCatalog).Assembly.GetManifestResourceStream("TryDreamposXGoapp.TestData.XlbProducts.json")
            If stream Is Nothing Then Throw New InvalidOperationException("Daftar SKU XLB tidak ditemukan pada aplikasi Try.")
            Using reader As New StreamReader(stream, Encoding.UTF8)
                Dim catalog = JsonConvert.DeserializeObject(Of CatalogData)(reader.ReadToEnd())
                If catalog Is Nothing OrElse catalog.Products Is Nothing OrElse catalog.Products.Count = 0 Then
                    Throw New InvalidOperationException("Daftar SKU XLB kosong.")
                End If
                Return catalog.Products
            End Using
        End Using
    End Function

    Friend Shared Function Contains(sku As String) As Boolean
        Return SkuSet.Contains(If(sku, String.Empty).Trim())
    End Function

    Friend Shared Function SelectProducts(transactionRef As String, lineCount As Integer) As List(Of OrderProductInfo)
        Dim hash As Byte()
        Using sha = SHA256.Create()
            hash = sha.ComputeHash(Encoding.UTF8.GetBytes(transactionRef & "|XLB"))
        End Using
        Dim count = Math.Min(lineCount, 1 + CInt(hash(0)) Mod 2)
        Dim start = CInt(hash(1)) Mod Products.Count
        Dim selected As New List(Of OrderProductInfo)()
        For index = 0 To count - 1
            Dim item = Products((start + index) Mod Products.Count)
            selected.Add(New OrderProductInfo With {.Sku = item.Sku, .Name = item.Name})
        Next
        Return selected
    End Function
End Class
