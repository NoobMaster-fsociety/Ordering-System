Imports MySql.Data.MySqlClient


Public Class frmQTY
    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
        Select Case Asc(e.KeyChar)
            Case 48 To 57
            Case 8
            Case 13
            Case Else
                e.Handled = True
        End Select
    End Sub
    Dim ProductID As String
    Dim Description As String
    Dim Duplicate As Boolean
    Dim Category As String
    Dim Stock As Double
    Dim Status As String
    Dim price As Double
    Dim Total As Double
    Dim Qty As Double

    Dim iDate As String = Now.ToString("yyyy - MM- dd ")
    Dim sdate As String = Date.Now


    Private Sub txtQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtQty.KeyDown

        If e.KeyCode = Keys.Escape Then
            Me.Dispose()
        ElseIf e.KeyCode = Keys.Enter Then
            If Stock < CInt(txtQty.Text) Then
                MsgBox("Not Enough Stock", vbExclamation)
                txtQty.Clear()
                Return
            End If
            If txtQty.Text <= 0 Then


                MsgBox("Please Input a Right Amount", vbExclamation)
                txtQty.Clear()
                Return



            ElseIf Stock >= CInt(txtQty.Text) Then

                Stock = Stock - CInt(txtQty.Text)
                Form1.computestock()


                cn.Open()
                cm = New MySqlCommand("select * from tblcart where ProductID ='" & ProductID & "'", cn)
                dr = cm.ExecuteReader
                dr.Read()
                If dr.HasRows Then

                    Duplicate = True

                    dr.Close()
                    cn.Close()

                Else
                    Duplicate = False
                    dr.Close()
                    cn.Close()

                End If
                dr.Close()
                cn.Close()



                If Duplicate = True Then
                    cn.Open()
                    cm = New MySqlCommand("Update tblcart set  Quantity = Quantity + @Quantity where ProductID ='" & ProductID & "'", cn)
                    cm.Parameters.AddWithValue("@Quantity", txtQty.Text)
                    cm.ExecuteNonQuery()
                    cn.Close()

                    cn.Open()
                    cm = New MySqlCommand("Update tblsales set  Quantity = Quantity + @Quantity where ProductID ='" & ProductID & "'", cn)
                    cm.Parameters.AddWithValue("@Quantity", txtQty.Text)
                    cm.ExecuteNonQuery()
                    cn.Close()

                    cn.Open()
                    cm = New MySqlCommand("Update tblreceipt set  Quantity = Quantity + @Quantity where ProductID ='" & ProductID & "'", cn)
                    cm.Parameters.AddWithValue("@Quantity", txtQty.Text)
                    cm.ExecuteNonQuery()
                    cn.Close()

                    cn.Open()
                    cm = New MySqlCommand("Update tblcart set Total = Quantity * Price", cn)
                    cm.ExecuteNonQuery()
                    cn.Close()

                    cn.Open()
                    cm = New MySqlCommand("Update tblsales set Total = Quantity * Price", cn)
                    cm.ExecuteNonQuery()
                    cn.Close()

                    cn.Open()
                    cm = New MySqlCommand("Update tblreceipt set Total = Quantity * Price", cn)
                    cm.ExecuteNonQuery()
                    cn.Close()




                    Me.Dispose()
                    loadsales()
                    loadreceipt()
                    receipt.details()
                    loadcart()
                Else
                    cn.Open()
                    cm = New MySqlCommand("INSERT INTO `tblcart`(`ProductID`,`Description`,`Category`, `Price`, `Quantity`, `Total`, `Stock`, `tdate`) VALUES ( @ProductID,@Description,@Category, @Price, @Quantity,@Total,@Stock,  @tdate)", cn)
                    With cm
                        .Parameters.AddWithValue("@ProductID", ProductID)
                        .Parameters.AddWithValue("@Description", Description)
                        .Parameters.AddWithValue("@Category", Category)
                        .Parameters.AddWithValue("@Quantity", CDbl(txtQty.Text))
                        .Parameters.AddWithValue("@Price", price)
                        .Parameters.AddWithValue("@Total", CDbl(txtQty.Text) * price)
                        .Parameters.AddWithValue("@Stock", Stock)
                        .Parameters.AddWithValue("@tdate", sdate)
                        .ExecuteNonQuery()



                    End With
                    cn.Close()





                    Total = CDbl(txtQty.Text) * price
                    cn.Open()
                    cm = New MySqlCommand("INSERT INTO `tblreceipt`(`ProductID`,`Description`, `Price`, `Quantity`, `Total`) VALUES (@ProductID,@Description, @Price, @Quantity, @Total)", cn)
                    With cm
                        .Parameters.AddWithValue("@ProductID", ProductID)
                        .Parameters.AddWithValue("@Description", Description)
                        .Parameters.AddWithValue("@Price", price)
                        .Parameters.AddWithValue("@Quantity", txtQty.Text)
                        .Parameters.AddWithValue("@Total", Total)
                        .ExecuteNonQuery()


                    End With
                    cn.Close()



                    
                    Total = CDbl(txtQty.Text) * price
                    Qty = txtQty.Text
                    cn.Open()
                    cm = New MySqlCommand("INSERT INTO `tblsales`(`ProductID`,`Description`,`Category`,`Quantity`, `Price`, `Stock`,`Total`, `tdate`) VALUES (@ProductID,@Description,@Category,@Quantity, @Price,@Stock,@Total,  @tdate)", cn)
                    With cm
                        .Parameters.AddWithValue("@ProductID", ProductID)
                        .Parameters.AddWithValue("@Description", Description)
                        .Parameters.AddWithValue("@Category", Category)
                        .Parameters.AddWithValue("@Quantity", txtQty.Text)
                        .Parameters.AddWithValue("@Price", price)
                        .Parameters.AddWithValue("@Stock", Stock)
                        .Parameters.AddWithValue("@Total", Total)
                        .Parameters.AddWithValue("@tdate", sdate)
                        .ExecuteNonQuery()



                    End With
                    cn.Close()
                    Form1.LoadMEnu()








                    loadreceipt()
                    loadsales()
                    receipt.details()

                    Me.Dispose()
                    txtQty.Clear()

                    loadcart()





                End If
            End If


        End If

      


    End Sub
    Sub AddCart(ProductID As String, Description As String, Category As String, price As Double, Stock As Double)
        Me.ProductID = ProductID
        Me.Description = Description
        Me.Category = Category
        Me.price = price
        Me.Stock = Stock
        Me.Status = Status

    End Sub

    Sub loadsales()

        With productlist
            .GridSales.Rows.Clear()
            Dim i As Integer
            Dim subTotal As Double
           


            cn.Open()

            cm = New MySqlCommand("Select*From tblsales", cn)
            dr = cm.ExecuteReader
            While dr.Read

                i += 1
                subTotal += CDbl(dr.Item("Total").ToString)
              

                .GridSales.Rows.Add(i, dr.Item("ProductID"), dr.Item("Description").ToString, dr.Item("Price").ToString, dr.Item("Quantity").ToString, (dr.Item("Total").ToString), (dr.Item("tdate").ToString), dr.Item("Status").ToString, dr.Item("User").ToString)


            End While
            dr.Close()
            cn.Close()


            For i = 0 To .GridSales.Rows.Count - 1

                Dim r As DataGridViewRow = .GridSales.Rows(i)
                r.Height = 20

            Next

        End With









    End Sub

    Sub loadreceipt()

        With receipt
            .DataGridView1.Rows.Clear()
            Dim i As Integer




            cn.Open()

            cm = New MySqlCommand("Select*From tblreceipt", cn)
            dr = cm.ExecuteReader
            While dr.Read

                i += 1



                .DataGridView1.Rows.Add(i, dr.Item("ProductID").ToString, dr.Item("Description").ToString, dr.Item("Price").ToString, dr.Item("Quantity").ToString, dr.Item("Total").ToString)


            End While
            dr.Close()
            cn.Close()


            For i = 0 To .DataGridView1.Rows.Count - 1

                Dim r As DataGridViewRow = .DataGridView1.Rows(i)
                r.Height = 20

            Next


        End With



    End Sub


    Sub loadcart()
        With Form1
            .DataGridView1.Rows.Clear()
            Dim i As Integer
            Dim subTotal As Double
            Dim Tax As Double
            Dim Total As Double


            cn.Open()

            cm = New MySqlCommand("Select*From tblcart", cn)
            dr = cm.ExecuteReader
            While dr.Read

                i += 1
                subTotal += CDbl(dr.Item("Total").ToString)
                Tax = subTotal * 0.12
                Total = subTotal + Tax

                .DataGridView1.Rows.Add(i, dr.Item("ProductID"), dr.Item("Description").ToString, (dr.Item("Price").ToString), dr.Item("Quantity").ToString, (dr.Item("Total").ToString))


            End While
            dr.Close()
            cn.Close()


            With Form1
                .tbSubtotal.Text =subTotal
                .lbltotal.Text = Format(subTotal, "₱#,##0.00")
                .tbVAT.Text = CDbl(Tax)
                .tbTotalA.Text = CDbl(Total)


            End With
            For i = 0 To .DataGridView1.Rows.Count - 1

                Dim r As DataGridViewRow = .DataGridView1.Rows(i)
                r.Height = 20

            Next

        End With


    End Sub




    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

  
    Private Sub frmQTY_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtQty.Focus()
    End Sub
End Class