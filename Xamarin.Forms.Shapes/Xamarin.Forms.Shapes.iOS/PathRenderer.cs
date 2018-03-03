using System.ComponentModel;

using CoreGraphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Path), 
                          typeof(Xamarin.Forms.Shapes.iOS.PathRenderer))]

namespace Xamarin.Forms.Shapes.iOS
{
    public class PathRenderer : ShapeRenderer<Path, PathUIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Path> args)
        {
            if (Control == null)
            {
                SetNativeControl(new PathUIView());
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetData();
            }
        }

        protected override void OnElementPropertyChanged(object sender, 
                                                         PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "Data": SetData(); break;
            }
        }

        void SetData()
        {
            Control.SetPath(Element.Data?.GetNativeObject() as CGPathPlus);
        }
    }
}
