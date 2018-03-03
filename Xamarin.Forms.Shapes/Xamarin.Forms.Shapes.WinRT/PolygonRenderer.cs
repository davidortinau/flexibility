using winMedia = Windows.UI.Xaml.Media;
using winShapes = Windows.UI.Xaml.Shapes;

using System.ComponentModel;

using Xamarin.Forms.Shapes;
using Xamarin.Forms.Media;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Polygon),
                          typeof(Xamarin.Forms.Shapes.WinRT.PolygonRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PolygonRenderer : ShapeRenderer<Polygon, winShapes.Polygon>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Polygon> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new winShapes.Polygon());
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetPoints();
                SetFillRule();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
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
            Control.Points = Shapes.ConvertPoints(Element.Points);
        }

        void SetFillRule()
        {
            Control.FillRule = Element.FillRule == FillRule.EvenOdd ? winMedia.FillRule.EvenOdd : 
                                                                      winMedia.FillRule.Nonzero;
        }
    }
}
