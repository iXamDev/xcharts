using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Abstract.Renders
{
    public interface IXAxisRender : IAxisRender
    {
        void DrawAxis(CGContext context, IViewPort viewPort, IXAxis xAxis);
    }
}
