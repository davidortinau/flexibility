using Android.Content;
using Android.Views;
using droidGraphics = Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace Xamarin.Forms.Shapes.Android
{
    public class LineDrawableView : ShapeDrawableView 
    {
        float x1, y1, x2, y2;

        public LineDrawableView(Context context) : base(context)
        {
        }

        void CreatePath()
        {
            droidGraphics.Path path = new droidGraphics.Path();
            path.MoveTo(x1, y1);
            path.LineTo(x2, y2);
            Path = path;
        }

        public void SetX1(float x1)
        {
            this.x1 = pixelsPerDip * x1;
            CreatePath();
        }

        public void SetY1(float y1)
        {
            this.y1 = pixelsPerDip * y1;
            CreatePath();
        }

        public void SetX2(float x2)
        {
            this.x2 = pixelsPerDip * x2;
            CreatePath();
        }

        public void SetY2(float y2)
        {
            this.y2 = pixelsPerDip * y2;
            CreatePath();
        }
    }
}