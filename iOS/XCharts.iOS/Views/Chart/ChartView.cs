using System;
using CoreGraphics;
using XCharts.iOS.Views.Base;
using Foundation;
using System.ComponentModel;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Managers;

namespace XCharts.iOS.Views.Chart
{
    [Register("ChartView"), DesignTimeVisible(true)]
    public class ChartView : BasePointChartView
    {
        #region Fields

        protected bool isScaleActual;

        #endregion

        #region Properties

        private IManager manager;
        public IManager Manager
        {
            get => manager;
            set
            {
                manager = value;
                isScaleActual = false;
                SetNeedsDisplay();
            }
        }

        #endregion

        #region Constructor

        public ChartView() : base()
        {
        }

        public ChartView(IntPtr ptr) : base(ptr)
        {
        }

        public ChartView(CGRect rect) : base(rect)
        {
        }

        public ChartView(NSCoder coder) : base(coder)
        {
        }


        #endregion

        #region Private

        #endregion

        #region Protected

        protected override bool CanDraw(CGRect rect)
        => base.CanDraw(rect) && Manager != null && Manager.IsReady;


        protected override void HandleContentTap(CGPoint contentTapLocation)
        => Manager?.HandleContentTap(Axises, contentTapLocation);

        protected override void ViewSizeChanged(CGSize newSize)
        {
            base.ViewSizeChanged(newSize);

            isScaleActual = false;//todo
        }

        protected override void PrepareToDraw(CGRect rect)
        {
            base.PrepareToDraw(rect);

            if (!isScaleActual)
            {
                isScaleActual = true;

                AdjustScale();
            }
        }

        protected virtual void AdjustScale()
        => Manager.AdjustAxisAndContentSize(Axises, ViewPort);

        protected override void DrawData(CGContext context)
        => Manager.DrawPlot(context, ViewPort, Axises);

        #endregion

        #region Public

        public override void ContentChanged()
        {
            isScaleActual = false;

            SetNeedsDisplay();
        }

        #endregion
    }
}
