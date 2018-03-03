using Android.App;

namespace Xamarin.Forms.Shapes.Android
{
    public static class Shapes
    {
        public static void Init(Activity activity)
        {
            Activity = activity;
            PixelsPerDip = activity.Resources.DisplayMetrics.Density;
        }

        public static Activity Activity
        {
            private set; get;
        }

        public static float PixelsPerDip
        {
            private set; get;
        }
    }
}
