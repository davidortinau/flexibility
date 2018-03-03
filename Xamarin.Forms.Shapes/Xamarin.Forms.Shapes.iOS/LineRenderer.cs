using System.ComponentModel;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Shapes.Line), 
                          typeof(Xamarin.Forms.Shapes.iOS.LineRenderer))]

namespace Xamarin.Forms.Shapes.iOS
{
    public class LineRenderer : ShapeRenderer<Line, LineUIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Line> args)
        {
            if (Control == null)
            {
                SetNativeControl(new LineUIView());
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
                case "Y1": SetY1(); break;
                case "X2": SetX2(); break;
                case "Y2": SetY2(); break;
            }
        }

        void SetX1()
        {
            Control.SetX1(Element.X1);
        }

        void SetY1()
        {
            Control.SetY1(Element.Y1);
        }

        void SetX2()
        {
            Control.SetX2(Element.X2);
        }

        void SetY2()
        {
            Control.SetY2(Element.Y2);
        }
    }
}
