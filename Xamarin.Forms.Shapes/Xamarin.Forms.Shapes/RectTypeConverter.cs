namespace Xamarin.Forms.Media
{
    // TODO: Deriving form RectangleTypeConverter in Xamarin.Forms is OK but it really should be modified to accept spaces.
    public class RectTypeConverter : RectangleTypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            Rectangle rectangle = (Rectangle)base.ConvertFromInvariantString(value);
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}
