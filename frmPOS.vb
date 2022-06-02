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

            'Display image w/ desc and price in menu 
            pic.Controls.Add(lblDesc)
            pic.Controls.Add(lblPrice)
            MenuFlowLayoutPanel.Controls.Add(pic)


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
            AddHandler btnCategory.Click, AddressOf filter_click

        End While
        dr.Close()
        cn.Close()
    End Sub


    'create event
    Public Sub filter_click(sender As Object, e As EventArgs)
        _filter = sender.text.ToString

        LoadMenu()
    End Sub

End Class
