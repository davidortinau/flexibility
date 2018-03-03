using System;
using System.Numerics;
using winFound = Windows.Foundation;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

using Xamarin.Forms.Media;
using System.Collections.Generic;

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PathCanvasControl : ShapeCanvasControl
    {
        Geometry xamGeometry;

        protected virtual CanvasFigureLoop CanvasFigureLoop
        {
            get { return CanvasFigureLoop.Closed; }
        }

        public void SetData(Geometry geometry)
        {
            xamGeometry = geometry;
            InvalidateGeometry();
        }

        protected override CanvasGeometry BuildBaseGeometry(ICanvasResourceCreator resourceCreator)
        {
            if (xamGeometry == null)
                return null;

            return BuildGeometry(resourceCreator, xamGeometry);
        }

        CanvasGeometry BuildGeometry(ICanvasResourceCreator resourceCreator, Geometry geometry)
        {
            CanvasGeometry canvasGeometry = null;

            // Determine what type of geometry we're dealing with.
            if (geometry is RectangleGeometry)
            {
                Rect xamRect = (geometry as RectangleGeometry).Rect;
                winFound.Rect winRect = new winFound.Rect(xamRect.X, xamRect.Y, xamRect.Width, xamRect.Height);
                canvasGeometry = CanvasGeometry.CreateRectangle(resourceCreator, winRect);
            }

            else if (geometry is EllipseGeometry)
            {
                EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;
                Vector2 center = ConvertPoint(ellipseGeometry.Center);
                canvasGeometry = CanvasGeometry.CreateEllipse(resourceCreator, center, (float)ellipseGeometry.RadiusX, (float)ellipseGeometry.RadiusY);
            }

            else if (geometry is GeometryGroup)
            {
                GeometryGroup geometryGroup = geometry as GeometryGroup;
                List<CanvasGeometry> geometries = new List<CanvasGeometry>();

                foreach (Geometry geometryChild in geometryGroup.Children)
                {
                    geometries.Add(BuildGeometry(resourceCreator, geometryChild));
                }

                canvasGeometry = CanvasGeometry.CreateGroup(resourceCreator, geometries.ToArray(), (CanvasFilledRegionDetermination)(int)geometryGroup.FillRule);
            }

            else
            {
                using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
                {
                    // Determine what type of geometry we're dealing with.
                    if (geometry is LineGeometry)
                    {
                        LineGeometry lineGeometry = geometry as LineGeometry;
                        pathBuilder.BeginFigure(ConvertPoint(lineGeometry.StartPoint), CanvasFigureFill.Default);
                        pathBuilder.AddLine(ConvertPoint(lineGeometry.EndPoint));
                        pathBuilder.EndFigure(CanvasFigureLoop.Open);
                    }

                    else if (geometry is PathGeometry)
                    {
                        PathGeometry pathGeometry = geometry as PathGeometry;
                        pathBuilder.SetFilledRegionDetermination((CanvasFilledRegionDetermination)(int)pathGeometry.FillRule);

                        foreach (PathFigure pathFigure in pathGeometry.Figures)
                        {
                            // TODO: Check this logic!
                            pathBuilder.BeginFigure(ConvertPoint(pathFigure.StartPoint),
                                                    pathFigure.IsFilled ? CanvasFigureFill.Default : CanvasFigureFill.DoesNotAffectFills);

                            foreach (PathSegment pathSegment in pathFigure.Segments)
                            {
                                // LineSegment
                                if (pathSegment is LineSegment)
                                {
                                    pathBuilder.AddLine(ConvertPoint((pathSegment as LineSegment).Point));
                                }

                                // PolylineSegment
                                else if (pathSegment is PolyLineSegment)
                                {
                                    foreach (Point point in (pathSegment as PolyLineSegment).Points)
                                    {
                                        pathBuilder.AddLine(ConvertPoint(point));
                                    }
                                }

                                // BezierSegment
                                else if (pathSegment is BezierSegment)
                                {
                                    BezierSegment bezierSegment = pathSegment as BezierSegment;

                                    pathBuilder.AddCubicBezier(ConvertPoint(bezierSegment.Point1),
                                                                ConvertPoint(bezierSegment.Point2),
                                                                ConvertPoint(bezierSegment.Point3));
                                }

                                // PolyBezierSegment
                                else if (pathSegment is PolyBezierSegment)
                                {
                                    PointCollection points = (pathSegment as PolyBezierSegment).Points;

                                    for (int i = 0; i < points.Count; i += 3)
                                    {
                                        pathBuilder.AddCubicBezier(ConvertPoint(points[i + 0]),
                                                                    ConvertPoint(points[i + 1]),
                                                                    ConvertPoint(points[i + 2]));
                                    }
                                }

                                // QuadraticBezierSegment
                                else if (pathSegment is QuadraticBezierSegment)
                                {
                                    QuadraticBezierSegment quadSegment = pathSegment as QuadraticBezierSegment;

                                    pathBuilder.AddQuadraticBezier(ConvertPoint(quadSegment.Point1),
                                                                    ConvertPoint(quadSegment.Point2));
                                }

                                // PolyQuadraticBezierSegment
                                else if (pathSegment is PolyQuadraticBezierSegment)
                                {
                                    PointCollection points = (pathSegment as PolyQuadraticBezierSegment).Points;

                                    for (int i = 0; i < points.Count; i += 2)
                                    {
                                        pathBuilder.AddQuadraticBezier(ConvertPoint(points[i + 0]),
                                                                        ConvertPoint(points[i + 1]));
                                    }
                                }

                                // ArcSegment
                                else if (pathSegment is ArcSegment)
                                {
                                    ArcSegment arcSegment = pathSegment as ArcSegment;

                                    pathBuilder.AddArc(ConvertPoint(arcSegment.Point),
                                                                    (float)arcSegment.Size.Width,
                                                                    (float)arcSegment.Size.Height,
                                                                    (float)arcSegment.RotationAngle,
                                                                    (CanvasSweepDirection)(int)arcSegment.SweepDirection,
                                                                    arcSegment.IsLargeArc ? CanvasArcSize.Large : CanvasArcSize.Small);

                                }
                            }

                            pathBuilder.EndFigure(pathFigure.IsClosed ? CanvasFigureLoop.Closed : CanvasFigureLoop.Open);
                        }
                    }

                    else
                    {
                        return null;
                    }

                    canvasGeometry = CanvasGeometry.CreatePath(pathBuilder);
                }
            }

            // Set transform
            if (geometry.Transform != null)
            {
                canvasGeometry = canvasGeometry.Transform((Matrix3x2)geometry.Transform.GetNativeObject());
            }

            return canvasGeometry;
        }
    }
}
