using System;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Abstract.Renders;
namespace XCharts.iOS.Abstract.Axises
{
    public interface IXAxis : IAxis
    {
        IXAxisRender Render { get; }
    }
}
