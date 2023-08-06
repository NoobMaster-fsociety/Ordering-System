Imports MySql.Data.MySqlClient


Public Class login
    Dim attempts As Integer




    Private Sub login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tbIDnumber.UseSystemPasswordChar = True
        tbIDnumber.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Timer1.Enabled = False Then

            If tbIDnumber.Text = String.Empty AndAlso tbuser.Text = String.Empty Then

                MsgBox("ID-Number or Username is Empty")

            End If

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

                CreateNew_Product.loadStatus()
                frmQTY.loadsales()
                Form1.Hide()

                Form1.available()
                Me.Dispose()
                With productlist
                    .loadRecords()
                    .Cancelreport()
                    .totalcancel()
                    .dailysales()
                    .countpaid()
                    Form1.countproduct()

                    .ShowDialog()
                End With





                attempts = 0

            End If
        End If

        If Timer1.Enabled = True Then


            MsgBox("Log attempt is each maximum attempts,Please Wait 5 Seconds")


        End If
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        attempts = 0
    End Sub











End Class