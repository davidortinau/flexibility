namespace Xamarin.Forms.Media
{
    public sealed class LinearGradientBrush : GradientBrush
    {
        public static readonly BindableProperty StartPointProperty =
            BindableProperty.Create("StartPoint",
                                    typeof(Point),
                                    typeof(LinearGradientBrush),
                                    new Point(0, 0),
                                    propertyChanged: InvalidateNativeObject);

        public static readonly BindableProperty EndPointProperty =
            BindableProperty.Create("EndPoint",
                                    typeof(Point),
                                    typeof(LinearGradientBrush),
                                    new Point(1, 1),
                                    propertyChanged:InvalidateNativeObject);

        public Point StartPoint
        {
            set { SetValue(StartPointProperty, value); }
            get { return (Point)GetValue(StartPointProperty); }
        }

        public Point EndPoint
        {
            set { SetValue(EndPointProperty, value); }
            get { return (Point)GetValue(EndPointProperty); }
        }
    }
}
