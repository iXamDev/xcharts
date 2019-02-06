using System;
using System.Collections.Generic;
using System.Linq;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Handlers.Highlight;

namespace XCharts.iOS.Implementation.DataSources
{
    public abstract class BaseChartDataSource<TItem> : IHighlightHandler<TItem>, ICartesianDataSource<TItem>
        where TItem : class
    {
        #region Fields

        #endregion

        #region Events

        public event EventHandler DataChanged;

        #endregion

        #region Properties

        public abstract long Count { get; }

        public virtual IEnumerable<TItem> HighlightedEntries { get; protected set; }

        #endregion

        #region Constructor

        public BaseChartDataSource()
        {

        }

        #endregion

        #region Protected

        protected virtual void SetHighlightedEntries(IEnumerable<TItem> entries)
        {
            HighlightedEntries = entries;

            RaiseDataChanged();
        }

        protected virtual void RaiseDataChanged()
        => DataChanged?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Public

        public void TapOnEntries(IEnumerable<TItem> entries)
        {
            SetHighlightedEntries(entries);
        }

        public void TapOnNoEntry()
        {
            SetHighlightedEntries(null);
        }

        public abstract IEnumerable<TItem> GetEntriesInRange(double fromX, double toX);

        #endregion
    }
}
