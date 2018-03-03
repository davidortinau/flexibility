using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    [ContentPropertyAttribute("GradientStops")]
    public class GradientBrush : Brush
    {
        public static readonly BindableProperty GradientStopsProperty =
            BindableProperty.Create("GradientStops",
                                    typeof(GradientStopCollection),
                                    typeof(GradientBrush),
                                    null,
                                    //defaultValueCreator: bindable => new GradientStopCollection(),
                                    propertyChanged: InvalidateCollection);

        public static readonly BindableProperty SpreadMethodProperty =
            BindableProperty.Create("SpreadMethod",
                                    typeof(GradientSpreadMethod),
                                    typeof(GradientBrush),
                                    GradientSpreadMethod.Pad,
                                    propertyChanged: InvalidateNativeObject);

        public GradientBrush()
        {
            GradientStops = new GradientStopCollection();
        }

        public GradientStopCollection GradientStops
        {
            set { SetValue(GradientStopsProperty, value); }
            get { return (GradientStopCollection)GetValue(GradientStopsProperty); }
        }

        public GradientSpreadMethod SpreadMethod
        {
            set { SetValue(SpreadMethodProperty, value); }
            get { return (GradientSpreadMethod)GetValue(SpreadMethodProperty); }
        }

        static void InvalidateCollection(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GradientBrush).InvalidateCollection(oldValue, newValue);
        }

        void InvalidateCollection(object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as GradientStopCollection).CollectionChanged -= OnCollectionChanged;
            }
            if (newValue != null)
            {
                (newValue as GradientStopCollection).CollectionChanged += OnCollectionChanged;
            }

            InvalidateNativeObject();
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            InvalidateNativeObject();
        }
    }
}
