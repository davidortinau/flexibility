using winShapes = Windows.UI.Xaml.Shapes;
using System.ComponentModel;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Rectangle),
                          typeof(Xamarin.Forms.Shapes.WinRT.RectangleRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class RectangleRenderer : ShapeRenderer<Rectangle, RectangleCanvasControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Rectangle> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new RectangleCanvasControl());
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetRadiusX();
                SetRadiusY();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
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
