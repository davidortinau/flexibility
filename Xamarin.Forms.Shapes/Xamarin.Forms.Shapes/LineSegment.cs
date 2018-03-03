namespace Xamarin.Forms.Media
{
    public sealed class LineSegment : PathSegment
    {
        public static readonly BindableProperty PointProperty =
            BindableProperty.Create("Point",
                                    typeof(Point),
                                    typeof(LineSegment),
                                    new Point(0, 0));

        public Point Point
        {
            set { SetValue(PointProperty, value); }
            get { return (Point)GetValue(PointProperty); }
        }
    }
}
