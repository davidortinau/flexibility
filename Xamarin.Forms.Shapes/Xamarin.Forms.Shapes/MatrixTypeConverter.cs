using System;

namespace Xamarin.Forms.Media
{
    public class MatrixTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            return FillMatrix(value);
        }

        internal static Matrix FillMatrix(string str)
        {
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException("MatrixTypeConverter requires non-empty string");

            string[] strs = str.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length != 6)
                throw new ArgumentException("MatrixTypeConverter requires six numbers");

            double[] values = new double[6];

            for (int i = 0; i < 6; i++)
                if (!Double.TryParse(strs[i], out values[i]))
                    throw new ArgumentException("MatrixTypeConverter requires numeric values");

            return new Matrix(values[0], values[1], values[2], values[3], values[4], values[5]);
        }
    }
}
