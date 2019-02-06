using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Axises;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.Managers;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.DataSources;
using System.Collections.Generic;
using XCharts.iOS.Abstract.Handlers.Tap;

namespace XCharts.iOS.Implementation.Manager
{
    public class DescreteManager<TDataSource, TRender> : BaseManager
        where TRender : class, IManualRender<TDataSource>
        where TDataSource : class
    {
        #region Fields

        protected ContentSizeProviderDelegate contentSizeProvider;

        protected WeakReference<IChartView> chartViewWeakReference;

        #endregion

        #region Delegates

        public delegate CGSize ContentSizeProviderDelegate(TDataSource dataSource, IViewPort viewPort, IEnumerable<IAxis> axises);

        public delegate Tuple<double, double> AxisRangeProviderDelegate(TDataSource dataSource);

        #endregion

        #region Properties

        public ITapHandler<TDataSource> TapHandler { get; set; }

        protected IChartView ChartView => chartViewWeakReference.TryGetTarget(out IChartView chartView) ? chartView : null;

        public Dictionary<IAxis, AxisRangeProviderDelegate> AxisesRangeProviders;

        public override bool IsReady => Render != null && DataSource != null;

        private TDataSource dataSource;
        public TDataSource DataSource
        {
            get => dataSource;
            set
            {
                SubscribeToDataSource(dataSource, false);

                dataSource = value;

                SubscribeToDataSource(dataSource, true);
            }
        }

        private TRender render;
        public TRender Render
        {
            get => render;
            set
            {
                render = value;

            }
        }

        #endregion

        #region Constructor

        public DescreteManager(IChartView chartView, ContentSizeProviderDelegate contentSizeProvider)
        {
            this.contentSizeProvider = contentSizeProvider;

            AxisesRangeProviders = new Dictionary<IAxis, AxisRangeProviderDelegate>();

            chartViewWeakReference = new WeakReference<IChartView>(chartView);
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected void SetupAxisRange(IAxis axis, AxisRangeProviderDelegate provider)
        {
            var range = provider(DataSource);

            axis.SetRange(range.Item1, range.Item2);
        }


        protected virtual void SubscribeToDataSource(TDataSource dataSource, bool subscribe)
        {
            if (dataSource is IChartDataSource chartDataSource)
            {
                if (subscribe)
                    chartDataSource.DataChanged += OnDataChaged;
                else
                    chartDataSource.DataChanged -= OnDataChaged;
            }
        }

        protected virtual void OnDataChaged(object sender, EventArgs e)
        {
            ChartView?.ContentChanged();
        }

        protected override void SetupAxisRanges(IEnumerable<IAxis> axises)
        {
            if (AxisesRangeProviders != null)
                foreach (var axis in axises)
                {
                    if (AxisesRangeProviders.ContainsKey(axis))
                        SetupAxisRange(axis, AxisesRangeProviders[axis]);
                }
        }

        protected override void AdjustContentSize(IEnumerable<IAxis> axises, IViewPort viewPort)
        {
            var intrinsicContentSize = contentSizeProvider(DataSource, viewPort, axises);

            if (intrinsicContentSize != viewPort.ContentSize)
                viewPort.ChangeContentSize(intrinsicContentSize.Width, intrinsicContentSize.Height);
        }

        #endregion

        #region Public

        public override void DrawPlot(CGContext context, IViewPort viewPort, IEnumerable<IAxis> axises)
         => Render?.DrawPlot(context, DataSource, viewPort, axises);

        public override void HandleContentTap(IEnumerable<IAxis> axises, CGPoint contentTapLocation)
        => TapHandler?.HandleContentTap(axises, contentTapLocation, DataSource);

        #endregion
    }
}
