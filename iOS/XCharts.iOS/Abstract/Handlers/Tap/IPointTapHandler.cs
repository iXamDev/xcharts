using System;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;
using CoreGraphics;
using XCharts.iOS.Abstract.Handlers.Highlight;
using XCharts.iOS.Abstract.Entries;
namespace XCharts.iOS.Abstract.Handlers.Tap
{
    public interface IPointTapHandler
    {
        bool Tap(CGPoint tapContentLocation, IChartView chartView, IPointDataSource pointDataSource, IPointHighlightHandler highlightHandler, IXAxis xAxis, IYAxis yAxis);
    }
}
