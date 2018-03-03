using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Media;
using Xamarin.Forms.Platform.Android;

using droidGraphics = Android.Graphics;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Polygon), 
                          typeof(Xamarin.Forms.Shapes.Android.PolygonRenderer))]

namespace Xamarin.Forms.Shapes.Android
{
    public class PolygonRenderer : ShapeRenderer<Polygon, PolygonDrawableView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Polygon> args)
        {
            if (Control == null)
            {
                SetNativeControl(new PolygonDrawableView(Context));
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
            Control.SetPoints(Element.Points);
        }

        void SetFillRule()
        {
            Control.SetFillType(Element.FillRule == FillRule.EvenOdd ? droidGraphics.Path.FillType.EvenOdd :
                                                                       droidGraphics.Path.FillType.Winding);
        }
    }
}