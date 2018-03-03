using System;
using System.Globalization;

namespace Xamarin.Forms.Media
{
    public class PointCollectionConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            string[] strs = (value).Split(new char[] { ' ', ',' });     // Want to avoid double commas.
            PointCollection pointCollection = new PointCollection();
            double x = 0;
            bool hasX = false; 

            foreach (string str in strs)
            {
                if (String.IsNullOrWhiteSpace(str))
                    continue;

                double number;

                if (Double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out number))
                {
                    if (!hasX)
                    {
                        x = number;
                        hasX = true;
                    }
                    else
                    {
                        pointCollection.Add(new Point(x, number));
                        hasX = false;
                    }
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Cannot convert \"{0}\" into {1}", str, typeof(Double)));
                }
            }

            // Check for odd number
            if (hasX)
                throw new InvalidOperationException(String.Format("Cannot convert string into PointCollection"));

            return pointCollection;

        }
    }
}
