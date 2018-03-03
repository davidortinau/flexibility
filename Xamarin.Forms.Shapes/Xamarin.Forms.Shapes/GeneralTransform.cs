namespace Xamarin.Forms.Media
{
    public class GeneralTransform : BindableObject
    {
        protected object nativeObject = null;

        internal static readonly BindableProperty ValueProperty =
            BindableProperty.Create("Value",
                                    typeof(Matrix),
                                    typeof(GeneralTransform),
                                    new Matrix(),
                                    propertyChanged: InvalidateNativeObject);

        protected GeneralTransform()
        {
        }

        // This Value property is an inconsistency with Microsoft implementations:
        //  The Value property is only defined by TransformGroup
        //  and is not backed by a bindable property.
        public Matrix Value
        {
            set { SetValue(ValueProperty, value); }
            get { return (Matrix)GetValue(ValueProperty); }
        }

        public Rect TransformBounds(Rect rect)
        {
            return new Rect(Value.Transform(new Point(rect.X, rect.Y)),
                            Value.Transform(new Point(rect.Right, rect.Bottom)));
        }

        public Point TransformPoint(Point point)
        {
            return Value.Transform(point);
        }

        // TODO: Other transform methods




        public object GetNativeObject()
        {
            if (nativeObject == null)
            {
                INativeTransform nativeTransform = DependencyService.Get<INativeTransform>();
                nativeObject = nativeTransform.ConvertToNative(this);
            }

            return nativeObject;
        }

        protected static void InvalidateNativeObject(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Transform).InvalidateNativeObject();
        }

        protected void InvalidateNativeObject()
        {
            nativeObject = null;
        }

    }
}
