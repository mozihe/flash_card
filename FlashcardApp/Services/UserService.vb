Imports MySql.Data.MySqlClient
Imports FlashcardApp.Models
Imports System.Security.Cryptography
Imports System.Text

Namespace Services
    Public Class UserService
        ' 用户注册
        Public Shared Function Register(username As String, password As String) As Boolean
            ' 检查用户名是否已存在
            If UserExists(username) Then
                Return False
            End If

            ' 密码加密
            Dim hashedPassword = HashPassword(password)

            Dim query As String = "INSERT INTO users (username, password, created_at) VALUES (@username, @password, @created_at)"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@password", hashedPassword)
                    cmd.Parameters.AddWithValue("@created_at", DateTime.Now)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        End Function

        ' 用户登录
        Public Shared Function Login(username As String, password As String) As User
            Dim query As String = "SELECT * FROM users WHERE username = @username"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim storedHashedPassword = reader("password").ToString()
                            If VerifyPassword(password, storedHashedPassword) Then
                                Return New User(
                                    reader("id"),
                                    reader("username"),
                                    storedHashedPassword,
                                    reader("created_at")
                                )
                            End If
                        End If
                    End Using
                End Using
            End Using
            Return Nothing
        End Function

        ' 检查用户名是否已存在
        Private Shared Function UserExists(username As String) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM users WHERE username = @username"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    conn.Open()
                    Dim count = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        End Function

        ' 密码加密
        Private Shared Function HashPassword(password As String) As String
            Using sha256 As SHA256 = SHA256.Create()
                Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
                Dim hash As Byte() = sha256.ComputeHash(bytes)
                Return Convert.ToBase64String(hash)
            End Using
        End Function


        ' 验证密码
        Private Shared Function VerifyPassword(inputPassword As String, storedHashedPassword As String) As Boolean
            Dim inputHashed = HashPassword(inputPassword)
            Return inputHashed = storedHashedPassword
        End Function
    End Class
End Namespace
