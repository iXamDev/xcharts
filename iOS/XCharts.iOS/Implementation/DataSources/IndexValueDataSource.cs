using System;
using XCharts.iOS.Abstract.Entries;
using System.Collections.Generic;
using System.Linq;
using XCharts.iOS.Implementation.Entries;
namespace XCharts.iOS.Implementation.DataSources
{
    public class IndexValueDataSource : BasePointChartDataSource<IPointEntry>
    {
        protected IEnumerable<IPointEntry> pointEntries;
        protected override IEnumerable<IPointEntry> PointEntries => pointEntries;

        public IndexValueDataSource(IEnumerable<double> values)
        {
            pointEntries = values.Select(CreatePointEntry).ToList();
        }

        protected virtual IPointEntry CreatePointEntry(double arg, int index)
        => new PointEntry() { X = index, Y = arg };
    }
}
