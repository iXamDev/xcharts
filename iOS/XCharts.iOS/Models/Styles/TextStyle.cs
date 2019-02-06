using System;
using UIKit;
using CoreGraphics;
namespace XCharts.iOS.Models.Styles
{
    public class TextStyle : BaseStyle
    {
        public UIColor Color { get; set; }

        public CGPoint Offset { get; set; }

        public UIFont Font { get; set; }

        public Position Position { get; set; }

        public TextStyle()
        {
            Color = UIColor.Black;

            Offset = new CGPoint(0, -5);

            Position = Position.Top | Position.CenterHorizontal;

            Font = UIFont.SystemFontOfSize(10f, UIFontWeight.Regular);
        }
    }
}
