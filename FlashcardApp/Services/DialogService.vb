Imports System.Threading.Tasks
Imports Avalonia
Imports Avalonia.Controls.ApplicationLifetimes
Imports FlashcardApp.FlashcardApp.Views.Dialogs
Imports FlashcardApp.Views

Namespace Services
    Public Class DialogService
        Public Shared Async Function ShowConfirmationAsync(title As String, message As String, confirmText As String, cancelText As String) As Task(Of Boolean)
            Dim dialog As New ConfirmDialog() With {
                    .DataContext = New ConfirmDialogViewModel(title, message, confirmText, cancelText)
                    }

            ' 获取主窗口引用
            Dim lifetime = TryCast(Application.Current.ApplicationLifetime, IClassicDesktopStyleApplicationLifetime)
            If lifetime Is Nothing OrElse lifetime.MainWindow Is Nothing Then
                Throw New Exception("无法获取主窗口引用")
            End If

            ' ShowDialog 返回对话框关闭时的结果
            Return Await dialog.ShowDialog(Of Boolean)(lifetime.MainWindow)
        End Function
    End Class
End Namespace
