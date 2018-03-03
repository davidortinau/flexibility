using System;
using droidGraphics = Android.Graphics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.Android.NativeTransform))]

namespace Xamarin.Forms.Media.Android
{
    class NativeTransform : NativeObject, INativeTransform
    {
        droidGraphics.Matrix droidMatrix = new droidGraphics.Matrix();

        public object ConvertToNative(GeneralTransform transform)
        {
            Matrix matrix = transform.Value;

            droidMatrix.SetValues(new float[] { (float)matrix.M11, (float)matrix.M21, PixelsPerDip * (float)matrix.OffsetX,
                                                (float)matrix.M12, (float)matrix.M22, PixelsPerDip * (float)matrix.OffsetY,
                                                (float)0, (float)0, (float)1 });
            return droidMatrix;
        }
    }
}