using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Ellipse), 
                          typeof(Xamarin.Forms.Shapes.iOS.EllipseRenderer))]

namespace Xamarin.Forms.Shapes.iOS
{
    public class EllipseRenderer : ShapeRenderer<Ellipse, EllipseUIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> args)
        {
            if (Control == null)
            {
                SetNativeControl(new EllipseUIView());
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
