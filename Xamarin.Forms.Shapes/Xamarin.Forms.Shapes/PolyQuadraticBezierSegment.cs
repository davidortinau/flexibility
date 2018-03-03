using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    public sealed class PolyQuadraticBezierSegment : PathSegment
    {
        public PolyQuadraticBezierSegment()
        {
            Points = new PointCollection();
        }

        public static readonly BindableProperty PointsProperty =
            BindableProperty.Create("Points",
                                    typeof(PointCollection),
                                    typeof(PolyQuadraticBezierSegment),
                                    null,
                                    propertyChanged: OnPointsPropertyChanged);

        public PointCollection Points
        {
            set { SetValue(PointsProperty, value); }
            get { return (PointCollection)GetValue(PointsProperty); }
        }

        static void OnPointsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as PointCollection).CollectionChanged -= (bindable as PolyQuadraticBezierSegment).OnPointCollectionChanged;
            }
            if (newValue != null)
            {
                (newValue as PointCollection).CollectionChanged += (bindable as PolyQuadraticBezierSegment).OnPointCollectionChanged;
            }
        }

        void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged("Points");        // simulate Points property changed 
        }
    }
}
