using System;
using System.Collections.Generic;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Entries;
using System.Linq;
using XCharts.iOS.Abstract.Handlers.Highlight;

namespace XCharts.iOS.Implementation.DataSources
{
    public abstract class BasePointChartDataSource<TEntry> : IPointDataSource, IPointHighlightHandler
        where TEntry : IPointEntry
    {
        #region Events

        public event EventHandler HighlightedEntriesChanged;

        public event EventHandler DataChanged;

        #endregion

        #region Properties

        protected abstract IEnumerable<TEntry> PointEntries { get; }

        public virtual double MaxEntryValue => (PointEntries?.Any() ?? false) ? PointEntries.Max(t => t.Y) : 0;

        public virtual double MinEntryValue => (PointEntries?.Any() ?? false) ? PointEntries.Min(t => t.Y) : 0;

        public virtual long Count => PointEntries?.Count() ?? 0;

        public IEnumerable<IPointEntry> HighlightedEntries { get; protected set; }

        #endregion

        #region Constructor

        protected BasePointChartDataSource()
        {
        }

        #endregion

        #region Protected

        protected virtual void RaiseHighlightedEntriesChanged()
        => HighlightedEntriesChanged?.Invoke(this, null);

        protected virtual void RaiseDataChanged()
        => DataChanged?.Invoke(this, null);

        protected virtual void SetHighlightedEntries(IEnumerable<IPointEntry> entries)
        {
            HighlightedEntries = entries;

            RaiseHighlightedEntriesChanged();
        }

        #endregion

        #region Public

        public virtual IEnumerable<IPointEntry> GetEntriesInRange(double fromX, double toX)
        => PointEntries?.Where(p => p.X >= fromX && p.X <= toX).Cast<IPointEntry>();

        public virtual void TapOnNoEntry()
        => SetHighlightedEntries(null);

        public virtual void TapOnEntries(IEnumerable<IPointEntry> entries)
        => SetHighlightedEntries(entries);

        #endregion
    }
}
