Imports ReactiveUI
Imports FlashcardApp.Models
Imports FlashcardApp.Services
Imports System.Reactive
Imports System.Text.Json
Imports System.IO

Namespace ViewModels
    Public Class CreateFlashcardViewModel
        Inherits ViewModelBase

        ' 私有字段
        Private _frontText As String
        Private _backText As String
        Private _statusMessage As String

        Private mainWindowViewModel As MainWindowViewModel

        ' 命令的支持字段
        Private ReadOnly _addFlashcardCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _backToMainCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _importFromJsonCommand As ReactiveCommand(Of Unit, Unit)

        ' 当需要导入json时触发此事件，由View监听
        Public Event ImportJsonRequested As EventHandler

        ' 公共属性
        Public Property FrontText As String
            Get
                Return _frontText
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_frontText, value)
            End Set
        End Property

        Public Property BackText As String
            Get
                Return _backText
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_backText, value)
            End Set
        End Property

        Public Property StatusMessage As String
            Get
                Return _statusMessage
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_statusMessage, value)
            End Set
        End Property

        ' 命令属性
        Public ReadOnly Property AddFlashcardCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _addFlashcardCommand
            End Get
        End Property

        Public ReadOnly Property BackToMainCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _backToMainCommand
            End Get
        End Property

        Public ReadOnly Property ImportFromJsonCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _importFromJsonCommand
            End Get
        End Property

        ' 构造函数
        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            Me.mainWindowViewModel = mainWindowViewModel

            ' 初始化命令
            _addFlashcardCommand = ReactiveCommand.Create(AddressOf AddFlashcard)
            _backToMainCommand = ReactiveCommand.Create(AddressOf BackToMain)
            _importFromJsonCommand = ReactiveCommand.Create(AddressOf ImportFromJson)
        End Sub

        ' 添加抽认卡方法
        Public Sub AddFlashcard()
            If String.IsNullOrWhiteSpace(FrontText) OrElse String.IsNullOrWhiteSpace(BackText) Then
                StatusMessage = "正面和反面内容不能为空！"
                Return
            End If

            Dim flashcard As New Flashcard With {
                .FrontText = FrontText,
                .BackText = BackText
            }

            Dim userId As Integer = MainWindowViewModel.CurrentUser.Id

            FlashcardService.AddFlashcard(flashcard, userId)
            StatusMessage = "抽认卡添加成功！"
            FrontText = String.Empty
            BackText = String.Empty
        End Sub

        ' 返回主界面方法
        Public Sub BackToMain()
            mainWindowViewModel.ShowStudyFlashcardView()
        End Sub

        ' 导入JSON命令执行时调用此方法
        Private Sub ImportFromJson()
            ' 触发事件，让 View 弹出文件选择对话框
            RaiseEvent ImportJsonRequested(Me, EventArgs.Empty)
        End Sub

        ' 从指定的JSON文件中导入抽认卡
        Public Sub ImportFlashcardsFromJson(filePath As String)
            If String.IsNullOrWhiteSpace(filePath) OrElse Not File.Exists(filePath) Then
                StatusMessage = "文件不存在或路径无效！"
                Return
            End If

            Try
                Dim json = File.ReadAllText(filePath)
                Dim flashcardsData = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, String)))(json)

                If flashcardsData Is Nothing OrElse flashcardsData.Count = 0 Then
                    StatusMessage = "JSON文件中没有有效的抽认卡数据！"
                    Return
                End If

                Dim userId As Integer = MainWindowViewModel.CurrentUser.Id

                For Each cardData In flashcardsData
                    Dim front = If(cardData.ContainsKey("front_text"), cardData("front_text"), Nothing)
                    Dim back = If(cardData.ContainsKey("back_text"), cardData("back_text"), Nothing)

                    If Not String.IsNullOrWhiteSpace(front) AndAlso Not String.IsNullOrWhiteSpace(back) Then
                        Dim flashcard As New Flashcard With {
                            .FrontText = front,
                            .BackText = back
                        }
                        FlashcardService.AddFlashcard(flashcard, userId)
                    End If
                Next

                StatusMessage = "JSON文件中的抽认卡已成功导入！"
            Catch ex As Exception
                StatusMessage = "导入时出错：" & ex.Message
            End Try
        End Sub
    End Class
End Namespace
