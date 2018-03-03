using System;
using System.ComponentModel;

using Xamarin.Forms.Media;
using Xamarin.Forms.Platform.iOS;

using CoreGraphics;

namespace Xamarin.Forms.Shapes.iOS
{
    public class ShapeRenderer<TShape, TNativeShape> : ViewRenderer<TShape, TNativeShape>
        where TShape : Xamarin.Forms.Shapes.Shape
        where TNativeShape : ShapeUIView
    {
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
                SetStrokeDashCap();
                SetStrokeLineJoin();
                SetStrokeMiterLimit();
                SetStretch();
            }
        }

        protected override void OnElementPropertyChanged(object sender,
                                                         PropertyChangedEventArgs args)
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
                case "StrokeDashCap": SetStrokeDashCap(); break;
                case "StrokeLineJoin": SetStrokeLineJoin(); break;
                case "StrokeMiterLimit": SetStrokeMiterLimit(); break;
                case "Stretch": SetStretch(); break;
            }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Control != null)
            {
                return Control.DrawingLayer.GetDesiredSize(widthConstraint, heightConstraint);
            }

            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        void SetSize()
        {
            Control.DrawingLayer.SetSize(new CGSize(new nfloat(width), new nfloat(height)));
        }

        void SetFill()
        {
            Control.DrawingLayer.SetFill(Element.Fill?.GetNativeObject(null));
        }

        void SetStroke()
        {
            Control.DrawingLayer.SetStroke(Element.Stroke?.GetNativeObject(null));
        }

        void SetStrokeThickness()
        {
            Control.DrawingLayer.SetStrokeThickness(Element.StrokeThickness);
        }

        void SetStrokeDashArray()
        {
            if (Element.StrokeDashArray == null || Element.StrokeDashArray.Count == 0)
            {
                Control.DrawingLayer.SetDashLengths(new nfloat[0]);
            }
            else
            {
                nfloat[] dashLengths = null;
                double[] array = null;

                // Even number
                if (Element.StrokeDashArray.Count % 2 == 0)
                {
                    array = new double[Element.StrokeDashArray.Count];
                    dashLengths = new nfloat[Element.StrokeDashArray.Count];
                    Element.StrokeDashArray.CopyTo(array, 0);
                }
                // Odd number
                else
                {
                    array = new double[2 * Element.StrokeDashArray.Count];
                    dashLengths = new nfloat[2 * Element.StrokeDashArray.Count];
                    Element.StrokeDashArray.CopyTo(array, 0);
                    Element.StrokeDashArray.CopyTo(array, Element.StrokeDashArray.Count);
                }

                double thickness = Element.StrokeThickness;

                for (int i = 0; i < array.Length; i++)
                {
                    dashLengths[i] = new nfloat(thickness * array[i]);
                }
                Control.DrawingLayer.SetDashLengths(dashLengths);
            }
        }

        void SetStrokeDashOffset()
        {
            Control.DrawingLayer.SetDashPhase(new nfloat(Element.StrokeDashOffset));
        }

        void SetStrokeStartLineCap()
        {
            PenLineCap winLineCap = Element.StrokeStartLineCap;
            CGLineCap iosLineCap = CGLineCap.Butt;

            switch (winLineCap)
            {
                case PenLineCap.Flat: iosLineCap = CGLineCap.Butt; break;
                case PenLineCap.Square: iosLineCap = CGLineCap.Square; break;
                case PenLineCap.Round: iosLineCap = CGLineCap.Round; break;
                case PenLineCap.Triangle: iosLineCap = CGLineCap.Round; break;
            }

            Control.DrawingLayer.SetStrokeLineCap(iosLineCap);
        }

        void SetStrokeEndLineCap()
        {
            
        }

        void SetStrokeDashCap()
        {
            
        }

        void SetStrokeLineJoin()
        {
            PenLineJoin winLineJoin = Element.StrokeLineJoin;
            CGLineJoin iosLineJoin = CGLineJoin.Miter;

            switch (winLineJoin)
            {
                case PenLineJoin.Miter: iosLineJoin = CGLineJoin.Miter; break;
                case PenLineJoin.Bevel: iosLineJoin = CGLineJoin.Bevel; break;
                case PenLineJoin.Round: iosLineJoin = CGLineJoin.Round; break;
            }

            Control.DrawingLayer.SetStrokeLineJoin(iosLineJoin);
        }

        void SetStrokeMiterLimit()
        {
            Control.DrawingLayer.SetStrokeMiterLimit(new nfloat(Element.StrokeMiterLimit));
        }

        void SetStretch()
        {
            Control.DrawingLayer.SetStretch(Element.Stretch);
        }
    }
}
