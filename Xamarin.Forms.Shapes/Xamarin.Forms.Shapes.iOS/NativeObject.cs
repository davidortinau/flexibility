using CoreGraphics;

namespace Xamarin.Forms.Media.iOS
{
    public class NativeObject
    {
        protected CGPoint ConvertPoint(Point point)
        {
            return new CGPoint(point.X, point.Y);
        }

    }
}
