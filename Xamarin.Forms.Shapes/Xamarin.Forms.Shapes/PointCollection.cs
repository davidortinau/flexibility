using System.Collections.ObjectModel;

namespace Xamarin.Forms.Media
{
    [TypeConverter(typeof(PointCollectionConverter))]
    public sealed class PointCollection : ObservableCollection<Point>
    {
    }
}
