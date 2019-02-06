using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Implementation.Renders;

namespace XCharts.iOS.Implementation.Axises
{
    public class XAxis : BaseAxis, IXAxis
    {
        protected double multiplier;

        public IXAxisRender Render { get; set; }

        public XAxis()
        {
            Render = new XAxisRender();
        }

        public override void Adjust(IViewPort viewPort)
        {
            multiplier = GetMultiplier(viewPort.ContentSize.Width - 2 * AxisZeroOffset);

            if (multiplier < 0)
                multiplier = 1;
        }

        public override void Draw(CGContext context, IViewPort viewPort)
        {
            Render?.DrawAxis(context, viewPort, this);
        }

        public override double GetContentValue(double sourceValue)
        => AxisZeroOffset + sourceValue * multiplier;

        public override double GetSourceValue(double contentValue)
        => multiplier != 0 ? (contentValue - AxisZeroOffset) / multiplier : 0;

        public void SetAxisZeroOffset(double offset)
        => AxisZeroOffset = offset;
    }
}
