namespace Xamarin.Forms.Shapes
{
    public sealed class Rectangle : Shape
    {
        public static readonly BindableProperty RadiusXProperty =
            BindableProperty.Create("RadiusX",
                                    typeof(double),
                                    typeof(Rectangle),
                                    0.0);

        public static readonly BindableProperty RadiusYProperty =
            BindableProperty.Create("RadiusY",
                                    typeof(double),
                                    typeof(Rectangle),
                                    0.0);

        public Rectangle()
        {
            Stretch = Xamarin.Forms.Media.Stretch.Fill;
        }

        public double RadiusX
        {
            set { SetValue(RadiusXProperty, value); }
            get { return (double)GetValue(RadiusXProperty); }
        }

        public double RadiusY
        {
            set { SetValue(RadiusYProperty, value); }
            get { return (double)GetValue(RadiusYProperty); }
        }
    }
}
