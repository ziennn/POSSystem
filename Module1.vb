'for mysqlconnection, import this

Imports MySql.Data.MySqlClient


Module Module1
    Public cn As New MySqlConnection
    Public cm As New MySqlCommand
    Public dr As MySqlDataReader

    Sub Connection()
        cn = New MySqlConnection
        With cn
            'enter all connection properties here
            
        End With
    End Sub
End Module
