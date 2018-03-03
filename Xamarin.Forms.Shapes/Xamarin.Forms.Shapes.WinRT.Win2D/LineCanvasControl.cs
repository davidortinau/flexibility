using System;
using System.Numerics;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;


namespace Xamarin.Forms.Shapes.WinRT
{
    public class LineCanvasControl : ShapeCanvasControl
    {
        Vector2 startPoint = new Vector2();
        Vector2 endPoint = new Vector2();

        public void SetX1(float x1)
        {
            startPoint.X = x1;
            InvalidateGeometry();
        }

        public void SetY1(float y1)
        {
            startPoint.Y = y1;
            InvalidateGeometry();
        }

        public void SetX2(float x2)
        {
            endPoint.X = x2;
            InvalidateGeometry();
        }

        public void SetY2(float y2)
        {
            endPoint.Y = y2;
            InvalidateGeometry();
        }

        protected override CanvasGeometry BuildBaseGeometry(ICanvasResourceCreator resourceCreator)
        {
            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(startPoint, CanvasFigureFill.Default);
                pathBuilder.AddLine(endPoint);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);
                return CanvasGeometry.CreatePath(pathBuilder);
            }
        }
    }
}
