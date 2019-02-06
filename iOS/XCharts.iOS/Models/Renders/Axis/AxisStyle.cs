using System;
using XCharts.iOS.Models.Styles;
namespace XCharts.iOS.Models.Renders.Axis
{
    public class AxisStyle
    {
        public LineStyle LineStyle { get; set; }

        public TextStyle TextStyle { get; set; }

        public GridStyle GridStyle { get; set; }

        public float AxisDrawOffset = 0;

        public AxisStyle()
        {
            LineStyle = new LineStyle();

            TextStyle = new TextStyle();

            GridStyle = new GridStyle() { ShouldDraw = false };
        }
    }
}
