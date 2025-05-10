Imports System

Namespace Models
    Public Class DailyStats
        Public Property StatsDate As DateTime
        Public Property FlashcardCount As Integer

        Public Sub New()
        End Sub

        Public Sub New([date] As DateTime, flashcardCount As Integer)
            Me.StatsDate = [date]
            Me.FlashcardCount = flashcardCount
        End Sub
    End Class
End Namespace
