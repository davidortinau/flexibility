using droidGraphics = Android.Graphics;

using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.Android.NativeBrush))]

namespace Xamarin.Forms.Media.Android
{
    public class NativeBrush : INativeBrush
    {
        public object ConvertToNative(Brush brush, object context)
        {
            if (brush is SolidColorBrush)
            {
                return (brush as SolidColorBrush).Color;
            }

            else if (brush is LinearGradientBrush)
            {
                // Conversion of the brush has been moved to ShapeDrawableView
                return brush;
            }

            else if (brush is ImageBrush)
            {
                // Ditto
                return brush;
            }

            return null;
        }

        int ConvertColor(Color color)
        {
            return droidGraphics.Color.Argb((int)(255 * color.A), (int)(255 * color.R), (int)(255 * color.G), (int)(255 * color.B));
        }
    }
}
