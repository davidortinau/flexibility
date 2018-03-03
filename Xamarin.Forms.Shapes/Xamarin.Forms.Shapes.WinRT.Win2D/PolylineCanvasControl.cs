using Microsoft.Graphics.Canvas.Geometry;

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PolylineCanvasControl : PolygonCanvasControl
    {
        protected override CanvasFigureLoop CanvasFigureLoop
        {
            get { return CanvasFigureLoop.Open; }
        }
    }
}
