using winFound = Windows.Foundation;
using winXaml = Windows.UI.Xaml;
using winMedia = Windows.UI.Xaml.Media;
using winShapes = Windows.UI.Xaml.Shapes;

using System.ComponentModel;

using Microsoft.Graphics.Canvas.Geometry;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

namespace Xamarin.Forms.Shapes.WinRT
{
    public class ShapeRenderer<TShape, TNativeShape> : ViewRenderer<TShape, TNativeShape>
        where TShape : Shape
        where TNativeShape : ShapeCanvasControl 
    {
        CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle();
        double width, height;

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
                SetStrokeStartLineCap();
                SetStrokeEndLineCap();
                SetStrokeDashCapProperty();
                SetStrokeLineJoinProperty();
                SetStrokeMiterLimit();
                SetStretch(); 
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "Width":
                    width = Element.Width;
                    SetSize();
                    break;

                case "Height":
                    height = Element.Height;
                    SetSize();
                    break;

                case "Fill": SetFill(); break;
                case "Stroke": SetStroke(); break;
                case "StrokeThickness": SetStrokeThickness(); break;
                case "StrokeDashArray": SetStrokeDashArray(); break;
                case "StrokeDashOffset": SetStrokeDashOffset(); break;
                case "StrokeStartLineCap": SetStrokeStartLineCap(); break;
                case "StrokeEndLineCap": SetStrokeEndLineCap(); break;
                case "StrokeDashCap": SetStrokeDashCapProperty(); break;
                case "StrokeLineJoinCap": SetStrokeLineJoinProperty(); break;
                case "StrokeMiterLimit": SetStrokeMiterLimit(); break;
                case "Stretch": SetStretch(); break;
            }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Control != null)
            {
                return Control.GetDesiredSize(widthConstraint, heightConstraint);
            }

            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        void SetSize()
        {
            Control.SetSize(new winFound.Size(width, height));
        }

        void SetFill()
        {
            Control.SetFill(Element.Fill);
        }

        void SetStroke()
        {
            Control.SetStroke(Element.Stroke);
        }

        void SetStrokeThickness()
        {
            Control.SetStrokeWidth((float)Element.StrokeThickness);
        }

        void SetStrokeDashArray()
        {
            strokeStyle.CustomDashStyle = null;

            if (Element.StrokeDashArray != null && Element.StrokeDashArray.Count > 0)
            {
                float[] dashes = new float[Element.StrokeDashArray.Count];

                for (int i = 0; i < Element.StrokeDashArray.Count; i++)
                {
                    dashes[i] = (float)Element.StrokeDashArray[i];
                }

                strokeStyle.CustomDashStyle = dashes;
            }

            Control.SetStrokeStyle(strokeStyle);
        }

        void SetStrokeDashOffset()
        {
            strokeStyle.DashOffset = (float)Element.StrokeDashOffset;
            Control.SetStrokeStyle(strokeStyle);
        }

        void SetStrokeStartLineCap()
        {
            strokeStyle.StartCap = (CanvasCapStyle)(int)Element.StrokeStartLineCap;
            Control.SetStrokeStyle(strokeStyle);
        }

        void SetStrokeEndLineCap()
        {
            strokeStyle.EndCap = (CanvasCapStyle)(int)Element.StrokeEndLineCap;
            Control.SetStrokeStyle(strokeStyle);
        }
        
        void SetStrokeDashCapProperty()
        {
            strokeStyle.DashCap = (CanvasCapStyle)(int)Element.StrokeDashCap;
            Control.SetStrokeStyle(strokeStyle);
        }

        void SetStrokeLineJoinProperty()
        {
            strokeStyle.LineJoin = (CanvasLineJoin)(int)Element.StrokeLineJoin;
            Control.SetStrokeStyle(strokeStyle);
        }

        void SetStrokeMiterLimit()
        {
            strokeStyle.MiterLimit = (float)Element.StrokeMiterLimit;
            Control.SetStrokeStyle(strokeStyle);
        }

        void SetStretch()
        {
            Control.SetStretch(Element.Stretch);
        }

        //global::Windows.UI.Color ToWindowsColor(global::Xamarin.Forms.Color color)
        //{
        //    return global::Windows.UI.Color.FromArgb((byte)(color.A * 255),
        //                                             (byte)(color.R * 255),
        //                                             (byte)(color.G * 255),
        //                                             (byte)(color.B * 255));
        //}
    }
}
