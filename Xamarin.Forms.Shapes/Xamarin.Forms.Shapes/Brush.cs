using System.ComponentModel;

namespace Xamarin.Forms.Media
{
    // TODO: Opacity and RelativeTransform properties


    [TypeConverter (typeof(BrushTypeConverter))]
    public class Brush : BindableObject
    {
        protected object nativeBrush = null;
        protected object context;                   // For Android

        // TODO: This should be considered unimplemented.
        public static readonly BindableProperty TransformProperty =
            BindableProperty.Create("Transform",
                                    typeof(Transform),
                                    typeof(Brush),
                                    null,
                                    propertyChanged: OnTransformPropertyChanged);

        // TODO: Opacity and RelativeTransform properties

        public Transform Transform
        {
            set { SetValue(TransformProperty, value); }
            get { return (Transform)GetValue(TransformProperty); }
        }

        //public object NativeBrush
        //{
        //    private set
        //    {
        //        nativeBrush = value;
        //        OnPropertyChanged("NativeBrush");
        //    }
        //    get
        //    {
        //        return nativeBrush;
        //    }
        //}

        // Changes in the Value property of GeneralTransform must be snagged. 
        static void OnTransformPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as Transform).PropertyChanged -= (bindable as Brush).OnTransformSubPropertyChanged;
            }
            if (newValue != null)
            {
                (newValue as Transform).PropertyChanged += (bindable as Brush).OnTransformSubPropertyChanged;
            }

            // TODO: This invalidates the Brush!
            InvalidateNativeObject(bindable, oldValue, newValue);
        }

        void OnTransformSubPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // This is the PropertyChanged event fired from GeneralTransform.
            if (args.PropertyName == GeneralTransform.ValueProperty.PropertyName)
            {
                // TODO: This invalidates the brush!
                InvalidateNativeObject();
                OnPropertyChanged("Transform");
            }
        }

        // TODO: Aside from the Transform subproperties, should we just have an 
        //  OnPropertyChanged override that calls InvalidateNativeObject?

        public object GetNativeObject(object context)
        {
            if (nativeBrush == null)
            {
                INativeBrush nativeGraphicsMedia = DependencyService.Get<INativeBrush>();
                nativeBrush = nativeGraphicsMedia.ConvertToNative(this, context);
                this.context = context;
            }

            return nativeBrush;
        }

        protected static void InvalidateNativeObject(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Brush).InvalidateNativeObject();
        }

        protected void InvalidateNativeObject()
        {
            nativeBrush = null;
        }
    }
}
