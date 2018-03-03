using System;

namespace Xamarin.Forms.Shapes
{
    // Ad hoc transform matrix for FlattenArc calculation

    // TODO: Replace with Matrix but need Invert

    class TempMatrix
    {
        double m11 = 1, m12 = 0, m13 = 0;
        double m21 = 0, m22 = 1, m23 = 0;
        double m31 = 0, m32 = 0, m33 = 1;

        public void Scale(double x, double y)
        {
            m11 *= x;
            m22 *= y;
        }

        public void Rotate(double angle)
        {
            double radians = Math.PI * angle / 180;

            double rot11 = m11 * Math.Cos(radians) - m12 * Math.Sin(radians);
            double rot12 = m11 * Math.Sin(radians) + m12 * Math.Cos(radians);
            double rot13 = m13;

            double rot21 = m21 * Math.Cos(radians) - m22 * Math.Sin(radians);
            double rot22 = m21 * Math.Sin(radians) + m22 * Math.Cos(radians);
            double rot23 = m23;

            double rot31 = m21 * Math.Cos(radians) - m32 * Math.Sin(radians);
            double rot32 = m31 * Math.Sin(radians) - m32 * Math.Cos(radians);
            double rot33 = m33;

            m11 = rot11;
            m12 = rot12;
            m21 = rot21;
            m22 = rot22;
            m31 = rot31;
            m32 = rot32; 

        }

        public Point Transform(Point pt)
        {
            double x = m11 * pt.X + m21 * pt.Y + m31;
            double y = m12 * pt.X + m22 * pt.Y + m32;
            return new Point(x, y);
        }

        public void Invert()
        {
            double det11 = m22 * m33 - m23 * m32;
            double det12 = m21 * m33 - m23 * m31;
            double det13 = m21 * m22 - m31 * m32;

            double det = m11 * m22 * m33 -
                         m11 * m23 * m32 -
                         m12 * m21 * m33 +
                         m12 * m23 * m31 +
                         m13 * m21 * m32 -
                         m13 * m22 * m31;

            det = m11 * det11 - m12 * det12 + m13 * det13;

            double det21 = m12 * m33 - m13 * m32;
            double det22 = m11 * m33 - m13 * m31;
            double det23 = m11 * m32 - m12 * m31;

            double det31 = m12 * m23 - m13 * m22;
            double det32 = m11 * m23 - m13 * m21;
            double det33 = m11 * m22 - m12 * m21;

            double adj11 = det11;
            double adj12 = -det21;
            double adj13 = det31;

            double adj21 = -det12;
            double adj22 = det22;
            double adj23 = -det32;

            double adj31 = det13;
            double adj32 = -det23;
            double adj33 = det33;

            m11 = adj11 / det;
            m12 = adj12 / det;
            m13 = adj13 / det;

            m21 = adj21 / det;
            m22 = adj22 / det;
            m23 = adj23 / det;

            m31 = adj31 / det;
            m32 = adj32 / det;
            m33 = adj33 / det;
        }
    }
}
