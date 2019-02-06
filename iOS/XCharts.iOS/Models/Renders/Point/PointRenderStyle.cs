using System;
using UIKit;
using XCharts.iOS.Models.Styles;
namespace XCharts.iOS.Models.Renders.Point
{
    public class PointRenderStyle
    {
        public LineStyle LineStyle { get; set; }

        public bool BezierCurve { get; set; } = true;

        public double CurvatureIntensity { get; set; } = 0.15f;

        public MarkerStyle MarkerStyle { get; set; }

        public TextStyle TextStyle { get; set; }

        public MarkerStyle HighlightMarkerStyle { get; set; }

        public TextStyle HighlightTextStyle { get; set; }

        public PointRenderStyle()
        {
            LineStyle = new LineStyle();

            MarkerStyle = new MarkerStyle();

            TextStyle = new TextStyle();

            HighlightMarkerStyle = new MarkerStyle()
            {
                Color = UIColor.Red,
                Size = new CoreGraphics.CGSize(5, 5)
            };

            HighlightTextStyle = new TextStyle()
            {
                Color = UIColor.Red,
                Font = UIFont.SystemFontOfSize(12f, UIFontWeight.Regular)
            };
        }
    }
}
