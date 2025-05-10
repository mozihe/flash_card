Imports Avalonia.Controls
Imports FlashcardApp.ViewModels

Namespace Views
    Public Class LoginView
        Inherits UserControl

        Private mainWindowViewModel As MainWindowViewModel

        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            InitializeComponent()
            Me.mainWindowViewModel = mainWindowViewModel
            DataContext = New LoginViewModel(mainWindowViewModel)
        End Sub

        Private Sub InitializeComponent()
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(Me)
        End Sub
    End Class
End Namespace
