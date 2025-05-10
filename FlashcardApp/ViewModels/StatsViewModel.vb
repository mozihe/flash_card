Imports ReactiveUI
Imports FlashcardApp.Services
Imports System.Reactive
Imports System.Reactive.Concurrency
Imports FlashcardApp.Utils
Imports LiveChartsCore
Imports LiveChartsCore.SkiaSharpView
Imports System.Collections.Generic
Imports System.Linq

Namespace ViewModels
    Public Class StatsViewModel
        Inherits ViewModelBase

        ' 私有字段
        Private _series As ISeries()
        Private _xAxes As Axis()
        Private _yAxes As Axis()
        Private _feedbackMessage As String
        Private mainWindowViewModel As MainWindowViewModel

        ' 命令的支持字段
        Private ReadOnly _backToMainCommand As ReactiveCommand(Of Unit, Unit)

        ' 公共属性
        Public Property Series As ISeries()
            Get
                Return _series
            End Get
            Set(value As ISeries())
                Me.RaiseAndSetIfChanged(_series, value)
            End Set
        End Property

        Public Property XAxes As Axis()
            Get
                Return _xAxes
            End Get
            Set(value As Axis())
                Me.RaiseAndSetIfChanged(_xAxes, value)
            End Set
        End Property

        Public Property YAxes As Axis()
            Get
                Return _yAxes
            End Get
            Set(value As Axis())
                Me.RaiseAndSetIfChanged(_yAxes, value)
            End Set
        End Property

        Public Property FeedbackMessage As String
            Get
                Return _feedbackMessage
            End Get
            Set(value As String)
                Me.RaiseAndSetIfChanged(_feedbackMessage, value)
            End Set
        End Property

        ' 命令属性
        Public ReadOnly Property BackToMainCommand As ReactiveCommand(Of Unit, Unit)
            Get
                Return _backToMainCommand
            End Get
        End Property

        ' 构造函数
        Public Sub New(mainWindowViewModel As MainWindowViewModel)
            Me.mainWindowViewModel = mainWindowViewModel

            ' 初始化命令，指定执行和通知调度器为 AvaloniaScheduler
            _backToMainCommand = ReactiveCommand.Create(AddressOf BackToMain, outputScheduler:=AvaloniaScheduler.Instance)

            LoadStats()
        End Sub

        ' 加载统计数据方法
        Private Sub LoadStats()
            Try
                Dim stats = StatsService.GetRecentStats(MainWindowViewModel.CurrentUser.Id, 7)

                ' 如果 stats 为空或没有数据，初始化空的 Series 和 Axes
                If stats Is Nothing OrElse stats.Count = 0 Then
                    Series = New ISeries() {}
                    XAxes = New Axis() {
                        New Axis With {
                            .Name = "日期",
                            .Labels = New String() {}
                        }
                    }
                    YAxes = New Axis() {
                        New Axis With {
                            .Name = "学习次数"
                        }
                    }
                    FeedbackMessage = "暂无统计数据。"
                    Return
                End If

                ' 准备数据
                Dim dates = stats.Select(Function(s) s.StatsDate.ToString("MM-dd")).ToArray()
                Dim values = stats.Select(Function(s) CDbl(s.flashcardCount)).ToArray()

                ' 创建图表系列
                Series = New ISeries() {
                    New ColumnSeries(Of Double) With {
                        .Values = values,
                        .Name = "学习次数"
                    }
                }

                ' 设置 X 轴的标签
                XAxes = New Axis() {
                    New Axis With {
                        .Labels = dates,
                        .Name = "日期"
                    }
                }

                ' 设置 Y 轴
                YAxes = New Axis() {
                    New Axis With {
                        .Name = "学习次数"
                    }
                }
            Catch ex As Exception
                FeedbackMessage = "加载统计数据时发生错误：" & ex.Message
            End Try
        End Sub

        ' 返回主界面方法
        Private Sub BackToMain()
            mainWindowViewModel.ShowStudyFlashcardView()
        End Sub
    End Class
End Namespace
