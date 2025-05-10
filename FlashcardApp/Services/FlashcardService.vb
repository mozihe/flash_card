Imports MySql.Data.MySqlClient
Imports FlashcardApp.Models

Namespace Services
    Public Class FlashcardService
        ' 添加新抽认卡
        Public Shared Sub AddFlashcard(flashcard As Flashcard, userId As Integer)
            Dim query As String = "INSERT INTO flashcards (user_id, front_text, back_text) VALUES (@user_id, @front_text, @back_text)"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    cmd.Parameters.AddWithValue("@front_text", flashcard.FrontText)
                    cmd.Parameters.AddWithValue("@back_text", flashcard.BackText)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ' 获取用户的所有未隐藏抽认卡
        Public Shared Function GetUserFlashcards(userId As Integer) As List(Of Flashcard)
            Dim flashcards As New List(Of Flashcard)
            Dim query As String = "SELECT * FROM flashcards WHERE user_id = @user_id AND is_hidden = FALSE"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim flashcard As New Flashcard(
                                reader("id"),
                                reader("front_text"),
                                reader("back_text"),
                                reader("correct_count"),
                                reader("incorrect_count"),
                                If(IsDBNull(reader("last_reviewed")), Nothing, reader("last_reviewed")),
                                reader("is_hidden")
                            )
                            flashcards.Add(flashcard)
                        End While
                    End Using
                End Using
            End Using
            Return flashcards
        End Function

        ' 获取一张随机抽认卡（动态概率）: 使用权重算法而不是ORDER BY RAND()
        Public Shared Function GetRandomFlashcard(userId As Integer) As Flashcard
            ' 获取所有未隐藏的卡片
            Dim flashcards = GetUserFlashcards(userId)
            If flashcards Is Nothing OrElse flashcards.Count = 0 Then
                Return Nothing
            End If

            ' 根据权重进行加权随机
            ' 定义权重函数： weight = 1 / (correct_count + 1)
            Dim totalWeight As Double = 0
            Dim weights As New List(Of Tuple(Of Flashcard, Double))

            For Each fc In flashcards
                Dim weight = 1.0 / (fc.CorrectCount + 1)
                weights.Add(Tuple.Create(fc, weight))
                totalWeight += weight
            Next

            ' 生成一个0到totalWeight之间的随机数
            Dim rnd As New Random()
            Dim r = rnd.NextDouble() * totalWeight

            ' 按照权重累积选择相应卡片
            Dim cumulative As Double = 0
            For Each item In weights
                cumulative += item.Item2
                If r < cumulative Then
                    Return item.Item1
                End If
            Next

            ' 理论上不应到达这里，如果到达这里就返回最后一张卡
            Return weights.Last().Item1
        End Function

        ' 更新抽认卡的统计信息
        Public Shared Sub UpdateFlashcardStats(flashcard As Flashcard, isCorrect As Boolean, userId As Integer)
            Dim query As String
            If isCorrect Then
                query = "UPDATE flashcards SET correct_count = correct_count + 1, last_reviewed = @last_reviewed WHERE id = @id AND user_id = @user_id"
            Else
                query = "UPDATE flashcards SET incorrect_count = incorrect_count + 1, last_reviewed = @last_reviewed WHERE id = @id AND user_id = @user_id"
            End If
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", flashcard.Id)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    cmd.Parameters.AddWithValue("@last_reviewed", DateTime.Now)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' 更新每日统计信息
            StatsService.UpdateDailyStats(userId)
        End Sub

        ' 隐藏熟悉的抽认卡
        Public Shared Sub HideFlashcard(flashcardId As Integer, userId As Integer)
            Dim query As String = "UPDATE flashcards SET is_hidden = TRUE WHERE id = @id AND user_id = @user_id"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", flashcardId)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ' 显示被隐藏的抽认卡
        Public Shared Sub ShowFlashcard(flashcardId As Integer, userId As Integer)
            Dim query As String = "UPDATE flashcards SET is_hidden = FALSE WHERE id = @id AND user_id = @user_id"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", flashcardId)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ' 将所有卡片都设为可见
        Public Shared Sub ShowAllFlashcards(userId As Integer)
            Dim query As String = "UPDATE flashcards SET is_hidden = FALSE WHERE user_id = @user_id"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ' 获取所有被隐藏的抽认卡
        Public Shared Function GetHiddenFlashcards(userId As Integer) As List(Of Flashcard)
            Dim flashcards As New List(Of Flashcard)
            Dim query As String = "SELECT * FROM flashcards WHERE user_id = @user_id AND is_hidden = TRUE"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim flashcard As New Flashcard(
                                reader("id"),
                                reader("front_text"),
                                reader("back_text"),
                                reader("correct_count"),
                                reader("incorrect_count"),
                                If(IsDBNull(reader("last_reviewed")), Nothing, reader("last_reviewed")),
                                reader("is_hidden")
                            )
                            flashcards.Add(flashcard)
                        End While
                    End Using
                End Using
            End Using
            Return flashcards
        End Function

        ' 删除抽认卡
        Public Shared Sub DeleteFlashcard(flashcardId As Integer, userId As Integer)
            Dim query As String = "DELETE FROM flashcards WHERE id = @id AND user_id = @user_id"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", flashcardId)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ' 更新抽认卡内容
        Public Shared Sub UpdateFlashcardContent(flashcard As Flashcard, userId As Integer)
            Dim query As String = "UPDATE flashcards SET front_text = @front_text, back_text = @back_text WHERE id = @id AND user_id = @user_id"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", flashcard.Id)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    cmd.Parameters.AddWithValue("@front_text", flashcard.FrontText)
                    cmd.Parameters.AddWithValue("@back_text", flashcard.BackText)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub
    End Class
End Namespace
