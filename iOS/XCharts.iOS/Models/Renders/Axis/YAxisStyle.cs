using System;
namespace XCharts.iOS.Models.Renders.Axis
{
    public class YAxisStyle : AxisStyle
    {
        public YAxisPosition Position { get; set; } = YAxisPosition.Left;

        public YAxisStyle()
        {
            TextStyle.Position = Styles.Position.CenterVertical | Styles.Position.Left;

            TextStyle.Offset = new CoreGraphics.CGPoint(0, -1);
        }
    }
}
