using winFound = Windows.Foundation;

namespace Xamarin.Forms.Media.WinRT
{
    public class NativeObject 
    {
        protected winFound.Point ConvertPoint(Point point)
        {
            return new winFound.Point(point.X, point.Y);
        }
    }
}
