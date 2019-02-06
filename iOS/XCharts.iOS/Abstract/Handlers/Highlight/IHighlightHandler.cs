using System;
using System.Collections.Generic;
namespace XCharts.iOS.Abstract.Handlers.Highlight
{
    public interface IHighlightHandler<TEntry>
    {
        IEnumerable<TEntry> HighlightedEntries { get; }

        void TapOnEntries(IEnumerable<TEntry> entries);

        void TapOnNoEntry();
    }
}
