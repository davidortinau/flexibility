using System;
using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    [ContentProperty("Segments")]
    public sealed class PathFigure : BindableObject, IAnimatable
    {
        public PathFigure()
        {
            Segments = new PathSegmentCollection();
        }


        public static readonly BindableProperty SegmentsProperty =
            BindableProperty.Create("Segments",
                                    typeof(PathSegmentCollection),
                                    typeof(PathFigure),
                                    null,
                                    propertyChanged: OnSegmentsPropertyChanged);

        public static readonly BindableProperty StartPointProperty =
            BindableProperty.Create("StartPoint",
                                    typeof(Point),
                                    typeof(PathFigure),
                                    new Point(0, 0));

        public static readonly BindableProperty IsClosedProperty =
            BindableProperty.Create("IsClosed",
                                    typeof(bool),
                                    typeof(PathFigure),
                                    false);

        public static readonly BindableProperty IsFilledProperty =
            BindableProperty.Create("IsFilled",
                                    typeof(bool),
                                    typeof(PathFigure),
                                    true);

        public PathSegmentCollection Segments
        {
            set { SetValue(SegmentsProperty, value); }
            get { return (PathSegmentCollection)GetValue(SegmentsProperty); }
        }

        public Point StartPoint
        {
            set { SetValue(StartPointProperty, value); }
            get { return (Point)GetValue(StartPointProperty); }
        }

        public bool IsClosed
        {
            set { SetValue(IsClosedProperty, value); }
            get { return (bool)GetValue(IsClosedProperty); }
        }

        public bool IsFilled
        {
            set { SetValue(IsFilledProperty, value); }
            get { return (bool)GetValue(IsFilledProperty); }
        }

        public void BatchBegin()
        {
            
        }

        public void BatchCommit()
        {
            
        }

        static void OnSegmentsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as PathSegmentCollection).CollectionChanged -= (bindable as PathFigure).OnSegmentsCollectionChanged;
                (oldValue as PathSegmentCollection).ItemPropertyChanged -= (bindable as PathFigure).OnSegmentsItemChanged;
            }

            if (newValue != null)
            {
                (newValue as PathSegmentCollection).CollectionChanged += (bindable as PathFigure).OnSegmentsCollectionChanged;
                (newValue as PathSegmentCollection).ItemPropertyChanged += (bindable as PathFigure).OnSegmentsItemChanged;
            }
        }

        void OnSegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged("Segments");
        }

        void OnSegmentsItemChanged(object sender, ItemPropertyChangedEventArgs args)
        {
            OnPropertyChanged("Segments");
        }
    }
}
