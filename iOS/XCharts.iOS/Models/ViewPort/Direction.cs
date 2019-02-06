using System;
namespace XCharts.iOS.Models.ViewPort
{
    [Flags]
    public enum Direction
    {
        None = 0,
        Right = 1 << 0,
        Left = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,

        Horizontal = Right | Left,
        Vertical = Up | Down
    }
}
