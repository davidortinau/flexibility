using winMedia = Windows.UI.Xaml.Media;
using winShapes = Windows.UI.Xaml.Shapes;
using System.ComponentModel;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Path),
                          typeof(Xamarin.Forms.Shapes.WinRT.PathRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class PathRenderer : ShapeRenderer<Path, winShapes.Path>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Path> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new winShapes.Path());
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetData();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "Data": SetData(); break;
            }
        }

        void SetData()
        {
            Control.Data = Element.Data.GetNativeObject() as winMedia.Geometry;
        }
    }
}
