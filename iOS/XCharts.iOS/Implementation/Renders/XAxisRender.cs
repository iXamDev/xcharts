using System;
using System.Collections.Generic;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Helpers;
using XCharts.iOS.Models.Renders.Axis;
using XCharts.iOS.Models.Styles;

namespace XCharts.iOS.Implementation.Renders
{
    public class XAxisRender : IXAxisRender
    {
        #region Fields

        #endregion

        #region Delegates

        public delegate string LabelValueFormatter(double position);

        #endregion

        #region Properties

        public double Step { get; set; } = 1;

        public LabelValueFormatter ValueFormatter { get; set; } = (entry) => entry.ToString();

        public XAxisStyle Style { get; set; } = new XAxisStyle();

        #endregion

        #region Protected

        protected virtual void DrawLabel(CGContext context, string text, CGPoint position, TextStyle textStyle)
        => DrawHelper.DrawText(context, text, position, textStyle);

        #endregion

        #region Public

        public virtual void DrawAxis(CGContext context, IViewPort viewPort, IXAxis xAxis)
        {
            var axisLineStart = viewPort.ViewPortInsets.Left;

            var axisLineEnd = viewPort.ViewPortInsets.Left + viewPort.ViewPortSize.Width;

            if (Style.Position.HasFlag(XAxisPosition.Top))
            {
                var topLineY = viewPort.ViewPortRect.Top + Style.AxisDrawOffset;

                if (Style.LineStyle.ShouldDraw)
                    DrawAxisLine(context, new CGPoint(axisLineStart, topLineY), new CGPoint(axisLineEnd, topLineY), Style.LineStyle);

                if (Style.TextStyle.ShouldDraw)
                    DrawLabels(context, viewPort, xAxis, axisLineStart, axisLineEnd, topLineY, Style.TextStyle);
            }

            if (Style.Position.HasFlag(XAxisPosition.Bottom))
            {
                var bottomLineY = viewPort.ViewPortRect.Bottom + Style.AxisDrawOffset;

                if (Style.LineStyle.ShouldDraw)
                    DrawAxisLine(context, new CGPoint(axisLineStart, bottomLineY), new CGPoint(axisLineEnd, bottomLineY), Style.LineStyle);

                if (Style.TextStyle.ShouldDraw)
                    DrawLabels(context, viewPort, xAxis, axisLineStart, axisLineEnd, bottomLineY, Style.TextStyle);
            }
        }

        protected virtual void DrawLabels(CGContext context, IViewPort viewPort, IXAxis xAxis, nfloat axisLineStart, nfloat axisLineEnd, nfloat axisYPosition, TextStyle textStyle)
        {
            var visibleSourceRangeStart = Math.Floor(xAxis.GetSourceValue(viewPort.ViewPortContentOffset.X));

            var visibleSourceRangeEnd = Math.Ceiling(xAxis.GetSourceValue(viewPort.ViewPortContentOffset.X + viewPort.ViewPortSize.Width));

            var labels = new List<Tuple<double, string>>();

            for (double i = visibleSourceRangeStart; i <= visibleSourceRangeEnd; i += Step)
            {
                var text = ValueFormatter(i);

                if (!string.IsNullOrEmpty(text))
                {
                    labels.Add(new Tuple<double, string>(i, text));
                }
            }

            DrawLabels(context, viewPort, xAxis, axisYPosition, axisLineStart, axisLineEnd, textStyle, labels);
        }

        private void DrawLabels(CGContext context, IViewPort viewPort, IXAxis xAxis, nfloat positionY, nfloat axisLineStart, nfloat axisLineEnd, TextStyle textStyle, List<Tuple<double, string>> labels)
        {
            foreach (var label in labels)
            {
                var labelX = xAxis.GetContentValue(label.Item1);

                var viewPortPositionX = viewPort.DisplayPositionX((nfloat)labelX);

                if (viewPortPositionX >= axisLineStart && viewPortPositionX <= axisLineEnd)
                    DrawLabel(context, label.Item2, new CGPoint(viewPortPositionX, positionY), textStyle);
            }
        }

        private void DrawAxisLine(CGContext context, CGPoint axisLineStart, CGPoint axisLineEnd, LineStyle lineStyle)
        => DrawHelper.DrawLines(context, new List<CGPoint>() { axisLineStart, axisLineEnd }, lineStyle);

        #endregion

    }
}