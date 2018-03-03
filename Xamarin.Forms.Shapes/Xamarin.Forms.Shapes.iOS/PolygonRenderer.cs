using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Media;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Polygon), 
                          typeof(Xamarin.Forms.Shapes.iOS.PolygonRenderer))]

namespace Xamarin.Forms.Shapes.iOS
{
    public class PolygonRenderer : ShapeRenderer<Polygon, PolygonUIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Polygon> args)
        {
            if (Control == null)
            {
                SetNativeControl(new PolygonUIView());
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
