using winFound = Windows.Foundation;
using winMedia = Windows.UI.Xaml.Media;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.WinRT.NativeGeometry))]

namespace Xamarin.Forms.Media.WinRT
{
    public class NativeGeometry : NativeObject, INativeGeometry
    {
        public object ConvertToNative(Geometry geometry)
        {
            winMedia.Geometry winGeometry = null;

            // Determine what type of geometry we're dealing with.
            if (geometry is LineGeometry)
            {
                LineGeometry xamGeometry = geometry as LineGeometry;
                winGeometry = new winMedia.LineGeometry
                {
                    StartPoint = ConvertPoint(xamGeometry.StartPoint),
                    EndPoint = ConvertPoint(xamGeometry.EndPoint)
                };
            }

            else if (geometry is RectangleGeometry)
            {
                Rect rect = (geometry as RectangleGeometry).Rect;
                winGeometry = new winMedia.RectangleGeometry
                {
                    Rect = new winFound.Rect(rect.X, rect.Y, rect.Width, rect.Height)
                };
            }

            else if (geometry is EllipseGeometry)
            {
                EllipseGeometry xamGeometry = geometry as EllipseGeometry;
                winGeometry = new winMedia.EllipseGeometry
                {
                    Center = ConvertPoint(xamGeometry.Center),
                    RadiusX = xamGeometry.RadiusX,
                    RadiusY = xamGeometry.RadiusY
                };
            }

            else if (geometry is GeometryGroup)
            {
                GeometryGroup xamGeometry = geometry as GeometryGroup;
                winGeometry = new winMedia.GeometryGroup
                {
                    FillRule = ConvertFillRule(xamGeometry.FillRule)
                };

                foreach (Geometry xamChild in xamGeometry.Children)
                {
                    winMedia.Geometry winChild = ConvertToNative(xamChild) as winMedia.Geometry;
                    (winGeometry as winMedia.GeometryGroup).Children.Add(winChild);
                }
            }

            else if (geometry is PathGeometry)
            {
                PathGeometry xamPathGeometry = geometry as PathGeometry;

                winMedia.PathGeometry winPathGeometry = new winMedia.PathGeometry
                {
                    FillRule = ConvertFillRule(xamPathGeometry.FillRule)
                };

                foreach (PathFigure xamPathFigure in xamPathGeometry.Figures)
                {
                    winMedia.PathFigure winPathFigure = new winMedia.PathFigure
                    {
                        StartPoint = ConvertPoint(xamPathFigure.StartPoint),
                        IsFilled = xamPathFigure.IsFilled,
                        IsClosed = xamPathFigure.IsClosed
                    };
                    winPathGeometry.Figures.Add(winPathFigure);

                    foreach (PathSegment xamPathSegment in xamPathFigure.Segments)
                    {
                        // LineSegment
                        if (xamPathSegment is LineSegment)
                        {
                            LineSegment xamSegment = xamPathSegment as LineSegment;

                            winMedia.LineSegment winSegment = new winMedia.LineSegment
                            {
                                Point = ConvertPoint(xamSegment.Point)
                            };

                            winPathFigure.Segments.Add(winSegment);
                        }

                        // PolylineSegment
                        if (xamPathSegment is PolyLineSegment)
                        {
                            PolyLineSegment xamSegment = xamPathSegment as PolyLineSegment;
                            winMedia.PolyLineSegment winSegment = new winMedia.PolyLineSegment();

                            foreach (Point point in xamSegment.Points)
                            {
                                winSegment.Points.Add(ConvertPoint(point));
                            }

                            winPathFigure.Segments.Add(winSegment);
                        }

                        // BezierSegment
                        if (xamPathSegment is BezierSegment)
                        {
                            BezierSegment xamSegment = xamPathSegment as BezierSegment;

                            winMedia.BezierSegment winSegment = new winMedia.BezierSegment
                            {
                                Point1 = ConvertPoint(xamSegment.Point1),
                                Point2 = ConvertPoint(xamSegment.Point2),
                                Point3 = ConvertPoint(xamSegment.Point3)
                            };

                            winPathFigure.Segments.Add(winSegment);
                        }

                        // PolyBezierSegment
                        else if (xamPathSegment is PolyBezierSegment)
                        {
                            PolyBezierSegment xamSegment = xamPathSegment as PolyBezierSegment;
                            winMedia.PolyBezierSegment winSegment = new winMedia.PolyBezierSegment();

                            foreach (Point point in xamSegment.Points)
                            {
                                winSegment.Points.Add(ConvertPoint(point));
                            }

                            winPathFigure.Segments.Add(winSegment);
                        }

                        // QuadraticBezierSegment
                        if (xamPathSegment is QuadraticBezierSegment)
                        {
                            QuadraticBezierSegment xamSegment = xamPathSegment as QuadraticBezierSegment;

                            winMedia.QuadraticBezierSegment winSegment = new winMedia.QuadraticBezierSegment
                            {
                                Point1 = ConvertPoint(xamSegment.Point1),
                                Point2 = ConvertPoint(xamSegment.Point2),
                            };

                            winPathFigure.Segments.Add(winSegment);
                        }

                        // PolyQuadraticBezierSegment
                        else if (xamPathSegment is PolyQuadraticBezierSegment)
                        {
                            PolyQuadraticBezierSegment xamSegment = xamPathSegment as PolyQuadraticBezierSegment;
                            winMedia.PolyQuadraticBezierSegment winSegment = new winMedia.PolyQuadraticBezierSegment();

                            foreach (Point point in xamSegment.Points)
                            {
                                winSegment.Points.Add(ConvertPoint(point));
                            }

                            winPathFigure.Segments.Add(winSegment);
                        }


                        // ArcSegment
                        else if (xamPathSegment is ArcSegment)
                        {
                            ArcSegment xamSegment = xamPathSegment as ArcSegment;
                            winMedia.ArcSegment winSegment = new winMedia.ArcSegment();

                            winSegment.Size = new winFound.Size(xamSegment.Size.Width, xamSegment.Size.Height);
                            winSegment.RotationAngle = xamSegment.RotationAngle;
                            winSegment.IsLargeArc = xamSegment.IsLargeArc;
                            winSegment.SweepDirection = xamSegment.SweepDirection == SweepDirection.Clockwise ? winMedia.SweepDirection.Clockwise : winMedia.SweepDirection.Counterclockwise;
                            winSegment.Point = ConvertPoint(xamSegment.Point);

                            winPathFigure.Segments.Add(winSegment);
                        }
                    }
                }

                winGeometry = winPathGeometry;
            }

            // Set transform.
            if (geometry.Transform != null)
            {
                winGeometry.Transform = (winMedia.Transform)geometry.Transform.GetNativeObject();
            }

            return winGeometry;
        }

        winMedia.FillRule ConvertFillRule(FillRule fillRule)
        {
            return fillRule == FillRule.EvenOdd ? winMedia.FillRule.EvenOdd : winMedia.FillRule.Nonzero;
        }
    }
}
