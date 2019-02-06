using System;
using System.Collections.Generic;
using System.Linq;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Implementation.Entries;

namespace XCharts.iOS.Implementation.DataSources
{
    public class PointEntryChartDataSource<TDataEntry> : BasePointChartDataSource<EntryPointWrapper<TDataEntry>>
    {
        public delegate double DataEntryValueProvider(TDataEntry entry);

        protected List<EntryPointWrapper<TDataEntry>> dataEntryWrappers;

        protected override IEnumerable<EntryPointWrapper<TDataEntry>> PointEntries => dataEntryWrappers;

        protected DataEntryValueProvider ValueProvider { get; }

        public
        PointEntryChartDataSource(DataEntryValueProvider valueProvider)
        {
            ValueProvider = valueProvider;
        }

        protected virtual EntryPointWrapper<TDataEntry> WrapDataEntry(TDataEntry dataEntry, int index)
        => new EntryPointWrapper<TDataEntry>(dataEntry)
        {
            X = index,
            Y = ValueProvider(dataEntry)
        };

        protected virtual void ResetHighlight()
        => SetHighlightedEntries(null);

        public virtual void SetItems(IEnumerable<TDataEntry> dataEntries, bool resetHighlight = true)
        {
            dataEntryWrappers = dataEntries?.Select(WrapDataEntry).ToList();

            if (resetHighlight)
                ResetHighlight();

            //RaiseDataChanged();
        }
    }
}
