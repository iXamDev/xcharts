using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Handlers.Tap;
using System.Linq;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Abstract.Handlers.Highlight;
using System.Collections.Generic;

namespace XCharts.iOS.Implementation.Handlers.Tap
{
    public class PointTapHandler : IPointTapHandler
    {
        #region Properties

        public double TapDistance { get; set; } = 100;

        public bool Multiselect { get; set; } = false;

        #endregion

        #region Protected

        protected double GetDistance(CGPoint tapLocation, IPointEntry entry, IXAxis xAxis, IYAxis yAxis)
        {
            var entryContentLocationX = xAxis.GetContentValue(entry.X);

            var entryContentLocationY = yAxis.GetContentValue(entry.Y);

            return Math.Sqrt(Math.Pow(entryContentLocationX - tapLocation.X, 2) + Math.Pow(entryContentLocationY - tapLocation.Y, 2));
        }

        #endregion

        #region Public

        public bool Tap(CGPoint tapContentLocation, IChartView chartView, IPointDataSource pointDataSource, IPointHighlightHandler highlightHandler, IXAxis xAxis, IYAxis yAxis)
        {
            double sourcePositionStart = xAxis.GetSourceValue(tapContentLocation.X - TapDistance);

            var sourcePositionEnd = xAxis.GetSourceValue(tapContentLocation.X + TapDistance);

            var entriesInTapRange = pointDataSource
                .GetEntriesInRange(sourcePositionStart, sourcePositionEnd)
                .Select((IPointEntry entry) => new { Entry = entry, Distance = GetDistance(tapContentLocation, entry, xAxis, yAxis) })
                .Where((t) => t.Distance <= TapDistance)
                .OrderBy(p => p.Distance);

            var entryNearestToTap = entriesInTapRange.FirstOrDefault();

            if (entryNearestToTap != null)
                highlightHandler.TapOnEntries(Multiselect ? entriesInTapRange.Select(t => t.Entry) : new List<IPointEntry> { entryNearestToTap.Entry });
            else
                highlightHandler.TapOnNoEntry();

            return entryNearestToTap != null;
        }

        #endregion
    }
}
