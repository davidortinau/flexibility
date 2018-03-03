using System;
using System.Collections.Generic;

using droidGraphics = Android.Graphics;

using Xamarin.Forms;
using Android.App;

[assembly: Dependency(typeof(Xamarin.Forms.Media.Android.NativeGeometry))]

namespace Xamarin.Forms.Media.Android
{
    public class NativeGeometry : NativeObject, INativeGeometry
    {
        public object ConvertToNative(Geometry geometry)
        {
            // TODO: Can we keep this object around and reset it?
            droidGraphics.Path path = new droidGraphics.Path();
            // TODO: Might we also save a non-transformed path and a transformed path? -- No, that fails for GeometryGroup

            // Determine what type of Geometry we're dealing with
            if (geometry is LineGeometry)
            {
                LineGeometry lineGeometry = geometry as LineGeometry;

                path.MoveTo(PixelsPerDip * (float)lineGeometry.StartPoint.X,
                            PixelsPerDip * (float)lineGeometry.StartPoint.Y);

                path.LineTo(PixelsPerDip * (float)lineGeometry.EndPoint.X,
                            PixelsPerDip * (float)lineGeometry.EndPoint.Y);
            }

            else if (geometry is RectangleGeometry)
            {
                Rect rect = (geometry as RectangleGeometry).Rect;

                path.AddRect(PixelsPerDip * (float)rect.Left,
                             PixelsPerDip * (float)rect.Top,
                             PixelsPerDip * (float)rect.Right,
                             PixelsPerDip * (float)rect.Bottom,
                             droidGraphics.Path.Direction.Cw);              // TODO: Check for compatibility!
            }

            else if (geometry is EllipseGeometry)
            {
                EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;

                path.AddOval(new droidGraphics.RectF(
                                PixelsPerDip * (float)(ellipseGeometry.Center.X - ellipseGeometry.RadiusX),
                                PixelsPerDip * (float)(ellipseGeometry.Center.Y - ellipseGeometry.RadiusY),
                                PixelsPerDip * (float)(ellipseGeometry.Center.X + ellipseGeometry.RadiusX),
                                PixelsPerDip * (float)(ellipseGeometry.Center.Y + ellipseGeometry.RadiusY)),
                             droidGraphics.Path.Direction.Cw);              // TODO: Check for compatibility!
            }

            else if (geometry is GeometryGroup)
            {
                GeometryGroup geometryGroup = geometry as GeometryGroup;
                SetFillRule(path, geometryGroup.FillRule);
                
                foreach (Geometry child in geometryGroup.Children)
                {
                    droidGraphics.Path childPath = ConvertToNative(child) as droidGraphics.Path;
                    path.AddPath(childPath);
                }
            }

