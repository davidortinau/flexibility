using System;
using UIKit;

namespace Xamarin.Forms.Shapes.iOS
{
    public class ShapeUIView : UIView
    {
        public ShapeUIView()
        {
            BackgroundColor = UIColor.Clear;

            DrawingLayer = new ShapeDrawingLayer(this);
            Layer.AddSublayer(DrawingLayer);
            Layer.MasksToBounds = false;
        }

        public ShapeDrawingLayer DrawingLayer
        {
            private set;
            get;
        }
    }
}
