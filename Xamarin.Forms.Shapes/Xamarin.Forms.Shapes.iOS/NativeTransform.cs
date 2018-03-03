using System;
using CoreGraphics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.iOS.NativeTransform))]

namespace Xamarin.Forms.Media.iOS
{
    class NativeTransform : NativeObject, INativeTransform
    {
        public object ConvertToNative(GeneralTransform transform)
        {
            Matrix matrix = transform.Value;

            return new CGAffineTransform(new nfloat(matrix.M11),
                                         new nfloat(matrix.M12),
                                         new nfloat(matrix.M21),
                                         new nfloat(matrix.M22),
                                         new nfloat(matrix.OffsetX),
                                         new nfloat(matrix.OffsetY));
        }
    }
}