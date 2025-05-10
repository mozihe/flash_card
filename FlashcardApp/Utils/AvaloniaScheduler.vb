Imports System.Reactive.Concurrency
Imports System.Reactive.Disposables
Imports Avalonia.Threading

Namespace Utils
    Public Class AvaloniaScheduler
        Implements IScheduler

        Public Shared ReadOnly Property Instance As IScheduler = New AvaloniaScheduler()

        Public ReadOnly Property Now As DateTimeOffset Implements IScheduler.Now
            Get
                Return DateTimeOffset.Now
            End Get
        End Property

        Public Function Schedule(Of TState)(state As TState, action As Func(Of IScheduler, TState, IDisposable)) As IDisposable Implements IScheduler.Schedule
            Dim d = New SingleAssignmentDisposable()
            Dispatcher.UIThread.Post(Sub()
                If Not d.IsDisposed Then
                    d.Disposable = action(Me, state)
                End If
            End Sub)
            Return d
        End Function

        Public Function Schedule(Of TState)(state As TState, dueTime As TimeSpan, action As Func(Of IScheduler, TState, IDisposable)) As IDisposable Implements IScheduler.Schedule
            Dim d = New SingleAssignmentDisposable()
            DispatcherTimer.RunOnce(
                Sub()
                    If Not d.IsDisposed Then
                        d.Disposable = action(Me, state)
                    End If
                End Sub, dueTime)
            Return d
        End Function

        Public Function Schedule(Of TState)(state As TState, dueTime As DateTimeOffset, action As Func(Of IScheduler, TState, IDisposable)) As IDisposable Implements IScheduler.Schedule
            Dim d = New SingleAssignmentDisposable()
            Dim delay = dueTime - Now
            DispatcherTimer.RunOnce(
                Sub()
                    If Not d.IsDisposed Then
                        d.Disposable = action(Me, state)
                    End If
                End Sub, delay)
            Return d
        End Function

    End Class
End Namespace