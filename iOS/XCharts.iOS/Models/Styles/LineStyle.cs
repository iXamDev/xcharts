using System;
using UIKit;
using CoreGraphics;

namespace XCharts.iOS.Models.Styles
{
    public class LineStyle : BaseStyle
    {
        public UIColor Color { get; set; }

        public nfloat LineWidth { get; set; }

        public CGLineCap LineCap { get; set; } = CGLineCap.Round;

        public LineStyle()
        {
            Color = UIColor.DarkGray;

            LineWidth = 1f;
        }
    }
}
