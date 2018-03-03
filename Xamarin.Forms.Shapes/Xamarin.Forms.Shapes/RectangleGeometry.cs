namespace Xamarin.Forms.Media
{
    public sealed class RectangleGeometry : Geometry
    {
        public static readonly BindableProperty RectProperty =
            BindableProperty.Create("Rect",
                                    typeof(Rect),
                                    typeof(RectangleGeometry),
                                    new Rect(),
                                    propertyChanged: OnGeometryPropertyChanged);

        public Rect Rect
        {
            set { SetValue(RectProperty, value); }
            get { return (Rect)GetValue(RectProperty); }
        }
    }
}
