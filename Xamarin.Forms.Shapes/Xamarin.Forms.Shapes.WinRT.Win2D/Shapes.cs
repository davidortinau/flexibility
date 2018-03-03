using System;
using System.Collections.Generic;
using System.Text;

using winFound = Windows.Foundation;
using Xamarin.Forms.Media;
using System.Numerics;

namespace Xamarin.Forms.Shapes.WinRT
{
    public static class Shapes
    {
        public static void Init()
        {
        }

        public static Vector2[] ConvertPoints(PointCollection srcPoints)
        {
            if (srcPoints == null)
            {
                return new Vector2[0];
            }

            Vector2[] dstPoints = new Vector2[srcPoints.Count];

            for (int i = 0; i < srcPoints.Count; i++)
            {
                dstPoints[i] = new Vector2((float)srcPoints[i].X, (float)srcPoints[i].Y);
            }

            return dstPoints;
        }
    }
}
