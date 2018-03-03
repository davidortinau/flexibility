using System;
using System.Globalization;

namespace Xamarin.Forms.Media
{
    // TODO: Deriving form PointTypeConverter in Xamarin.Forms is OK for now but it really should be modified to accept spaces.
    public class SizeTypeConverter : PointTypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            Point point = (Point)base.ConvertFromInvariantString(value);
            return new Size(point.X, point.Y);
        }
    }
}
