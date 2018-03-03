using System;

namespace Xamarin.Forms.Media
{
    // The UWP Rect is in Windows.Foundation

    [TypeConverter(typeof(RectTypeConverter))] 
    public struct Rect
    {
        double width, height;

        public Rect(double x, double y, double width, double height) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect(Point point1, Point point2) : this()
        {
            // See https://msdn.microsoft.com/en-us/library/windows/apps/hh763880.aspx
            //  for definition that Rect must contain both points.
            Left = Math.Min(point1.X, point2.X);
            Top = Math.Min(point1.Y, point2.Y);
            Right = Math.Max(point1.X, point2.Y);
            Bottom = Math.Max(point1.Y, point2.Y);
        }

        public Rect(Point point, Size size) : this()
        {
            X = point.X;
            Y = point.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public double X
        {
            set; get;
        }

        public double Y
        {
            set; get;
        }

        public double Width
        {
            set
            {
                if (value < 0)
                    throw new ArgumentException("Rect Width cannot be less than zero.");

                width = value;
            }
            get
            {
                return width;
            }
        }

        public double Height
        {
            set
            {
                if (value < 0)
                    throw new ArgumentException("Rect Height cannot be less than zero.");

                height = value;
            }
            get
            {
                return height;
            }
        }

        public double Left
        {
            set { X = value; }
            get { return X; }
        }

        public double Top
        {
            set { Y = value; }
            get { return Y; }
        }

        public double Right
        {
            set { Width = value - X; }
            get { return X + Width; }
        }

        public double Bottom
        {
            set { Height = value - Y; }
            get { return Y + Height; }
        }

        public bool IsEmpty
        {
            get { return Width == 0 || Height == 0; }
        }



        // TODO: Equaliy, Inequality, ToString, Methods
    }
}
