Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Process.Start(TextBox1.Text, TextBox2.Text)
    End Sub
End Class
