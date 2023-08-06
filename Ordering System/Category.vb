
Imports MySql.Data.MySqlClient


Public Class Category



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Try
            If MsgBox("Save This Category", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("INSERT INTO `poscategory` (`Category`) VALUES ('" & txtcategory.Text & "')", cn)
                cm.Parameters.AddWithValue("@Category", txtcategory.Text)
                cm.ExecuteNonQuery()

                cn.Close()

                MsgBox("Category Has Been Save")
                txtcategory.Clear()
                txtcategory.Focus()
                With CreateNew_Product

                    .Category1()

                End With
            End If


        Catch ex As Exception

            cn.Close()
            MsgBox(ex.Message, vbCritical)


        End Try



    End Sub
End Class