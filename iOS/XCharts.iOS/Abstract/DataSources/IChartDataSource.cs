using System;
using System.Collections.Generic;
namespace XCharts.iOS.Abstract.DataSources
{
    public interface IChartDataSource
    {
        long Count { get; }

        event EventHandler DataChanged;
    }
}
