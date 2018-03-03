using System;
using System.Collections.Generic;
using CoreGraphics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.iOS.NativeGeometry))]

namespace Xamarin.Forms.Media.iOS
{
    public class NativeGeometry : NativeObject, INativeGeometry
    {
        public object ConvertToNative(Geometry geometry)
        {
            // Create the CGPathPlus 
            CGPathPlus cgPath = new CGPathPlus();   // TODO: Has a Dispose. What are the implications?

            // Obtain the native transform to apply to the geometry
            CGAffineTransform transform = new CGAffineTransform();

            if (geometry.Transform == null)
            {
                transform = CGAffineTransform.MakeIdentity();
            }
            else
            {
                transform = (CGAffineTransform)geometry.Transform.GetNativeObject();
            }


            // Determine what type of Geometry we're dealing with
            if (geometry is LineGeometry)
            {
                LineGeometry lineGeometry = geometry as LineGeometry;
                cgPath.MoveToPoint(transform, ConvertPoint(lineGeometry.StartPoint));
                cgPath.AddLineToPoint(transform, ConvertPoint(lineGeometry.EndPoint));
            }

            else if (geometry is RectangleGeometry)
            {
                Rect rect = (geometry as RectangleGeometry).Rect;
                cgPath.AddRect(transform, new CGRect(rect.X, rect.Y, rect.Width, rect.Height));
            }

            else if (geometry is EllipseGeometry)
            {
                EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;

                CGRect rect = new CGRect(ellipseGeometry.Center.X - ellipseGeometry.RadiusX,
                                         ellipseGeometry.Center.Y - ellipseGeometry.RadiusY,
                                         ellipseGeometry.RadiusX * 2,
                                         ellipseGeometry.RadiusY * 2);

                cgPath.AddEllipseInRect(transform, rect);
            }

            else if (geometry is GeometryGroup)
            {
                GeometryGroup geometryGroup = geometry as GeometryGroup;

                cgPath.IsNonzeroFill = geometryGroup.FillRule == FillRule.Nonzero;      // make a little method?

                foreach (Geometry child in geometryGroup.Children)
                {
                    CGPath pathChild = ConvertToNative(child) as CGPath;
                    cgPath.AddPath(pathChild);          // Should there be a transform here as the first argument????
                }
            }

            else if (geometry is PathGeometry)
            {
                PathGeometry pathGeometry = geometry as PathGeometry;

                cgPath.IsNonzeroFill = pathGeometry.FillRule == FillRule.Nonzero;

                foreach (PathFigure pathFigure in pathGeometry.Figures)
                {
                    cgPath.MoveToPoint(transform, ConvertPoint(pathFigure.StartPoint));
                    Point lastPoint = pathFigure.StartPoint;

                    foreach (PathSegment pathSegment in pathFigure.Segments)
                    {
                        // LineSegment
                        if (pathSegment is LineSegment)
                        {
                            LineSegment lineSegment = pathSegment as LineSegment;

                            cgPath.AddLineToPoint(transform, ConvertPoint(lineSegment.Point));
                            lastPoint = lineSegment.Point;
                        }

                        // PolyLineSegment
                        else if (pathSegment is PolyLineSegment)
                        {
                            PolyLineSegment polylineSegment = pathSegment as PolyLineSegment;
                            PointCollection points = polylineSegment.Points;

                            // TODO: Or AddLines

                            for (int i = 0; i < points.Count; i++)
                            {
                                cgPath.AddLineToPoint(transform, ConvertPoint(points[i]));
                            }
                            lastPoint = points[points.Count - 1];
                        }

                        // BezierSegment
                        if (pathSegment is BezierSegment)
                        {
                            BezierSegment bezierSegment = pathSegment as BezierSegment;

                            cgPath.AddCurveToPoint(transform,
                                                   ConvertPoint(bezierSegment.Point1),
                                                   ConvertPoint(bezierSegment.Point2),
                                                   ConvertPoint(bezierSegment.Point3));

                            lastPoint = bezierSegment.Point3;
                        }

                        // PolyBezierSegment
                        else if (pathSegment is PolyBezierSegment)
                        {
                            PolyBezierSegment polyBezierSegment = pathSegment as PolyBezierSegment;
                            PointCollection points = polyBezierSegment.Points;

                            for (int i = 0; i < points.Count; i += 3)
                            {
                                cgPath.AddCurveToPoint(transform,
                                                       ConvertPoint(points[i]), 
                                                       ConvertPoint(points[i + 1]), 
                                                       ConvertPoint(points[i + 2]));
                            }
                            lastPoint = points[points.Count - 1];
                        }

                        // QuadraticBezierSegment
                        if (pathSegment is QuadraticBezierSegment)
                        {
                            QuadraticBezierSegment bezierSegment = pathSegment as QuadraticBezierSegment;

                            cgPath.AddQuadCurveToPoint(transform,
                                                       new nfloat(bezierSegment.Point1.X), 
                                                       new nfloat(bezierSegment.Point1.Y),
                                                       new nfloat(bezierSegment.Point2.X), 
                                                       new nfloat(bezierSegment.Point2.Y));

                            lastPoint = bezierSegment.Point2;
                        }

                        // PolyQuadraticBezierSegment
                        else if (pathSegment is PolyQuadraticBezierSegment)
                        {
                            PolyQuadraticBezierSegment polyBezierSegment = pathSegment as PolyQuadraticBezierSegment;
                            PointCollection points = polyBezierSegment.Points;

                            for (int i = 0; i < points.Count; i += 2)
                            {
                                cgPath.AddQuadCurveToPoint(transform,
                                                           new nfloat(points[i + 0].X),
                                                           new nfloat(points[i + 0].Y),
                                                           new nfloat(points[i + 1].X),
                                                           new nfloat(points[i + 1].Y));
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

                            CGPoint[] cgpoints = new CGPoint[points.Count];

                            for (int i = 0; i < points.Count; i++)
                            {
                                cgpoints[i] = transform.TransformPoint(ConvertPoint(points[i]));
                            }
                            cgPath.AddLines(cgpoints);

                            lastPoint = points[points.Count - 1];
                        }
                    }

                    if (pathFigure.IsClosed)
                    {
                        cgPath.CloseSubpath();
                    }
                }
            }

            return cgPath;
        }
    }
}
