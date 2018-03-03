namespace Xamarin.Forms.Media
{
    public interface INativeBrush
    {
        object ConvertToNative(Brush brush, object context);

     //   object GetImageBrushBitmap();
    }
}
