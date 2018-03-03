using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    [ContentProperty("Figures")]
    public sealed class PathGeometry : Geometry
    {
        public PathGeometry()
        {
            Figures = new PathFigureCollection();
        }

        public static readonly BindableProperty FiguresProperty =
            BindableProperty.Create("Figures",
                                    typeof(PathFigureCollection),
                                    typeof(PathGeometry),
                                    null,
                                    propertyChanged: OnFiguresPropertyChanged);

        public static readonly BindableProperty FillRuleProperty =
            BindableProperty.Create("FillRule",
                                    typeof(FillRule),
                                    typeof(PathGeometry),
                                    FillRule.EvenOdd,
                                    propertyChanged: InvalidateNativeObject);

        [TypeConverter(typeof(PathFigureCollectionConverter))]
        public PathFigureCollection Figures
        {
            set { SetValue(FiguresProperty, value); }
            get { return (PathFigureCollection)GetValue(FiguresProperty); }
        }

        public FillRule FillRule
        {
            set { SetValue(FillRuleProperty, value); }
            get { return (FillRule)GetValue(FillRuleProperty); }
        }

        // Fire a PropertyChanged event for "Figures" when the collection changes, or 
        //      any item in the collection changes.
        static void OnFiguresPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as PathFigureCollection).CollectionChanged -= (bindable as PathGeometry).OnFiguresCollectionChanged;
                (oldValue as PathFigureCollection).ItemPropertyChanged -= (bindable as PathGeometry).OnFiguresItemChanged;
            }

            if (newValue != null)
            {
                (newValue as PathFigureCollection).CollectionChanged += (bindable as PathGeometry).OnFiguresCollectionChanged;
                (newValue as PathFigureCollection).ItemPropertyChanged += (bindable as PathGeometry).OnFiguresItemChanged;
            }

            InvalidateNativeObject(bindable, oldValue, newValue);
        }

        void OnFiguresCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            InvalidateNativeObject();
            OnPropertyChanged("Figures");
        }

        void OnFiguresItemChanged(object sender, ItemPropertyChangedEventArgs args)
        {
            InvalidateNativeObject();
            OnPropertyChanged("Figures");
        }
    }
}
