using System;
using System.Collections.Generic;
using XCharts.iOS.Abstract.ViewPort;
namespace XCharts.iOS.Abstract.Axises
{
    public interface IAxisValuesPositionsProvider
    {
        IEnumerable<double> ValuePositions(IAxis axis, IViewPort viewPort);
    }
}
