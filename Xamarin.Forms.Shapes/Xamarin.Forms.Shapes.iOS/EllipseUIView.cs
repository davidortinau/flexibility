using System;
using CoreGraphics;

namespace Xamarin.Forms.Shapes.iOS
{
    public class EllipseUIView : ShapeUIView
    {
        public EllipseUIView()
        {
            CreateNewPath();
        }

        void CreateNewPath()
        {
            CGPath path = new CGPath();
            path.AddEllipseInRect(new CGRect(0, 0, 1, 1));
            DrawingLayer.SetBasicPath(path);
        }
    }
}
