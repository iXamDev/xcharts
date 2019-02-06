using System;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;

namespace XCharts.iOS.Implementation.Axises
{
    public abstract class PointRangeProvider : IAxisRangeProvider
    {
        public PointRangeProvider()
        {
        }

        protected Tuple<double, double> GetAxisMinMax(IChartDataSource dataSource)
        {
            double min = 0;
            double max = 0;

            if (dataSource is IPointDataSource pointDataSource)
            {
                min = pointDataSource.MinEntryValue;
                max = pointDataSource.MaxEntryValue;
            }
            else
                Console.WriteLine("OffsetAxisRangeProvider: unknown axis range, dataSource should be IPointDataSource");

            return new Tuple<double, double>(min, max);
        }

        public abstract Tuple<double, double> GetRange(IChartView chartView, IChartDataSource dataSource);
    }
}
