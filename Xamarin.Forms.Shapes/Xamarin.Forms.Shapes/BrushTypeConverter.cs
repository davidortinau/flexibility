namespace Xamarin.Forms.Media
{
    public class BrushTypeConverter : TypeConverter
    {
        ColorTypeConverter colorTypeConverter = new ColorTypeConverter();

        public override object ConvertFromInvariantString(string value)
        {
            return new SolidColorBrush()
            {
                Color = (Color)colorTypeConverter.ConvertFromInvariantString(value)
            };
        }
    }
}
