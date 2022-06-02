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

            Dim id As String = sender.tag.ToString()

            cn.Open()
            cm = New MySqlCommand("select * From tblproduct where id like '" & id & "'", cn)
            dr = cm.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                price = CDbl(dr.Item("price").ToString)

            End If
            dr.Close()
            cn.Close()

            'pass here
            AddToCart(id, price)
            'MsgBox("")
        Catch ex As Exception
            cn.Close()
            MsgBox(ex.Message, vbCritical)
        End Try
    End Sub

    Sub AddToCart(ByVal id As String, ByVal price As Double)
        Dim sdate As String = Now.ToString("yyyy-MM-dd")
        cn.Open()
        cm = New MySqlCommand("insert into tblcart (transno, pid, price, tdate, tableno)values(@transno, @pid, @price, @tdate, @tableno)", cn)
        With cm
            .Parameters.AddWithValue("@transno", lblTransNo.Text)
            .Parameters.AddWithValue("@pid", id)
            .Parameters.AddWithValue("@price", price)
            .Parameters.AddWithValue("@tdate", sdate)
            .Parameters.AddWithValue("@tableno", lbltable.Text)
            .ExecuteNonQuery()
        End With
        cn.Close()

        cn.Open()
        cm = New MySqlCommand("update tblcart set total = price * qty", cn)
        cm.ExecuteNonQuery()
        cn.Close()

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
            cm = New MySqlCommand("select * from tblcart where transno like '" & sdate & "%' order by id desc", cn)
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

End Class
