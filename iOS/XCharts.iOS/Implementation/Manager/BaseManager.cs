using System;
using System.Collections.Generic;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Managers;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Implementation.Manager
{
    public abstract class BaseManager : IManager
    {
        public abstract bool IsReady { get; }

        protected abstract void SetupAxisRanges(IEnumerable<IAxis> axises);

        protected abstract void AdjustContentSize(IEnumerable<IAxis> axises, IViewPort viewPort);

        protected virtual void AdjustAxises(IEnumerable<IAxis> axises, IViewPort viewPort)
        {
            foreach (var item in axises)
                item.Adjust(viewPort);
        }

        public abstract void DrawPlot(CGContext context, IViewPort viewPort, IEnumerable<IAxis> axises);

        public void AdjustAxisAndContentSize(IEnumerable<IAxis> axises, IViewPort viewPort)
        {
            SetupAxisRanges(axises);

            AdjustContentSize(axises, viewPort);

            AdjustAxises(axises, viewPort);
        }

        public abstract void HandleContentTap(IEnumerable<IAxis> axises, CGPoint contentTapLocation);
    }
}
