using System;
using System.Globalization;

namespace Xamarin.Forms.Media
{
    public class DoubleCollectionConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            string[] strs = (value).Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            DoubleCollection doubleCollection = new DoubleCollection();

            foreach (string str in strs)
            {
                double number;

                if (Double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out number))
                {
                    doubleCollection.Add(number);
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Cannot convert \"{0}\" into {1}", str, typeof(Double)));
                }
            }

            return doubleCollection;
        }
    }
}
