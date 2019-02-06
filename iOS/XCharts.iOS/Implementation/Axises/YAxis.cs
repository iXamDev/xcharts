using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Implementation.Renders;

namespace XCharts.iOS.Implementation.Axises
{
    public class YAxis : BaseAxis, IYAxis
    {
        protected double multiplier;

        public IYAxisRender Render { get; set; }

        public YAxis()
        {
            Render = new YAxisRender();
        }

        public override void Adjust(IViewPort viewPort)
        {
            multiplier = GetMultiplier(viewPort.ViewPortSize.Height);

            AxisZeroOffset = Max * multiplier;
        }

        public override void Draw(CGContext context, IViewPort ViewPort)
        => Render?.DrawAxis(context, ViewPort, this);

        public override double GetContentValue(double sourceValue)
        => AxisZeroOffset - sourceValue * multiplier;

        public override double GetSourceValue(double contentValue)
        => multiplier != 0 ? (contentValue - AxisZeroOffset) / multiplier : 0;
    }
}
