using System;
using CoreGraphics;
using UIKit;
using XCharts.iOS.Abstract.ViewPort;
using XCharts.iOS.Models.ViewPort;
using XCharts.iOS.Extensions;

namespace XCharts.iOS.Implementation.ViewPort
{
    public class ViewPort : IViewPort
    {
        #region Fields

        #endregion

        #region Properties

        public CGSize ViewSize { get; private set; }

        public CGSize ViewPortSize { get; private set; }

        private CGSize contentSize = new CGSize(0, 0);
        public CGSize ContentSize => contentSize;

        private CGPoint viewPortOffsetOffset = new CGPoint(0, 0);
        public CGPoint ViewPortContentOffset => viewPortOffsetOffset;

        public CGRect ViewPortRect => new CGRect(new CGPoint(ViewPortInsets.Left, ViewPortInsets.Top), ViewPortSize);

        private UIEdgeInsets viewPortInsets = new UIEdgeInsets();
        public UIEdgeInsets ViewPortInsets
        {
            get => viewPortInsets;
            set
            {
                viewPortInsets = value;
                AdjustViewPortSize();
                AdjustOffset();
            }
        }

        #endregion

        #region Constructor

        public ViewPort()
        {
        }

        #endregion

        #region Private

        private bool AdjustOffset(CGPoint? lastOffset = null)
        {
            bool adjusted = false;

            if (viewPortOffsetOffset.X + ViewPortSize.Width > ContentSize.Width)
            {
                viewPortOffsetOffset.X = ContentSize.Width - ViewPortSize.Width;
                adjusted |= (!lastOffset.HasValue || lastOffset.Value.X != 0);
            }

            if (viewPortOffsetOffset.Y + ViewPortSize.Height > ContentSize.Height)
            {
                viewPortOffsetOffset.Y = ContentSize.Height - ViewPortSize.Height;
                adjusted |= (!lastOffset.HasValue || lastOffset.Value.Y != 0);
            }

            if (viewPortOffsetOffset.X < 0)
            {
                viewPortOffsetOffset.X = 0;
                adjusted |= (!lastOffset.HasValue || lastOffset.Value.X != 0);
            }

            if (viewPortOffsetOffset.Y < 0)
            {
                viewPortOffsetOffset.Y = 0;
                adjusted |= (!lastOffset.HasValue || lastOffset.Value.Y != 0);
            }

            return adjusted;
        }

        private void AdjustViewPortSize()
        {
            ViewPortSize = new CGSize(ViewSize.Width - ViewPortInsets.Left - ViewPortInsets.Right, ViewSize.Height - ViewPortInsets.Top - ViewPortInsets.Bottom);
        }

        #endregion

        #region Protected

        #endregion

        #region Public

        public bool Offset(CGPoint offset)
        {
            viewPortOffsetOffset.X += offset.X;
            viewPortOffsetOffset.Y += offset.Y;

            return !AdjustOffset(offset);
        }

        public void SetViewSize(CGSize size)
        {
            ViewSize = size;

            AdjustViewPortSize();

            AdjustOffset();
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}| size: ({Math.Round(ViewPortRect.Width, 2)};{Math.Round(ViewPortRect.Height, 2)}) | (x,y): ({Math.Round(ViewPortRect.X, 2)};{Math.Round(ViewPortRect.Y, 2)}) | Content size: (({ContentSize.Width},{ContentSize.Height}))";
        }

        public bool CanMove(Direction direction)
        {
            var canMove = true;

            if (direction.HasFlag(Direction.Left))
                canMove &= ViewPortContentOffset.X > 0;

            if (direction.HasFlag(Direction.Right))
                canMove &= ViewPortContentOffset.X + ViewPortSize.Width < ContentSize.Width;

            if (direction.HasFlag(Direction.Up))
                canMove &= ViewPortContentOffset.Y > 0;

            if (direction.HasFlag(Direction.Down))
                canMove &= ViewPortContentOffset.Y + ViewPortSize.Height < ContentSize.Height;

            return canMove;
        }

        public void ChangeContentSize(double? width = null, double? height = null)
        {
            contentSize = new CGSize(width ?? contentSize.Width, height ?? contentSize.Height);

            AdjustOffset();
        }

        public CGPoint DisplayPosition(CGPoint contentPosition)
        => new CGPoint(DisplayPositionX(contentPosition.X), DisplayPositionY(contentPosition.Y));

        public CGPoint ContentPosition(CGPoint viewPortPosition)
        => new CGPoint(viewPortPosition.X + ViewPortContentOffset.X - ViewPortInsets.Left, viewPortPosition.Y + ViewPortContentOffset.Y - ViewPortInsets.Top);

        public nfloat DisplayPositionY(nfloat contentPositionY)
        => contentPositionY - ViewPortContentOffset.Y + ViewPortInsets.Top;

        public nfloat DisplayPositionX(nfloat contentPositionX)
        => contentPositionX - ViewPortContentOffset.X + ViewPortInsets.Left;

        public nfloat DisplayPositionY(double contentPositionY)
        => DisplayPositionY((nfloat)contentPositionY);

        public nfloat DisplayPositionX(double contentPositionX)
        => DisplayPositionX((nfloat)contentPositionX);

        #endregion
    }
}
