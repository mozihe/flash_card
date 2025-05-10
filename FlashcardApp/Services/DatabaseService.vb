Imports MySql.Data.MySqlClient

Namespace Services
    Public Class DatabaseService
        Private Shared connectionString As String = "Server=localhost;Database=flashcard_app;Uid=flashcard_user;Pwd=111111;"

        Public Shared Function GetConnection() As MySqlConnection
            Return New MySqlConnection(connectionString)
        End Function
    End Class
End Namespace
