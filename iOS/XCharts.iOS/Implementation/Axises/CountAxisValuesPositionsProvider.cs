using System;
using System.Collections.Generic;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Implementation.Axises
{
    public class CountAxisValuesPositionsProvider : IAxisValuesPositionsProvider
    {
        public double Count { get; set; } = 10D;

        public double? Mod { get; set; } = null;

        public bool RoundStep { get; set; } = true;

        public IEnumerable<double> ValuePositions(IAxis axis, IViewPort viewPort)
        {
            var axisLength = axis.Max - axis.Min;

            if (axisLength == 0)
                return new List<double> { 0D };

            var step = axisLength / Count;

            var start = axis.Min;

            if (RoundStep)
            {
                var mod = Mod ?? GetMod(step);

                if (step > mod && step % mod != 0)
                    step -= step % mod;

                if (axis.Min % mod != 0)
                    start -= axis.Min % mod + mod;
            }

            var positions = new List<double>();

            for (double i = start; i <= axis.Max; i += step)
                positions.Add(i);

            return positions;
        }

        private static double GetMod(double step)
        {
            var mod = Math.Pow(10, Math.Round(Math.Log10(step)));

            if (step < mod)
                mod /= 10;

            return mod;
        }
    }
}