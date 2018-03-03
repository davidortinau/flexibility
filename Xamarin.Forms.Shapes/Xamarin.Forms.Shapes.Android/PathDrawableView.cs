using Android.Content;
using Android.Views;
using droidGraphics = Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace Xamarin.Forms.Shapes.Android
{
    public class PathDrawableView : ShapeDrawableView 
    {
        public PathDrawableView(Context context) : base(context)
        {
        }

        public void SetPath(droidGraphics.Path path)
        {
            Path = path;
        }
    }
}