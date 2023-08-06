Public Class receipt

   
    Private Sub receipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        Label5.Text = Date.Now

    End Sub

    Sub details()

        lblcash.Text = Form1.tbAmountT.Text
        lbldiscount.Text = Form1.lbldiscount.Text
        lbltotal.Text = Form1.tbTotalA.Text
        lblvat.Text = Form1.tbVAT.Text
        lblchange.Text = Form1.tbAMountC.Text





    End Sub







    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.TopMost = True
        PrintPreviewDialog1.StartPosition = FormStartPosition.CenterScreen
        PrintPreviewDialog1.ShowDialog()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        With Form1
            .tbAMountC.Clear()
            .tbAmountT.Clear()
            .tbSubtotal.Clear()
            .tbTotalA.Clear()
            .tbVAT.Clear()
            .lbldiscount.Clear()
            .lbltotal.Text = "0.00"

        End With
        Me.Dispose()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim bm As New Bitmap(Me.Panel2.Width, Me.Panel2.Height)
        Panel2.DrawToBitmap(bm, New Rectangle(100, 100, Me.Panel2.Width, Me.Panel2.Height))
        e.Graphics.DrawImage(bm, 0, 0)
    End Sub
End Class