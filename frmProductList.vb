'Import mysql here
Imports MySql.Data.MySqlClient
Imports System.IO

Public Class frmProductList
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        With frmProduct
            .btnSave.Enabled = True
            .btnUpdate.Enabled = False
            .LoadCategory()
            .ShowDialog()
        End With
    End Sub

    Sub LoadRecord()
        'remove duplicates
        DataGridView1.Rows.Clear()

        'adding item to Productlist records
        Dim i As Integer

        cn.Open()
        cm = New MySqlCommand("select * from tblproduct", cn)
        dr = cm.ExecuteReader
        While dr.Read
            i += 1
            DataGridView1.Rows.Add(i, dr.Item("id").ToString, dr.Item("description").ToString, dr.Item("category").ToString, dr.Item("price").ToString,
                                   dr.Item("size").ToString, dr.Item("weight"), dr.Item("image"), dr.Item("status").ToString)
        End While
        dr.Close()
        cn.Close()

        For i = 0 To DataGridView1.Rows.Count - 1
            Dim r As DataGridViewRow = DataGridView1.Rows(i)
            r.Height = 100
        Next

        Dim imagecolumn = DirectCast(DataGridView1.Columns("Column8"), DataGridViewImageColumn)
        imagecolumn.ImageLayout = DataGridViewImageCellLayout.Stretch


    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim colname As String = DataGridView1.Columns(e.ColumnIndex).Name
        If colname = "colEdit" Then
            With frmProduct
                cn.Open()
                cm = New MySqlCommand("select image from tblproduct where id like '" & DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString & "'", cn)
                dr = cm.ExecuteReader
                While dr.Read
                    Dim len As Long = dr.GetBytes(0, 0, Nothing, 0, 0)
                    Dim array(CInt(len)) As Byte
                    dr.GetBytes(0, 0, array, 0, CInt(len))
                    Dim ms As New MemoryStream(array)
                    Dim bitmap As New System.Drawing.Bitmap(ms)
                    .PictureBox1.BackgroundImage = bitmap
                    .txtID.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString
                    .txtDescription.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString
                    .cboCategory.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString
                    .txtPrice.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
                    .cboSize.Text = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
                    .CheckBox1.Checked = CBool(DataGridView1.Rows(e.RowIndex).Cells(6).Value.ToString)
                    .cboStatus.Text = DataGridView1.Rows(e.RowIndex).Cells(8).Value.ToString
                End While
                dr.Close()
                cn.Close()

                .LoadCategory()

                .btnSave.Enabled = False
                .btnUpdate.Enabled = True
                .ShowDialog()
            End With

        ElseIf colname = "colDelete" Then
            If MsgBox("Delete this record?", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("delete from tblproduct where id like '" & DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString & "'", cn)
                cm.ExecuteNonQuery()
                cn.Close()
                MsgBox("Record has been successfully deleted!", vbInformation)
                LoadRecord()
            End If
        End If

    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Me.Dispose()
    End Sub
End Class