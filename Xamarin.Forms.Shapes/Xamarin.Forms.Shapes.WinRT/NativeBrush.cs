using winFound = Windows.Foundation;
using winMedia = Windows.UI.Xaml.Media;
using Xamarin.Forms;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

[assembly: Dependency(typeof(Xamarin.Forms.Media.WinRT.NativeBrush))]

namespace Xamarin.Forms.Media.WinRT
{
    public class NativeBrush : NativeObject, INativeBrush
    {
        public object ConvertToNative(Brush brush, object context)
        {
            winMedia.Brush winBrush = null;

            // SolidColorBrush
            if (brush is SolidColorBrush)
            {
                SolidColorBrush xamBrush = brush as SolidColorBrush;

                winBrush = new winMedia.SolidColorBrush
                {
                    Color = ConvertColor(xamBrush.Color)
                };
            }

            // LinearGradientBrush
            else if (brush is LinearGradientBrush)
            {
                LinearGradientBrush xamBrush = brush as LinearGradientBrush;

                winBrush = new winMedia.LinearGradientBrush
                {
                    StartPoint = ConvertPoint(xamBrush.StartPoint),
                    EndPoint = ConvertPoint(xamBrush.EndPoint),
                    SpreadMethod = ConvertGradientSpread(xamBrush.SpreadMethod)
                };

                foreach (GradientStop xamGradientStop in xamBrush.GradientStops)
                {
                    winMedia.GradientStop winGradientStop = new winMedia.GradientStop
                    {
                        Color = ConvertColor(xamGradientStop.Color),
                        Offset = xamGradientStop.Offset
                    };

                    (winBrush as winMedia.LinearGradientBrush).GradientStops.Add(winGradientStop);
                }
            }

            else if (brush is ImageBrush)
            {
                ImageBrush xamBrush = brush as ImageBrush;

                winBrush = new winMedia.ImageBrush
                {
                    Stretch = (winMedia.Stretch)(int)xamBrush.Stretch,
                    AlignmentX = (winMedia.AlignmentX)(int)xamBrush.AlignmentX,
                    AlignmentY = (winMedia.AlignmentY)(int)xamBrush.AlignmentY,
                };

                ImageSource xamImageSource = (brush as ImageBrush).ImageSource;

                if (xamImageSource != null)
                {
                    IImageSourceHandler handler = null;

                    if (xamImageSource.GetType() == typeof(FileImageSource))
                    {
                        handler = new FileImageSourceHandler();
                    }
                    else if (xamImageSource.GetType() == typeof(StreamImageSource))
                    {
                        handler = new StreamImageSourceHandler();
                    }
                    else if (xamImageSource.GetType() == typeof(UriImageSource))
                    {
                        handler = new UriImageSourceHandler();
                    }

                    if (handler != null)
                    {
                        Task<winMedia.ImageSource> winImageSourceTask = handler.LoadImageAsync(xamImageSource);

                        winImageSourceTask.ContinueWith((task) =>
                        {
                            winFound.IAsyncAction asyncAction = winBrush.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                (winBrush as winMedia.ImageBrush).ImageSource = task.Result;
                            });
                        });
                    }
                }
            }

            if (winBrush != null)
            {
                winBrush.Transform = brush.Transform?.GetNativeObject() as winMedia.MatrixTransform;

                    // TODO: RelativeTransform and Opacity
            }

            return winBrush;
        }

        Windows.UI.Color ConvertColor(Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(255 * color.A),
                                             (byte)(255 * color.R),
                                             (byte)(255 * color.G),
                                             (byte)(255 * color.B));
        }

        winMedia.GradientSpreadMethod ConvertGradientSpread(GradientSpreadMethod gradientSpreadMethod)
        {
            switch (gradientSpreadMethod)
            {
                case GradientSpreadMethod.Pad: return winMedia.GradientSpreadMethod.Pad;
                case GradientSpreadMethod.Refect: return winMedia.GradientSpreadMethod.Reflect;
                case GradientSpreadMethod.Repeat: return winMedia.GradientSpreadMethod.Repeat;
            }

            return winMedia.GradientSpreadMethod.Pad;
        }
    }
}
