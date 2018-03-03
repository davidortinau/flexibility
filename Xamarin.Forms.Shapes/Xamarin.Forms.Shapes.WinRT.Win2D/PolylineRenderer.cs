using System.ComponentModel;

using Microsoft.Graphics.Canvas.Geometry;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Polyline),
                          typeof(Xamarin.Forms.Shapes.WinRT.PolylineRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PolylineRenderer : ShapeRenderer<Polyline, PolylineCanvasControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Polyline> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new PolylineCanvasControl());
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
