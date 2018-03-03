using System.Threading;
using System.Threading.Tasks;

using Android.Content;
using Android.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(Xamarin.Forms.Media.Android.NativeBitmap))]

namespace Xamarin.Forms.Media.Android
{
    public class NativeBitmap : INativeBitmap
    {

        // Returns Task<UIImage>
        public async Task<object> ConvertToNative(ImageSource imageSource, object context, CancellationToken cancellationToken)
        {
            if (imageSource == null)
                return null;

            IImageSourceHandler handler = null;

            if (imageSource.GetType() == typeof(FileImageSource))
            {
                handler = new FileImageSourceHandler();
            }
            else if (imageSource.GetType() == typeof(StreamImageSource))
            {
                handler = new StreamImagesourceHandler();
            }
            else if (imageSource.GetType() == typeof(UriImageSource))
            {
                handler = new ImageLoaderSourceHandler();
            }

            if (handler == null)
                return null;

            // TODO: The context field is null at this pint, but the Bitmap still seems to be created.

            Bitmap bitmap = await handler.LoadImageAsync(imageSource, context as Context, cancellationToken);

            return bitmap;
        }
    }
}