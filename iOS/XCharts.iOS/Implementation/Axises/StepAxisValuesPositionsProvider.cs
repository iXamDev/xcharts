using System;
using System.Collections.Generic;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Implementation.Axises
{
    public class StepAxisValuesPositionsProvider : IAxisValuesPositionsProvider
    {
        public double Step { get; set; } = 1D;

        public StepAxisValuesPositionsProvider()
        {
        }

        public IEnumerable<double> ValuePositions(IAxis axis, IViewPort viewPort)
        {
            var visibleSourceRangeStart = axis.Min - (axis.Min % Step) - Step;

            var visibleSourceRangeEnd = axis.Max - (axis.Max % Step) + Step;

            var positions = new List<double>();

            for (double i = visibleSourceRangeStart; i <= visibleSourceRangeEnd; i += Step)
            {
                positions.Add(i);
            }

            return positions;
        }
    }
}
