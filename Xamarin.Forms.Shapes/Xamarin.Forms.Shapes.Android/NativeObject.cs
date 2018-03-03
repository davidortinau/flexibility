using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

namespace Xamarin.Forms.Media.Android
{
    public class NativeObject 
    {
        public NativeObject()
        {
            Activity activity = Xamarin.Forms.Shapes.Android.Shapes.Activity;
            PixelsPerDip = activity.Resources.DisplayMetrics.Density;
        }

        public float PixelsPerDip { set; get; }
    }
}