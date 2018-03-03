using System.ComponentModel;

using Microsoft.Graphics.Canvas.Geometry;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Polygon),
                          typeof(Xamarin.Forms.Shapes.WinRT.PolygonRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PolygonRenderer : ShapeRenderer<Polygon, PolygonCanvasControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Polygon> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new PolygonCanvasControl()); 
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
            Control.SetPoints(Shapes.ConvertPoints(Element.Points));
        }

        void SetFillRule()
        {
            Control.SetFillRule((CanvasFilledRegionDetermination)(int)Element.FillRule);
        }
    }
}
