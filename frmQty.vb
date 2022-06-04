Imports MySql.Data.MySqlClient

Public Class frmQty
    Dim id As String, price As Double

    Private Sub frmQty_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub

    Private Sub frmQty_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Dispose()
        ElseIf e.KeyCode = Keys.Enter Then
            Dim sdate As String = Now.ToString("yyyy-MM-dd")
            cn.Open()
            cm = New MySqlCommand("insert into tblcart (transno, pid, price, tdate, tableno, qty)values(@transno, @pid, @price, @tdate, @tableno, @qty)", cn)
            With cm
                .Parameters.AddWithValue("@transno", frmPOS.lblTransNo.Text)
                .Parameters.AddWithValue("@pid", id)
                .Parameters.AddWithValue("@price", price)
                .Parameters.AddWithValue("@tdate", sdate)
                .Parameters.AddWithValue("@tableno", frmPOS.lbltable.Text)
                .Parameters.AddWithValue("@qty", CDbl(txtQty.Text))
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            cm = New MySqlCommand("update tblcart set total = price * qty", cn)
            cm.ExecuteNonQuery()
            cn.Close()
            frmPOS.LoadCart()
            Me.Dispose()
        End If
    End Sub

    Sub AddToCart(ByVal id As String, ByVal price As Double, weight As Boolean)
        If weight = False Then lblQty.Text = "Quantity" Else lblQty.Text = "Weight"
        Me.price = price
        Me.id = id
    End Sub

    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
        Select Case Asc(e.KeyChar)
            Case 48 To 57
            Case 46
            Case 8
            Case 13
            Case Else
                e.Handled = True
        End Select
    End Sub
End Class