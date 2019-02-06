using System;
namespace XCharts.iOS.Models.Renders.Axis
{
    [Flags]
    public enum YAxisPosition
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        BothSides = Left | Right
    }
}
