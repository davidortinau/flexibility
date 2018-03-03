using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    [ContentProperty("Children")]
    public sealed class GeometryGroup : Geometry
    {
        public static readonly BindableProperty ChildrenProperty =
            BindableProperty.Create("Children",
                                    typeof(GeometryCollection),
                                    typeof(GeometryGroup),
                                    null,
                                    propertyChanged: OnChildrenPropertyChanged);

        public static readonly BindableProperty FillRuleProperty =
            BindableProperty.Create("FillRule",
                                    typeof(FillRule),
                                    typeof(GeometryGroup),
                                    FillRule.EvenOdd,
                                    propertyChanged: OnGeometryPropertyChanged);

        public GeometryGroup()
        {
            Children = new GeometryCollection();
        }

        public GeometryCollection Children
        {
            set { SetValue(ChildrenProperty, value); }
            get { return (GeometryCollection)GetValue(ChildrenProperty); }
        }

        public FillRule FillRule
        {
            set { SetValue(FillRuleProperty, value); }
            get { return (FillRule)GetValue(FillRuleProperty); }
        }

        // This is called when the Children property itself changes.
        static void OnChildrenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as GeometryCollection).CollectionChanged -= (bindable as GeometryGroup).OnChildrenCollectionChanged;
                (oldValue as GeometryCollection).ItemPropertyChanged -= (bindable as GeometryGroup).OnChildrenItemChanged;
            }
            if (newValue != null)
            {
                (newValue as GeometryCollection).CollectionChanged += (bindable as GeometryGroup).OnChildrenCollectionChanged;
                (newValue as GeometryCollection).ItemPropertyChanged += (bindable as GeometryGroup).OnChildrenItemChanged;
            }
        }

        // This is called when items are added to or removed from the collection.
        void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnGeometryPropertyChanged();
        }

        // This is called when a property of an item in the collection changes
        void OnChildrenItemChanged(object sender, ItemPropertyChangedEventArgs args)
        {
            OnGeometryPropertyChanged();
        }
    }
}
