using CoreGraphics;
using XCharts.iOS.Abstract.ViewPort;
namespace XCharts.iOS.Abstract.Axises
{
    public interface IAxis
    {
        double AxisZeroOffset { get; }

        double GetContentValue(double sourceValue);

        double GetSourceValue(double contentValue);

        double Min { get; }

        double Max { get; }

        void SetRange(double min, double max);

        void Adjust(IViewPort viewPort);

        void Draw(CGContext context, IViewPort viewPort);
    }
}
