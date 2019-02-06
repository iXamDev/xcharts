using System;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;
namespace XCharts.iOS.Abstract.Axises
{
    public interface IAxisRangeProvider
    {
        Tuple<double, double> GetRange(IChartView chartView, IChartDataSource dataSource);
    }
}
