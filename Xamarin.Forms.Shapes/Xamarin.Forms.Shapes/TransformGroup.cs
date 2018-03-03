using System;
using System.Collections.Specialized;

namespace Xamarin.Forms.Media
{
    [ContentProperty("Children")]
    public sealed class TransformGroup : Transform
    {
        public static readonly BindableProperty ChildrenProperty =
            BindableProperty.Create("Children",
                                    typeof(TransformCollection),
                                    typeof(TransformGroup),
                                    null,
                                    propertyChanged: OnTransformGroupChanged);

        public TransformGroup()
        {
            Children = new TransformCollection();
        }

        public TransformCollection Children
        {
            set { SetValue(ChildrenProperty, value); }
            get { return (TransformCollection)GetValue(ChildrenProperty); }
        }

        // Called when the Children property itself changes, i.e., when a new TransformCollection is set
        static void OnTransformGroupChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as TransformCollection).CollectionChanged -= (bindable as TransformGroup).OnChildrenCollectionChanged;
                (oldValue as TransformCollection).ItemPropertyChanged -= (bindable as TransformGroup).OnChildItemPropertyChanged;
            }

            if (newValue != null)
            {
                (newValue as TransformCollection).CollectionChanged += (bindable as TransformGroup).OnChildrenCollectionChanged;
                (newValue as TransformCollection).ItemPropertyChanged += (bindable as TransformGroup).OnChildItemPropertyChanged;
            }

            (bindable as TransformGroup).RecalculateTransformMatrix();
        }

        // Called when Transform items are added to or removed from the collection.
        void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RecalculateTransformMatrix();
        }

        // Called when the Value property of any Transform child changes.
        void OnChildItemPropertyChanged(object sender, ItemPropertyChangedEventArgs args)
        {
            if (args.PropertyName == GeneralTransform.ValueProperty.PropertyName)
            {
                RecalculateTransformMatrix();
            }
        }

        void RecalculateTransformMatrix()
        {
            Matrix matrix = new Matrix();

            foreach (Transform child in Children)
            {
                matrix = Matrix.Multiply(matrix, child.Value);
            }

            Value = matrix;
        }
    }
}
