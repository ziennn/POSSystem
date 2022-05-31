Public Class frmPOS
    Private Sub frmPOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connection()
    End Sub

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        With frmProductList
            .LoadRecord()
            .ShowDialog()
        End With
    End Sub
End Class
