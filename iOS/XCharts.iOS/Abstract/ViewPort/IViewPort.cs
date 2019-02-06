using System;
using CoreGraphics;
using XCharts.iOS.Models.ViewPort;
using UIKit;
namespace XCharts.iOS.Abstract.ViewPort
{
    public interface IViewPort
    {
        UIEdgeInsets ViewPortInsets { get; set; }

        CGRect ViewPortRect { get; }

        CGPoint ViewPortContentOffset { get; }

        CGSize ViewSize { get; }

        CGSize ViewPortSize { get; }

        CGSize ContentSize { get; }

        void ChangeContentSize(double? width = null, double? height = null);

        bool Offset(CGPoint offset);

        void SetViewSize(CGSize size);

        bool CanMove(Direction direction);

        CGPoint DisplayPosition(CGPoint contentPosition);

        nfloat DisplayPositionY(nfloat contentPositionY);

        nfloat DisplayPositionX(nfloat contentPositionX);

        nfloat DisplayPositionY(double contentPositionY);

        nfloat DisplayPositionX(double contentPositionX);

        CGPoint ContentPosition(CGPoint displayPosition);
    }
}
