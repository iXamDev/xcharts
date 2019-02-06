using System;
using CoreGraphics;
using UIKit;
using XCharts.iOS.Models.Styles;
using System.Collections.Generic;
using System.Linq;
using XCharts.iOS.Extensions;

namespace XCharts.iOS.Helpers
{
    public static class DrawHelper
    {
        public static void DrawMarker(CGContext context, CGPoint position, MarkerStyle markerStyle)
        {
            markerStyle.Color.SetFill();

            var markerRect = new CGRect(new CGPoint(position.X + markerStyle.Offset.X - markerStyle.Size.Width / 2f, position.Y + markerStyle.Offset.Y - markerStyle.Size.Height / 2f), markerStyle.Size);

            UIGraphics.PushContext(context);

            UIBezierPath path = null;

            switch (markerStyle.Type)
            {
                case MarkerStyle.MarkerType.Oval:
                    path = UIBezierPath.FromOval(markerRect);
                    break;

                case MarkerStyle.MarkerType.Rect:
                    path = UIBezierPath.FromRect(markerRect);
                    break;
            }

            path.Fill();

            UIGraphics.PopContext();
        }

        public static void DrawLines(CGContext context, IEnumerable<CGPoint> points, LineStyle lineStyle)
        {
            if ((points?.Count() ?? 0) < 2)
                return;

            context.SetLineWidth(lineStyle.LineWidth);

            lineStyle.Color.SetStroke();

            var path = new CGPath();

            context.SetLineCap(lineStyle.LineCap);

            path.AddLines(new CGPoint[] { points.ElementAt(0), points.ElementAt(1) });

            for (int i = 2; i < points.Count(); i++)
            {
                path.AddLineToPoint(points.ElementAt(i));
            }

            context.AddPath(path);

            context.DrawPath(CGPathDrawingMode.Stroke);
        }


        public static void DrawText(CGContext context, string text, CGPoint position, TextStyle textStyle)
        {
            if (string.IsNullOrEmpty(text))
                return;

            UIGraphics.PushContext(context);

            textStyle.Color.SetColor();

            var textRect = new CGRect(new CGPoint(), text.StringSize(textStyle.Font));

            position = position.Add(textStyle.Offset);

            textRect = textRect.LocateAtPosition(position, textStyle.Position);

            text.DrawString(textRect, textStyle.Font);

            UIGraphics.PopContext();
        }

        public static void DrawCurve(CGContext context, IEnumerable<CGPoint> points, LineStyle lineStyle, double intensity = 0.15f)
        {
            if ((points?.Count() ?? 0) < 2)
                return;

            var curvePath = new CGPath();

            var pointsCount = points.Count();

            var currentDx = new Func<int, CGPoint>((int arg) => points.ElementAt(arg + 1 < pointsCount ? arg + 1 : arg).Subtract(points.ElementAt(arg - 1)).Multiply(intensity));

            var prevDx = new Func<int, CGPoint>((int arg) => points.ElementAt(arg).Subtract(points.ElementAt(arg - (arg > 1 ? 2 : 1))).Multiply(intensity));

            var curveDirections = points
                .Skip(1)
                .Take(pointsCount - 1)
                .Select((CGPoint point, int index) => new
                {
                    Point = point,
                    Control1 = points.ElementAt(index).Add(prevDx(index + 1)),
                    Control2 = point.Subtract(currentDx(index + 1))
                });

            curvePath.MoveToPoint(points.ElementAt(0));

            foreach (var curve in curveDirections)
            {
                curvePath.AddCurveToPoint(curve.Control1, curve.Control2, curve.Point);
            }

            context.AddPath(curvePath);

            context.SetLineWidth(lineStyle.LineWidth);

            context.SetStrokeColor(lineStyle.Color.CGColor);

            context.StrokePath();
        }
        public static void DrawFilledCurvePath(CGContext context, IEnumerable<CGPoint> points, IEnumerable<CGPoint> pointsBottom, UIColor fillColor, double intensity = 0.15f, bool reverse = true)
        {
            var pointsBottomReversed = reverse ? pointsBottom.Reverse() : pointsBottom;

            if ((points?.Count() ?? 0) < 2)
                return;

            var fillPath = new CGPath();

            var pointsCount = points.Count();

            var currentDx = new Func<int, IEnumerable<CGPoint>, CGPoint>((int arg, IEnumerable<CGPoint> p) => p.ElementAt(arg + 1 < p.Count() ? arg + 1 : arg).Subtract(p.ElementAt(arg - 1)).Multiply(intensity));

            var prevDx = new Func<int, IEnumerable<CGPoint>, CGPoint>((int arg, IEnumerable<CGPoint> p) => p.ElementAt(arg).Subtract(p.ElementAt(arg - (arg > 1 ? 2 : 1))).Multiply(intensity));

            var curveDirections = points
                .Skip(1)
                .Take(pointsCount - 1)
                .Select((CGPoint point, int index) => new
                {
                    Point = point,
                    Control1 = points.ElementAt(index).Add(prevDx(index + 1, points)),
                    Control2 = point.Subtract(currentDx(index + 1, points))
                });

            fillPath.MoveToPoint(points.ElementAt(0));

            foreach (var curve in curveDirections)
            {
                fillPath.AddCurveToPoint(curve.Control1, curve.Control2, curve.Point);
            }

            pointsCount = pointsBottomReversed.Count();

            fillPath.AddLineToPoint(pointsBottomReversed.ElementAt(0));

            var curveDirectionsSecond = pointsBottomReversed
               .Skip(1)
               .Take(pointsCount - 1)
               .Select((CGPoint point, int index) => new
               {
                   Point = point,
                   Control1 = pointsBottomReversed.ElementAt(index).Add(prevDx(index + 1, pointsBottomReversed)),
                   Control2 = point.Subtract(currentDx(index + 1, pointsBottomReversed))
               });

            foreach (var curve in curveDirectionsSecond)
            {
                fillPath.AddCurveToPoint(curve.Control1, curve.Control2, curve.Point);
            }

            fillPath.CloseSubpath();

            context.AddPath(fillPath);

            fillColor.SetFill();

            context.FillPath();
        }
        public static void DrawFilledPath(CGContext context, IEnumerable<CGPoint> points, IEnumerable<CGPoint> pointsBottom, UIColor fillColor, bool reverse = true)
        {
            var pointsBottomReversed = reverse ? pointsBottom.Reverse() : pointsBottom;

            if ((points?.Count() ?? 0) < 2)
                return;

            var fillPath = new CGPath();

            fillPath.MoveToPoint(points.ElementAt(0));

            var linePoints = points.Skip(1).AsEnumerable();

            foreach (var point in linePoints)
                fillPath.AddLineToPoint(point);


            fillPath.AddLineToPoint(pointsBottomReversed.ElementAt(0));

            var bottomPoints = pointsBottomReversed.Skip(1).AsEnumerable();

            foreach (var point in bottomPoints)
                fillPath.AddLineToPoint(point);

            fillPath.CloseSubpath();

            context.AddPath(fillPath);

            fillColor.SetFill();

            context.FillPath();
        }
    }
}
