using System;
using System.Collections.Specialized;
using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes
{
    public sealed class Polygon : Shape
    {
        public Polygon()
        {
            // TODO: What's the right way to do this? This or defaultValueCreator?
            // See Polyline also
   //         Points = new PointCollection();
        }

        public static readonly BindableProperty PointsProperty =
            BindableProperty.Create("Points",
                                    typeof(PointCollection),
                                    typeof(Polygon),
                                    null,
                                    defaultValueCreator: bindable => new PointCollection(),
                                    propertyChanged: OnPointsPropertyChanged);

        public static readonly BindableProperty FillRuleProperty =
            BindableProperty.Create("FillRule",
                                    typeof(FillRule),
                                    typeof(Polygon),
                                    FillRule.EvenOdd);

        public PointCollection Points
        {
            set { SetValue(PointsProperty, value); }
            get { return (PointCollection)GetValue(PointsProperty); }
        }

        public FillRule FillRule
        {
            set { SetValue(FillRuleProperty, value); }
            get { return (FillRule)GetValue(FillRuleProperty); }
        }

        static void OnPointsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as PointCollection).CollectionChanged -= (bindable as Polygon).OnPointCollectionChanged;
            }
            if (newValue != null)
            {
                (newValue as PointCollection).CollectionChanged += (bindable as Polygon).OnPointCollectionChanged;
            }

            OnGeometrySizePropertyChanged(bindable, oldValue, newValue);
        }

        void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnGeometrySizePropertyChanged();
            OnPropertyChanged("Points");        // simulate Points property changed 
        }
    }
}
