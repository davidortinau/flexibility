using Android.Content;
using Android.Graphics.Drawables.Shapes;
using droidGraphics = Android.Graphics;
using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes.Android
{
    public class PolylineDrawableView : ShapeDrawableView 
    {
        PointCollection points;
        droidGraphics.Path.FillType fillType = droidGraphics.Path.FillType.EvenOdd;

        public PolylineDrawableView(Context context) : base(context)
        {
        }

        void CreatePath()
        {
            droidGraphics.Path path = null;

            if (points != null && points.Count > 1)
            {
                path = new droidGraphics.Path();
                path.SetFillType(fillType);

                path.MoveTo(pixelsPerDip * (float)points[0].X,
                            pixelsPerDip * (float)points[0].Y);

                for (int index = 1; index < points.Count; index++)
                {
                    path.LineTo(pixelsPerDip * (float)points[index].X,
                                pixelsPerDip * (float)points[index].Y);
                }

                if (this is PolygonDrawableView)
                {
                    path.Close();
                }
            }

            Path = path;
        }

        public void SetPoints(PointCollection points)
        {
            this.points = points;
            CreatePath();
        }

        public void SetFillType(droidGraphics.Path.FillType fillType)
        {
            this.fillType = fillType;
            CreatePath();
        }
    }
}