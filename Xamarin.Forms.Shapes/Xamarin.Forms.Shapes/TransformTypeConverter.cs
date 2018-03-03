namespace Xamarin.Forms.Media
{
    public class TransformTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            return new MatrixTransform
            {
                Matrix = MatrixTypeConverter.FillMatrix(value)
            };
        }
    }
}
