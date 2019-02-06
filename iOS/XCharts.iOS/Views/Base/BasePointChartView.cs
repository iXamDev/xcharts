using System;
using CoreGraphics;
using Foundation;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Implementation.Axises;
using System.Collections.Generic;
using XCharts.iOS.Implementation.Renders;

namespace XCharts.iOS.Views.Base
{
    public abstract class BasePointChartView : BaseChartView
    {
        #region Constructor

        protected BasePointChartView()
        {
        }

        protected BasePointChartView(IntPtr ptr) : base(ptr)
        {
        }

        protected BasePointChartView(CGRect rect) : base(rect)
        {
        }

        protected BasePointChartView(NSCoder coder) : base(coder)
        {
        }

        #endregion

        #region Protected

        protected override List<IAxis> CreateAxises()
        => new List<IAxis>
            {
                new XAxis(),
                new YAxis()
            };

        #endregion
    }
}
