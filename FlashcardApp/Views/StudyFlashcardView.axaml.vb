Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml
Imports FlashcardApp.ViewModels

Namespace Views
    Public Class StudyFlashcardView
        Inherits UserControl

        Private mainWindowViewModel As MainWindowViewModel

        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            InitializeComponent()
            Me.mainWindowViewModel = mainWindowViewModel
            DataContext = New StudyFlashcardViewModel(mainWindowViewModel)
        End Sub

        Private Sub InitializeComponent()
            AvaloniaXamlLoader.Load(Me)
        End Sub
    End Class
End Namespace
