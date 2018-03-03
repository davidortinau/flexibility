namespace Xamarin.Forms.Media
{
    public sealed class GradientStop : BindableObject
    {
        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create("Color",
                                    typeof(Color),
                                    typeof(GradientStop),
                                    Color.Transparent);

        public static readonly BindableProperty OffsetProperty =
            BindableProperty.Create("Offset",
                                    typeof(double),
                                    typeof(GradientStop),
                                    0.0);

        public Color Color
        {
            set { SetValue(ColorProperty, value); }
            get { return (Color)GetValue(ColorProperty); }
        }

        public double Offset
        {
            set { SetValue(OffsetProperty, value); }
            get { return (double)GetValue(OffsetProperty); }
        }
    }
}
