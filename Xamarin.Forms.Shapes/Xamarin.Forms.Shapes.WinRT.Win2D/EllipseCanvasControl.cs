using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;


namespace Xamarin.Forms.Shapes.WinRT
{
    public class EllipseCanvasControl : ShapeCanvasControl
    {
        protected override CanvasGeometry BuildBaseGeometry(ICanvasResourceCreator resourceCreator)
        {
            return CanvasGeometry.CreateEllipse(resourceCreator, new Vector2(0, 0), 1, 1);
        }
    }
}
