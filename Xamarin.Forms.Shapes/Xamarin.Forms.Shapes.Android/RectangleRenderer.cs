using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Rectangle), 
                          typeof(Xamarin.Forms.Shapes.Android.RectangleRenderer))]

namespace Xamarin.Forms.Shapes.Android
{
    public class RectangleRenderer : ShapeRenderer<Rectangle, RectangleDrawableView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Rectangle> args)
        {
            if (Control == null)
            {
                SetNativeControl(new RectangleDrawableView(Context));
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetRadiusX();
                SetRadiusY();
            }
        }

        protected override void OnElementPropertyChanged(object sender, 
                                                         PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "RadiusX": SetRadiusX(); break;
                case "RadiusY": SetRadiusY(); break;
            }
        }

        void SetRadiusX()
        {
            Control.SetRadiusX((float)Element.RadiusX);
        }

        void SetRadiusY()
        {
            Control.SetRadiusY((float)Element.RadiusY);
        }
    }
}