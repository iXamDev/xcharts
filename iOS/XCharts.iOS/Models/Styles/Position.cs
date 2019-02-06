using System;
namespace XCharts.iOS.Models.Styles
{
    [Flags]
    public enum Position
    {
        Default = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Top = 1 << 2,
        Bottom = 1 << 3,
        CenterHorizontal = 1 << 4,
        CenterVertical = 1 << 5,
        Center = CenterVertical | CenterHorizontal
    }
}
