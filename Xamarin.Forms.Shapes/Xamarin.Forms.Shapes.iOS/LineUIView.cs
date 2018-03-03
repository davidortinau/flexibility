using System;
using CoreGraphics;
using UIKit;

namespace Xamarin.Forms.Shapes.iOS
{
    public class LineUIView : ShapeUIView
    {
        nfloat x1, y1, x2, y2;

        public void SetX1(double x1)
        {
            this.x1 = new nfloat(x1);
            CreateNewPath();
        }

        public void SetY1(double y1)
        {
            this.y1 = new nfloat(y1);
            CreateNewPath();
        }

        public void SetX2(double x2)
        {
            this.x2 = new nfloat(x2);
            CreateNewPath();
        }

        public void SetY2(double y2)
        {
            this.y2 = new nfloat(y2);
            CreateNewPath();
        }

        void CreateNewPath()
        {
            CGPath path = new CGPath();
            path.MoveToPoint(x1, y1);
            path.AddLineToPoint(x2, y2);
            DrawingLayer.SetBasicPath(path);
        }
    }
}
