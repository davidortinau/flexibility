using System;

namespace Xamarin.Forms.Media
{
    [TypeConverter(typeof(MatrixTypeConverter))]
    public struct Matrix
    {
        double m11, m22;

        public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY) : this()
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public double M11
        {
            set { m11 = value - 1; }
            get { return m11 + 1; }
        }

        public double M12
        {
            set; get;
        }

        public double M21
        {
            set; get;
        }

        public double M22
        {
            set { m22 = value - 1; }
            get { return m22 + 1; }
        }

        public double OffsetX
        {
            set; get;
        }

        public double OffsetY
        {
            set; get;
        }

        public bool IsIdentity
        {
            get
            {
                return M11 == 1 && M12 == 0 && M21 == 0 && M22 == 1 && OffsetX == 0 && OffsetY == 0;
            }
        }

        public static Matrix Identity
        {
            get { return new Matrix(); }
        }

        public Point Transform(Point pt)
        {
            return new Point(M11 * pt.X + M21 * pt.Y + OffsetX,
                             M12 * pt.X + M22 * pt.Y + OffsetY);
        }

        internal static Matrix Multiply(Matrix A, Matrix B)
        {
            if (A.IsIdentity)
                return B;

            if (B.IsIdentity)
                return A;

            Matrix product = new Matrix(A.M11 * B.M11 + A.M12 * B.M21,
                                        A.M11 * B.M12 + A.M12 * B.M22,
                                        A.M21 * B.M11 + A.M22 * B.M21,
                                        A.M21 * B.M12 + A.M22 * B.M22,
                                        A.OffsetX * B.M11 + A.OffsetY * B.M21 + B.OffsetX,
                                        A.OffsetX * B.M12 + A.OffsetY * B.M22 + B.OffsetY);
            return product;
        }


        // TODO: Equals, Equality and Inequality operators, ToString


    }
}
