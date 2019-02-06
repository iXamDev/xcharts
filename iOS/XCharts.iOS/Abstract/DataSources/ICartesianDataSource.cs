using System.Collections.Generic;
namespace XCharts.iOS.Abstract.DataSources
{
    public interface ICartesianDataSource<TEntry> : IChartDataSource
    {
        IEnumerable<TEntry> GetEntriesInRange(double fromX, double toX);
    }
}
