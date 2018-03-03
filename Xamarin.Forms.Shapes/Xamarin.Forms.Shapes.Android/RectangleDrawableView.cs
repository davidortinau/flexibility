using System;

using Android.Content;
using droidGraphics = Android.Graphics;

namespace Xamarin.Forms.Shapes.Android
{
    public class RectangleDrawableView : ShapeDrawableView 
    {
        public RectangleDrawableView(Context context) : base(context)
        {
            CreatePath();
        }

        public float RadiusX
        {
            set; get;
        }

        public float RadiusY
        {
            set; get;
        }

        void CreatePath()
        {
            droidGraphics.Path path = new droidGraphics.Path();
            path.AddRect(new droidGraphics.RectF(0, 0, 1, 1), droidGraphics.Path.Direction.Cw);           // TODO: Is direction correct?
            Path = path;
        }

        public void SetRadiusX(float radiusX)
        {
            RadiusX = pixelsPerDip * radiusX;
            Invalidate();
        }

        public void SetRadiusY(float radiusY)
        {
            RadiusY = pixelsPerDip * radiusY;
            Invalidate();
        }
    }
}

