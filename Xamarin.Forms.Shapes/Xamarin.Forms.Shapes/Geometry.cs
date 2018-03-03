using System.ComponentModel;

namespace Xamarin.Forms.Media
{
    public class Geometry : BindableObject
    {
        protected object nativeObject = null;

        public static readonly BindableProperty TransformProperty =
            BindableProperty.Create("Transform",
                                    typeof(Transform),
                                    typeof(Geometry),
                                    null, 
                                    propertyChanged:OnTransformPropertyChanged);

        public Transform Transform
        {
            set { SetValue(TransformProperty, value); }
            get { return (Transform)GetValue(TransformProperty); }
        }

        // Called from descendent classes when a property of the geometry changes.
        protected static void OnGeometryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            InvalidateNativeObject(bindable, oldValue, newValue);
        }

        protected void OnGeometryPropertyChanged()
        {
            InvalidateNativeObject();
        }

        // Called when the Transform property itself changes.
        static void OnTransformPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as Transform).PropertyChanged -= (bindable as Geometry).OnTransformPropertyChanged;
            }
            if (newValue != null)
            {
                (newValue as Transform).PropertyChanged += (bindable as Geometry).OnTransformPropertyChanged;
            }

            // Invalidate the whole geometry!
            //  This might seem to be overkill because you could retain the untransformed 
            //  geometry and just apply a new transform. And that would be OK in many 
            //  cases except that it would be a total mess with GeometryGroup.
            InvalidateNativeObject(bindable, oldValue, newValue);
        }

        // Called when a property of the Transform object changes.
        void OnTransformPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // This is the PropertyChanged event fired from GeneralTransform.
            if (args.PropertyName == GeneralTransform.ValueProperty.PropertyName)
            {
                // Invalidating the whole geometry!
                InvalidateNativeObject();
                OnPropertyChanged("Transform");     // "Geometry.Transform" ??? 
            }
        }

        // TODO
        // To cache or not to cache. 
        // The problem is that Windows will raise a tough-to-diagnose error if the same
        //  PathGeometry object is used in multiple Path objects,
        //  such as in the Flower demo

        public object GetNativeObject()
        {
   //         if (nativeObject == null)
            {
                INativeGeometry nativeGraphicsMedia = DependencyService.Get<INativeGeometry>();
                nativeObject = nativeGraphicsMedia.ConvertToNative(this);
            }

            return nativeObject;
        }

        protected static void InvalidateNativeObject(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Geometry).InvalidateNativeObject();
        }

        protected void InvalidateNativeObject()
        {
            nativeObject = null;
        }
    }
}
