Imports ReactiveUI
Imports System.Reactive

Namespace FlashcardApp.Views.Dialogs
    Public Class ConfirmDialogViewModel
        Inherits ReactiveObject

        Private _title As String
        Private _message As String
        Private _confirmText As String
        Private _cancelText As String

        Public Property CloseAction As Action(Of Boolean)

        Public Sub New(title As String, message As String, confirmText As String, cancelText As String)
            _title = title
            _message = message
            _confirmText = confirmText
            _cancelText = cancelText

            ConfirmCommand = ReactiveCommand.Create(Sub() Confirm())
            CancelCommand = ReactiveCommand.Create(Sub() Cancel())
        End Sub

        Public Property Title As String
            Get
                Return _title
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_title, value)
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

        Public Property ConfirmText As String
            Get
                Return _confirmText
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_confirmText, value)
            End Set
        End Property

        Public Property CancelText As String
            Get
                Return _cancelText
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_cancelText, value)
            End Set
        End Property

        Public Property ConfirmCommand As ReactiveCommand(Of Unit, Unit)
        Public Property CancelCommand As ReactiveCommand(Of Unit, Unit)

        Private Sub Confirm()
            CloseAction?.Invoke(True)
        End Sub

        Private Sub Cancel()
            CloseAction?.Invoke(False)
        End Sub
    End Class
End Namespace
