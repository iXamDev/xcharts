using System;
using CoreGraphics;
using XCharts.iOS.Models.Styles;
namespace XCharts.iOS.Extensions
{
    public static class CGExtensions
    {
        public static CGPoint Add(this CGPoint point, CGPoint second)
        => new CGPoint(point.X + second.X, point.Y + second.Y);

        public static CGPoint Subtract(this CGPoint point, CGPoint second)
        => new CGPoint(point.X - second.X, point.Y - second.Y);

        public static CGPoint Multiply(this CGPoint point, double multiplier)
        => new CGPoint(point.X * multiplier, point.Y * multiplier);

        public static CGRect LocateAtPosition(this CGRect rect, CGPoint point, Position position)
        {
            var newLocation = point;

            if (position.HasFlag(Position.Left))
                newLocation.X -= rect.Width;

            if (position.HasFlag(Position.Top))
                newLocation.Y -= rect.Height;

            if (position.HasFlag(Position.CenterHorizontal))
                newLocation.X = point.X - rect.Width / 2;

            if (position.HasFlag(Position.CenterVertical))
                newLocation.Y = point.Y - rect.Height / 2;

            rect.Location = newLocation;

            return rect;
        }
    }
}
