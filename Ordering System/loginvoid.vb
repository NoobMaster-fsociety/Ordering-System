Imports MySql.Data.MySqlClient




Public Class loginvoid

    Dim attempts As Integer
    Dim iDate As String = Now.ToString("yyyy - MM- dd ")
    Dim sdate As String = Date.Now
    Dim Stock As Double

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Timer1.Enabled = False Then


            If tbIDnumber.Text = "1234" = False Then
                attempts += 1
                If attempts = 3 Then
                    Timer1.Start()


                    tbIDnumber.Clear()

                    MessageBox.Show("You Have Reached your attempts, Login form is locked ,Please wait for 5 Seconds ")
                    Timer1.Enabled = True
                End If
                If attempts < 3 Then


                    MessageBox.Show(" Incorrect ID, You have used your " & attempts & " of 3 of your login attempts")
                    tbIDnumber.Clear()
                End If
            End If


            If tbIDnumber.Text = "1234" = True Then

                cn.Open()
                cm = New MySqlCommand("INSERT INTO `tblcancelreport`(`ProductID`, `Description`, `Price`, `Quantity`, `Total`,`tdate`, `Reason`) VALUES (@ProductID, @Description, @Price, @Quantity, @Total,@tdate, @Reason)", cn)
                With cm


                    .Parameters.AddWithValue("@ProductID", cancel.TextBox1.Text)
                    .Parameters.AddWithValue("@Description", cancel.TextBox2.Text)
                    .Parameters.AddWithValue("@Price", cancel.TextBox3.Text)
                    .Parameters.AddWithValue("@Quantity", cancel.TextBox4.Text)
                    .Parameters.AddWithValue("@Total", cancel.TextBox5.Text)
                    .Parameters.AddWithValue("@tdate", sdate)
                    .Parameters.AddWithValue("@Reason", cancel.cmbreason.Text)
                    .ExecuteNonQuery()


                End With
                cn.Close()
                Form1.cancelled()




                Form1.retrievestock()







                cn.Open()
                cm = New MySqlCommand("Delete From tblcart where ProductID = '" & cancel.TextBox1.Text & "'", cn)

                cm.ExecuteNonQuery()
                cn.Close()

                cn.Open()
                cm = New MySqlCommand("Delete From tblreceipt where ProductID = '" & cancel.TextBox1.Text & "'", cn)

                cm.ExecuteNonQuery()
                cn.Close()







                If MsgBox("Item Has been Cancelled", vbOKOnly + vbInformation) = vbOK Then


                    frmQTY.loadcart()

                    void.loadvoid()

                End If
            End If





            cancel.Dispose()
            Me.Dispose()

            attempts = 0

        End If


        If Timer1.Enabled = True Then


            MsgBox("Log attempt is each maximum attempts,Please Wait 5 Seconds")


        End If

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        attempts = 0
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        tbIDnumber.UseSystemPasswordChar = False
        PictureBox2.Hide()
        PictureBox1.Show()

    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        tbIDnumber.UseSystemPasswordChar = True
        PictureBox1.Hide()
        PictureBox2.Show()
    End Sub

    Private Sub loginvoid_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tbIDnumber.UseSystemPasswordChar = True
        PictureBox1.Hide()
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Me.Dispose()
    End Sub





End Class