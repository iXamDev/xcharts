using System;
using System.Collections.Generic;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using System.Collections;
using XCharts.iOS.Abstract.Handlers.Tap;
using XCharts.iOS.Abstract.DataSources;
using System.Linq;

namespace XCharts.iOS.Implementation.Handlers.Tap
{
    public abstract class BaseTapHandler<TItem, TDataSource> : ITapHandler<TDataSource>
      where TDataSource : ICartesianDataSource<TItem>
    {


        #region Properties

        public double TapDistance { get; set; } = 100;

        public bool Multiselect { get; set; } = false;

        #endregion

        protected BaseTapHandler()
        {

        }

        #region Protected

        protected abstract double DistanceToItem(IEnumerable<IAxis> axises, CGPoint contentPosition, TItem item, TDataSource dataSource);

        protected abstract void TapOnNoEntry(TDataSource dataSource);

        protected abstract void TapOnEntries(IEnumerable<TItem> enumerable, TDataSource dataSource);

        protected double GetDistance(CGPoint point1, CGPoint point2)
        => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));

        protected virtual IXAxis GetXAxis(IEnumerable<IAxis> axises)
        => axises?.OfType<IXAxis>().FirstOrDefault();

        #endregion

        #region Public

        public void HandleContentTap(IEnumerable<IAxis> axises, CGPoint contentPosition, TDataSource dataSource)
        {
            var xAxis = GetXAxis(axises);

            double sourcePositionStart = xAxis.GetSourceValue(contentPosition.X - TapDistance);

            double sourcePositionEnd = xAxis.GetSourceValue(contentPosition.X + TapDistance);

            var entriesInTapRange = dataSource
                .GetEntriesInRange(sourcePositionStart, sourcePositionEnd)
                .Select((entry) => new { Entry = entry, Distance = DistanceToItem(axises, contentPosition, entry, dataSource) })
                .Where((t) => t.Distance <= TapDistance)
                .OrderBy(p => p.Distance);

            var entryNearestToTap = entriesInTapRange.FirstOrDefault();

            if (entryNearestToTap != null)
                TapOnEntries(Multiselect ? entriesInTapRange.Select(t => t.Entry) : new List<TItem> { entryNearestToTap.Entry }, dataSource);
            else
                TapOnNoEntry(dataSource);
        }

        #endregion
    }
}