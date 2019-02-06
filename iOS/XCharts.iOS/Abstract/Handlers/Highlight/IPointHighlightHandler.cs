using System;
using XCharts.iOS.Abstract.Entries;
namespace XCharts.iOS.Abstract.Handlers.Highlight
{
    public interface IPointHighlightHandler : IHighlightHandler<IPointEntry>
    {
        event EventHandler HighlightedEntriesChanged;
    }
}
