Imports ReactiveUI
Imports Avalonia.Controls
Imports FlashcardApp.Views
Imports FlashcardApp.Models

Namespace ViewModels
    Public Class MainWindowViewModel
        Inherits ViewModelBase

        ' 保存当前用户信息
        Public Shared Property CurrentUser As User

        Private _currentView As UserControl
        Public Property CurrentView As UserControl
            Get
                Return _currentView
            End Get
            Set(value As UserControl)
                Me.RaiseAndSetIfChanged(_currentView, value)
            End Set
        End Property

        Public Sub New()
            ' 默认显示登录视图
            ShowLoginView()
        End Sub

        ' 切换到登录视图
        Public Sub ShowLoginView()
            CurrentView = New LoginView(Me)
        End Sub

        ' 切换到注册视图
        Public Sub ShowRegisterView()
            CurrentView = New RegisterView(Me)
        End Sub

        ' 切换到创建抽认卡视图
        Public Sub ShowCreateFlashcardView()
            CurrentView = New CreateFlashcardView(Me)
        End Sub

        ' 切换到学习抽认卡视图
        Public Sub ShowStudyFlashcardView()
            CurrentView = New StudyFlashcardView(Me)
        End Sub

        ' 切换到复习抽认卡视图
        Public Sub ShowReviewFlashcardView()
            CurrentView = New ReviewFlashcardView(Me)
        End Sub

        ' 切换到统计视图
        Public Sub ShowStatsView()
            CurrentView = New StatsView(Me)
        End Sub
    End Class
End Namespace
