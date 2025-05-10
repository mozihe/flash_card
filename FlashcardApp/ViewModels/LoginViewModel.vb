Imports ReactiveUI
Imports FlashcardApp.Services
Imports FlashcardApp.Models
Imports System.Reactive
Imports System.Reactive.Concurrency
Imports FlashcardApp.Utils

Namespace ViewModels
    Public Class LoginViewModel
        Inherits ViewModelBase

        ' 私有字段
        Private _username As String
        Private _password As String
        Private _message As String

        Private mainWindowViewModel As MainWindowViewModel

        ' 命令的支持字段
        Private ReadOnly _loginCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _goToRegisterCommand As ReactiveCommand(Of Unit, Unit)

        ' 公共属性
        Public Property Username As String
            Get
                Return _username
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_username, value)
            End Set
        End Property

        Public Property Password As String
            Get
                Return _password
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_password, value)
            End Set
        End Property

        Public Property Message As String
            Get
                Return _message
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_message, value)
            End Set
        End Property

        ' 命令属性
        Public ReadOnly Property LoginCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _loginCommand
            End Get
        End Property

        Public ReadOnly Property GoToRegisterCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _goToRegisterCommand
            End Get
        End Property

        ' 构造函数
        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            Me.mainWindowViewModel = mainWindowViewModel

            ' 初始化命令，指定执行和通知调度器为 AvaloniaScheduler
            _loginCommand = ReactiveCommand.Create(AddressOf Login, outputScheduler:=AvaloniaScheduler.Instance)
            _goToRegisterCommand = ReactiveCommand.Create(AddressOf GoToRegister, outputScheduler:=AvaloniaScheduler.Instance)
        End Sub

        ' 登录方法
        Private Sub Login()
            Dim user = UserService.Login(Username, Password)
            If user IsNot Nothing Then
                ' 保存当前用户信息
                MainWindowViewModel.CurrentUser = user
                FlashcardService.ShowAllFlashcards(user.Id)
                mainWindowViewModel.ShowStudyFlashcardView()
            Else
                Message = "用户名或密码错误！"
            End If
        End Sub

        ' 跳转到注册方法
        Private Sub GoToRegister()
            mainWindowViewModel.ShowRegisterView()
        End Sub
    End Class
End Namespace
