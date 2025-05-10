Imports MySql.Data.MySqlClient
Imports FlashcardApp.Models

Namespace Services
    Public Class StatsService
        ' 更新每日统计
        Public Shared Sub UpdateDailyStats(userId As Integer)
            Dim querySelect As String = "SELECT * FROM stats WHERE user_id = @user_id AND study_date = @study_date"
            Dim queryInsert As String = "INSERT INTO stats (user_id, study_date, flashcards_studied) VALUES (@user_id, @study_date, 1)"
            Dim queryUpdate As String = "UPDATE stats SET flashcards_studied = flashcards_studied + 1 WHERE id = @id"

            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmdSelect As New MySqlCommand(querySelect, conn)
                    cmdSelect.Parameters.AddWithValue("@user_id", userId)
                    cmdSelect.Parameters.AddWithValue("@study_date", DateTime.Today)
                    conn.Open()
                    Using reader = cmdSelect.ExecuteReader()
                        If reader.Read() Then
                            ' 已有记录，更新数量
                            Dim id = reader("id")
                            conn.Close()
                            Using cmdUpdate As New MySqlCommand(queryUpdate, conn)
                                cmdUpdate.Parameters.AddWithValue("@id", id)
                                conn.Open()
                                cmdUpdate.ExecuteNonQuery()
                            End Using
                        Else
                            ' 没有记录，插入新记录
                            conn.Close()
                            Using cmdInsert As New MySqlCommand(queryInsert, conn)
                                cmdInsert.Parameters.AddWithValue("@user_id", userId)
                                cmdInsert.Parameters.AddWithValue("@study_date", DateTime.Today)
                                conn.Open()
                                cmdInsert.ExecuteNonQuery()
                            End Using
                        End If
                    End Using
                End Using
            End Using
        End Sub

        ' 获取最近几天的统计数据
        Public Shared Function GetRecentStats(userId As Integer, days As Integer) As List(Of DailyStats)
            Dim stats As New List(Of DailyStats)
            Dim query As String = "SELECT study_date, flashcards_studied FROM stats WHERE user_id = @user_id AND study_date >= @start_date ORDER BY study_date"
            Using conn As MySqlConnection = DatabaseService.GetConnection()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@user_id", userId)
                    cmd.Parameters.AddWithValue("@start_date", DateTime.Today.AddDays(-days + 1))
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim stat As New DailyStats(
                                reader("study_date"),
                                reader("flashcards_studied")
                            )
                            stats.Add(stat)
                        End While
                    End Using
                End Using
            End Using
            Return stats
        End Function
    End Class
End Namespace
