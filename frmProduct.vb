'Import sql library here to import datas to database
Imports MySql.Data.MySqlClient
Imports System.IO


Public Class frmProduct
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Me.Dispose()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        With frmCategory
            .ShowDialog()
        End With
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using ofd As New OpenFileDialog With {.Filter = "(Image Files)|*.jpg;*.png;*bmp;*gif;*ico|Jpg,|*.jpg|Png,|*.png|Bmp,|*.bmp|Gif,|*.gif|Ico,|*.ico",
                .Multiselect = False, .Title = "Select Image"}
            If ofd.ShowDialog = 1 Then
                PictureBox1.BackgroundImage = Image.FromFile(ofd.FileName)
                OpenFileDialog1.FileName = ofd.FileName
            End If
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            'If no image is set, display error
            If OpenFileDialog1.FileName = "OpenFileDialog1" Then
                MsgBox("Please select image!", vbCritical)
                Return
            End If
            'Display error msg if textboxes are empty
            If txtDescription.Text = String.Empty Or txtPrice.Text = String.Empty Then
                MsgBox("Please input data!", vbCritical)
                Return
            End If
            'Display error msg if combo boxes are empty
            If cboCategory.Text = String.Empty Or cboSize.Text = String.Empty Or cboStatus.Text = String.Empty Then
                MsgBox("Please select data from the list!", vbCritical)
                Return
            End If


            'Saving record 
            If MsgBox("Save this record?", vbYesNo + vbQuestion) = vbYes Then
                Dim mstream As New MemoryStream
                PictureBox1.BackgroundImage.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
                Dim arrImage() As Byte = mstream.GetBuffer

                cn.Open()
                cm = New MySqlCommand("Insert into tblproduct(description, price, category, size, weight, image)values(@description, @price, @category, @size, @weight, @image)", cn)
                With cm.Parameters
                    .AddWithValue("@description", txtDescription.Text)
                    .AddWithValue("@price", CDbl(txtPrice.Text))
                    .AddWithValue("@category", cboCategory.Text)
                    .AddWithValue("@size", cboSize.Text)
                    .AddWithValue("@weight", CheckBox1.Checked.ToString)
                    .AddWithValue("@image", arrImage)
                End With
                cm.ExecuteNonQuery()
                cn.Close()
                MsgBox("Record has been successfully saved!", vbInformation)
                Clear()
            End If
        Catch ex As Exception
            cn.Close()
            MsgBox(ex.Message, vbCritical)
        End Try
    End Sub

    Sub LoadCategory()
        cboCategory.Items.Clear()
        cn.Open()
        cm = New MySqlCommand("select * from tblcategory", cn)
        dr = cm.ExecuteReader
        While dr.Read
            cboCategory.Items.Add(dr.Item("category").ToString)
        End While
        dr.Close()
        cn.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged

    End Sub

    Private Sub txtPrice_TextChanged(sender As Object, e As EventArgs) Handles txtPrice.TextChanged

    End Sub

    Sub Clear()
        txtDescription.Clear()
        txtID.Text = "(Auto)"
        txtPrice.Clear()
        cboCategory.Text = ""
        cboSize.Text = ""
        cboStatus.Text = ""
        btnSave.Enabled = True
        btnSave.Enabled = False
        txtDescription.Focus()
    End Sub



    Private Sub txtPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrice.KeyPress
        Select Case Asc(e.KeyChar)
            Case 48 To 57
            Case 46
            Case 8
            Case Else
                e.Handled = True
        End Select
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Clear()
    End Sub
End Class