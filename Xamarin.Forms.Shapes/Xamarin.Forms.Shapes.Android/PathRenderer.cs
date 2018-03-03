using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Path), 
                          typeof(Xamarin.Forms.Shapes.Android.PathRenderer))]

namespace Xamarin.Forms.Shapes.Android
{
    public class PathRenderer : ShapeRenderer<Path, PathDrawableView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Path> args)
        {
            if (Control == null)
            {
                SetNativeControl(new PathDrawableView(Context));
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
            Control.SetPath(Element.Data?.GetNativeObject() as global::Android.Graphics.Path);
        }
    }
}