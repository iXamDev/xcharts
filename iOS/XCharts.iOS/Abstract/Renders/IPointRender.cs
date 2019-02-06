using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Abstract.DataSources;
using System.Collections.Generic;
using XCharts.iOS.Abstract.Entries;

namespace XCharts.iOS.Abstract.Renders
{
    public interface IPointRender : IRender
    {
        void DrawPlot(CGContext context, IPointDataSource pointDataSource, IEnumerable<IPointEntry> highlightedEntries, IViewPort viewPort, IXAxis xAxis, IYAxis yAxis);
    }
}
