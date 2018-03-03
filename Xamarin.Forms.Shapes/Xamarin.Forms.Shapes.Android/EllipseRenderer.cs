using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Ellipse), 
                          typeof(Xamarin.Forms.Shapes.Android.EllipseRenderer))]

namespace Xamarin.Forms.Shapes.Android
{
    public class EllipseRenderer : ShapeRenderer<Ellipse, EllipseDrawableView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> args)
        {
            if (Control == null)
            {
                SetNativeControl(new EllipseDrawableView(Context));
            }

            base.OnElementChanged(args);
        }

        protected override void OnElementPropertyChanged(object sender, 
                                                         PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
        }
    }
}