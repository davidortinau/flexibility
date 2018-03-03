using System;
using System.Collections.Generic;
using System.Text;

using winFound = Windows.Foundation;
using winMedia = Windows.UI.Xaml.Media;
using formsMedia = Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes.WinRT
{
    public static class Shapes
    {
        public static void Init()
        {
        }

        public static winMedia.PointCollection ConvertPoints(formsMedia.PointCollection srcPoints)
        {
            if (srcPoints == null || srcPoints.Count == 0)
            {
                return new winMedia.PointCollection();
            }

            winMedia.PointCollection dstPoints = new winMedia.PointCollection();
            Point[] array = new Point[srcPoints.Count];
            srcPoints.CopyTo(array, 0);

            foreach (Point point in array)
            {
                dstPoints.Add(new winFound.Point(point.X, point.Y));
            }

            return dstPoints;
        }
    }
}
