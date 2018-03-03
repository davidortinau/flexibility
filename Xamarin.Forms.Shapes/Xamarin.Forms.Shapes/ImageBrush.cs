using System;
using System.Threading;

namespace Xamarin.Forms.Media
{
    public class ImageBrush : TileBrush
    {
        object nativeBitmap;
        CancellationTokenSource cancellationTokenSource;



        // TODO: Can these two events be implemented?

        // Original is RoutedEventHandler
 //       public event EventHandler ImageOpened;

        // Original is ExceptionRoutedEventArgs
  //      public event EventHandler ImageFailed;




        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create("ImageSource",
                                    typeof(ImageSource),
                                    typeof(ImageBrush),
                                    null,
                                    propertyChanged: OnImageSourceChanged);

        public ImageSource ImageSource
        {
            set { SetValue(ImageSourceProperty, value); }
            get { return (ImageSource)GetValue(ImageSourceProperty); }
        }

        public object NativeBitmap
        {
            private set
            {
                nativeBitmap = value;
                OnPropertyChanged("NativeBitmap");
            }
            get
            {
                return nativeBitmap;
            }
        }

        static void OnImageSourceChanged(BindableObject sender, object oldValue, object newValue)
        {
            (sender as ImageBrush).OnImageSourceChanged(oldValue as ImageSource, newValue as ImageSource);
        }

        async void OnImageSourceChanged(ImageSource oldValue, ImageSource newValue)
        {
            if (oldValue != null)
            {
                cancellationTokenSource.Cancel();
                NativeBitmap = null;
            }

            if (newValue != null)
            {
                NativeBitmap = null;
                cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    object obj = await DependencyService.Get<INativeBitmap>().ConvertToNative(newValue, context, cancellationTokenSource.Token);

                    NativeBitmap = obj;
                }
                catch
                {
                    ;
                }
            }
        }
    }
}
