using System;
using CoreGraphics;
using UIKit;

namespace Xamarin.Forms.Shapes.iOS
{
    public class RectangleUIView : ShapeUIView
    {
        public RectangleUIView()
        {
            CreateNewPath();
        }

        public nfloat RadiusX
        {
            set; get;
        }

        public nfloat RadiusY
        {
            set; get;
        }

        void CreateNewPath()
        {
            CGPath path = new CGPath();
            path.AddRect(new CGRect(0, 0, 1, 1));
            DrawingLayer.SetBasicPath(path);
        }

        public void SetRadiusX(double radiusX)
        {
            RadiusX = new nfloat(radiusX);
            DrawingLayer.SetNeedsDisplay();
        }

        public void SetRadiusY(double radiusY)
        {
            RadiusY = new nfloat(radiusY);
            DrawingLayer.SetNeedsDisplay();
        }
    }
}
