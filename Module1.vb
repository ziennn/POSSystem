'for mysqlconnection, import this

Imports MySql.Data.MySqlClient


Module Module1
    Public cn As New MySqlConnection
    Public cm As New MySqlCommand

    Sub Connection()
        cn = New MySqlConnection
        With cn
            'enter all connection properties here
            .ConnectionString = "server=db4free.net;user id=kiessnacks;password=kiessnacks;database=kiessnacksposdb"
        End With
    End Sub
End Module
