using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.ViewPort;
using System.Collections.Generic;

namespace XCharts.iOS.Abstract.Renders
{
    public interface IManualRender<TDataSource> : IRender
    {
        void DrawPlot(CGContext context, TDataSource dataSource, IViewPort viewPort, IEnumerable<IAxis> axises);
    }
}
