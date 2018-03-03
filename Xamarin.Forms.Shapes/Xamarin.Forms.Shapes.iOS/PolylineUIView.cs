using CoreGraphics;

namespace Xamarin.Forms.Shapes.iOS
{
    public class PolylineUIView : ShapeUIView
    {
        public void SetPoints(CGPoint[] points)
        {
            CGPath path = new CGPath();
            path.AddLines(points);

            if (this is PolygonUIView)
            {
                path.CloseSubpath();
            }
            DrawingLayer.SetBasicPath(path);
        }

        public void SetFillMode(bool isNonzeroFill)
        {
            DrawingLayer.SetIsNonzeroFill(isNonzeroFill);
        }
    }
}
