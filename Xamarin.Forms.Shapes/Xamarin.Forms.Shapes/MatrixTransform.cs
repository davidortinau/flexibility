namespace Xamarin.Forms.Media
{
    public class MatrixTransform : Transform
    {
        public static readonly BindableProperty MatrixProperty =
            BindableProperty.Create("Matrix",
                                    typeof(Matrix),
                                    typeof(MatrixTransform),
                                    new Matrix(),
                                    propertyChanged: OnTransformPropertyChanged);

        public Matrix Matrix
        {
            set { SetValue(MatrixProperty, value); }
            get { return (Matrix)GetValue(MatrixProperty); }
        }

        static void OnTransformPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as MatrixTransform).OnTransformPropertyChanged();
        }

        void OnTransformPropertyChanged()
        {
            Value = Matrix;
        }
    }
}
