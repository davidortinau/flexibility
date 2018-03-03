using System;

using Android.Content;
using Android.Views;
using droidGraphics = Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace Xamarin.Forms.Shapes.Android
{
    public class EllipseDrawableView : ShapeDrawableView 
    {
        public EllipseDrawableView(Context context) : base(context)
        {
            CreatePath();
        }

        void CreatePath()
        {
            droidGraphics.Path path = new droidGraphics.Path();
            path.AddCircle(0, 0, 1, droidGraphics.Path.Direction.Cw);           // TODO: Is direction correct?
            Path = path;
        }
    }
}