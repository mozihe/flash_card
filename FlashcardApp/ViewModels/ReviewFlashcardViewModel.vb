Imports ReactiveUI
Imports FlashcardApp.Models
Imports FlashcardApp.Services
Imports System.Reactive

Namespace ViewModels
    Public Class ReviewFlashcardViewModel
        Inherits ViewModelBase

        ' 私有字段
        Private _flashcards As List(Of Flashcard)
        Private mainWindowViewModel As MainWindowViewModel

        ' 命令的支持字段
        Private ReadOnly _backToMainCommand As ReactiveCommand(Of Unit, Unit)

        ' 公共属性
        Public Property Flashcards As List(Of Flashcard)
            Get
                Return _flashcards
            End Get
            Set(value As List(Of Flashcard))
                Me.RaiseAndSetIfChanged(_flashcards, value)
            End Set
        End Property

        ' 命令属性
        Public ReadOnly Property BackToMainCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _backToMainCommand
            End Get
        End Property

        ' 构造函数
        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            Me.mainWindowViewModel = mainWindowViewModel

            ' 初始化命令
            _backToMainCommand = ReactiveCommand.Create(AddressOf BackToMain)

            LoadFlashcards()
        End Sub

        ' 加载抽认卡方法
        Public Sub LoadFlashcards()
            Dim userId As Integer = MainWindowViewModel.CurrentUser.Id
            Flashcards = FlashcardService.GetUserFlashcards(userId)
        End Sub

        ' 返回主界面方法
        Public Sub BackToMain()
            mainWindowViewModel.ShowStudyFlashcardView()
        End Sub
    End Class
End Namespace
