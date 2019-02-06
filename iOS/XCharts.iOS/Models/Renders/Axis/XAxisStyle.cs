using System;
namespace XCharts.iOS.Models.Renders.Axis
{
    public class XAxisStyle : AxisStyle
    {
        public XAxisPosition Position { get; set; } = XAxisPosition.Bottom;

        public XAxisStyle()
        {
            TextStyle.Position = Styles.Position.Bottom | Styles.Position.CenterHorizontal;

            TextStyle.Offset = new CoreGraphics.CGPoint(0, 0);
        }
    }
}
