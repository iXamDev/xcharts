using System;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;
using CoreGraphics;
using XCharts.iOS.Abstract.Chart;
using System.Collections.Generic;

namespace XCharts.iOS.Abstract.Managers
{
    public interface IManager
    {
        bool IsReady { get; }

        void AdjustAxisAndContentSize(IEnumerable<IAxis> axises, IViewPort viewPort);

        void DrawPlot(CGContext context, IViewPort viewPort, IEnumerable<IAxis> axises);

        void HandleContentTap(IEnumerable<IAxis> axises, CGPoint contentTapLocation);
    }
}
