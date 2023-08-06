Imports MySql.Data.MySqlClient

Public Class void
    Dim Stock As Double


    Sub loadvoid()
        DataGridView1.Rows.Clear()
        Dim i As Integer


        cn.Open()

        cm = New MySqlCommand("Select*From tblcart", cn)
        dr = cm.ExecuteReader
        While dr.Read

            i += 1

            DataGridView1.Rows.Add(i, dr.Item("ProductID"), dr.Item("Description").ToString, (dr.Item("Price").ToString), dr.Item("Quantity").ToString, dr.Item("Total").ToString, dr.Item("Stock").ToString)


        End While
        dr.Close()
        cn.Close()

        For i = 0 To DataGridView1.Rows.Count - 1

            Dim r As DataGridViewRow = DataGridView1.Rows(i)
            r.Height = 30

        Next




    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim edit As String = DataGridView1.Columns(e.ColumnIndex).Name

        If edit = "colDel" Then

            Dim row As DataGridViewRow = DataGridView1.CurrentRow



            With cancel

                .TextBox1.Text = row.Cells(1).Value.ToString()
                .TextBox2.Text = row.Cells(2).Value.ToString()
                .TextBox3.Text = row.Cells(3).Value.ToString()
                .TextBox4.Text = row.Cells(4).Value.ToString()
                .TextBox6.Text = row.Cells(4).Value.ToString()
                .TextBox5.Text = row.Cells(5).Value.ToString()
                .TextBox9.Text = row.Cells(6).Value.ToString

                .ShowDialog()





            End With



            Me.Dispose()





                End If



    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs)

        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub
End Class