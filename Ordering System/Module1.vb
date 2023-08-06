Imports MySql.Data.MySqlClient

Module Module1

    Public cn As MySqlConnection
    Public cm As MySqlCommand
    Public dr As MySqlDataReader
    Public User As String
    Public Password As String
    Public Designation As String


    Sub Connection()



        cn = New MySqlConnection
        With cn
            .ConnectionString = "server=localhost;database=cart;user id=MMresto;password=database; "

        End With

    End Sub






End Module
