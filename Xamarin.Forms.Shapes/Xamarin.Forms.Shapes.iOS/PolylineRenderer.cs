using System.ComponentModel;

using CoreGraphics;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Media;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Polyline), 
                          typeof(Xamarin.Forms.Shapes.iOS.PolylineRenderer))]

namespace Xamarin.Forms.Shapes.iOS
{
    public class PolylineRenderer : ShapeRenderer<Polyline, PolylineUIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Polyline> args)
        {
            if (Control == null)
            {
                SetNativeControl(new PolylineUIView());
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetPoints();
                SetFillRule();
            }
        }

        protected override void OnElementPropertyChanged(object sender, 
                                                         PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "Points": SetPoints(); break;
                case "FillRule": SetFillRule(); break;
            }
        }

        void SetPoints()
        {
            Control.SetPoints(Shapes.ConvertPoints(Element.Points));
        }

        public void SetFillRule()
        {
            Control.SetFillMode(Element.FillRule == FillRule.Nonzero);
        }
    }
}