            else if (geometry is PathGeometry)
            {
                PathGeometry pathGeometry = geometry as PathGeometry;

                SetFillRule(path, pathGeometry.FillRule);

                foreach (PathFigure pathFigure in pathGeometry.Figures)
                {
                    path.MoveTo(PixelsPerDip * (float)pathFigure.StartPoint.X,
                                PixelsPerDip * (float)pathFigure.StartPoint.Y);
                    Point lastPoint = pathFigure.StartPoint;

                    foreach (PathSegment pathSegment in pathFigure.Segments)
                    {
                        // LineSegment
                        if (pathSegment is LineSegment)
                        {
                            LineSegment lineSegment = pathSegment as LineSegment;

                            path.LineTo(PixelsPerDip * (float)lineSegment.Point.X,
                                        PixelsPerDip * (float)lineSegment.Point.Y);
                            lastPoint = lineSegment.Point;
                        }

                        // PolylineSegment
                        else if (pathSegment is PolyLineSegment)
                        {
                            PolyLineSegment polylineSegment = pathSegment as PolyLineSegment;
                            PointCollection points = polylineSegment.Points;

                            for (int i = 0; i < points.Count; i++)
                            {
                                path.LineTo(PixelsPerDip * (float)points[i].X,
                                            PixelsPerDip * (float)points[i].Y);
                            }
                            lastPoint = points[points.Count - 1];
                        }

                        // BezierSegment
                        else if (pathSegment is BezierSegment)
                        {
                            BezierSegment bezierSegment = pathSegment as BezierSegment;

                            path.CubicTo(PixelsPerDip * (float)bezierSegment.Point1.X, PixelsPerDip * (float)bezierSegment.Point1.Y,
                                         PixelsPerDip * (float)bezierSegment.Point2.X, PixelsPerDip * (float)bezierSegment.Point2.Y,
                                         PixelsPerDip * (float)bezierSegment.Point3.X, PixelsPerDip * (float)bezierSegment.Point3.Y);

                            lastPoint = bezierSegment.Point3;
                        }

                        // PolyBezierSegment
                        else if (pathSegment is PolyBezierSegment)
                        {
                            PolyBezierSegment polyBezierSegment = pathSegment as PolyBezierSegment;
                            PointCollection points = polyBezierSegment.Points;

                            for (int i = 0; i < points.Count; i += 3)
                            {
                                path.CubicTo(PixelsPerDip * (float)points[i + 0].X, PixelsPerDip * (float)points[i + 0].Y,
                                             PixelsPerDip * (float)points[i + 1].X, PixelsPerDip * (float)points[i + 1].Y,
                                             PixelsPerDip * (float)points[i + 2].X, PixelsPerDip * (float)points[i + 2].Y);
                            }
                            lastPoint = points[points.Count - 1];
                        }

                        // QuadraticBezierSegment
                        else if (pathSegment is QuadraticBezierSegment)
                        {
                            QuadraticBezierSegment bezierSegment = pathSegment as QuadraticBezierSegment;

                            path.QuadTo(PixelsPerDip * (float)bezierSegment.Point1.X, PixelsPerDip * (float)bezierSegment.Point1.Y,
                                        PixelsPerDip * (float)bezierSegment.Point2.X, PixelsPerDip * (float)bezierSegment.Point2.Y);

                            lastPoint = bezierSegment.Point2;
                        }

                        // PolyQuadraticBezierSegment
                        else if (pathSegment is PolyQuadraticBezierSegment)
                        {
                            PolyQuadraticBezierSegment polyBezierSegment = pathSegment as PolyQuadraticBezierSegment;
                            PointCollection points = polyBezierSegment.Points;

                            for (int i = 0; i < points.Count; i += 2)
                            {
                                path.QuadTo(PixelsPerDip * (float)points[i + 0].X, PixelsPerDip * (float)points[i + 0].Y,
                                            PixelsPerDip * (float)points[i + 1].X, PixelsPerDip * (float)points[i + 1].Y);
                            }
                            lastPoint = points[points.Count - 1];
                        }

                        // ArcSegment
                        else if (pathSegment is ArcSegment)
                        {
                            ArcSegment arcSegment = pathSegment as ArcSegment;

                            List<Point> points = new List<Point>();
                            Xamarin.Forms.Shapes.Shapes.FlattenArc(points,
                                                                   lastPoint,
                                                                   arcSegment.Point,
                                                                   arcSegment.Size.Width,
                                                                   arcSegment.Size.Height,
                                                                   arcSegment.RotationAngle,
                                                                   arcSegment.IsLargeArc,
                                                                   arcSegment.SweepDirection == SweepDirection.Counterclockwise,
                                                                   1);

                            for (int i = 0; i < points.Count; i++)
                            {
                                path.LineTo(PixelsPerDip * (float)points[i].X,
                                            PixelsPerDip * (float)points[i].Y);
                            }
                            lastPoint = points[points.Count - 1];
                        }
                    }

                    if (pathFigure.IsClosed)
                    {
                        path.Close();
                    }
                }
            }

            // Apply transform
            if (geometry.Transform != null)
            {
                path.Transform((droidGraphics.Matrix)geometry.Transform.GetNativeObject());
            }

            return path;
        }

        void SetFillRule(droidGraphics.Path path, FillRule fillRule)
        {
            path.SetFillType(fillRule == FillRule.Nonzero ? droidGraphics.Path.FillType.Winding : 
                                                            droidGraphics.Path.FillType.EvenOdd);
        }
    }
}
