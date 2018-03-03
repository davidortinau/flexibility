namespace Xamarin.Forms.Media
{
    public sealed class SolidColorBrush : Brush
    {
        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create("Color", 
                                    typeof(Color), 
                                    typeof(SolidColorBrush), 
                                    Color.Transparent,
                                    propertyChanged: InvalidateNativeObject);

        public Color Color
        {
            set { SetValue(ColorProperty, value); }
            get { return (Color)GetValue(ColorProperty); }
        }
    }
}
