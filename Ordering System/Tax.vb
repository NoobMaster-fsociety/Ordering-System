Imports MySql.Data.MySqlClient

Public Class Tax



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()

    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click

        Try
            If MsgBox("Save This New Discount", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("INSERT INTO `tbltax` (`Discount`,`Discount_name`) VALUES (@Discount,@Discount_name)", cn)
                cm.Parameters.AddWithValue("@Discount", txtDIS.Text)
                cm.Parameters.AddWithValue("@Discount_name", txtDisName.Text)
                cm.ExecuteNonQuery()

                cn.Close()

                MsgBox("Discount Has Been Save")
                txtDisName.Clear()
                txtDIS.Clear()
                txtDisName.Focus()
                productlist.loaddiscount()
            End If


        Catch ex As Exception

            cn.Close()
            MsgBox(ex.Message, vbCritical)


        End Try

    End Sub

End Class