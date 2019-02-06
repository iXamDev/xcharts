using System;
using System.Collections.Generic;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Helpers;
using XCharts.iOS.Models.Renders.Axis;
using XCharts.iOS.Models.Styles;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Implementation.Axises;
using System.Linq;

namespace XCharts.iOS.Implementation.Renders
{
    public class YAxisRender : IYAxisRender
    {
        #region Fields

        #endregion

        #region Delegates

        public delegate string LabelValueFormatter(double position);

        #endregion

        #region Properties

        public IAxisValuesPositionsProvider AxisValuesPositionsProvider { get; set; } = new CountAxisValuesPositionsProvider();

        public LabelValueFormatter ValueFormatter { get; set; } = (entry) => entry.ToString();

        public YAxisStyle Style { get; set; } = new YAxisStyle();

        #endregion

        #region Constructor

        public YAxisRender()
        {
        }

        #endregion

        #region Protected

        protected virtual void DrawLabel(CGContext context, string text, CGPoint position, TextStyle textStyle)
        => DrawHelper.DrawText(context, text, position, textStyle);

        protected virtual void DrawLabels(CGContext context, IViewPort viewPort, IYAxis yAxis, nfloat axisLineStart, nfloat axisLineEnd, nfloat axisXPosition, TextStyle textStyle)
        {
            var positions = AxisValuesPositionsProvider?.ValuePositions(yAxis, viewPort);

            if (!positions?.Any() ?? false)
                return;

            var labels = new List<Tuple<double, string>>();

            foreach (var item in positions)
            {
                var text = ValueFormatter(item);

                if (!string.IsNullOrEmpty(text))
                {
                    labels.Add(new Tuple<double, string>(item, text));
                }
            }

            DrawLabels(context, viewPort, yAxis, axisXPosition, axisLineStart, axisLineEnd, textStyle, labels);
        }

        protected virtual void DrawGrid(CGContext context, IViewPort viewPort, IYAxis yAxis, nfloat axisLineStart, nfloat axisLineEnd, GridStyle gridStyle)
        {
            var Step = gridStyle.Step;

            var visibleSourceRangeStart = yAxis.Min - (yAxis.Min % Step) - Step;

            var visibleSourceRangeEnd = yAxis.Max - (yAxis.Max % Step) + Step;

            var leftAxisX = viewPort.ViewPortRect.Left + gridStyle.Offset.X;

            var rightAxisX = viewPort.ViewPortRect.Right - gridStyle.Offset.X;

            for (double i = visibleSourceRangeStart; i <= visibleSourceRangeEnd; i += Step)
            {
                var y = yAxis.GetContentValue(i);

                var viewPortPositionY = viewPort.DisplayPositionY((nfloat)y) + gridStyle.Offset.Y;

                if (viewPortPositionY >= axisLineStart && viewPortPositionY <= axisLineEnd)
                    DrawHelper.DrawLines(context, new List<CGPoint> { new CGPoint(leftAxisX, viewPortPositionY), new CGPoint(rightAxisX, viewPortPositionY) }, gridStyle);
            }
        }

        #endregion

        #region Public

        public virtual void DrawAxis(CGContext context, IViewPort viewPort, IYAxis yAxis)
        {
            var axisLineStart = viewPort.ViewPortInsets.Top;

            var axisLineEnd = viewPort.ViewPortInsets.Top + viewPort.ViewPortSize.Height;

            if (Style.GridStyle.ShouldDraw)
                DrawGrid(context, viewPort, yAxis, axisLineStart, axisLineEnd, Style.GridStyle);

            if (Style.Position.HasFlag(YAxisPosition.Left))
            {
                var leftAxisX = viewPort.ViewPortRect.Left + Style.AxisDrawOffset;

                if (Style.LineStyle.ShouldDraw)
                    DrawAxisLine(context, new CGPoint(leftAxisX, axisLineStart), new CGPoint(leftAxisX, axisLineEnd), Style.LineStyle);

                if (Style.TextStyle.ShouldDraw)
                    DrawLabels(context, viewPort, yAxis, axisLineStart, axisLineEnd, leftAxisX, Style.TextStyle);
            }

            if (Style.Position.HasFlag(YAxisPosition.Right))
            {
                var rightAxisX = viewPort.ViewPortRect.Right + Style.AxisDrawOffset;

                if (Style.LineStyle.ShouldDraw)
                    DrawAxisLine(context, new CGPoint(rightAxisX, axisLineStart), new CGPoint(rightAxisX, axisLineEnd), Style.LineStyle);

                if (Style.TextStyle.ShouldDraw)
                    DrawLabels(context, viewPort, yAxis, axisLineStart, axisLineEnd, rightAxisX, Style.TextStyle);
            }
        }


        private void DrawLabels(CGContext context, IViewPort viewPort, IYAxis yAxis, nfloat positionX, nfloat axisLineStart, nfloat axisLineEnd, TextStyle textStyle, List<Tuple<double, string>> labels)
        {
            foreach (var label in labels)
            {
                var labelY = yAxis.GetContentValue(label.Item1);

                var viewPortPositionY = viewPort.DisplayPositionY((nfloat)labelY);

                if (viewPortPositionY >= axisLineStart && viewPortPositionY <= axisLineEnd)
                    DrawLabel(context, label.Item2, new CGPoint(positionX, viewPortPositionY), textStyle);
            }
        }

        private void DrawAxisLine(CGContext context, CGPoint axisLineStart, CGPoint axisLineEnd, LineStyle lineStyle)
        => DrawHelper.DrawLines(context, new List<CGPoint>() { axisLineStart, axisLineEnd }, lineStyle);

        #endregion

    }
}