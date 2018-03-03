using System;
using System.Globalization;

namespace Xamarin.Forms.Media
{
    public class PathFigureCollectionConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            PathFigureCollection figures = new PathFigureCollection();

            FillFigures(figures, value, false);

            return figures;
        }


        // TODO: Change to private class, perhaps MoveAndDrawCommands.Parse
        // TODO: Implement allowFillMode
        // TODO: All the relative stuff
        // TODO: Finish implementation, dude!

        public static void FillFigures(PathFigureCollection figures, string str, bool allowFillMode)
        { 
            PathFigure pathFigure = null;

            int index = 0;
            Point currentPoint = new Point(0, 0);
            Point previousBezierCtrlPoint = new Point(0, 0);
            char lastCommand = ' ';

            // Reused in switch statement
            Point point = new Point(0, 0);
            PointCollection points = null;

            while (index < str.Length)
            {
                while (str[index] == ' ')
                    index++;

                bool isRelative = (Char.IsLower(str[index]));

                char command = str[index++];

                switch (command)
                {
                    // Fill mode
                    case 'F':

                        // TK

                        break;


                    // Move: start point
                    case 'M':
                    case 'm':
                        point = GetNextPoint(str, ref index);        // Can be multiple points!!!!
                                                                     //        context.BeginFigure(pt, false, true);
                        pathFigure = new PathFigure
                        {
                            StartPoint = AddPoint(point, currentPoint, isRelative)
                        };
                        figures.Add(pathFigure);
                        currentPoint = point;
                        break;

                    // Line: line(s) to
                    case 'L':
                    case 'l':
                        points = GetNextPoints(str, ref index);

                        if (points.Count == 0)
                            throw new FormatException(String.Format("Line command must have at least one point at offset{0}.", index));

                        if (points.Count == 1)
                        {
                            LineSegment lineSegment = new LineSegment
                            {
                                Point = AddPoint(points[0], currentPoint, isRelative)
                            };
                            pathFigure.Segments.Add(lineSegment);
                            currentPoint = points[0];
                        }
                        else
                        {
                            PolyLineSegment polylineSegment = new PolyLineSegment();

                            foreach (Point pt in points)
                            {
                                polylineSegment.Points.Add(AddPoint(pt, currentPoint, isRelative));
                            }
                            pathFigure.Segments.Add(polylineSegment);
                            currentPoint = points[0];
                        }
                        break;

                    // Horizontal line
                    case 'H':
                    case 'h':
                        currentPoint.X += GetNextDouble(str, ref index);
                        pathFigure.Segments.Add(new LineSegment
                        {
                            Point = currentPoint
                        });
                        break;

                    // Vertical line
                    case 'V':
                    case 'v':
                        currentPoint.Y += GetNextDouble(str, ref index);
                        pathFigure.Segments.Add(new LineSegment
                        {
                            Point = currentPoint
                        });
                        break;

                    // Cubic Bezier
                    case 'C':
                    case 'c':
                        points = GetNextPoints(str, ref index);

                        if (points.Count == 0 || points.Count % 3 != 0)
                            throw new FormatException(String.Format("Cubic bezier must have a number of points divisible by three at offset{0}.", index));

                        if (points.Count == 3)
                        {
                            BezierSegment bezierSegment = new BezierSegment
                            {
                                Point1 = points[0],                 /// Add for relative
                                Point2 = points[1],
                                Point3 = points[2]
                            };
                            pathFigure.Segments.Add(bezierSegment);
                            currentPoint = points[2];
                            previousBezierCtrlPoint = points[1];
                        }
                        else
                        {
                            PolyBezierSegment polyBezierSegment = new PolyBezierSegment
                            {
                                Points = points
                            };
                            pathFigure.Segments.Add(polyBezierSegment);
                            currentPoint = points[points.Count - 1];
                            previousBezierCtrlPoint = points[points.Count - 2];
                        }
                        break;

                    // S: Smooth Bezier continuation
                    case 'S':
                    case 's':
                        points = GetNextPoints(str, ref index);

                        if (points.Count == 0 || points.Count % 2 != 0)
                            throw new FormatException(String.Format("Smooth cubic bezier must have a number of points divisible by two at offset{0}.", index));

                        if (lastCommand != 'C' && lastCommand != 'c' && lastCommand != 'S' && lastCommand != 's')
                        {
                            previousBezierCtrlPoint = currentPoint;
                        }

                        for (int i = 0; i < points.Count; i += 2)
                        {
                            BezierSegment bezierSegment = new BezierSegment
                            {
                                Point1 = new Point(2 * currentPoint.X - previousBezierCtrlPoint.X,
                                                   2 * currentPoint.Y - previousBezierCtrlPoint.Y), /// TODO: Add for relative
                                Point2 = points[i + 0],
                                Point3 = points[i + 1]
                            };
                            pathFigure.Segments.Add(bezierSegment);
                            currentPoint = points[i + 1];
                            previousBezierCtrlPoint = points[i + 0];
                        }

                        break;

                    // A: Arc command
                    case 'A':
                    case 'a':
                        Point size = GetNextPoint(str, ref index);
                        double angle = GetNextDouble(str, ref index);
                        bool isLargeArc = GetNextBool(str, ref index);
                        bool isClockwise = GetNextBool(str, ref index);
                        point = GetNextPoint(str, ref index);

                        ArcSegment arcSegment = new ArcSegment
                        {
                            Size = new Size(size.X, size.Y),
                            RotationAngle = angle,
                            IsLargeArc = isLargeArc,
                            SweepDirection = isClockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise,
                            Point = AddPoint(point, currentPoint, isRelative)
                        };

                        pathFigure.Segments.Add(arcSegment);
                        currentPoint = point;

                        break;


                    // Z: close command
                    case 'Z':
                    case 'z':
                        pathFigure.IsClosed = true;
                        break;

                    default:
                        throw new FormatException(String.Format("Invalid character at offset {0}.", index));

                }

                lastCommand = command;
            }

       //     return pathGeometry;

     //       context.Close();
        //    return streamGeometry;
        }

        // Returns true if end of string
        static bool SkipWhiteSpace(string str, ref int index)
        {
            while (index < str.Length && (str[index] == ' ' || str[index] == '\t' || str[index] == '\r' || str[index] == '\n'))
                index++;

            return index == str.Length;
        }

        static bool SkipUntilWhiteSpaceOrComma(string str, ref int index)
        {
            while (index < str.Length && str[index] != ' ' && str[index] != '\t' && str[index] != '\r' && str[index] != '\n' && str[index] != ',')
                index++;

            return index == str.Length;
        }

        static double GetNextDouble(string str, ref int index)
        {
            if (SkipWhiteSpace(str, ref index))
                throw new FormatException(String.Format("Expecting a double at offset {0}", index));

            int index2 = index;

            SkipUntilWhiteSpaceOrComma(str, ref index2);

            if (index2 == index)
                throw new FormatException(String.Format("Expecting a double at offset {0}", index));

            double value;
            if (!Double.TryParse(str.Substring(index, index2 - index), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                throw new FormatException(String.Format("Expecting a double at offset {0}", index));

            index = index2;

            return value;
        }

        static Point GetNextPoint(string str, ref int index)
        {
            double x = GetNextDouble(str, ref index);

            if (SkipWhiteSpace(str, ref index))
                throw new FormatException(String.Format("Expecting the y coordinate of a Point at offset {0}", index));

            // Allow one comma between x and y
            if (index < str.Length && str[index] == ',')
                index++;

            double y = GetNextDouble(str, ref index);
            return new Point(x, y);
        }

        static PointCollection GetNextPoints(string str, ref int index)
        {
            PointCollection points = new PointCollection();

            points.Add(GetNextPoint(str, ref index));

            while (index < str.Length)
            {
                int saveIndex = index;

                try
                {
                    SkipWhiteSpace(str, ref index);

                    // Allow one comma between points
                    if (str[index] == ',')
                        index++;

                    points.Add(GetNextPoint(str, ref index));
                }
                catch
                {
                    index = saveIndex;
                    break;
                }
            }

            return points;
        }

        static bool GetNextBool(string str, ref int index)
        {
            if (SkipWhiteSpace(str, ref index))
                throw new FormatException(String.Format("Expecting a flag at offset {0}", index));

            char ch = str[index++];

            if (ch == '1')
                return true;

            if (ch == '0')
                return false;

            throw new FormatException(String.Format("Expecting a flag at offset {0}", index));
        }

        static Point AddPoint(Point pt1, Point pt2, bool addSecondPoint)
        {
            return new Point(pt1.X + (addSecondPoint ? pt2.X : 0),
                             pt1.Y + (addSecondPoint ? pt2.Y : 0));
        }
    }
}
