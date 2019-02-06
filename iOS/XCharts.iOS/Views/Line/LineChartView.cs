using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;
using XCharts.iOS.Views.Base;
using XCharts.iOS.Abstract.Renders;
using XCharts.iOS.Abstract.DataSources;
using XCharts.iOS.Abstract.Handlers.Highlight;
using XCharts.iOS.Abstract.Entries;
using XCharts.iOS.Abstract.Handlers.Tap;
using XCharts.iOS.Implementation.Renders;
using XCharts.iOS.Implementation.Handlers.Tap;
using XCharts.iOS.Abstract.Axises;
using System.Linq;
using XCharts.iOS.Implementation.Axises;

namespace XCharts.iOS.Views.Line
{
    [Register("LineChartView"), DesignTimeVisible(true)]
    public class LineChartView : BasePointChartView
    {
        #region Fields

        protected bool isScaleActual;

        #endregion

        #region Properties

        public IPointRender PointRender { get; set; }

        private IPointDataSource dataSource;
        public IPointDataSource DataSource
        {
            get => dataSource;
            set
            {
                SubscribeToSource(dataSource, false);

                dataSource = value;

                SubscribeToSource(dataSource, true);

                DataSourceChanged(value);
            }
        }

        private IPointHighlightHandler highlightHandler;
        public IPointHighlightHandler HighlightHandler
        {
            get => highlightHandler;
            set
            {
                SubscribeToHighlightHandler(highlightHandler, false);

                highlightHandler = value;

                SubscribeToHighlightHandler(highlightHandler, true);

                HighlightHandlerChanged(value);
            }
        }

        public IPointTapHandler PointTapHandler { get; set; }

        public IXAxis XAxis => Axises?.OfType<IXAxis>().FirstOrDefault();

        public IYAxis YAxis => Axises?.OfType<IYAxis>().FirstOrDefault();

        public IXAxisRender XAxisRender => XAxis?.Render;

        public IYAxisRender YAxisRender => YAxis?.Render;

        public IAxisRangeProvider XAxisRangeProvider { get; set; } = new EntriesCountAxisRangeProvider();

        public IAxisRangeProvider YAxisRangeProvider { get; set; } = new OffsetAxisRangeProvider();

        public double SpaceBetweenPoints { get; set; } = 100;

        #endregion

        #region Constructor

        public LineChartView() : base()
        {
        }

        public LineChartView(IntPtr ptr) : base(ptr)
        {

        }

        public LineChartView(CGRect rect) : base(rect)
        {

        }

        public LineChartView(NSCoder coder) : base(coder)
        {

        }

        #endregion

        #region Protected

        protected override bool CanDraw(CGRect rect)
        => base.CanDraw(rect) && DataSource != null && PointRender != null && XAxis != null && YAxis != null;

        protected override void Initiallize()
        {
            base.Initiallize();

            ViewPort.ViewPortInsets = new UIEdgeInsets(40, 40, 40, 40);

            PointRender = new PointRender();

            PointTapHandler = new PointTapHandler();
        }

        protected override void ViewSizeChanged(CGSize newSize)
        {
            base.ViewSizeChanged(newSize);

            isScaleActual = false;
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
        {
            SetupAxisRanges();

            var count = dataSource.Count;

            count = count > 1 ? count - 1 : count;

            ViewPort.ChangeContentSize(count * SpaceBetweenPoints + 2 * XAxis.AxisZeroOffset, ViewPort.ViewPortSize.Height);

            foreach (var axis in Axises)
                axis.Adjust(ViewPort);
        }

        protected virtual void SetupAxisRanges()
        {
            if (XAxisRangeProvider != null)
            {
                var xAxisRange = XAxisRangeProvider.GetRange(this, DataSource);

                XAxis.SetRange(xAxisRange.Item1, xAxisRange.Item2);
            }

            if (YAxisRangeProvider != null)
            {
                var yAxisRange = YAxisRangeProvider.GetRange(this, DataSource);

                YAxis.SetRange(yAxisRange.Item1, yAxisRange.Item2);
            }
        }

        protected override void HandleContentTap(CGPoint contentTapLocation)
        {
            Trace($"Tap at ({contentTapLocation.X}/{contentTapLocation.Y})");

            var handler = HighlightHandler;

            Trace($"HighlightHandler != null : {handler != null} | PointTapHandler != null : {PointTapHandler != null}");

            if (handler != null && PointTapHandler != null)
                PointTapHandler.Tap(contentTapLocation, this, DataSource, handler, XAxis, YAxis);
        }

        protected override void DrawData(CGContext context)
        => PointRender.DrawPlot(context, DataSource, SelectionEnable ? HighlightHandler?.HighlightedEntries : null, ViewPort, XAxis, YAxis);

        #region Data

        protected virtual void SubscribeToSource(IPointDataSource dataSource, bool subscribe)
        {
            if (dataSource == null)
                return;

            if (subscribe)
                dataSource.DataChanged += OnDataChanged;
            else
                dataSource.DataChanged -= OnDataChanged;
        }

        protected virtual void SubscribeToHighlightHandler(IPointHighlightHandler highlightHandler, bool subscribe)
        {
            if (highlightHandler == null)
                return;

            if (subscribe)
                highlightHandler.HighlightedEntriesChanged += OnHighlightedEntriesChanged;
            else
                highlightHandler.HighlightedEntriesChanged -= OnHighlightedEntriesChanged;
        }

        protected virtual void HighlightHandlerChanged(IPointHighlightHandler value)
        => SetNeedsDisplay();

        protected virtual void OnHighlightedEntriesChanged(object sender, EventArgs e)
        => SetNeedsDisplay();

        protected virtual void OnDataChanged(object sender, EventArgs e)
        => OnDataChanged();

        protected virtual void DataSourceChanged(IPointDataSource value)
        {
            if (value is IPointHighlightHandler datasourceWithHighlight)
                HighlightHandler = datasourceWithHighlight;

            OnDataChanged();
        }

        protected virtual void OnDataChanged()
        {
            isScaleActual = false;

            SetNeedsDisplay();
        }

        #endregion

        #endregion

        #region Public

        public override void ContentChanged()
        => OnDataChanged();

        public CGPoint GetContentPosition(double sourceX, double sourceY)
        => new CGPoint(XAxis.GetContentValue(sourceX), YAxis.GetContentValue(sourceY));

        #endregion
    }
}
