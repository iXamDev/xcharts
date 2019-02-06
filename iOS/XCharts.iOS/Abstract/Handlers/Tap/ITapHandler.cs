using System;
using System.Collections.Generic;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;

namespace XCharts.iOS.Abstract.Handlers.Tap
{
    public interface ITapHandler<TDataSource>
    {
        void HandleContentTap(IEnumerable<IAxis> axises, CGPoint contentPosition, TDataSource dataSource);
    }
}
