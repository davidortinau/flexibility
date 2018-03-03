namespace Xamarin.Forms.Media
{
    public class TileBrush : Brush
    {
        public static readonly BindableProperty AlignmentXProperty =
            BindableProperty.Create("AlignmentX",
                                    typeof(AlignmentX),
                                    typeof(TileBrush),
                                    AlignmentX.Center,
                                    propertyChanged: InvalidateNativeObject);

        public static readonly BindableProperty AlignmentYProperty =
            BindableProperty.Create("AlignmentY",
                                    typeof(AlignmentX),
                                    typeof(TileBrush),
                                    AlignmentX.Center,
                                    propertyChanged: InvalidateNativeObject);

        public static readonly BindableProperty StretchProperty =
            BindableProperty.Create("Stretch",
                                    typeof(Stretch),
                                    typeof(TileBrush),
                                    Stretch.Fill,
                                    propertyChanged: InvalidateNativeObject);

        public AlignmentX AlignmentX
        {
            set { SetValue(AlignmentXProperty, value); }
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
        }

        public AlignmentY AlignmentY
        {
            set { SetValue(AlignmentYProperty, value); }
            get { return (AlignmentY)GetValue(AlignmentYProperty); }
        }

        public Stretch Stretch
        {
            set { SetValue(StretchProperty, value); }
            get { return (Stretch)GetValue(StretchProperty); }
        }
    }
}
