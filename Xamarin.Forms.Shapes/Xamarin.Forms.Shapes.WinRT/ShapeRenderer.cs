using winXaml = Windows.UI.Xaml;
using winMedia = Windows.UI.Xaml.Media;
using winShapes = Windows.UI.Xaml.Shapes;

using System.ComponentModel;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

namespace Xamarin.Forms.Shapes.WinRT
{
    public class ShapeRenderer<TShape, TNativeShape> : ViewRenderer<TShape, TNativeShape>
        where TShape : Shape
        where TNativeShape : winShapes.Shape
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TShape> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetFill();
                SetStroke();
                SetStrokeThickness();
                SetStrokeDashArray();
                SetStrokeDashOffset();
                SetEnumProperty(winShapes.Shape.StrokeStartLineCapProperty, Shape.StrokeStartLineCapProperty);
                SetEnumProperty(winShapes.Shape.StrokeEndLineCapProperty, Shape.StrokeEndLineCapProperty);
                SetEnumProperty(winShapes.Shape.StrokeDashCapProperty, Shape.StrokeDashCapProperty);
                SetEnumProperty(winShapes.Shape.StrokeLineJoinProperty, Shape.StrokeLineJoinProperty);
                SetStrokeMiterLimit();
                SetEnumProperty(winShapes.Shape.StretchProperty, Shape.StretchProperty); 
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "Fill": SetFill(); break;
                case "Stroke": SetStroke(); break;
                case "StrokeThickness": SetStrokeThickness(); break;
                case "StrokeDashArray": SetStrokeDashArray(); break;
                case "StrokeDashOffset": SetStrokeDashOffset(); break;
                case "StrokeStartLineCap": SetEnumProperty(winShapes.Shape.StrokeStartLineCapProperty, Shape.StrokeStartLineCapProperty); break;
                case "StrokeEndLineCap": SetEnumProperty(winShapes.Shape.StrokeEndLineCapProperty, Shape.StrokeEndLineCapProperty); break;
                case "StrokeDashCap": SetEnumProperty(winShapes.Shape.StrokeDashCapProperty, Shape.StrokeDashCapProperty); break;
                case "StrokeLineJoinCap": SetEnumProperty(winShapes.Shape.StrokeLineJoinProperty, Shape.StrokeLineJoinProperty); break;
                case "StrokeMiterLimit": SetStrokeMiterLimit(); break;
                case "Stretch": SetEnumProperty(winShapes.Shape.StretchProperty, Shape.StretchProperty); break;
            }
        }

        void SetFill()
        {
            Control.Fill = Element.Fill?.GetNativeObject(null) as winMedia.Brush;
        }

        void SetStroke()
        {
            Control.Stroke = Element.Stroke?.GetNativeObject(null) as winMedia.Brush;
        }

        void SetStrokeThickness()
        {
            Control.StrokeThickness = Element.StrokeThickness;
        }

        void SetStrokeDashArray()
        {
            if (Control.StrokeDashArray != null)
            {
                Control.StrokeDashArray.Clear();
            }

            if (Element.StrokeDashArray != null && Element.StrokeDashArray.Count > 0)
            {
                if (Control.StrokeDashArray == null)
                {
                    Control.StrokeDashArray = new winMedia.DoubleCollection();
                }

                double[] array = new double[Element.StrokeDashArray.Count];
                Element.StrokeDashArray.CopyTo(array, 0);

                foreach (double value in array)
                {
                    Control.StrokeDashArray.Add(value);
                }
            }
        }

        void SetStrokeDashOffset()
        {
            Control.StrokeDashOffset = Element.StrokeDashOffset;
        }

        void SetEnumProperty(winXaml.DependencyProperty dstProperty, BindableProperty srcProperty)
        {
            Control.SetValue(dstProperty, (int)Element.GetValue(srcProperty));
        }

        void SetStrokeMiterLimit()
        {
            Control.StrokeMiterLimit = Element.StrokeMiterLimit;
        }

        global::Windows.UI.Color ToWindowsColor(global::Xamarin.Forms.Color color)
        {
            return global::Windows.UI.Color.FromArgb((byte)(color.A * 255),
                                                     (byte)(color.R * 255),
                                                     (byte)(color.G * 255),
                                                     (byte)(color.B * 255));
        }
    }
}
