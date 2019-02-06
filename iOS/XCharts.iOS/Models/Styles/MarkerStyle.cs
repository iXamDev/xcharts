using System;
using UIKit;
using CoreGraphics;
namespace XCharts.iOS.Models.Styles
{
    public class MarkerStyle : BaseStyle
    {
        public enum MarkerType
        {
            Oval,
            Rect
        }

        public MarkerType Type { get; set; }

        public UIColor Color { get; set; }

        public CGPoint Offset { get; set; }

        public CGSize Size { get; set; }

        public MarkerStyle()
        {
            Color = UIColor.Black;

            Offset = new CGPoint();

            Size = new CGSize(3, 3);
        }
    }
}
