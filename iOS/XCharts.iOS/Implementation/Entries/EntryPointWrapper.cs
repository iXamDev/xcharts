using System;
namespace XCharts.iOS.Implementation.Entries
{
    public class EntryPointWrapper<TEntry> : PointEntry
    {
        public TEntry Entry { get; }

        public EntryPointWrapper(TEntry entry)
        {
            Entry = entry;
        }
    }
}
