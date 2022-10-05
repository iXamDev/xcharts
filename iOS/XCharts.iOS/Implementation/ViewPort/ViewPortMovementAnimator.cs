using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Models.ViewPort;
using XCharts.iOS.Extensions;

namespace XCharts.iOS.Implementation.ViewPort
{
    public class ViewPortMovementAnimator : IViewPortMovementAnimator
    {
        #region Fields

        protected CADisplayLink link;

        protected double decelerationVelocityX, decelerationVelocityY;

        protected double decelerationX, decelerationY, lastDecelerationFramePositionX, lastDecelerationFramePositionY;

        protected double decelerationStartTime;

        protected IViewPort viewPort;

        #endregion

        #region Events

        public event EventHandler RedrawIsNeeded;

        #endregion

        #region Properties

        public nfloat DecelerationMultiplier { get; set; } = 1.8f;

        /// <summary>
        /// The movement time in seconds.
        /// Time of scroll to position.
        /// </summary>
        public double MovementTime = 0.8;

        #endregion

        #region Constructor

        public ViewPortMovementAnimator()
        {
        }

        #endregion

        #region Protected

        protected virtual void InitDeceleration(CGPoint velocity)
        {
            link = CADisplayLink.Create(OnDisplayLinkLoop);

            decelerationVelocityX = velocity.X;

            decelerationVelocityY = velocity.Y;

            var decelerationAgility = Math.Max(Math.Abs(decelerationVelocityX), Math.Abs(decelerationVelocityY)) * DecelerationMultiplier;

            var decelerationTimeX = decelerationVelocityX / decelerationAgility;

            var decelerationTimeY = decelerationVelocityY / decelerationAgility;

            var ratio = Math.Abs(decelerationTimeX) / Math.Abs(decelerationTimeY);

            var fasterScrollingByX = Math.Abs(decelerationTimeX) > Math.Abs(decelerationTimeY);

            if (Math.Abs(decelerationVelocityX) > 0)
            {
                decelerationX = fasterScrollingByX ? decelerationAgility : decelerationAgility * ratio;

                if (decelerationVelocityX < 0)
                    decelerationX *= -1;
            }
            else
                decelerationX = 0;

            if (Math.Abs(decelerationVelocityY) > 0)
            {
                decelerationY = fasterScrollingByX ? decelerationAgility / ratio : decelerationAgility;

                if (decelerationVelocityY < 0)
                    decelerationY *= -1;
            }
            else
                decelerationY = 0;

            lastDecelerationFramePositionX = 0;

            lastDecelerationFramePositionY = 0;

            decelerationStartTime = CAAnimation.CurrentMediaTime();

            link.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Default);
        }

        protected virtual void InitMovement(IViewPort viewPort, CGPoint positionToMove)
        {
            link = CADisplayLink.Create(OnDisplayLinkLoop);

            var positionToMoveOffset = positionToMove.Subtract(new CGPoint(viewPort.ViewPortSize.Width / 2, viewPort.ViewSize.Height / 2));

            var moveDelta = positionToMoveOffset.Subtract(viewPort.ViewPortContentOffset);

            decelerationVelocityX = 2D * moveDelta.X / MovementTime;

            decelerationVelocityY = 2D * moveDelta.Y / MovementTime;

            decelerationX = 2D * moveDelta.X / Math.Pow(MovementTime, 2);

            decelerationY = 2D * moveDelta.Y / Math.Pow(MovementTime, 2);

            lastDecelerationFramePositionX = 0;

            lastDecelerationFramePositionY = 0;

            decelerationStartTime = CAAnimation.CurrentMediaTime();

            link.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Default);
        }

        protected virtual void OnDisplayLinkLoop()
        {
            if (link == null || viewPort == null)
                return;

            var actualFrameDuration = link.TargetTimestamp - link.Timestamp;

            var decelerationEllapsedTime = link.Timestamp - decelerationStartTime;

            double frameOffsetX, frameOffsetY;

            if (Math.Abs(decelerationVelocityX) > 0)
            {
                var framePositionX = decelerationVelocityX * decelerationEllapsedTime - decelerationX * Math.Pow(decelerationEllapsedTime, 2) / 2;

                frameOffsetX = framePositionX - lastDecelerationFramePositionX;

                lastDecelerationFramePositionX = framePositionX;

                if (Math.Abs(decelerationVelocityX) - Math.Abs(decelerationX * decelerationEllapsedTime) <= 1)
                    decelerationVelocityX = 0;
            }
            else
                frameOffsetX = 0;

            if (Math.Abs(decelerationVelocityY) > 0)
            {
                var framePositionY = decelerationVelocityY * decelerationEllapsedTime - decelerationY * Math.Pow(decelerationEllapsedTime, 2) / 2;

                frameOffsetY = framePositionY - lastDecelerationFramePositionY;

                lastDecelerationFramePositionY = framePositionY;

                if (Math.Abs(decelerationVelocityY) - Math.Abs(decelerationY * decelerationEllapsedTime) <= 1)
                    decelerationVelocityY = 0;
            }
            else
                frameOffsetY = 0;

            var movementVector = new CGPoint(frameOffsetX, frameOffsetY);

            var success = viewPort.Offset(movementVector);

            if (!success)
            {
                var movementDirections = GetVectorDirection(movementVector);

                decelerationVelocityX = viewPort.CanMove(movementDirections & Direction.Horizontal) ? decelerationVelocityX : 0;

                decelerationVelocityY = viewPort.CanMove(movementDirections & Direction.Vertical) ? decelerationVelocityY : 0;
            }

            if (decelerationEllapsedTime > 0.3f && Math.Abs(frameOffsetX) < 0.1 && Math.Abs(frameOffsetY) < 0.1)
                StopDeceleration();

            FireRedrawIsNeeded();
        }

        protected virtual void StopDeceleration()
        {
            link?.Invalidate();

            link = null;
        }

        protected virtual Direction GetVectorDirection(CGPoint vector)
        {
            var horizontalDirection = vector.X == 0 ? Direction.None : (vector.X > 0 ? Direction.Right : Direction.Left);

            var verticalDirection = vector.Y == 0 ? Direction.None : (vector.Y > 0 ? Direction.Down : Direction.Up);

            return horizontalDirection | verticalDirection;
        }

        protected void FireRedrawIsNeeded()
        => RedrawIsNeeded?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Public

        public virtual void StartDeceleration(IViewPort viewPort, CGPoint velocityVector)
        {
            StopDeceleration();

            this.viewPort = viewPort;

            InitDeceleration(velocityVector);
        }

        public virtual void StopMovement(IViewPort viewPort)
        {
            if (this.viewPort == viewPort)
                this.viewPort = null;

            StopDeceleration();
        }

        public virtual void MoveTo(IViewPort viewPort, CGPoint positionToMove)
        {
            StopDeceleration();

            this.viewPort = viewPort;

            if (viewPort.ViewSize.Height > 0 && viewPort.ViewSize.Width > 0)
                InitMovement(viewPort, positionToMove);
        }

        #endregion
    }
}
