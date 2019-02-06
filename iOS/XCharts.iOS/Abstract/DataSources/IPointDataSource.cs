using System;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Models.Axises;
namespace XCharts.iOS.Abstract.DataSources
{
    public interface IPointDataSource : ICartesianDataSource<IPointEntry>
    {
        double MaxEntryValue { get; }

        double MinEntryValue { get; }
    }
}
