Imports FlashcardApp.Models
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series


Namespace Utils
    Public Class ChartHelper
        Public Shared Function CreateLineChart(stats As List(Of DailyStats)) As PlotModel
            Dim model As New PlotModel With {.Title = "每日抽卡数量"}
            Dim dateAxis As New DateTimeAxis With {
                    .Position = AxisPosition.Bottom,
                    .StringFormat = "MM-dd",
                    .Title = "日期"
                    }
            Dim valueAxis As New LinearAxis With {
                    .Position = AxisPosition.Left,
                    .Title = "抽卡数量"
                    }
            model.Axes.Add(dateAxis)
            model.Axes.Add(valueAxis)

            Dim lineSeries As New LineSeries()
            For Each stat In stats
                lineSeries.Points.Add(New DataPoint(DateTimeAxis.ToDouble(stat.StatsDate), stat.FlashcardCount))
            Next
            model.Series.Add(lineSeries)
            Return model
        End Function
    End Class
End Namespace
