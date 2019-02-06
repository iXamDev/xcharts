using System;
using System.Linq;
using CoreGraphics;
using UIKit;
using XCharts.iOS.Abstract.ViewPort;
namespace XCharts.iOS.Helpers
{
    public static class PerfomanceHelper
    {
        private static int GridSpace = 100;

        private static int CircleSize = 10;

        private static UIColor[] colors = new UIColor[10000];

        static PerfomanceHelper()
        {
            var r = new Random();

            for (int i = 0; i < colors.Count(); i++)
            {
                colors[i] = UIColor.FromRGB(r.Next(255), r.Next(255), r.Next(255));
            }
        }

        public static void DrawGrid(IViewPort viewPort)
        {
            var colsStart = (int)Math.Floor(viewPort.ViewPortContentOffset.X / GridSpace);

            var colsEnd = (int)Math.Ceiling((viewPort.ViewPortContentOffset.X + viewPort.ViewPortSize.Width) / GridSpace);

            for (int i = colsStart; i < colsEnd; i++)
            {
                DrawCol(viewPort, colors.ElementAt(i % 10000), i);
            }
        }

        private static void DrawCol(IViewPort viewPort, UIColor color, int collnumer)
        {
            color.SetFill();

            var x = collnumer * GridSpace;

            var rowStart = (int)Math.Floor(viewPort.ViewPortContentOffset.Y / GridSpace);

            var rowEnd = (int)Math.Ceiling((viewPort.ViewPortContentOffset.Y + viewPort.ViewPortSize.Height) / GridSpace);

            int y = 0;

            for (int i = rowStart; i < rowEnd; i++)
            {
                y = i * GridSpace;

                DrawPoint(viewPort, x, y);
            }
        }

        private static void DrawPoint(IViewPort viewPort, int x, int y)
        {
            var pointRect = new CGRect(viewPort.DisplayPosition(new CGPoint(x, y)), new CGSize(CircleSize, CircleSize));

            var path = UIBezierPath.FromOval(pointRect);
            path.Fill();

        }
    }
}
