using System;
using winMedia = Windows.UI.Xaml.Media;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Media.WinRT.NativeTransform))]

namespace Xamarin.Forms.Media.WinRT
{
    public class NativeTransform : NativeObject, INativeTransform
    {
        public object ConvertToNative(GeneralTransform transform)
        {
            Matrix matrix = transform.Value;

            return new winMedia.MatrixTransform
            {
                Matrix = new winMedia.Matrix(matrix.M11, matrix.M12,
                                             matrix.M21, matrix.M22,
                                             matrix.OffsetX, matrix.OffsetY)
            };
        }
    }
}
