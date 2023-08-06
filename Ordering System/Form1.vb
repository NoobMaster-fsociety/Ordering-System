Imports MySql.Data.MySqlClient
Imports System.IO



Public Class Form1
    Dim lblstatus As New Label
    Dim pic As New PictureBox
    Dim lblname As New Label
    Dim lblPrice As New Label
    Dim btnCategory As New Button
    Dim total As Integer
    Dim _filter As String = ""
    Dim Choose As String = ""
    Dim description As String = ""
    Dim ProductID As String = ""
    Dim Category As String = ""
    Dim price As Double
    Dim Stock As Double
    Dim Status As String = ""
    Dim User As String = " Cashier"

    Dim sdate As String = Date.Now


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Connection()
        FlowLayoutPanel2.Hide()
        LoadCategory()
        LoadMEnu()
        frmQTY.loadcart()
        FlowLayoutPanel1.Hide()
        TabControl1.SelectTab(0)
        Label1.Text = Date.Now
        discount()
        countproduct()
        productlist.countpaid()
        btn100.Enabled = False
        btn200.Enabled = False
        btn500.Enabled = False
        btn1000.Enabled = False
        btn1500.Enabled = False
        btn2000.Enabled = False
        btnreceipt.Enabled = False
        Button1.Enabled = False



    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        login.ShowDialog()

    End Sub






    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Me.Close()
    End Sub

    Sub LoadMEnu()
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.Controls.Clear()

        cn.Open()
        cm = New MySqlCommand("Select Image,ProductID,Description,Category,Price,Stock,Status From tblnewproduct Where Category like'" & _filter & "%' Order by Description", cn)
        dr = cm.ExecuteReader
        While dr.Read
            Dim len As Long = dr.GetBytes(0, 0, Nothing, 0, 0)
            Dim Array(CInt(len)) As Byte

            dr.GetBytes(0, 0, Array, 0, CInt(len))
            pic = New PictureBox
            pic.Width = 120
            pic.Height = 120
            pic.BackgroundImageLayout = ImageLayout.Stretch
            pic.Cursor = Cursors.Hand


            Dim ms As New MemoryStream(Array)
            Dim Bitmap As New System.Drawing.Bitmap(ms)
            pic.BackgroundImage = Bitmap
            pic.Tag = dr.Item("ProductID").ToString

            lblname = New Label
            lblname.BackColor = Color.Orange
            lblname.ForeColor = Color.White
            lblname.AutoSize = False
            lblname.Text = dr.Item("Description").ToString
            lblname.Dock = DockStyle.Bottom
            lblname.Cursor = Cursors.Hand


            lblPrice = New Label
            lblPrice.BackColor = Color.Orange
            lblPrice.ForeColor = Color.Black

            lblPrice.Text = dr.Item("Price").ToString
            lblPrice.Dock = DockStyle.Bottom
            lblPrice.Cursor = Cursors.Hand
            lblPrice.AutoSize = True
            lblPrice.TextAlign = ContentAlignment.MiddleCenter
            lblPrice.Dock = DockStyle.Right


            lblstatus = New Label
            lblstatus.ForeColor = Color.Black
            lblstatus.Text = dr.Item("Status").ToString
            lblstatus.Dock = DockStyle.Top
            lblstatus.Cursor = Cursors.Hand
            lblstatus.AutoSize = True
            lblstatus.BackColor = Color.Transparent
            lblstatus.TextAlign = ContentAlignment.MiddleLeft
            If lblstatus.Text = "Out Of Stock" Then

                lblstatus.ForeColor = Color.Red
            ElseIf lblstatus.Text = "Unavailable" Then

                frmQTY.txtQty.Enabled = False
                lblstatus.ForeColor = Color.Black

            End If


            TabControl1.SelectTab(1)
                FlowLayoutPanel1.Show()
            pic.Controls.Add(lblname)
            pic.Controls.Add(lblstatus)
            lblname.Controls.Add(lblPrice)
                FlowLayoutPanel1.Controls.Add(pic)

                AddHandler pic.Click, AddressOf Choose_Click

        End While
        dr.Close()
        cn.Close()

    End Sub

    Sub LoadCategory()

        cn.Open()
        cm = New MySqlCommand("Select * From poscategory", cn)
        dr = cm.ExecuteReader
        While dr.Read
            btnCategory = New Button
            btnCategory.Width = 150
            btnCategory.Height = 35
            btnCategory.Text = dr.Item("Category").ToString
            btnCategory.FlatAppearance.BorderSize = 0
            btnCategory.FlatStyle = FlatStyle.Flat
            btnCategory.BackColor = Color.Transparent
            btnCategory.ForeColor = Color.Black
            btnCategory.Cursor = Cursors.Hand
            FlowLayoutPanel2.Controls.Add(btnCategory)


            AddHandler btnCategory.Click, AddressOf filter_Click
        End While
        dr.Close()
        cn.Close()
    End Sub

    Sub computestock()


        cn.Open()
        cm = New MySqlCommand("UPDATE `tblnewproduct` SET Price=@Price,Stock=@Stock where ProductID=@ProductID", cn)
        With cm


            .Parameters.AddWithValue("@Price", price)
            .Parameters.AddWithValue("@Stock", Stock - Val(frmQTY.txtQty.Text))
            .Parameters.AddWithValue("@ProductID", ProductID)



            .ExecuteNonQuery()
        End With
        cn.Close()


    End Sub




    Public Sub Choose_Click(ByVal sender As Object, ByVal e As EventArgs)




        cn.Open()
        cm = New MySqlCommand("select * From `tblnewproduct` where ProductID like'" & sender.tag & "'", cn)
        dr = cm.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            ProductID = dr.Item("ProductID").ToString
            description = dr.Item("Description").ToString
            Category = dr.Item("Category").ToString
            price = dr.Item("Price").ToString
            Stock = dr.Item("Stock").ToString
            Status = dr.Item("Status").ToString
            frmQTY.lblstock.Text = Stock


        End If
        dr.Close()
        cn.Close()








        With frmQTY

            If Stock <= 15 AndAlso Stock > 5 Then

                .lblstock.ForeColor = Color.Orange
                MsgBox("Your Stock Has Reach the Warning Limit, Pls. Add Some Stocks or it may Cause a Delay to your transactions", vbExclamation)

            End If
            If Stock <= 5 AndAlso Stock > 1 Then

                .lblstock.ForeColor = Color.Red
                MsgBox("Your Stock Has  Reach  the Critical Limit, Pls. Add Some Stocks or it may Cause a Delay to your transactions", vbExclamation)

            End If
            If Stock = 0 Then


                MsgBox("Out Of Stock", vbExclamation)

                .txtQty.Enabled = False
                outofstock()

            End If
            If Stock > 0 Then

                available()

            End If





            .AddCart(ProductID, description, Category, price, Stock)
            .ShowDialog()


        End With

    End Sub
    Sub discount()


        adddiscount.cmbdiscount.Items.Clear()

        cn.Open()
        cm = New MySqlCommand("Select * from tbltax", cn)
        dr = cm.ExecuteReader
        While dr.Read

            adddiscount.cmbdiscount.Items.Add(dr.Item("Discount_name").ToString)


        End While



        dr.Close()
        cn.Close()


    End Sub

    Public Sub filter_Click(ByVal sender As Object, ByVal e As EventArgs)

        _filter = sender.text.ToString

        LoadMEnu()


    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FlowLayoutPanel2.Show()





    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        FlowLayoutPanel2.Hide()
        TabControl1.SelectTab(0)
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVoid.Click
        If Me.DataGridView1.Rows.Count <= 0 Then

            MsgBox("There Are No Order in the Cart", vbInformation)

            Return

        ElseIf Me.DataGridView1.Rows.Count >= 0 Then
            void.loadvoid()
            btnVoid.Enabled = True


            void.ShowDialog()

        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button9.Click
        adddiscount.ShowDialog()
        discount()
    End Sub

   


    Private Sub btnPay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPay.Click



        If Me.DataGridView1.Rows.Count <= 0 Then

            MsgBox("There Are No Order in the Cart", vbInformation)
            Return
        End If
        If Val(tbAmountT.Text) < Val(tbTotalA.Text) Then


            MsgBox("Please Input Right Amount", vbCritical)
            tbAmountT.Clear()
            Return

        End If

        If Val(tbAmountT.Text) >= Val(tbTotalA.Text) Then


            Try



                tbTotalA.Text = CDbl(tbSubtotal.Text) - CDbl(lbldiscount.Text) + CDbl(tbVAT.Text)


                cn.Open()
                cm = New MySqlCommand("INSERT INTO `tblsalepayment`(`tdate`, `Subtotal`, `Tax`, `Discount`, `Total`) VALUES (@tdate, @Subtotal, @Tax, @Discount, @Total)", cn)
                With cm
                    .Parameters.AddWithValue("@tdate", sdate)
                    .Parameters.AddWithValue("@Subtotal", tbSubtotal.Text)
                    .Parameters.AddWithValue("@Tax", tbVAT.Text)
                    .Parameters.AddWithValue("@Discount", lbldiscount.Text)
                    .Parameters.AddWithValue("@Total", tbTotalA.Text)
                    .ExecuteNonQuery()
                End With
                cn.Close()
                MsgBox("Payment Has Been Made", vbInformation)
                tbAMountC.Text = Val(tbAmountT.Text) - Val(tbTotalA.Text)
                PAid()
                PAidsales()

            Catch ex As Exception


                MsgBox(ex.Message)

            End Try
            btnreceipt.Enabled = True

          
            frmQTY.loadreceipt()
            receipt.details()

            receipt.ShowDialog()

            If MsgBox("Do you Want to Create a New Order?", vbInformation + vbQuestion) = vbOK Then

                Try

                    DataGridView1.SelectAll()
                    cn.Open()
                    cm = New MySqlCommand("Delete From tblcart where 1", cn)

                    cm.ExecuteNonQuery()
                    cn.Close()
                    MsgBox("Cashier is now Open", MsgBoxStyle.Information)
                    DataGridView1.Rows.Clear()
                    TabControl1.SelectTab(0)
                    Button1.Enabled = False
                    FlowLayoutPanel2.Hide()
                Catch ex As Exception
                    MsgBox(ex.Message)
                    cn.Close()
                End Try


            End If
        End If

    End Sub

    Sub Payment()






    End Sub








    Sub countproduct()
        Try
            cn.Open()
            cm = New MySqlCommand("Select count(*) from  tblnewproduct", cn)
            productlist.tbproduct.Text = CInt(cm.ExecuteScalar)
            cn.Close()

        Catch ex As Exception
            cn.Close()
            MsgBox(ex.Message, vbCritical)


        End Try





    End Sub



    Sub retrievestock()



        cn.Open()
        cm = New MySqlCommand("UPDATE `tblnewproduct` SET Stock=@Stock where ProductID='" & cancel.TextBox1.Text & "'", cn)
        With cm



            .Parameters.AddWithValue("@Stock", Val(cancel.TextBox9.Text) + Val(cancel.TextBox4.Text))



            .ExecuteNonQuery()
        End With
        cn.Close()


    End Sub
    Sub cancelled()



        cn.Open()
        cm = New MySqlCommand("Update tblsales set Status = 'Cancelled' Where ProductID ='" & ProductID & "'", cn)
        cm.ExecuteNonQuery()
        cn.Close()



    End Sub


    Sub PAid()
        cn.Open()
        cm = New MySqlCommand("Update tblcart set Status = 'Paid' Where ProductID ='" & ProductID & "'", cn)
        cm.ExecuteNonQuery()
        cn.Close()
    End Sub
    Sub PAidsales()
        cn.Open()
        cm = New MySqlCommand("Update tblsales set Status = 'Paid' Where ProductID ='" & ProductID & "'", cn)
        cm.ExecuteNonQuery()
        cn.Close()
    End Sub


    Sub outofstock()

        cn.Open()
        cm = New MySqlCommand("Update tblnewproduct set Status = 'Out of Stock' Where ProductID ='" & ProductID & "'", cn)
        cm.ExecuteNonQuery()
        cn.Close()

    End Sub
    Sub available()

        cn.Open()
        cm = New MySqlCommand("Update tblnewproduct set Status = 'Available' Where ProductID ='" & ProductID & "'", cn)
        cm.ExecuteNonQuery()
        cn.Close()

    End Sub


    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbAmountT.KeyPress
        Dim DecimalSeparator As String = Application.CurrentCulture.NumberFormat.NumberDecimalSeparator
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or
                        Asc(e.KeyChar) = 8 Or
                        (e.KeyChar = DecimalSeparator And sender.Text.IndexOf(DecimalSeparator) = -1))



    End Sub





    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TabControl1.SelectTab(2)
        tbAmountT.Focus()

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox1.Checked = True Then

            btn100.Enabled = True
            btn200.Enabled = True
            btn500.Enabled = True
            btn1000.Enabled = True
            btn1500.Enabled = True
            btn2000.Enabled = True

            For Each control As Control In GroupBox1.Controls
                If TypeOf control Is Button Then

                    CType(control, Button).Enabled = False
                End If
            Next



        Else
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False


            For Each control As Control In GroupBox1.Controls
                If TypeOf control Is Button Then

                    CType(control, Button).Enabled = True
                End If
            Next








        End If






    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "0"
        Else
            tbAmountT.Text = "0"

        End If




    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If tbAmountT.Text <> "00" Then
            tbAmountT.Text += "00"

            If tbAmountT.Text >= 100 Then

                tbAmountT.Text = tbAmountT.Text + 0

            End If


        Else
            tbAmountT.Text = "00"

        End If

    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        tbAmountT.Clear()
        tbAmountT.Focus()
        If CheckBox1.Checked = True Then
            btn100.Enabled = True
            btn200.Enabled = True
            btn500.Enabled = True
            btn1000.Enabled = True
            btn1500.Enabled = True
            btn2000.Enabled = True
        End If
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If tbAmountT.Text <> "000" Then
            tbAmountT.Text += "000"
        Else
            tbAmountT.Text = "000"

        End If
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "1"
        Else
            tbAmountT.Text = "1"

        End If
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "2"
        Else
            tbAmountT.Text = "2"

        End If
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "3"
        Else
            tbAmountT.Text = "3"

        End If
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "4"
        Else
            tbAmountT.Text = "4"

        End If
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "5"
        Else
            tbAmountT.Text = "5"

        End If
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "6"
        Else
            tbAmountT.Text = "6"

        End If
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "9"
        Else
            tbAmountT.Text = "9"

        End If
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "7"
        Else
            tbAmountT.Text = "7"

        End If
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "8"
        Else
            tbAmountT.Text = "8"

        End If
    End Sub

    Private Sub btn100_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn100.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "100"
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False

        Else
            tbAmountT.Text = "100"

        End If
    End Sub

    Private Sub btn200_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn200.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "200"
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False
        Else
            tbAmountT.Text = "200"

        End If
    End Sub

    Private Sub btn500_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn500.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "500"
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False
        Else
            tbAmountT.Text = "500"

        End If
    End Sub

    Private Sub btn1000_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1000.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "1000"
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False
        Else
            tbAmountT.Text = "1000"

        End If
    End Sub

    Private Sub btn1500_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1500.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "1500"
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False
        Else
            tbAmountT.Text = "1500"

        End If
    End Sub

    Private Sub btn2000_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn2000.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "2000"
            btn100.Enabled = False
            btn200.Enabled = False
            btn500.Enabled = False
            btn1000.Enabled = False
            btn1500.Enabled = False
            btn2000.Enabled = False
        Else
            tbAmountT.Text = "2000"

        End If
    End Sub

    Private Sub tbAmountT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbAmountT.TextChanged
        If tbAmountT.Text >= tbTotalA.Text Then

            btnPay.Enabled = True



        End If
    End Sub

    Private Sub Button7_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If tbAmountT.Text <> "0" Then
            tbAmountT.Text += "."
        Else
            tbAmountT.Text = "."

        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnreceipt.Click
        frmQTY.loadcart()

        With receipt
            .ShowDialog()
            frmQTY.loadreceipt()
            .details()
        End With

    End Sub

    Private Sub Button19_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click

        Button1.Enabled = True
        btnreceipt.Enabled = False
        If Status = "Out Of Stock" Then
            lblstatus.ForeColor = Color.Red

        End If


        DataGridView1.SelectAll()
        receipt.DataGridView1.SelectAll()
        Try
            cn.Open()
            cm = New MySqlCommand("DELETE from `tblcart` WHERE 1", cn)

            cm.ExecuteNonQuery()
            cn.Close()

            cn.Open()
            cm = New MySqlCommand("DELETE from `tblreceipt` WHERE 1", cn)

            cm.ExecuteNonQuery()
            cn.Close()


            MsgBox("Cashier is now Open", MsgBoxStyle.Information)
            DataGridView1.Rows.Clear()
            lbltotal.Text = "₱ 0.00"
            cn.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
            cn.Close()
        End Try

    End Sub

    
End Class
