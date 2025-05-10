Imports System

Namespace Models
    Public Class Flashcard
        Public Property Id As Integer
        Public Property FrontText As String
        Public Property BackText As String
        Public Property CorrectCount As Integer
        Public Property IncorrectCount As Integer
        Public Property LastReviewed As DateTime?
        Public Property IsHidden As Boolean

        Public Sub New()
        End Sub

        Public Sub New(id As Integer, frontText As String, backText As String, correctCount As Integer, incorrectCount As Integer, lastReviewed As DateTime?, isHidden As Boolean)
            Me.Id = id
            Me.FrontText = frontText
            Me.BackText = backText
            Me.CorrectCount = correctCount
            Me.IncorrectCount = incorrectCount
            Me.LastReviewed = lastReviewed
            Me.IsHidden = isHidden
        End Sub
    End Class
End Namespace
