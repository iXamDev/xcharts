using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Implementation.Renders
{
    public class BaseRender : IRender
    {
        protected virtual CGPoint GetDrawPosition(double sourceX, double sourceY, IViewPort viewPort, IXAxis xAxis, IYAxis yAxis)
        => viewPort.DisplayPosition(new CGPoint(xAxis.GetContentValue(sourceX), yAxis.GetContentValue(sourceY)));

        protected Tuple<double, double> VisibleSourceXRange(IXAxis xAxis, IViewPort viewPort)
        => new Tuple<double, double>(xAxis.GetSourceValue(viewPort.ViewPortContentOffset.X), xAxis.GetSourceValue(viewPort.ViewPortContentOffset.X + viewPort.ViewPortSize.Width));
    }
}
