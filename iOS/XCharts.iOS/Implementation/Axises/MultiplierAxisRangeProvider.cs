using System;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;

namespace XCharts.iOS.Implementation.Axises
{
    public class MultiplierAxisRangeProvider : PointRangeProvider
    {
        public double MinMultiplier { get; set; } = 1D;

        public double MaxMultiplier { get; set; } = 1D;

        public MultiplierAxisRangeProvider()
        {
        }

        public override Tuple<double, double> GetRange(IChartView chartView, IChartDataSource dataSource)
        {
            var axisMinMax = GetAxisMinMax(dataSource);

            return new Tuple<double, double>(axisMinMax.Item1 * MinMultiplier, axisMinMax.Item2 * MaxMultiplier);
        }
    }
}
