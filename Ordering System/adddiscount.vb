Imports MySql.Data.MySqlClient

Public Class adddiscount
    Private Sub cmbdiscount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbdiscount.SelectedIndexChanged
        cn.Open()
        cm = New MySqlCommand("select * from tbltax where Discount_name ='" & cmbdiscount.Text & "'", cn)
        dr = cm.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            txtdiscount.Text = dr.Item("Discount").ToString
            Form1.lbldiscount.Text = dr.Item("Discount").ToString
        End If
        dr.Close()
        cn.Close()


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If cmbdiscount.Text = "" Then Return
        Dim discount As String
        Dim netsubtotal As String



        With Form1
            discount = (txtdiscount.Text) / 100
            netsubtotal = .tbSubtotal.Text * discount
            
            .lbldiscount.Text = netsubtotal
            .tbTotalA.Text = .tbSubtotal.Text - .lbldiscount.Text + .tbVAT.Text


        End With



        Me.Dispose()
        txtdiscount.Clear()



    End Sub

    
End Class