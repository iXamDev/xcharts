using System;
using CoreGraphics;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Abstract.Axises;

namespace XCharts.iOS.Abstract.Renders
{
    public interface IYAxisRender : IAxisRender
    {
        void DrawAxis(CGContext context, IViewPort viewPort, IYAxis yAxis);
    }
}
