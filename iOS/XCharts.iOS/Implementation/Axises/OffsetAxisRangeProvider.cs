using System;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;

namespace XCharts.iOS.Implementation.Axises
{
    public class OffsetAxisRangeProvider : PointRangeProvider
    {
        public double MinOffset { get; set; } = 10D;

        public double MaxOffset { get; set; } = 10D;

        public OffsetAxisRangeProvider()
        {
        }

        public override Tuple<double, double> GetRange(IChartView chartView, IChartDataSource dataSource)
        {
            var axisMinMax = GetAxisMinMax(dataSource);

            return new Tuple<double, double>(axisMinMax.Item1 - MinOffset, axisMinMax.Item2 + MaxOffset);
        }
    }
}
