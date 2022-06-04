'Import mysql here
Imports MySql.Data.MySqlClient
Imports System.IO


Public Class frmPOS
    Dim btnCategory As New Button
    Dim pic As New PictureBox
    Dim lblDesc As New Label
    Dim lblPrice As New Label


    Dim _filter As String = ""

    Private Sub frmPOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connection()
        LoadCategory()
        LoadMenu()

    End Sub

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        With frmProductList
            .LoadRecord()
            .ShowDialog()
        End With
    End Sub

    'Display products to POS Menu
    Sub LoadMenu()
        MenuFlowLayoutPanel.AutoScroll = True
        MenuFlowLayoutPanel.Controls.Clear()

        cn.Open()
        cm = New MySqlCommand("select image, id, description, price, size, weight, status from tblproduct where category like '" & _filter & "%' order by description", cn)
        dr = cm.ExecuteReader
        While dr.Read
            Dim len As Long = dr.GetBytes(0, 0, Nothing, 0, 0)
            Dim array(CInt(len)) As Byte
            dr.GetBytes(0, 0, array, 0, CInt(len))

            'menu pic
            pic = New PictureBox
            pic.Width = 100
            pic.Height = 100
            pic.BackgroundImageLayout = ImageLayout.Stretch
            pic.Cursor = Cursors.Hand
            'get tag of id
            pic.Tag = dr.Item("id").ToString

            Dim ms As New MemoryStream(array)
            Dim bitmap As New System.Drawing.Bitmap(ms)
            pic.BackgroundImage = bitmap

            'Description in Product menu pic
            lblDesc = New Label
            lblDesc.BackColor = Color.DimGray
            lblDesc.ForeColor = Color.White
            lblDesc.Font = New Font("Calibri", 8, FontStyle.Regular)
            lblDesc.TextAlign = ContentAlignment.MiddleCenter
            lblDesc.Text = dr.Item("description").ToString
            lblDesc.Dock = DockStyle.Top
            lblDesc.AutoSize = False
            lblDesc.Cursor = Cursors.Hand
            lblDesc.Tag = dr.Item("id").ToString


            'Price in Product menu pic
            lblPrice = New Label
            lblPrice.BackColor = Color.FromArgb(200, 100, 0)
            lblPrice.ForeColor = Color.White
            lblPrice.Font = New Font("Calibri", 12, FontStyle.Regular)
            lblPrice.TextAlign = ContentAlignment.MiddleCenter
            lblPrice.Text = dr.Item("price").ToString
            lblPrice.Dock = DockStyle.Bottom
            lblPrice.AutoSize = True
            lblPrice.Cursor = Cursors.Hand
            lblPrice.Tag = dr.Item("id").ToString

            'Display image w/ desc and price in menu 
            pic.Controls.Add(lblDesc)
            pic.Controls.Add(lblPrice)
            MenuFlowLayoutPanel.Controls.Add(pic)

            '11 | add event
            AddHandler pic.Click, AddressOf select_Click
            AddHandler lblDesc.Click, AddressOf select_Click
            AddHandler lblPrice.Click, AddressOf select_Click


        End While
        dr.Close()
        cn.Close()
    End Sub

    'Display categories to panel
    Sub LoadCategory()
        cn.Open()
        cm = New MySqlCommand("select * from tblcategory", cn)
        dr = cm.ExecuteReader
        While dr.Read
            btnCategory = New Button
            btnCategory.Width = 95
            btnCategory.Height = 35
            btnCategory.Text = dr.Item("category").ToString
            btnCategory.FlatStyle = FlatStyle.Flat
            btnCategory.BackColor = Color.FromArgb(200, 100, 0)
            btnCategory.ForeColor = Color.White
            btnCategory.Cursor = Cursors.Hand
            btnCategory.TextAlign = ContentAlignment.MiddleLeft
            CategoryFlowLayoutPanel.Controls.Add(btnCategory)


            'Filtering
            AddHandler btnCategory.Click, AddressOf Filter_click

        End While
        dr.Close()
        cn.Close()
    End Sub
    '11 | Create event for selecting item
    Public Sub Select_Click(sender As Object, e As EventArgs)
        Try
            Dim price As Double
            Dim weight As Boolean
            Dim id As String = sender.tag.ToString()

            cn.Open()
            cm = New MySqlCommand("select * From tblproduct where id like '" & id & "'", cn)
            dr = cm.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                price = CDbl(dr.Item("price").ToString)
                weight = CBool(dr.Item("weight").ToString)
            End If
            dr.Close()
            cn.Close()

            '12
            With frmQty
                'pass here
                .AddToCart(id, price, weight)
                .Show()
            End With


            'MsgBox("")
        Catch ex As Exception
            cn.Close()
            MsgBox(ex.Message, vbCritical)
        End Try
    End Sub
    '12 |
    Sub LoadCart()
        Dim _total As Double
        DataGridView1.Rows.Clear()
        cn.Open()
        cm = New MySqlCommand("select c.id, p.description, p.size, c.price, c.qty, c.total from tblcart as c inner join tblproduct as p on p.id = c.pid where c.transno like '" & lblTransNo.Text & "'", cn)
        dr = cm.ExecuteReader
        While dr.Read
            _total += CDbl(dr.Item("total").ToString)
            DataGridView1.Rows.Add(dr.Item("id").ToString, dr.Item("description").ToString, dr.Item("size").ToString, dr.Item("price").ToString, dr.Item("qty").ToString, dr.Item("total").ToString)
        End While
        dr.Close()
        cn.Close()

        lblTotal.Text = Format(_total, "#,##0.00")
    End Sub

    'create event
    Public Sub Filter_click(sender As Object, e As EventArgs)
        If lblTransNo.Text = String.Empty Then
            MsgBox("Click New Order First", vbCritical)
            Return
        End If

        _filter = sender.text.ToString

        LoadMenu()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnNewOrder.Click
        lblTransNo.Text = GetTransno()
        With frmSelectTable
            .LoadTable()

            .ShowDialog()
        End With
    End Sub

    Private Sub btnTable_Click(sender As Object, e As EventArgs) Handles btnTable.Click
        With frmTable
            .btnUpdate.Enabled = False
            .Loadrecord()
            .ShowDialog()
        End With
    End Sub

    'Transaction num
    Function GetTransno() As String
        Try
            Dim sdate As String = Now.ToString("yyyyMMdd")
            cn.Open()
            cm = New MySqlCommand("Select * from tblcart where transno Like '" & sdate & "%' order by id desc", cn)
            dr = cm.ExecuteReader
            dr.Read()

            If dr.HasRows Then
                GetTransno = CLng(dr.Item("transno").ToString) + 1
            Else
                GetTransno = sdate & "0001"
            End If

            dr.Close()
            cn.Close()
            Return GetTransno

        Catch ex As Exception
            cn.Close()
            MsgBox(ex.Message, vbCritical)
        End Try
        Return GetTransno()
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim colName As String = DataGridView1.Columns(e.ColumnIndex).Name
        If colName = "colRemove" Then
            If MsgBox("Remove this item from the list?", vbYesNo + vbQuestion) = vbYes Then
                cn.Open()
                cm = New MySqlCommand("delete from tblcart where id like '" & DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString & "'", cn)
                cm.ExecuteNonQuery()
                cn.Close()
                MsgBox("Item successfully removed from the cart!", vbInformation)
                LoadCart()
            End If
        End If

    End Sub
End Class
