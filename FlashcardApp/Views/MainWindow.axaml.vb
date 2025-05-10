Imports Avalonia.Controls
Imports FlashcardApp.ViewModels

Namespace Views
    Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainWindowViewModel()
        End Sub

        Private Sub InitializeComponent()
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(Me)
        End Sub
    End Class
End Namespace
