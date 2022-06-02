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
            .ConnectionString = "server=db4free.net;user id=kiessnacks;password=kiessnacks;database=kiessnacksposdb"
            '.ConnectionString = "server=sql207.epizy.com;user id=epiz_31844092;password=Nv5QxaNabZAK2;database=epiz_31844092_kiessnackspos"
        End With
    End Sub
End Module
