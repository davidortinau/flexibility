using System.ComponentModel;
using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes
{
    public class Shape : View
    {
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create("Fill",
                                    typeof(Brush),
                                    typeof(Shape),
                                    null,
                                    propertyChanged: OnFillPropertyChanged);

        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create("Stroke",
                                    typeof(Brush),
                                    typeof(Shape),
                                    null,
                                    propertyChanged:OnStrokePropertyChanged);

        // Contrary to UWP documentation (https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.shapes.shape.strokethickness.aspx),
        //  the default value is 1.
        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create("StrokeThickness",
                                    typeof(double),
                                    typeof(Shape),
                                    1.0,
                                    propertyChanged: OnGeometrySizePropertyChanged);


        // TODO: Limit StrokeThickness to multiples of 2 !!!!


        // TODO: Need handler for changes in the StrokeDashArray collection !!!!!!!!


        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create("StrokeDashArray",
                                    typeof(DoubleCollection),
                                    typeof(Shape),
                                    null,
                                    defaultValueCreator: bindable => new DoubleCollection());


        public static readonly BindableProperty StrokeDashOffsetProperty =
            BindableProperty.Create("StrokeDashOffset",
                                    typeof(double),
                                    typeof(Shape),
                                    0.0);

        public static readonly BindableProperty StrokeStartLineCapProperty =
            BindableProperty.Create("StrokeStartLineCap",
                                    typeof(PenLineCap),
                                    typeof(Shape),
                                    PenLineCap.Flat,
                                    propertyChanged: OnGeometrySizePropertyChanged);

        public static readonly BindableProperty StrokeEndLineCapProperty =
            BindableProperty.Create("StrokeEndLineCap",
                                    typeof(PenLineCap),
                                    typeof(Shape),
                                    PenLineCap.Flat,
                                    propertyChanged: OnGeometrySizePropertyChanged);

        public static readonly BindableProperty StrokeDashCapProperty =
            BindableProperty.Create("StrokeDashCap",
                                    typeof(PenLineCap),
                                    typeof(Shape),
                                    PenLineCap.Flat);

        public static readonly BindableProperty StrokeLineJoinProperty =
            BindableProperty.Create("StrokeLineJoin",
                                    typeof(PenLineJoin),
                                    typeof(Shape),
                                    PenLineJoin.Miter,
                                    propertyChanged: OnGeometrySizePropertyChanged);

        public static readonly BindableProperty StrokeMiterLimitProperty =
            BindableProperty.Create("StrokeMiterLimit",
                                    typeof(double),
                                    typeof(Shape),
                                    10.0,
                                    propertyChanged: OnGeometrySizePropertyChanged);

        public static readonly BindableProperty StretchProperty =
            BindableProperty.Create("Stretch",
                                    typeof(Stretch),
                                    typeof(Shape),
                                    Stretch.None);

        public Shape()
        {
        }

        public Brush Fill
        {
            set { SetValue(FillProperty, value); }
            get { return (Brush)GetValue(FillProperty); }
        }

        public Brush Stroke
        {
            set { SetValue(StrokeProperty, value); }
            get { return (Brush)GetValue(StrokeProperty); }
        }

        public double StrokeThickness
        {
            set { SetValue(StrokeThicknessProperty, value); }
            get { return (double)GetValue(StrokeThicknessProperty); }
        }

        public DoubleCollection StrokeDashArray
        {
            set { SetValue(StrokeDashArrayProperty, value); }
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
        }

        public double StrokeDashOffset
        {
            set { SetValue(StrokeDashOffsetProperty, value); }
            get { return (double)GetValue(StrokeDashOffsetProperty); }
        }

        public PenLineCap StrokeStartLineCap
        {
            set { SetValue(StrokeStartLineCapProperty, value); }
            get { return (PenLineCap)GetValue(StrokeStartLineCapProperty); }
        }

        public PenLineCap StrokeEndLineCap
        {
            set { SetValue(StrokeEndLineCapProperty, value); }
            get { return (PenLineCap)GetValue(StrokeEndLineCapProperty); }
        }

        public PenLineCap StrokeDashCap
        {
            set { SetValue(StrokeDashCapProperty, value); }
            get { return (PenLineCap)GetValue(StrokeDashCapProperty); }
        }

        public PenLineJoin StrokeLineJoin
        {
            set { SetValue(StrokeLineJoinProperty, value); }
            get { return (PenLineJoin)GetValue(StrokeLineJoinProperty); }
        }

        public double StrokeMiterLimit
        {
            set { SetValue(StrokeMiterLimitProperty, value); }
            get { return (double)GetValue(StrokeMiterLimitProperty); }
        }

        public Stretch Stretch
        {
            set { SetValue(StretchProperty, value); }
            get { return (Stretch)GetValue(StretchProperty); }
        }

        // Must set handler for properties of the Brush attached to the Fill property.
        static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as Brush).PropertyChanged -= (bindable as Shape).OnFillBrushPropertyChanged;
            }

            if (newValue != null)
            {
                (newValue as Brush).PropertyChanged += (bindable as Shape).OnFillBrushPropertyChanged;
            }
        }

        void OnFillBrushPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged("Fill");
        }

        // Must also set handler for properties of the Brush attached to the Stroke property.
        static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as Brush).PropertyChanged -= (bindable as Shape).OnStrokeBrushPropertyChanged;
            }

            if (newValue != null)
            {
                (newValue as Brush).PropertyChanged += (bindable as Shape).OnStrokeBrushPropertyChanged;
            }
        }

        void OnStrokeBrushPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged("Stroke");
        }


        protected static void OnGeometrySizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Shape).OnGeometrySizePropertyChanged();
        }

        protected void OnGeometrySizePropertyChanged()
        {
            InvalidateMeasure();
        }
    }
}
