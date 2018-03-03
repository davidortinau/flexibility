using System.ComponentModel;
using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes
{
    public sealed class Path : Shape
    {
        public static readonly BindableProperty DataProperty =
            BindableProperty.Create("Data",
                                    typeof(Geometry),
                                    typeof(Path),
                                    null,
                                    propertyChanged: OnDataPropertyChanged);

        [TypeConverter(typeof(PathGeometryConverter))]
        public Geometry Data
        {
            set { SetValue(DataProperty, value); }
            get { return (Geometry)GetValue(DataProperty); }
        }

        static void OnDataPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as Geometry).PropertyChanged -= (bindable as Path).OnGeometryPropertyChanged;
            }
            if (newValue != null)
            {
                (newValue as Geometry).PropertyChanged += (bindable as Path).OnGeometryPropertyChanged;
            }

            // Implemented in Shape.
            OnGeometrySizePropertyChanged(bindable, oldValue, newValue);
        }

        void OnGeometryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnGeometrySizePropertyChanged();
            OnPropertyChanged("Data");
        }
    }
}
