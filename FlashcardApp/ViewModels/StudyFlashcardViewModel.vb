Imports ReactiveUI
Imports FlashcardApp.Models
Imports FlashcardApp.Services
Imports System.Reactive
Imports System.Reactive.Concurrency
Imports FlashcardApp.Utils
Imports System.Threading.Tasks
Imports FlashcardApp.FlashcardApp.Services

Namespace ViewModels
    Public Class StudyFlashcardViewModel
        Inherits ViewModelBase

        ' 私有字段
        Private _flashcard As Flashcard
        Private _userInput As String
        Private _feedbackMessage As String

        Private mainWindowViewModel As MainWindowViewModel

        ' 命令的支持字段
        Private ReadOnly _checkAnswerCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _backToMainCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _viewStatsCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _nextFlashcardCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _deleteFlashcardCommand As ReactiveCommand(Of Unit, Unit)

        ' 公共属性
        Public ReadOnly Property FrontText As String
            Get
                Return _flashcard?.FrontText
            End Get
        End Property

        Public Property UserInput As String
            Get
                Return _userInput
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_userInput, value)
            End Set
        End Property

        Public Property FeedbackMessage As String
            Get
                Return _feedbackMessage
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_feedbackMessage, value)
            End Set
        End Property

        ' 命令属性
        Public ReadOnly Property CheckAnswerCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _checkAnswerCommand
            End Get
        End Property

        Public ReadOnly Property BackToMainCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _backToMainCommand
            End Get
        End Property

        Public ReadOnly Property ViewStatsCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _viewStatsCommand
            End Get
        End Property

        Public ReadOnly Property NextFlashcardCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _nextFlashcardCommand
            End Get
        End Property

        Public ReadOnly Property DeleteFlashcardCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _deleteFlashcardCommand
            End Get
        End Property

        ' 构造函数
        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            Me.mainWindowViewModel = mainWindowViewModel

            ' 初始化命令，指定执行和通知调度器为 AvaloniaScheduler
            _checkAnswerCommand = ReactiveCommand.Create(AddressOf CheckAnswer, outputScheduler:=AvaloniaScheduler.Instance)
            _backToMainCommand = ReactiveCommand.Create(AddressOf BackToMain, outputScheduler:=AvaloniaScheduler.Instance)
            _viewStatsCommand = ReactiveCommand.Create(AddressOf ViewStats, outputScheduler:=AvaloniaScheduler.Instance)
            _nextFlashcardCommand = ReactiveCommand.Create(AddressOf LoadNextFlashcard, outputScheduler:=AvaloniaScheduler.Instance)
            _deleteFlashcardCommand = ReactiveCommand.Create(AddressOf DeleteFlashcard, outputScheduler:=AvaloniaScheduler.Instance)

            LoadNextFlashcard()
        End Sub

        ' 加载下一张抽认卡方法
        Private Sub LoadNextFlashcard()
            Dim userId As Integer = MainWindowViewModel.CurrentUser.Id
            _flashcard = FlashcardService.GetRandomFlashcard(userId)
            UserInput = String.Empty
            FeedbackMessage = String.Empty
            Me.RaisePropertyChanged(NameOf(FrontText))
        End Sub

        ' 检查答案方法
        Private Sub CheckAnswer()
            If _flashcard Is Nothing Then
                FeedbackMessage = "没有更多的抽认卡，请添加新的抽认卡。"
                Return
            End If

            If UserInput.Trim() = _flashcard.BackText.Trim() Then
                FeedbackMessage = "回答正确！"
                ' 更新抽认卡统计信息
                FlashcardService.UpdateFlashcardStats(_flashcard, True, MainWindowViewModel.CurrentUser.Id)
                ' 检查是否需要隐藏卡片
                If _flashcard.CorrectCount + 1 >= 5 Then
                    FlashcardService.HideFlashcard(_flashcard.Id, MainWindowViewModel.CurrentUser.Id)
                End If
            Else
                FeedbackMessage = "回答错误，正确答案是：" & _flashcard.BackText
                ' 更新抽认卡统计信息
                FlashcardService.UpdateFlashcardStats(_flashcard, False, MainWindowViewModel.CurrentUser.Id)
            End If


        End Sub

        ' 删除当前抽认卡方法（需要二次确认）
        Private Async Sub DeleteFlashcard()
            If _flashcard Is Nothing Then
                FeedbackMessage = "当前没有可删除的抽认卡。"
                Return
            End If

            ' 弹出二次确认对话框，下面为伪代码，请根据实际UI框架调整
            ' 假设 DialogService 是你自己的弹窗服务类
            Dim result As Boolean = Await DialogService.ShowConfirmationAsync("确认删除", "你确定要删除这张抽认卡吗？", "确定", "取消")

            If result Then
                FlashcardService.DeleteFlashcard(_flashcard.Id, MainWindowViewModel.CurrentUser.Id)
                FeedbackMessage = "卡片已删除。"
                LoadNextFlashcard()
            Else
                FeedbackMessage = "已取消删除操作。"
            End If
        End Sub

        ' 返回主界面方法
        Private Sub BackToMain()
            mainWindowViewModel.ShowCreateFlashcardView()
        End Sub

        ' 显示统计视图方法
        Private Sub ViewStats()
            mainWindowViewModel.ShowStatsView()
        End Sub
    End Class
End Namespace
