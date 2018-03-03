namespace Xamarin.Forms.Shapes.iOS
{
    public class PathUIView : ShapeUIView
    {
        public void SetPath(CoreGraphics.CGPathPlus path)
        {
            DrawingLayer.SetBasicPath(path);
            DrawingLayer.SetIsNonzeroFill(path == null ? false : path.IsNonzeroFill);
        }
    }
}
