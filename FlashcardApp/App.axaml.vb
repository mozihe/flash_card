Imports Avalonia
Imports Avalonia.Controls.ApplicationLifetimes
Imports Avalonia.Markup.Xaml
Imports FlashcardApp.Views
Imports ReactiveUI
Imports FlashcardApp.Utils

Public Class App
    Inherits Application

    Public Overrides Sub Initialize()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Overrides Sub OnFrameworkInitializationCompleted()
        If TypeOf ApplicationLifetime Is IClassicDesktopStyleApplicationLifetime Then
            Dim desktop = CType(ApplicationLifetime, IClassicDesktopStyleApplicationLifetime)
            desktop.MainWindow = New MainWindow()
            
            desktop.MainWindow.Width = 800 
            desktop.MainWindow.Height = 450
        End If

        ' 设置 RxApp.MainThreadScheduler 为 AvaloniaScheduler
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance

        MyBase.OnFrameworkInitializationCompleted()
    End Sub
End Class
