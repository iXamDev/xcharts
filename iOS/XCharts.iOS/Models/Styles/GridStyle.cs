using System;
using CoreGraphics;

namespace XCharts.iOS.Models.Styles
{
    public class GridStyle : LineStyle
    {
        public CGPoint Offset { get; set; } = new CGPoint();

        public int Step { get; set; } = 10;
    }
}
