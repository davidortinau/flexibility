using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Xamarin.Forms.Media.iOS.NativeBitmap))]

namespace Xamarin.Forms.Media.iOS
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
                handler = new StreamImageSourceHandler();       // lowercase 'source' to use original
            }
            else if (imageSource.GetType() == typeof(UriImageSource))
            {
                handler = new ImageLoaderSourceHandler();
            }

            if (handler == null)
                return null;

            UIImage image = await handler.LoadImageAsync(imageSource, cancellationToken);

            return image;
        }
    }

    // A substitution because the original didn't work
    public sealed class StreamImageSourceHandler : IImageSourceHandler
    {
        public async Task<UIImage> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = default(CancellationToken), float scale = 1f)
        {
            UIImage image = null;
            var streamsource = imagesource as StreamImageSource;
            if (streamsource != null && streamsource.Stream != null)
            {
                var streamImage = await streamsource.GetStreamAsync(cancelationToken).ConfigureAwait(false);
                if (streamImage != null)
                    image = UIImage.LoadFromData(Foundation.NSData.FromStream(streamImage), (nfloat)scale);
            }
            return image;
        }
    }

    public static class StreamExtensions
    {
        static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static async Task<Stream> GetStreamAsync(this StreamImageSource streamImageSource, CancellationToken userToken = default(CancellationToken))
        {
            if (streamImageSource.Stream == null)
                return null;

     //       OnLoadingStarted();
            userToken.Register(CancellationTokenSource.Cancel);
            Stream stream = null;
            try
            {
                stream = await streamImageSource.Stream(CancellationTokenSource.Token);
         //       OnLoadingCompleted(false);
            }
            catch (OperationCanceledException)
            {
         //       OnLoadingCompleted(true);
                throw;
            }
            return stream;
        }



    }
}