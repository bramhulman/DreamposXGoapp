Imports System
Imports System.Windows.Forms

Public Class Program
    <STAThread()>
    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New FormMain())
    End Sub
End Class
