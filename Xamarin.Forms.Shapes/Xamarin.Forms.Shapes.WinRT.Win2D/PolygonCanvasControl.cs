using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PolygonCanvasControl : ShapeCanvasControl
    {
        Vector2[] points;
        CanvasFilledRegionDetermination fillRule;

        protected virtual CanvasFigureLoop CanvasFigureLoop
        {
            get { return CanvasFigureLoop.Closed; }
        }

        public void SetPoints(Vector2[] points)
        {
            this.points = points;
            InvalidateGeometry();
        }

        internal void SetFillRule(CanvasFilledRegionDetermination fillRule)
        {
            this.fillRule = fillRule;
            InvalidateGeometry();
        }

        protected override CanvasGeometry BuildBaseGeometry(ICanvasResourceCreator resourceCreator)
        {
            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.SetFilledRegionDetermination(fillRule);

                if (points != null)
                {
                    if (points.Length > 0)
                    {
                        pathBuilder.BeginFigure(points[0]);
                    }

                    for (int i = 1; i < points.Length; i++)
                    {
                        pathBuilder.AddLine(points[i]);
                    }
                }

                pathBuilder.EndFigure(CanvasFigureLoop);

                return CanvasGeometry.CreatePath(pathBuilder);
            }
        }
    }
}
