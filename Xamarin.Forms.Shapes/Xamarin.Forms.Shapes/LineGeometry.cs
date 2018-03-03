namespace Xamarin.Forms.Media
{
    public sealed class LineGeometry : Geometry
    {
        public static readonly BindableProperty StartPointProperty =
            BindableProperty.Create("StartPoint",
                                    typeof(Point),
                                    typeof(LineGeometry),
                                    new Point(),
                                    propertyChanged: OnGeometryPropertyChanged);

        public static readonly BindableProperty EndPointProperty =
            BindableProperty.Create("EndPoint",
                                    typeof(Point),
                                    typeof(LineGeometry),
                                    new Point(),
                                    propertyChanged: OnGeometryPropertyChanged);

        public Point StartPoint
        {
            set { SetValue(StartPointProperty, value); }
            get { return (Point)GetValue(StartPointProperty); }
        }

        public Point EndPoint
        {
            set { SetValue(StartPointProperty, value); }
            get { return (Point)GetValue(StartPointProperty); }
        }
    }
}
