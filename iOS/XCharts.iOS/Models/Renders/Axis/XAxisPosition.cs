using System;
namespace XCharts.iOS.Models.Renders.Axis
{
    [Flags]
    public enum XAxisPosition
    {
        None = 0,
        Top = 1 << 0,
        Bottom = 1 << 1,
        BothSides = Top | Bottom
    }
}
