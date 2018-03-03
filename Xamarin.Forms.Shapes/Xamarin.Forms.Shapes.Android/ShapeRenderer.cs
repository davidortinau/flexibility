using System.ComponentModel;

using droidGraphics = Android.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Media;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.Forms.Shapes.Android
{
    public class ShapeRenderer<TShape, TNativeShape> : ViewRenderer<TShape, TNativeShape>
        where TShape : Xamarin.Forms.Shapes.Shape
        where TNativeShape : Xamarin.Forms.Shapes.Android.ShapeDrawableView
    {
        double width, height;

        protected override void OnElementChanged(ElementChangedEventArgs<TShape> args)
        {
            base.OnElementChanged(args);

            // Prevent clipping of negative coordinates, Step 1
            // TODO: Is there a better place / way to do this?
            this.SetClipChildren(false);

            if (args.NewElement != null)
            {
                SetSize();

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
                case "StrokeDashCap": SetStrokeDashCap(); break;
                case "StrokeLineJoin": SetStrokeLineJoin(); break;
                case "StrokeMiterLimit": SetStrokeMiterLimit(); break;
                case "Stretch": SetStretch(); break;
            }
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            if (Element != null)
            {
                return Control.GetDesiredSize(widthConstraint, heightConstraint);
            }

            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        void SetSize()
        {
            Control.SetSize(width, height);
        }

        // TODO: This seems to be an entirely inadequate way to get the Context
        //  object into the Brush so it can be used for creating the ImageBrush bitmap.

        void SetFill()
        {
            Control.SetFillColor(Element.Fill?.GetNativeObject(Context));
        }

        void SetStroke()
        {
            Control.SetStrokeColor(Element.Stroke?.GetNativeObject(Context));
        }

        void SetStrokeDashArray()
        {
            Control.SetDashIntervals(Element.StrokeDashArray?.ToFloatArray());
        }

        void SetStrokeDashOffset()
        {
            Control.SetDashPhase((float)Element.StrokeDashOffset);
        }

        void SetStrokeThickness()
        {
            Control.SetStrokeWidth((float)Element.StrokeThickness);
        }

        void SetStrokeStartLineCap()
        {
            PenLineCap winLineCap = Element.StrokeStartLineCap;
            droidGraphics.Paint.Cap lineCap = droidGraphics.Paint.Cap.Butt;

            switch (winLineCap)
            {
                case PenLineCap.Flat: lineCap = droidGraphics.Paint.Cap.Butt; break;
                case PenLineCap.Square: lineCap = droidGraphics.Paint.Cap.Square; break;
                case PenLineCap.Round: lineCap = droidGraphics.Paint.Cap.Round; break;
                case PenLineCap.Triangle: lineCap = droidGraphics.Paint.Cap.Round; break;
            }

            Control.SetStrokeCap(lineCap);
        }

        // No equivalent for these two properties in the Android API

        void SetStrokeEndLineCap()
        {

        }

        void SetStrokeDashCap()
        {

        }

        void SetStrokeLineJoin()
        {
            PenLineJoin winLineJoin = Element.StrokeLineJoin;
            droidGraphics.Paint.Join lineJoin = droidGraphics.Paint.Join.Miter;

            switch (winLineJoin)
            {
                case PenLineJoin.Miter: lineJoin = droidGraphics.Paint.Join.Miter; break;
                case PenLineJoin.Bevel: lineJoin = droidGraphics.Paint.Join.Bevel; break;
                case PenLineJoin.Round: lineJoin = droidGraphics.Paint.Join.Round; break;
            }

            Control.SetStrokeJoin(lineJoin);
        }

        void SetStrokeMiterLimit()
        {
            Control.SetStrokeMiterLimit((float)Element.StrokeMiterLimit);
        }

        void SetStretch()
        {
            Control.SetStretch(Element.Stretch);
        }
    }
}