using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas;

namespace Xamarin.Forms.Shapes.WinRT
{
    public class RectangleCanvasControl : ShapeCanvasControl
    {
        public float RadiusX
        {
            set; get;
        }

        public float RadiusY
        {
            set; get;
        }

        protected override CanvasGeometry BuildBaseGeometry(ICanvasResourceCreator resourceCreator)
        {
            return CanvasGeometry.CreateRoundedRectangle(resourceCreator, new Windows.Foundation.Rect(-1, -1, 2, 2), 0, 0);
        }

        public void SetRadiusX(float radiusX)
        {
            RadiusX = radiusX;
            CanvasControl.Invalidate();
        }

        public void SetRadiusY(float radiusY)
        {
            RadiusY = radiusY;
            CanvasControl.Invalidate();
        }
    }
}
