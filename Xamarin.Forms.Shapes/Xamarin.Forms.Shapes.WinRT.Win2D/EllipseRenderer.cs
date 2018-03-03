using winShapes = Windows.UI.Xaml.Shapes;
using System.ComponentModel;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Ellipse),
                          typeof(Xamarin.Forms.Shapes.WinRT.EllipseRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class EllipseRenderer : ShapeRenderer<Ellipse, EllipseCanvasControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new EllipseCanvasControl());
            }

            base.OnElementChanged(args);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
        }
    }
}
