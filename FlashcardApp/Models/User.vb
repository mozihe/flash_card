Imports System

Namespace Models
    Public Class User
        Public Property Id As Integer
        Public Property Username As String
        Public Property Password As String  ' 需要加密存储
        Public Property CreatedAt As DateTime

        Public Sub New()
        End Sub

        Public Sub New(id As Integer, username As String, password As String, createdAt As DateTime)
            Me.Id = id
            Me.Username = username
            Me.Password = password
            Me.CreatedAt = createdAt
        End Sub
    End Class
End Namespace
