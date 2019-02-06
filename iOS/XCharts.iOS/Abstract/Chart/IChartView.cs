using System;
using System.Collections.Generic;
using XCharts.iOS.Abstract.Axises;
using CoreGraphics;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Abstract.Chart
{
    public interface IChartView
    {
        void ContentChanged();

        void Redraw();

        void ScrollTo(CGPoint contentCenter);

        IEnumerable<IAxis> Axises { get; }

        IViewPort ViewPort { get; }
    }
}
