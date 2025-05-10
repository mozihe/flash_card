Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml
Imports FlashcardApp.FlashcardApp.Views.Dialogs

Namespace Views
    Public Class ConfirmDialog
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            AvaloniaXamlLoader.Load(Me)
        End Sub

        Protected Overrides Sub OnOpened(e As EventArgs)
            MyBase.OnOpened(e)
            Dim vm = TryCast(DataContext, ConfirmDialogViewModel)
            If vm IsNot Nothing Then
                ' 当用户在ViewModel中执行Confirm或Cancel命令时关闭窗口并返回结果
                vm.CloseAction = AddressOf Close
            End If
        End Sub
    End Class
End Namespace
