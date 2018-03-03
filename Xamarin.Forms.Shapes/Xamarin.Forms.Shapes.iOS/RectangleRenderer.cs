using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Rectangle), 
                          typeof(Xamarin.Forms.Shapes.iOS.RectangleRenderer))]

namespace Xamarin.Forms.Shapes.iOS
{
    public class RectangleRenderer : ShapeRenderer<Rectangle, RectangleUIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Rectangle> args)
        {
            if (Control == null)
            {
                SetNativeControl(new RectangleUIView());
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
            Control.SetRadiusX(Element.RadiusX);
        }

        void SetRadiusY()
        {
            Control.SetRadiusY(Element.RadiusY);
        }
    }
}
