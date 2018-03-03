namespace Xamarin.Forms.Media
{
    public sealed class EllipseGeometry : Geometry
    {
        public static readonly BindableProperty CenterProperty =
            BindableProperty.Create("Center",
                                    typeof(Point),
                                    typeof(EllipseGeometry),
                                    new Point(),
                                    propertyChanged: OnGeometryPropertyChanged);

        public static readonly BindableProperty RadiusXProperty =
            BindableProperty.Create("RadiusX",
                                    typeof(double),
                                    typeof(EllipseGeometry),
                                    0.0,
                                    propertyChanged: OnGeometryPropertyChanged);

        public static readonly BindableProperty RadiusYProperty =
            BindableProperty.Create("RadiusY",
                                    typeof(double),
                                    typeof(EllipseGeometry),
                                    0.0,
                                    propertyChanged: OnGeometryPropertyChanged);

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
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
