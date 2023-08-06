Imports MySql.Data.MySqlClient
Imports System.IO

Public Class productlist

    Dim ProductID As String
    Dim Description As String
    Dim Duplicate As Boolean
    Dim Category As String
    Dim Stock As Double
    Dim price As Double
    Dim User As String
    Dim iDate As String = Now.ToString("yyyy - MM- dd ")
    Dim sdate As DateTime = DateTime.Parse(iDate)




    Private Sub productlist_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lbltime.Text = DateAndTime.Now
        lbltime2.Text = DateAndTime.Now



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        With CreateNew_Product
            .Category1()
            .Clear()
            .ShowDialog()

        End With


    End Sub
   

    Sub loadRecords()
        DataGridView1.Rows.Clear()
        Dim i As Integer


        cn.Open()

        cm = New MySqlCommand("Select*From tblnewproduct", cn)
        dr = cm.ExecuteReader
        While dr.Read

            i += 1

            DataGridView1.Rows.Add(i, dr.Item("ProductID"), dr.Item("Description").ToString, dr.Item("Category"), dr.Item("Price").ToString, dr.Item("Image"), dr.Item("Stock").ToString, dr.Item("Status").ToString)


        End While
        dr.Close()
        cn.Close()

        For i = 0 To DataGridView1.Rows.Count - 1

            Dim r As DataGridViewRow = DataGridView1.Rows(i)
            r.Height = 90

        Next
        Dim Imagecolumn = DirectCast(DataGridView1.Columns("Column5"), DataGridViewImageColumn)
        Imagecolumn.ImageLayout = DataGridViewImageCellLayout.Stretch




    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub




    Private Sub Button5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button5.Click
        TabControl1.SelectTab(2)
        loaddiscount()






    End Sub
    Sub Cancelreport()
        DataCancel.Rows.Clear()
        Dim i As Integer


        cn.Open()

        cm = New MySqlCommand("Select*From tblcancelreport", cn)
        dr = cm.ExecuteReader
        While dr.Read

            i += 1

            DataCancel.Rows.Add(i, dr.Item("ProductID").ToString, dr.Item("Description").ToString, dr.Item("Price").ToString, dr.Item("Quantity").ToString, dr.Item("Total").ToString, dr.Item("tdate"), dr.Item("Reason").ToString, dr.Item("User").ToString, dr.Item("Status").ToString)


        End While
        dr.Close()
        cn.Close()

        For i = 0 To DataCancel.Rows.Count - 1

            Dim r As DataGridViewRow = DataCancel.Rows(i)
            r.Height = 30

        Next







    End Sub

    Sub countpaid()

        Try

            cn.Open()
            cm = New MySqlCommand("Select count(*) from  tblsalepayment", cn)
            tbpaid.Text = CInt(cm.ExecuteScalar)
            cn.Close()
        Catch ex As Exception
            cn.Close()
        End Try



    End Sub

    Sub dailysales()
        Try

            cn.Open()
            cm = New MySqlCommand("Select sum(SubTotal + Tax) from  tblsalepayment where Status = 'Paid'", cn)
            lbltotalsales.Text = Format(CInt(cm.ExecuteScalar), "₱ #,##.00")
     
            cn.Close()
        Catch ex As Exception
            cn.Close()
        End Try
    End Sub



    Sub totalcancel()


        Dim sum As Integer = 0
        For i As Integer = 0 To DataCancel.Rows.Count() - 1 Step +1
            sum = sum + DataCancel.Rows(i).Cells(5).Value
        Next

        lbltotalcance.Text = Format(sum, "₱#,##00")




    End Sub

    Sub loadpayment()
        datapayment.Rows.Clear()
        Dim i As Integer
        Dim subTotal As Double


        cn.Open()

        cm = New MySqlCommand("Select*From tblsalepayment", cn)
        dr = cm.ExecuteReader
        While dr.Read

            i += 1
            subTotal += CDbl(dr.Item("Subtotal").ToString)

            datapayment.Rows.Add(i, dr.Item("tdate"), dr.Item("Subtotal").ToString, dr.Item("Tax").ToString, dr.Item("Discount").ToString, dr.Item("Total").ToString, dr.Item("Status").ToString, dr.Item("User").ToString)


        End While
        dr.Close()
        cn.Close()

        For i = 0 To datapayment.Rows.Count - 1

            Dim r As DataGridViewRow = datapayment.Rows(i)
            r.Height = 30

        Next





    End Sub





    Sub loaddiscount()
        DataGridView2.Rows.Clear()
        Dim i As Integer


        cn.Open()

        cm = New MySqlCommand("Select*From tbltax", cn)
        dr = cm.ExecuteReader
        While dr.Read

            i += 1

            DataGridView2.Rows.Add(i, dr.Item("Discount_name"), dr.Item("Discount").ToString)


        End While
        dr.Close()
        cn.Close()

        For i = 0 To DataGridView2.Rows.Count - 1

            Dim r As DataGridViewRow = DataGridView2.Rows(i)
            r.Height = 30

        Next
    End Sub

    Private Sub DataGridView2_CellContentClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        Dim edit1 As String = DataGridView2.Columns(e.ColumnIndex).Name

        If edit1 = "colDel" Then
            If MsgBox("Delete This Record?", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("Delete From tbltax where Discount_name like '" & DataGridView2.Rows(e.RowIndex).Cells(1).Value.ToString & "'", cn)
                cm.ExecuteNonQuery()
                cn.Close()
                MsgBox("Discount Has been Deleted Succesfully", vbInformation)
                loaddiscount()


            End If


        End If
    End Sub



    Private Sub Button6_Click_1(ByVal sender As Object, ByVal e As EventArgs) Handles Button6.Click

        With Tax

            .ShowDialog()


        End With



    End Sub

    Private Sub DataGridView1_CellContentClick_1(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim edit As String = DataGridView1.Columns(e.ColumnIndex).Name

        If edit = "ColEdit" Then
            With CreateNew_Product
                cn.Open()
                cm = New MySqlCommand("Select image from `tblnewproduct` where `ProductID` like'" & DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString & "'", cn)
                dr = cm.ExecuteReader
                While dr.Read
                    Dim len As Long = dr.GetBytes(0, 0, Nothing, 0, 0)
                    Dim Array(CInt(len)) As Byte
                    dr.GetBytes(0, 0, Array, 0, CInt(len))
                    Dim ms As New MemoryStream(Array)
                    Dim bitmap As New System.Drawing.Bitmap(ms)
                    .browse.Image = bitmap

                    .txtProductID.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString
                    .txtdescription.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString
                    .cmbCategory.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString
                    .txtPrice.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
                    .txtstock.Text = DataGridView1.Rows(e.RowIndex).Cells(6).Value.ToString


                End While
                dr.Close()
                cn.Close()
                .Category1()
                .loadStatus()

                .btnsave.Enabled = False
                .btnupdate.Enabled = True
                .Button5.Enabled = False
                .Button2.Enabled = False
                .ShowDialog()
            End With
        ElseIf edit = "delete" Then
            If MsgBox("Delete This Record?", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("Delete From tblnewproduct where ProductID like '" & DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString & "'", cn)
                cm.ExecuteNonQuery()



                cn.Close()
                MsgBox("Record HAs been Deleted Succesfully", vbInformation)
                loadRecords()


            End If






        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        TabControl1.SelectTab(3)
        frmQTY.loadsales()
        loadpayment()
        DataGridView2.Rows.Clear()


    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TabControl1.SelectTab(1)
    End Sub

   
    Private Sub txtTax_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Select Case Asc(e.KeyChar)
            Case 48 To 57
            Case 8
            Case 13
            Case Else
                e.Handled = True
        End Select
    End Sub


    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TabControl1.SelectTab(5)
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Me.Dispose()
        Form1.LoadMEnu()
        Form1.Show()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        TabControl1.SelectTab(0)

    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        TabControl1.SelectTab(4)
        Form1.countproduct()
    End Sub
    Private bitmap As Bitmap
    
    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.TopMost = True
        PrintPreviewDialog1.StartPosition = FormStartPosition.CenterScreen
        PrintPreviewDialog1.ShowDialog()




    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.GridSales.Width, Me.GridSales.Height)
        GridSales.DrawToBitmap(bm, New Rectangle(100, 100, Me.GridSales.Width, Me.GridSales.Height))
        e.Graphics.DrawImage(bm, 0, 0)

    End Sub

    Private Sub Button9_Click_1(sender As Object, e As EventArgs) Handles Button9.Click

        PrintPreviewDialog1.Document = PrintDocument2
        PrintPreviewDialog1.TopMost = True
        PrintPreviewDialog1.StartPosition = FormStartPosition.CenterScreen
        PrintPreviewDialog1.ShowDialog()


    End Sub

    Private Sub PrintDocument2_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Dim bm As New Bitmap(Me.datapayment.Width, Me.datapayment.Height)
        datapayment.DrawToBitmap(bm, New Rectangle(100, 100, Me.datapayment.Width, Me.datapayment.Height))
        e.Graphics.DrawImage(bm, 0, 0)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        PrintPreviewDialog1.Document = PrintDocument3
        PrintPreviewDialog1.TopMost = True
        PrintPreviewDialog1.StartPosition = FormStartPosition.CenterScreen
        PrintPreviewDialog1.ShowDialog()

    End Sub

    Private Sub PrintDocument3_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument3.PrintPage
        Dim bm As New Bitmap(Me.DataCancel.Width, Me.DataCancel.Height)
        DataCancel.DrawToBitmap(bm, New Rectangle(100, 100, Me.DataCancel.Width, Me.DataCancel.Height))
        e.Graphics.DrawImage(bm, 0, 0)
    End Sub

 
   
End Class