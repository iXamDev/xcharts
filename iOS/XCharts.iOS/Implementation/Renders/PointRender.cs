using System;
using System.Collections.Generic;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Models.Renders.Point;
using System.Linq;
using UIKit;
using XCharts.iOS.Helpers;
using XCharts.iOS.Models.Styles;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Handlers.Highlight;

namespace XCharts.iOS.Implementation.Renders
{
    public class PointRender : BaseRender, IPointRender
    {
        #region Delegates

        public delegate string PointValueFormatter(IPointEntry entry);

        #endregion

        #region Properties

        public PointValueFormatter ValueFormatter { get; set; } = (entry) => entry.Y.ToString("F");

        public PointRenderStyle Style { get; set; } = new PointRenderStyle();

        #endregion

        #region Protected

        protected virtual void DrawLinearLine(CGContext context, List<CGPoint> points, LineStyle lineStyle)
        => DrawHelper.DrawLines(context, points, lineStyle);

        protected virtual void DrawCurveLine(CGContext context, List<CGPoint> points, LineStyle lineStyle, double curvatureIntensity)
         => DrawHelper.DrawCurve(context, points, lineStyle, curvatureIntensity);

        protected virtual void DrawMarker(CGContext context, CGPoint position, MarkerStyle markerStyle)
        => DrawHelper.DrawMarker(context, position, markerStyle);

        protected virtual void DrawEntryValue(CGContext context, IPointEntry pointEntry, CGPoint position, TextStyle textStyle)
        => DrawHelper.DrawText(context, ValueFormatter.Invoke(pointEntry), position, textStyle);

        protected virtual void Draw(CGContext context, IEnumerable<IPointEntry> entries, IEnumerable<IPointEntry> highlightedEntries, IViewPort viewPort, IXAxis xAxis, IYAxis yAxis, PointRenderStyle renderStyle)
        {
            var points = entries
                ?.Select(t => new
                {
                    Point = GetDrawPosition(t.X, t.Y, viewPort, xAxis, yAxis),
                    IsHighlight = highlightedEntries?.Contains(t) ?? false,
                    Entry = t
                })
                .ToList();

            if (points != null)
            {
                context.SaveState();

                context.ClipToRect(viewPort.ViewPortRect);

                if (renderStyle.LineStyle.ShouldDraw)
                    DrawLine(context, points.Select(t => t.Point).ToList(), renderStyle);

                if (renderStyle.MarkerStyle.ShouldDraw)
                    DrawNotHighlightedMarkers(context, points.Where(t => !t.IsHighlight).Select(t => t.Point).ToList(), renderStyle.MarkerStyle);

                if (renderStyle.HighlightMarkerStyle.ShouldDraw)
                    DrawHighlightedMarkers(context, points.Where(t => t.IsHighlight).Select(t => t.Point).ToList(), renderStyle.HighlightMarkerStyle);

                foreach (var point in points)
                    if (ShouldDraw(point.IsHighlight, renderStyle.TextStyle, renderStyle.HighlightTextStyle))
                        DrawEntryValue(context, point.Entry, point.Point, point.IsHighlight ? renderStyle.HighlightTextStyle : renderStyle.TextStyle);

                context.RestoreState();
            }
        }

        protected virtual void DrawHighlightedMarkers(CGContext context, List<CGPoint> points, MarkerStyle markerStyle)
        => DrawMarkers(context, points, markerStyle);

        protected virtual void DrawNotHighlightedMarkers(CGContext context, List<CGPoint> points, MarkerStyle markerStyle)
        => DrawMarkers(context, points, markerStyle);

        protected virtual void DrawMarkers(CGContext context, List<CGPoint> points, MarkerStyle markerStyle)
        {
            foreach (var point in points)
                DrawMarker(context, point, markerStyle);
        }

        protected virtual void DrawLine(CGContext context, List<CGPoint> points, PointRenderStyle renderStyle)
        {
            if (renderStyle.BezierCurve)
                DrawCurveLine(context, points, renderStyle.LineStyle, renderStyle.CurvatureIntensity);
            else
                DrawLinearLine(context, points, renderStyle.LineStyle);
        }

        protected bool ShouldDraw(bool isHighlight, BaseStyle style, BaseStyle highlightStyle)
        => isHighlight ? highlightStyle.ShouldDraw : style.ShouldDraw;

        #endregion

        #region Public

        public void DrawPlot(CGContext context, IPointDataSource dataSource, IEnumerable<IPointEntry> highlightedEntries, IViewPort viewPort, IXAxis xAxis, IYAxis yAxis)
        {
            var visibleSourceRange = VisibleSourceXRange(xAxis, viewPort);

            var visibleSourceRangeStart = Math.Floor(visibleSourceRange.Item1);

            var visibleSourceRangeEnd = Math.Ceiling(visibleSourceRange.Item2);

            if (Style.BezierCurve)
            {//для отображения кривых нужны дополнительные точки для рассчета опорных точек
             //TODO учитывать масштаб
                visibleSourceRangeStart--;
                visibleSourceRangeEnd++;
            }

            var visibleEntries = dataSource.GetEntriesInRange(visibleSourceRangeStart, visibleSourceRangeEnd);

            Draw(context, visibleEntries, highlightedEntries, viewPort, xAxis, yAxis, Style);
        }


        #endregion

    }
}
