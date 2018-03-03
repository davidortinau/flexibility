using CoreGraphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Xamarin.Forms.Media.iOS.NativeBrush))]

namespace Xamarin.Forms.Media.iOS
{
    public class NativeBrush : NativeObject, INativeBrush
    {
        public object ConvertToNative(Brush brush, object context)
        {
            if (brush is SolidColorBrush)
            {
                SolidColorBrush xamBrush = brush as SolidColorBrush;
                return xamBrush.Color.ToCGColor();
            }
            else if (brush is LinearGradientBrush)
            {
                // TODO: The CGGradient could be created here, but other properties -- such as Transform and Opacity --
                //  need to go to the drawing loop.

                return brush;
            }
            else if (brush is ImageBrush)
            {
                return brush;
            }

            return null;
        }
    }
}