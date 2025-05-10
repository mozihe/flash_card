Imports ReactiveUI
Imports FlashcardApp.Services
Imports System.Reactive

Namespace ViewModels
    Public Class RegisterViewModel
        Inherits ViewModelBase

        ' 私有字段
        Private _username As String
        Private _password As String
        Private _confirmPassword As String
        Private _message As String

        Private mainWindowViewModel As MainWindowViewModel

        ' 命令的支持字段
        Private ReadOnly _registerCommand As ReactiveCommand(Of Unit, Unit)
        Private ReadOnly _backToLoginCommand As ReactiveCommand(Of Unit, Unit)

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

        Public Property ConfirmPassword As String
            Get
                Return _confirmPassword
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_confirmPassword, value)
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
        Public ReadOnly Property RegisterCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _registerCommand
            End Get
        End Property

        Public ReadOnly Property BackToLoginCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _backToLoginCommand
            End Get
        End Property

        ' 构造函数
        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            Me.mainWindowViewModel = mainWindowViewModel

            ' 初始化命令
            _registerCommand = ReactiveCommand.Create(AddressOf Register)
            _backToLoginCommand = ReactiveCommand.Create(AddressOf BackToLogin)
        End Sub

        ' 注册方法
        Public Sub Register()
            If Password <> ConfirmPassword Then
                Message = "两次输入的密码不一致！"
                Return
            End If

            If UserService.Register(Username, Password) Then
                Message = "注册成功，请登录！"
                mainWindowViewModel.ShowLoginView()
            Else
                Message = "用户名已存在！"
            End If
        End Sub

        ' 返回登录界面
        Public Sub BackToLogin()
            mainWindowViewModel.ShowLoginView()
        End Sub
    End Class
End Namespace
