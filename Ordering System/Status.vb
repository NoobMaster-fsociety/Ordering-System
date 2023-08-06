Imports MySql.Data.MySqlClient

Public Class Status

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click


        Try
            If MsgBox("Save This Status", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("INSERT INTO `tblstatus` (`Status`) VALUES ('" & txtstatus.Text & "')", cn)
                cm.Parameters.AddWithValue("@Status", txtstatus.Text)
                cm.ExecuteNonQuery()

                cn.Close()

                MsgBox("Status Has Been Save")
                txtstatus.Clear()
                txtstatus.Focus()
                With CreateNew_Product

                    .loadStatus()

                End With
             
            End If


        Catch ex As Exception

            cn.Close()
            MsgBox(ex.Message, vbCritical)


        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub
End Class