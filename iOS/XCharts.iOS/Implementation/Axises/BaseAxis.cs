using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.ViewPort;

namespace XCharts.iOS.Implementation.Axises
{
    public abstract class BaseAxis : IAxis
    {
        #region Properties

        public virtual double AxisLength => Math.Abs(Max - Min);

        public virtual double Min { get; protected set; }

        public virtual double Max { get; protected set; }

        public virtual double AxisZeroOffset { get; protected set; }

        #endregion

        #region Protected

        protected double GetMultiplier(double viewAxisLength)
        => AxisLength != 0 ? viewAxisLength / AxisLength : 0;

        #endregion

        #region Public

        public abstract void Adjust(IViewPort viewPort);

        public virtual void SetRange(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public abstract double GetContentValue(double sourceValue);

        public abstract double GetSourceValue(double contentValue);

        public abstract void Draw(CGContext context, IViewPort ViewPort);

        #endregion
    }
}
