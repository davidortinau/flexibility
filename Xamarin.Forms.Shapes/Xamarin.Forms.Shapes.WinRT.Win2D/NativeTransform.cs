using System;
using System.Numerics;

using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.WinRT.NativeTransform))]

namespace Xamarin.Forms.Media.WinRT
{
    public class NativeTransform : INativeTransform
    {
        public object ConvertToNative(GeneralTransform transform)
        {
            Matrix matrix = transform.Value;

            return new Matrix3x2((float)matrix.M11,
                                 (float)matrix.M12,
                                 (float)matrix.M21, 
                                 (float)matrix.M22,
                                 (float)matrix.OffsetX, 
                                 (float)matrix.OffsetY);
        }
    }
}
