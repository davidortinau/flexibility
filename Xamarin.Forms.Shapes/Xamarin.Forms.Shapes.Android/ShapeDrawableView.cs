using System;

using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using droidGraphics = Android.Graphics;

using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes.Android
{
    public class ShapeDrawableView : global::Android.Views.View
    {
        ShapeDrawable drawable;
        protected float pixelsPerDip;

        droidGraphics.Path pathBackingField;

        // Classes that should be structures
        droidGraphics.RectF pathFillBounds = new droidGraphics.RectF();
        droidGraphics.RectF pathStrokeBounds = new droidGraphics.RectF();

        object stroke;
        object fill;
        droidGraphics.Shader strokeShader;
        droidGraphics.Shader fillShader;

        float strokeWidth;
        float[] dashIntervals;
        float dashPhase;
        Stretch stretch = Stretch.None;

        public ShapeDrawableView(Context context) : base(context)
        {
            drawable = new ShapeDrawable(null);
            pixelsPerDip = Resources.DisplayMetrics.Density;
        }

        // Set from descendent classes
        protected droidGraphics.Path Path
        {
            set
            {
                pathBackingField = value;
                CreatePathShape();
            }
            get
            {
                return pathBackingField;
            }
        }

        protected void CreatePathShape()
        {
            if (Path != null && !drawable.Bounds.IsEmpty)
            {
                drawable.Shape = new PathShape(Path, drawable.Bounds.Width(), drawable.Bounds.Height());
            }
            else
            {
                drawable.Shape = null;
            }

            // Find the path bounds.
            // Can't use ComputeFounds on the path because that includes Bezier control points.
            // Need to obtain the fill path first.
            if (Path != null)
            {
                using (droidGraphics.Path fillPath = new droidGraphics.Path())
                {
                    drawable.Paint.StrokeWidth = 0.01f;
                    drawable.Paint.SetStyle(droidGraphics.Paint.Style.Stroke);
                    drawable.Paint.GetFillPath(Path, fillPath);
                    fillPath.ComputeBounds(pathFillBounds, false);
                    drawable.Paint.StrokeWidth = strokeWidth;
                }
            }
            else
            {
                pathFillBounds.SetEmpty();
            }

            fillShader = null;              // really only necessary for LinearGradientBrush
            CalculatePathStrokeBounds();
        }

        // Set from above and when stroke width or style changes
        void CalculatePathStrokeBounds()
        {
            if (Path != null)
            {
                using (droidGraphics.Path strokePath = new droidGraphics.Path())
                {
                    drawable.Paint.SetStyle(droidGraphics.Paint.Style.Stroke);
                    drawable.Paint.GetFillPath(Path, strokePath);
                    strokePath.ComputeBounds(pathStrokeBounds, false);
                }
            }
            else
            {
                pathStrokeBounds.SetEmpty();
            }

            strokeShader = null;        // really only necessary of LinearGradientBrush
            Invalidate();
        }

        protected override void OnDraw(droidGraphics.Canvas canvas)
        {
            base.OnDraw(canvas);

            // Prevent clipping of negative coordinates Step 2
            // TODO: Is there a better place / way to do this?
            if (Parent != null)
                if (Parent.Parent as global::Android.Views.ViewGroup != null)
                    (Parent.Parent as global::Android.Views.ViewGroup).SetClipChildren(false);

            if (Path == null)
                return;

            droidGraphics.Matrix stretchMatrix = ComputeStretchMatrix();

            Path.Transform(stretchMatrix);
            stretchMatrix.MapRect(pathFillBounds);
            stretchMatrix.MapRect(pathStrokeBounds);

            // Special processing for Rectangle because RadiusX, RadiusY can't be subject to Stretch transform
            if (this is RectangleDrawableView)
            {
                float radiusX = (this as RectangleDrawableView).RadiusX;
                float radiusY = (this as RectangleDrawableView).RadiusY;

                Path.Reset();
                Path.AddRoundRect(pathFillBounds, radiusX, radiusY, droidGraphics.Path.Direction.Cw);       // TODO: is the direction right?
            }

            if (fill != null)
            {
                drawable.Paint.SetStyle(droidGraphics.Paint.Style.Fill);

                if (fill is Color)
                {
                    Color fillColor = (Color)fill;

                    drawable.Paint.SetARGB((int)(255 * fillColor.A),
                                           (int)(255 * fillColor.R),
                                           (int)(255 * fillColor.G),
                                           (int)(255 * fillColor.B));
                }
                else if (fillShader == null)
                {
                    if (fill is LinearGradientBrush)
                    {
                        fillShader = CreateLinearGradient(fill as LinearGradientBrush, pathFillBounds, stretchMatrix);
                    }
                    else if (fill is ImageBrush)
                    {
                        fillShader = CreateBitmapShader(fill as ImageBrush);
                    }

                    drawable.Paint.SetShader(fillShader);
                }

                drawable.Draw(canvas);
                drawable.Paint.SetShader(null);
            }

            if (stroke != null)
            {
                drawable.Paint.SetStyle(droidGraphics.Paint.Style.Stroke);

                if (stroke is Color)
                {
                    Color strokeColor = (Color)stroke;

                    drawable.Paint.SetARGB((int)(255 * strokeColor.A),
                                           (int)(255 * strokeColor.R),
                                           (int)(255 * strokeColor.G),
                                           (int)(255 * strokeColor.B));
                }
                else if (strokeShader == null)
                {
                    if (stroke is LinearGradientBrush)
                    {
                        strokeShader = CreateLinearGradient(stroke as LinearGradientBrush, pathStrokeBounds, stretchMatrix);
                    }
                    else if (stroke is ImageBrush)
                    {
                        strokeShader = CreateBitmapShader(stroke as ImageBrush);
                    }

                    drawable.Paint.SetShader(strokeShader);
                }

                drawable.Draw(canvas);
                drawable.Paint.SetShader(null);
            }

            // Return everything back to its pre-stretched state
            droidGraphics.Matrix inverseStretchMatrix = new droidGraphics.Matrix();
            stretchMatrix.Invert(inverseStretchMatrix);

            Path.Transform(inverseStretchMatrix);
            inverseStretchMatrix.MapRect(pathFillBounds);
            inverseStretchMatrix.MapRect(pathStrokeBounds);
        }

        droidGraphics.Matrix ComputeStretchMatrix()
        {
            droidGraphics.Matrix matrix = new droidGraphics.Matrix();

            // Get the drawable bounds decreased by stroke thickness
            droidGraphics.RectF drawableBounds = new droidGraphics.RectF(drawable.Bounds);
            float halfStrokeWidth = drawable.Paint.StrokeWidth / 2;
            drawableBounds.Left += halfStrokeWidth;
            drawableBounds.Top += halfStrokeWidth;
            drawableBounds.Right -= halfStrokeWidth;
            drawableBounds.Bottom -= halfStrokeWidth;

            switch (stretch)
            {
                case Stretch.None:
                    break;

                case Stretch.Fill:
                    matrix.SetRectToRect(pathFillBounds, drawableBounds, droidGraphics.Matrix.ScaleToFit.Fill);
                    break;

                case Stretch.Uniform:
                    matrix.SetRectToRect(pathFillBounds, drawableBounds, droidGraphics.Matrix.ScaleToFit.Center);
                    break;

                case Stretch.UniformToFill:
                    float widthScale = drawableBounds.Width() / pathFillBounds.Width();
                    float heightScale = drawableBounds.Height() / pathFillBounds.Height();
                    float maxScale = Math.Max(widthScale, heightScale);

                    matrix.SetScale(maxScale, maxScale);
                    matrix.PostTranslate(drawableBounds.Left - maxScale * pathFillBounds.Left,
                                         drawableBounds.Top - maxScale * pathFillBounds.Top);
                    break;
            }
            return matrix;
        }

        droidGraphics.LinearGradient CreateLinearGradient(LinearGradientBrush xamBrush, 
                                                          droidGraphics.RectF pathBounds,
                                                          droidGraphics.Matrix stretchMatrix)
        {
            if (Path == null)
                return null;

            int[] colors = new int[xamBrush.GradientStops.Count];
            float[] offsets = new float[xamBrush.GradientStops.Count];

            for (int index = 0; index < xamBrush.GradientStops.Count; index++)
            {
                colors[index] = ConvertColor(xamBrush.GradientStops[index].Color);
                offsets[index] = (float)xamBrush.GradientStops[index].Offset;
            }

            droidGraphics.Shader.TileMode tilemode = droidGraphics.Shader.TileMode.Clamp;

            switch (xamBrush.SpreadMethod)
            {
                case GradientSpreadMethod.Pad:
                    tilemode = droidGraphics.Shader.TileMode.Clamp;
                    break;

                case GradientSpreadMethod.Refect:
                    tilemode = droidGraphics.Shader.TileMode.Mirror;
                    break;

                case GradientSpreadMethod.Repeat:
                    tilemode = droidGraphics.Shader.TileMode.Repeat;
                    break;
            }

            // pathBounds has already been stretched
            using (droidGraphics.RectF xformedBounds = new droidGraphics.RectF(pathBounds))
            {
                if (xamBrush.Transform != null)
                {
                    // But the brush transform offsets needs to be stretched
                    droidGraphics.Matrix transform = xamBrush.Transform.GetNativeObject() as droidGraphics.Matrix;

                    float[] stretchValues = new float[9];
                    stretchMatrix.GetValues(stretchValues);

                    float[] transformValues = new float[9];
                    transform.GetValues(transformValues);

                    // Scale x-offset by stretch
                    transformValues[2] *= stretchValues[0];

                    // Scale y-offset by stretch
                    transformValues[5] *= stretchValues[4]; 

                    using (droidGraphics.Matrix matx = new droidGraphics.Matrix())
                    {
                        matx.SetValues(transformValues); 

                        float[] a2 = new float[9];
                        matx.GetValues(a2);

                        matx.MapRect(xformedBounds);
                    }
                }

                return new droidGraphics.LinearGradient((float)xamBrush.StartPoint.X * xformedBounds.Width() + xformedBounds.Left,
                                                        (float)xamBrush.StartPoint.Y * xformedBounds.Height() + xformedBounds.Top,
                                                        (float)xamBrush.EndPoint.X * xformedBounds.Width() + xformedBounds.Left,
                                                        (float)xamBrush.EndPoint.Y * xformedBounds.Height() + xformedBounds.Top,
                                                        colors, offsets, tilemode);
            }
        }

        droidGraphics.BitmapShader CreateBitmapShader(ImageBrush imageBrush)
        {
            droidGraphics.Bitmap bitmap = imageBrush.NativeBitmap as droidGraphics.Bitmap;

            if (bitmap == null)
                return null;

            return new droidGraphics.BitmapShader(bitmap, droidGraphics.Shader.TileMode.Repeat,
                                                          droidGraphics.Shader.TileMode.Repeat);
        }

        int ConvertColor(Color color)
        {
            return droidGraphics.Color.Argb((int)(255 * color.A), (int)(255 * color.R), (int)(255 * color.G), (int)(255 * color.B));


        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Path != null)
            {
                return new SizeRequest(new Size(Math.Max(0, pathStrokeBounds.Right),
                                                Math.Max(0, pathStrokeBounds.Bottom)));
            }

            return new SizeRequest();
        }

        // Methods called from ShapeRenderer
        public void SetFillColor(object color)
        {
            fill = color;
            fillShader = null;
            Invalidate();
        }

        public void SetStrokeColor(object color)
        {
            stroke = color;
            strokeShader = null;
            Invalidate();
        }

        public void SetStrokeWidth(float width)
        {
            strokeWidth = pixelsPerDip * width;
            drawable.Paint.StrokeWidth = strokeWidth;
            SetDashPathEffect();
        }

        public void SetDashIntervals(float[] intervals)
        {
            dashIntervals = intervals;
            SetDashPathEffect();
        }

        public void SetDashPhase(float phase)
        {
            dashPhase = phase;
            SetDashPathEffect();
        }

        public void SetDashPathEffect()
        {
            if (dashIntervals != null && dashIntervals.Length > 1)
            {
                float[] adjIntervals = new float[dashIntervals.Length];

                for (int i = 0; i < dashIntervals.Length; i++)
                    adjIntervals[i] = dashIntervals[i] * strokeWidth;

                drawable.Paint.SetPathEffect(new droidGraphics.DashPathEffect(adjIntervals, dashPhase * strokeWidth));
            }
            else
            {
                drawable.Paint.SetPathEffect(null);         // does this get rid of the previous one???
            }

            CalculatePathStrokeBounds();
        }

        public void SetStrokeCap(droidGraphics.Paint.Cap strokeCap)
        {
            drawable.Paint.StrokeCap = strokeCap;
            CalculatePathStrokeBounds();
        }

        public void SetStrokeJoin(droidGraphics.Paint.Join strokeJoin)
        {
            drawable.Paint.StrokeJoin = strokeJoin;
            Invalidate();
        }

        public void SetStrokeMiterLimit(float miterLimit)
        {
            drawable.Paint.StrokeMiter = miterLimit * 2;        // TODO: Check if this is correct
            CalculatePathStrokeBounds();
        }

        public void SetSize(double width, double height)
        {
            float pixelsPerDip = Resources.DisplayMetrics.Density;
            drawable.SetBounds(0, 0, (int)(width * pixelsPerDip),
                                     (int)(height * pixelsPerDip));
            CreatePathShape();
        }

        public void SetStretch(Stretch stretch)
        {
            this.stretch = stretch;
            fillShader = null;
            strokeShader = null;
            Invalidate();
        }
    }
}