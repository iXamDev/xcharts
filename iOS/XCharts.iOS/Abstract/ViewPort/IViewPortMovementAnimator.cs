using System;
using CoreGraphics;
using XCharts.iOS.Abstract.Chart;

namespace XCharts.iOS.Abstract.ViewPort
{
    public interface IViewPortMovementAnimator
    {
        event EventHandler RedrawIsNeeded;

        void StopMovement(IViewPort viewPort);

        void StartDeceleration(IViewPort viewPort, CGPoint velocityVector);

        void MoveTo(IViewPort viewPort, CGPoint veiwportPosition);
    }
}
