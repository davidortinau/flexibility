using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    public sealed class PolyLineSegment : PathSegment
    {
        public PolyLineSegment()
        {
            Points = new PointCollection();
        }

        public static readonly BindableProperty PointsProperty =
            BindableProperty.Create("Points",
                                    typeof(PointCollection),
                                    typeof(PolyLineSegment),
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
                (oldValue as PointCollection).CollectionChanged -= (bindable as PolyLineSegment).OnPointCollectionChanged;
            }
            if (newValue != null)
            {
                (newValue as PointCollection).CollectionChanged += (bindable as PolyLineSegment).OnPointCollectionChanged;
            }
        }

        void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged("Points");        // simulate Points property changed 
        }
    }
}
