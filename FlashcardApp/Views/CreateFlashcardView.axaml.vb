Imports Avalonia.Controls
Imports FlashcardApp.ViewModels
Imports Avalonia.Markup.Xaml

Namespace Views
    Public Class CreateFlashcardView
        Inherits UserControl

        Private ReadOnly viewModel As CreateFlashcardViewModel

        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            InitializeComponent()
            viewModel = New CreateFlashcardViewModel(mainWindowViewModel)
            DataContext = viewModel

            ' 订阅事件
            AddHandler viewModel.ImportJsonRequested, AddressOf OnImportJsonRequested
        End Sub

        Private Sub InitializeComponent()
            AvaloniaXamlLoader.Load(Me)
        End Sub

        Private Async Sub OnImportJsonRequested(sender As Object, e As EventArgs)
            Dim dialog = New OpenFileDialog() With {
                    .Title = "选择JSON文件",
                    .Filters = New List(Of FileDialogFilter) From {
                    New FileDialogFilter() With {.Name = "JSON Files", .Extensions = New List(Of String) From {"json"}}
                    }
                    }

            ' 获取当前窗口
            Dim window = GetWindow()
            Dim result = Await dialog.ShowAsync(window)
            If result IsNot Nothing AndAlso result.Length > 0 Then
                Dim filePath = result(0)
                viewModel.ImportFlashcardsFromJson(filePath)
            End If
        End Sub

        Private Function GetWindow() As Window
            Dim topLevel = TryCast(Me.VisualRoot, Window)
            Return topLevel
        End Function
    End Class
End Namespace
