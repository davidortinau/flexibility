using System;

namespace Xamarin.Forms.Media
{
    public class RotateTransform : Transform
    {
        public static readonly BindableProperty AngleProperty =
            BindableProperty.Create("Angle",
                                    typeof(double),
                                    typeof(RotateTransform),
                                    0.0,
                                    propertyChanged: OnTransformPropertyChanged);

        public static readonly BindableProperty CenterXProperty =
            BindableProperty.Create("CenterX",
                                    typeof(double),
                                    typeof(RotateTransform),
                                    0.0,
                                    propertyChanged: OnTransformPropertyChanged);

        public static readonly BindableProperty CenterYProperty =
            BindableProperty.Create("CenterY",
                                    typeof(double),
                                    typeof(RotateTransform),
                                    0.0,
                                    propertyChanged: OnTransformPropertyChanged);

        public double Angle
        {
            set { SetValue(AngleProperty, value); }
            get { return (double)GetValue(AngleProperty); }
        }

        public double CenterX
        {
            set { SetValue(CenterXProperty, value); }
            get { return (double)GetValue(CenterXProperty); }
        }

        public double CenterY
        {
            set { SetValue(CenterYProperty, value); }
            get { return (double)GetValue(CenterYProperty); }
        }

        static void OnTransformPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as RotateTransform).OnTransformPropertyChanged();
        }

        void OnTransformPropertyChanged()
        {
            double radians = Math.PI * Angle / 180;
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            // Setting Value in RotateTransform triggers another PropertyChanged
            Value = new Matrix(cos, 
                               sin, 
                               -sin, 
                               cos, 
                               CenterX * (1 - cos) + CenterY * sin, 
                               CenterY * (1 - cos) - CenterX * sin);
        }
    }
}
