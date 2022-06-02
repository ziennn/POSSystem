Imports MySql.Data.MySqlClient

Public Class frmSelectTable
    Dim btnTable As New Button

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Me.Dispose()
    End Sub
    Sub LoadTable()
        cn.Open()
        cm = New MySqlCommand("select * from tbltable", cn)
        dr = cm.ExecuteReader
        While dr.Read
            btnTable = New Button
            btnTable.Width = 95
            btnTable.Height = 35
            btnTable.Text = dr.Item("tableno").ToString
            btnTable.FlatStyle = FlatStyle.Flat
            btnTable.BackColor = Color.FromArgb(200, 100, 0)
            btnTable.ForeColor = Color.White
            btnTable.Cursor = Cursors.Hand
            btnTable.TextAlign = ContentAlignment.MiddleLeft
            TableFlowLayoutPanel.Controls.Add(btnTable)


            'Filtering
            'AddHandler btnTable.Click, AddressOf filter_click

        End While
        dr.Close()
        cn.Close()
    End Sub
End Class