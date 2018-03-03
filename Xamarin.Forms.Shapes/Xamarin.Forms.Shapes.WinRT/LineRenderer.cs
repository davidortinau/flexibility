using winShapes = Windows.UI.Xaml.Shapes;
using System.ComponentModel;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Line),
                          typeof(Xamarin.Forms.Shapes.WinRT.LineRenderer))]

namespace Xamarin.Forms.Shapes.WinRT
{
    public class LineRenderer : ShapeRenderer<Line, winShapes.Line>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Line> args)
        {
            if (Control == null && args.NewElement != null)             // special check for WinRT
            {
                SetNativeControl(new winShapes.Line());
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

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            switch (args.PropertyName)
            {
                case "X1": SetX1(); break;
                case "Y1": SetY1(); break;
                case "X2": SetX2(); break;
                case "Y2": SetY2(); break;
            }
        }

        void SetX1()
        {
            Control.X1 = Element.X1;
        }

        void SetY1()
        {
            Control.Y1 = Element.Y1;
        }

        void SetX2()
        {
            Control.X2 = Element.X2;
        }

        void SetY2()
        {
            Control.Y2 = Element.Y2;
        }
    }
}
