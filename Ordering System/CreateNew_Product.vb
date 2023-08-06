Imports MySql.Data.MySqlClient
Imports System.IO

Public Class CreateNew_Product

    Private Sub CreateNew_Product_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        With Category
            .ShowDialog()
        End With
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim opf As New OpenFileDialog

        opf.Filter = "Choose Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif"

        If opf.ShowDialog = DialogResult.OK Then

            browse.Image = Image.FromFile(opf.FileName)
            OpenFileDialog1.FileName = opf.FileName



        End If

    End Sub

    
    Sub Category1()


        cmbCategory.Items.Clear()

        cn.Open()
        cm = New MySqlCommand("Select * from poscategory", cn)
        dr = cm.ExecuteReader
        While dr.Read

            cmbCategory.Items.Add(dr.Item("Category").ToString)

        End While



        dr.Close()
        cn.Close()


    End Sub

    Sub loadStatus()


        cmbstatus.Items.Clear()

        cn.Open()
        cm = New MySqlCommand("Select * from tblstatus", cn)
        dr = cm.ExecuteReader
        While dr.Read

            cmbstatus.Items.Add(dr.Item("Status").ToString)

        End While



        dr.Close()
        cn.Close()


    End Sub






    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave.Click

        Try

            If OpenFileDialog1.FileName = "OpenFileDialog1" Then
                MsgBox("Please Select A Image", vbCritical)
                Return


                If txtdescription.Text = String.Empty Or txtPrice.Text = String.Empty Then

                    MsgBox("[Description][Price] One of this is Empty!!", vbCritical)


                End If
                If txtstock.Text = String.Empty Or cmbCategory.Text = String.Empty Then

                    MsgBox("[Category][Stock] One of this is Empty!", vbCritical)

                End If
                If cmbstatus.Text = String.Empty Then

                    MsgBox("Please Input Data!", vbCritical)
                End If



            End If




            If MsgBox("Save This Item?", vbYesNo + vbQuestion) = vbYes Then
                Dim ms As New MemoryStream
                browse.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                Dim arrImage() As Byte = ms.GetBuffer
                If txtstock.Text <= 0 Then


                    MsgBox("Please Input Right Number Of Stocks it must Greater than 1 ", vbCritical)
                    Return


                ElseIf txtstock.Text > 1 Then


                    cn.Open()
                    cm = New MySqlCommand("INSERT INTO `tblnewproduct` (`Description`, `Category`, `Price`, `Image` , `Stock`) VALUES (@Description,@Category,@Price,@Image,@Stock)", cn)
                    With cm

                        .Parameters.AddWithValue("@Description", txtdescription.Text)
                        .Parameters.AddWithValue("@Category", cmbCategory.Text)
                        .Parameters.AddWithValue("@Price", txtPrice.Text)
                        .Parameters.AddWithValue("@Image", arrImage)
                        .Parameters.AddWithValue("@Stock", txtstock.Text)
                        .ExecuteNonQuery()

                    End With
                    cn.Close()

                    MsgBox("Item Has Been Save", vbInformation)
                    Clear()


                    With productlist
                        .loadRecords()


                    End With
                    loadStatus()
                End If
            End If

        Catch ex As Exception

            cn.Close()
            MsgBox(ex.Message, vbCritical)



        End Try




    End Sub

    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Dispose()
    End Sub

    Private Sub txtstock_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtstock.KeyDown


        If Val(txtstock.Text) > 25 Then
            If e.KeyCode = Keys.Enter Then


                MsgBox("Please input Valid Number", vbCritical)

            End If


        End If




    End Sub


    Sub Clear()

        txtdescription.Clear()
        txtProductID.Text = "(Auto)"
        If txtProductID.Enabled = False Then
            txtProductID.Text = "(Auto)"
        End If
        txtPrice.Clear()
        cmbCategory.Text = ""
        txtstock.Text = ""
        browse.Image = Nothing

        btnsave.Enabled = True
        btnupdate.Enabled = False
        txtdescription.Focus()









    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

    Private Sub txtPrice_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPrice.KeyPress
        Select Case Asc(e.KeyChar)
            Case 48 To 57
            Case 46
            Case 8
            Case Else
                e.Handled = True
        End Select





    End Sub

    Private Sub btnupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnupdate.Click

        Try


            If txtdescription.Text = String.Empty Or txtPrice.Text = String.Empty Then

                MsgBox("Please Input Data!", vbCritical)


            End If

            If txtdescription.Text = String.Empty Or txtPrice.Text = String.Empty Then

                MsgBox("[Description][Price] One of this is Empty!!", vbCritical)


            End If
            If txtstock.Text = String.Empty Or cmbCategory.Text = String.Empty Then

                MsgBox("[Category][Stock] One of this is Empty!", vbCritical)

            End If
            If cmbstatus.Text = String.Empty Then

                MsgBox("Please Input Data!", vbCritical)
            End If



            If MsgBox("Update This Item?", vbYesNo + vbQuestion) = vbYes Then
                Dim ms As New MemoryStream
                browse.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                Dim arrImage() As Byte = ms.GetBuffer



                cn.Open()
                cm = New MySqlCommand("UPDATE `tblnewproduct` SET Description=@Description,Category=@Category,Price=@Price,Image=@Image,Stock=@Stock,Status=@Status where ProductID=@ProductID", cn)
                With cm

                    .Parameters.AddWithValue("@Description", txtdescription.Text)
                    .Parameters.AddWithValue("@Category", cmbCategory.Text)
                    .Parameters.AddWithValue("@Price", txtPrice.Text)
                    .Parameters.AddWithValue("@Image", arrImage)
                    .Parameters.AddWithValue("@Stock", txtstock.Text)
                    .Parameters.AddWithValue("@Status", cmbstatus.Text)
                    .Parameters.AddWithValue("@ProductID", txtProductID.Text)


                    .ExecuteNonQuery()
                End With
                cn.Close()

                MsgBox("Item Has Been Updated", vbInformation)
                Me.Dispose()
                productlist.DataGridView1.Rows.Clear()
                With productlist
                    .loadRecords()


                End With
                loadStatus()

            End If


        Catch ex As Exception

            cn.Close()
            MsgBox(ex.Message, vbCritical)



        End Try




    End Sub








   
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Status.ShowDialog()
    End Sub
End Class