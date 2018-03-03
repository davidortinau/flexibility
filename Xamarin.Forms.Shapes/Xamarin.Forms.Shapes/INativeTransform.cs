namespace Xamarin.Forms.Media
{
    public interface INativeTransform
    {
        object ConvertToNative(GeneralTransform transform);
    }
}
