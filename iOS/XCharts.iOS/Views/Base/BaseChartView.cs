using System;
using UIKit;
using Foundation;
using CoreGraphics;
using XCharts.iOS.Implementation.ViewPort;
using System.Diagnostics;
using XCharts.iOS.Abstract.Chart;
using XCharts.iOS.Abstract.ViewPort;
using System.Collections.Generic;
using XCharts.iOS.Abstract.Axises;

namespace XCharts.iOS.Views.Base
{
    public abstract class BaseChartView : UIView, IChartView
    {
        #region Fields

        private CGPoint panGestureLastTranslation;

        #endregion

        #region Properties

        public bool ShowDebugMessages { get; set; } = false;

        public IViewPort ViewPort { get; private set; }

        public IViewPortMovementAnimator ViewPortMovementAnimator { get; set; }

        public virtual List<IAxis> Axises { get; set; }

        IEnumerable<IAxis> IChartView.Axises => Axises;

        public bool SelectionEnable { get; set; } = true;

        #endregion

        #region Constructor

        public BaseChartView() : base()
        {
            InitView();
        }

        public BaseChartView(IntPtr ptr) : base(ptr)
        {
            InitView();
        }

        public BaseChartView(CGRect rect) : base(rect)
        {
            InitView();
        }

        public BaseChartView(NSCoder coder) : base(coder)
        {
            InitView();
        }

        #endregion

        #region Private

        private void InitView()
        {
            this.UserInteractionEnabled = true;

            this.Opaque = true;

            this.BackgroundColor = UIColor.White;

            Initiallize();
        }

        private UIPanGestureRecognizer SetupPanGestureRecognizer()
        {
            var panGestureRecongnizer = new UIPanGestureRecognizer
            {
                MaximumNumberOfTouches = 1,

                MinimumNumberOfTouches = 1
            };

            panGestureRecongnizer.AddTarget(() => HandlePan(panGestureRecongnizer));

            this.AddGestureRecognizer(panGestureRecongnizer);

            return panGestureRecongnizer;
        }

        private UITapGestureRecognizer SetupTapGestureRecognizer()
        {
            var tapGestureRecongnizer = new UITapGestureRecognizer
            {
            };

            tapGestureRecongnizer.AddTarget(() => HandleTap(tapGestureRecongnizer));

            this.AddGestureRecognizer(tapGestureRecongnizer);

            return tapGestureRecongnizer;
        }

        private void HandlePan(UIPanGestureRecognizer recognizer)
        {
            var translation = recognizer.TranslationInView(this);

            Trace($"Pan translation: {translation.X}/{translation.Y}");

            StopDeceleration();

            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:

                    panGestureLastTranslation = recognizer.TranslationInView(this);

                    ViewPort.Offset(panGestureLastTranslation);

                    break;

                case UIGestureRecognizerState.Changed:

                    var translationOffset = new CGPoint(panGestureLastTranslation.X - translation.X, panGestureLastTranslation.Y - translation.Y);

                    ViewPort.Offset(translationOffset);

                    Trace($"Pan translationOffset: {translationOffset.X}/{translationOffset.Y}");

                    panGestureLastTranslation = translation;

                    break;

                case UIGestureRecognizerState.Ended:
                    Trace($"Pan gesture ended");

                    var velocity = recognizer.VelocityInView(this);

                    Trace($"Pan gesture velocity: {velocity.X}/{velocity.Y}");

                    InitDeceleration(velocity);

                    break;

                default:
                    break;
            }

            SetNeedsDisplay();
        }

        private void HandleTap(UITapGestureRecognizer recognizer)
        {
            var location = recognizer.LocationInView(this);

            if (SelectionEnable && recognizer.State == UIGestureRecognizerState.Ended)
            {
                Trace($"Tap gesture location: {location.X}/{location.Y}");

                if (ViewPort.ViewPortRect.Contains(location))
                    HandleContentTap(ViewPort.ContentPosition(location));
            }

            StopDeceleration();
        }

        #endregion

        #region Protected

        protected abstract void HandleContentTap(CGPoint contentTapLocation);

        protected virtual void Initiallize()
        {
            var panGestureRecongnizer = SetupPanGestureRecognizer();

            var tapGestureRecongnizer = SetupTapGestureRecognizer();

            panGestureRecongnizer.RequireGestureRecognizerToFail(tapGestureRecongnizer);

            tapGestureRecongnizer.RequireGestureRecognizerToFail(panGestureRecongnizer);

            Axises = CreateAxises();

            ViewPort = new ViewPort();

            ViewPortMovementAnimator = new ViewPortMovementAnimator();

            //todo

            ViewPortMovementAnimator.RedrawIsNeeded += (object sender, EventArgs e) => { SetNeedsDisplay(); };
        }

        protected abstract List<IAxis> CreateAxises();

        protected void StopDeceleration()
        => ViewPortMovementAnimator.StopMovement(ViewPort);

        protected void InitDeceleration(CGPoint velocity)
        => ViewPortMovementAnimator.StartDeceleration(ViewPort, new CGPoint() { X = -velocity.X, Y = -velocity.Y });

        protected virtual bool CanDraw(CGRect rect)
        => ViewPort != null && Axises != null;

        protected abstract void DrawData(CGContext context);

        protected virtual void DrawAxises(CGContext context)
        {
            foreach (var axis in Axises)
                axis.Draw(context, ViewPort);
        }

        protected virtual void DrawContent(CGContext context)
        {
            DrawAxises(context);

            DrawData(context);
        }

        protected virtual void PrepareToDraw(CGRect rect)
        {

        }

        protected virtual void ViewSizeChanged(CGSize newSize)
        => ViewPort.SetViewSize(newSize);

        protected virtual void Trace(string message)
        {
            if (ShowDebugMessages)
                Console.WriteLine(message);
        }

        #endregion

        #region Public

        public sealed override void Draw(CGRect rect)
        {
            base.Draw(rect);

            if (!CanDraw(rect))
                return;

            Stopwatch sw = null;

            if (ShowDebugMessages)
                sw = Stopwatch.StartNew();

            if (rect.Size != ViewPort.ViewPortSize)
                ViewSizeChanged(rect.Size);

            PrepareToDraw(rect);

            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                context.SetShouldAntialias(true);
                DrawContent(context);
            }

            if (ShowDebugMessages)
            {
                sw?.Stop();

                Trace($"[{ViewPort}]: Perfomance is {(sw?.ElapsedMilliseconds < 1000f / 60 ? "Ok" : "Poor")}");
            }
        }

        public void Redraw()
        {
            SetNeedsDisplay();
        }

        public abstract void ContentChanged();

        public virtual void ScrollTo(CGPoint contentCenter)
        => ViewPortMovementAnimator.MoveTo(ViewPort, contentCenter);

        #endregion
    }
}
