using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Media
{
    public interface INativeBitmap
    {
        Task<object> ConvertToNative(ImageSource imageSource, object context, CancellationToken cancellationToken);
    }
}
