using System;
namespace XCharts.iOS.Abstract.Axises
{
    public interface IAxisDisplayValueFormatter<TSourceValue>
    {
        string DisplayValue(TSourceValue value);
    }
}
