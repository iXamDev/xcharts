using System;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;

namespace XCharts.iOS.Implementation.Axises
{
    public class EntriesCountAxisRangeProvider : IAxisRangeProvider
    {
        public Tuple<double, double> GetRange(IChartView chartView, IChartDataSource dataSource)
        {
            var itemsCount = dataSource?.Count ?? 0;

            return new Tuple<double, double>(0, itemsCount - 1 > 0 ? itemsCount - 1 : itemsCount);
        }
    }
}
