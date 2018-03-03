using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Line), 
                          typeof(Xamarin.Forms.Shapes.Android.LineRenderer))]

namespace Xamarin.Forms.Shapes.Android
{
    public class LineRenderer : ShapeRenderer<Line, LineDrawableView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Line> args)
        {
            if (Control == null)
            {
                SetNativeControl(new LineDrawableView(Context));
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                SetX1();
                SetY1();
                SetX2();
                SetY2();
            }
        }

        protected override void OnElementPropertyChanged(object sender, 
                                                         PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "X1": SetX1(); break;
                case "Y1": SetX2(); break;
                case "X2": SetY1(); break;
                case "Y2": SetY2(); break;
            }
        }

        void SetX1()
        {
            Control.SetX1((float)Element.X1);
        }

        void SetY1()
        {
            Control.SetY1((float)Element.Y1);
        }

        void SetX2()
        {
            Control.SetX2((float)Element.X2);
        }

        void SetY2()
        {
            Control.SetY2((float)Element.Y2);
        }
    }
}