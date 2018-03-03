using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using winFound = Windows.Foundation;
using winUI = Windows.UI;
using winMedia = Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes.WinRT
{
    static class Matrix3x2Extensions
    {
        public static winFound.Point Transform(this Matrix3x2 matrix, winFound.Point point)
        {
            return new winFound.Point(matrix.M11 * point.X + matrix.M21 * point.Y + matrix.M31,
                                      matrix.M12 * point.X + matrix.M22 * point.Y + matrix.M32);
        }

        public static winFound.Rect Transform(this Matrix3x2 matrix, winFound.Rect rect)
        {
            winFound.Point point1 = matrix.Transform(new winFound.Point(rect.Left, rect.Top));
            winFound.Point point2 = matrix.Transform(new winFound.Point(rect.Right, rect.Bottom));

            return new winFound.Rect(point1, point2);
        }
    }


    class BrushWrapper
    {
        public ICanvasBrush Brush { set; get; }

        public Vector2 StartPoint { set; get; }

        public Vector2 EndPoint { set; get; }

        public CanvasBitmap Bitmap { set; get; }

        public void SetGradientPoints(winFound.Rect bounds)
        {
            CanvasLinearGradientBrush brush = Brush as CanvasLinearGradientBrush;

            if (brush == null)
                return;

            brush.StartPoint = new Vector2((float)(bounds.Left + StartPoint.X * bounds.Width),
                                           (float)(bounds.Top + StartPoint.Y * bounds.Height));

            brush.EndPoint = new Vector2((float)(bounds.Left + EndPoint.X * bounds.Width),
                                         (float)(bounds.Top + EndPoint.Y * bounds.Height));
        }

        public bool SetImageBrushBitmap()
        {
            if (Bitmap == null)
                return false;

            CanvasImageBrush brush = Brush as CanvasImageBrush;

            if (brush == null)
                return false;

            brush.Image = Bitmap;

            return true;
        }
    }

    public abstract class ShapeCanvasControl : UserControl
    {
        CanvasGeometry baseGeometry;        // Includes all geometry transforms
        winFound.Rect geometryBounds;
        winFound.Rect geometryStrokeBounds;

        CanvasGeometry stretchedGeometry;   // After applicatin of Stretch property
        winFound.Rect stretchedGeometryBounds;
        winFound.Rect stretchedGeometryStrokeBounds;

        // For display of negative coordinates.
        Canvas canvas;
        Matrix3x2 renderMatrix = Matrix3x2.Identity;

        Brush xamStroke, xamFill;           // TODO: Move these to the wrappers?

        readonly BrushWrapper strokeBrushWrapper = new BrushWrapper();
        readonly BrushWrapper fillBrushWrapper = new BrushWrapper();

        winFound.Size elementSize;
        float strokeWidth;
        CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle();
        Stretch stretch;

        // TODO: Handle Loaded and Unloaded to create, set event handlers, and later remove from visual tree

        public ShapeCanvasControl()
        {
            CanvasControl = new CanvasControl();
            Content = CanvasControl; //                   new Canvas { Children = { CanvasControl } };
            CanvasControl.Draw += OnCanvasControlDraw;
            CanvasControl.CreateResources += OnCanvasControlCreateResources;

            canvas = new Canvas();
        }

        // Needs to be public because RectangleCanvasControl uses it
        //  to invalidate the CanvasControl
        public CanvasControl CanvasControl
        {
            private set; get;
        }

        void OnCanvasControlCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            BuildGeometry(sender);
            ConvertBrush(sender, xamStroke, strokeBrushWrapper);
            ConvertBrush(sender, xamFill, fillBrushWrapper);

            InvalidateMeasure();
        }

        void OnCanvasControlDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            CanvasDrawingSession session = args.DrawingSession;

            // Recreate the geometry if necessary
            if (baseGeometry == null)
            {
                BuildGeometry(session);
            }

            // Don't continue if stretchedGeometry does not exist
            if (stretchedGeometry == null)
                return;

            // Recreate the brushes if necessary.
            if (xamStroke != null && strokeBrushWrapper.Brush == null)
            {
                ConvertBrush(session, xamStroke, strokeBrushWrapper);
            }

            if (xamFill != null && fillBrushWrapper.Brush == null)
            {
                ConvertBrush(session, xamFill, fillBrushWrapper);
            }

            // Don't continue if both brushes are null.
            if (strokeBrushWrapper.Brush == null && fillBrushWrapper.Brush == null)
                return;

            // If it's a Rectangle, RadiusX and RadiusY must be independent of Stretch
            if (this is RectangleCanvasControl)
            {
                float radiusX = (this as RectangleCanvasControl).RadiusX;
                float radiusY = (this as RectangleCanvasControl).RadiusY;
                stretchedGeometry = CanvasGeometry.CreateRoundedRectangle(session, stretchedGeometryBounds, radiusX, radiusY);
            }

            // Shift so negative coordinates are visible.
            session.Transform = renderMatrix;

            // Brush-fixing logic. TODO: This is similar for stroke and fill and can probably
            //  be consolidated in a Func. 
            if (fillBrushWrapper.Brush != null)
            {
                bool okToRender = true;

                if (fillBrushWrapper.Brush is CanvasLinearGradientBrush)
                {
                    fillBrushWrapper.SetGradientPoints(stretchedGeometryBounds);
                }
                else if (fillBrushWrapper.Brush is CanvasImageBrush)
                {
                    okToRender = fillBrushWrapper.SetImageBrushBitmap();
                }

                if (okToRender)
                {
                    session.FillGeometry(stretchedGeometry, fillBrushWrapper.Brush);
                }
            }
            if (strokeBrushWrapper.Brush != null)
            {
                bool okToRender = true;

                if (strokeBrushWrapper.Brush is CanvasLinearGradientBrush)
                {
                    strokeBrushWrapper.SetGradientPoints(stretchedGeometryStrokeBounds);
                }
                else if (strokeBrushWrapper.Brush is CanvasImageBrush)
                {
                    okToRender = strokeBrushWrapper.SetImageBrushBitmap();
                }

                if (okToRender)
                {
                    session.DrawGeometry(stretchedGeometry, strokeBrushWrapper.Brush, strokeWidth, strokeStyle);
                }
            }
        }

        // Called from descendent classes to re-create geometry during redrawing
        protected void InvalidateGeometry()
        {
            baseGeometry = null;
            CanvasControl.Invalidate();
        }

        void BuildGeometry(ICanvasResourceCreator resourceCreator)
        {
            baseGeometry = BuildBaseGeometry(resourceCreator);
            CalculateGeometryBounds();
        }

        // Called from above and whenever strokeWidth or strokeStyle changes.
        void CalculateGeometryBounds()
        {
            ComputeGeometryBounds(baseGeometry, ref geometryBounds, ref geometryStrokeBounds);
            BuildStretchGeometry();
        }

        // Called from above whenever elementSize or Stretch property changes
        void BuildStretchGeometry()
        {
            AdaptForNegativeCoordinates();
            stretchedGeometry = StretchGeometry();
            ComputeGeometryBounds(stretchedGeometry, ref stretchedGeometryBounds, ref stretchedGeometryStrokeBounds);
        }

        // Method implemented by descendent classes
        protected abstract CanvasGeometry BuildBaseGeometry(ICanvasResourceCreator resourceCreator);

        // Used for both baseGeometry and stretchedGeometry
        void ComputeGeometryBounds(CanvasGeometry geometry, ref winFound.Rect geometryBounds, ref winFound.Rect geometryStrokeBounds)
        {
            if (geometry != null)
            {
                geometryBounds = geometry.ComputeBounds();
                geometryStrokeBounds = geometry.ComputeStrokeBounds(strokeWidth, strokeStyle);
            }
            else
            {
                geometryBounds = new winFound.Rect();
                geometryStrokeBounds = new winFound.Rect();
            }
        }

        CanvasGeometry StretchGeometry()
        {
            if (baseGeometry == null)
                return null;

            if (elementSize.Width == 0 || elementSize.Height == 0)
                return null;

            Matrix3x2 matrix = Matrix3x2.Identity;

            // Get bounds of this control reduced by stroke thickness.
            winFound.Rect ctrlBounds = new winFound.Rect(strokeWidth / 2,
                                                         strokeWidth / 2,
                                                         Math.Max(0, elementSize.Width - strokeWidth),
                                                         Math.Max(0, elementSize.Height - strokeWidth));

            // Calculate scaling factors
            float widthScale = (float)(ctrlBounds.Width / geometryBounds.Width);
            float heightScale = (float)(ctrlBounds.Height / geometryBounds.Height);

            switch (stretch)
            {
                case Stretch.None:
                    break;

                case Stretch.Fill:
                    matrix = Matrix3x2.Multiply(Matrix3x2.CreateScale(widthScale, heightScale),
                                                Matrix3x2.CreateTranslation((float)(ctrlBounds.Left - widthScale * geometryBounds.Left),
                                                                            (float)(ctrlBounds.Top - heightScale * geometryBounds.Top)));
                    break;

                case Stretch.Uniform:
                    float minScale = Math.Min(widthScale, heightScale);
                    matrix = Matrix3x2.Multiply(Matrix3x2.CreateScale(minScale, minScale),
                                                Matrix3x2.CreateTranslation((float)(ctrlBounds.Left - minScale * geometryBounds.Left +
                                                                                        (ctrlBounds.Width - minScale * geometryBounds.Width) / 2),
                                                                            (float)(ctrlBounds.Top - minScale * geometryBounds.Top +
                                                                                        (ctrlBounds.Height - minScale * geometryBounds.Height) / 2)));
                    break;

                case Stretch.UniformToFill:
                    float maxScale = Math.Max(widthScale, heightScale);
                    matrix = Matrix3x2.Multiply(Matrix3x2.CreateScale(maxScale, maxScale),
                                                Matrix3x2.CreateTranslation((float)(ctrlBounds.Left - maxScale * geometryBounds.Left),
                                                                            (float)(ctrlBounds.Top - maxScale * geometryBounds.Top)));
                    break;
            }

            return baseGeometry.Transform(matrix);
        }

        void AdaptForNegativeCoordinates()
        {
            if (stretch == Stretch.None)
            {
                // Check that the visual tree is correct
                if (!(Content is Canvas))
                {
                    Content = null;
                    Content = canvas;
                    canvas.Children.Add(CanvasControl);
                }

                CanvasControl.Width = geometryStrokeBounds.Right - Math.Min(0, geometryStrokeBounds.Left);
                CanvasControl.Height = geometryStrokeBounds.Bottom - Math.Min(0, geometryStrokeBounds.Top);

                Canvas.SetLeft(CanvasControl, Math.Min(0, geometryStrokeBounds.Left));
                Canvas.SetTop(CanvasControl, Math.Min(0, geometryStrokeBounds.Top));

                renderMatrix = Matrix3x2.CreateTranslation((float)-Math.Min(0, geometryStrokeBounds.Left),
                                                           (float)-Math.Min(0, geometryStrokeBounds.Top));
            }

            else
            {
                // Check that the visual tree is correct
                if (!(Content is CanvasControl))
                {
                    Content = null;
                    canvas.Children.Clear();
                    Content = CanvasControl;
                }

                CanvasControl.Width = Double.NaN;
                CanvasControl.Height = Double.NaN;

                renderMatrix = Matrix3x2.Identity;
            }
        }

        // Methods called from ShapeRenderer
        public void SetSize(winFound.Size size)
        {
            elementSize = size;
            BuildStretchGeometry();
            CanvasControl.Invalidate();
        }

        public void SetStroke(Brush brush)
        {
            xamStroke = brush;
            strokeBrushWrapper.Brush = null;
            CanvasControl.Invalidate();
        }

        public void SetFill(Brush brush)
        {
            xamFill = brush;
            fillBrushWrapper.Brush = null;
            CanvasControl.Invalidate();
        }

        public void SetStrokeStyle(CanvasStrokeStyle strokeStyle)
        {
            this.strokeStyle = strokeStyle;
            CalculateGeometryBounds();
            CanvasControl.Invalidate();
        }

        public void SetStrokeWidth(float strokeWidth)
        {
            this.strokeWidth = strokeWidth;
            CalculateGeometryBounds();
            CanvasControl.Invalidate();
        }

        public void SetStretch(Stretch stretch)
        {
            this.stretch = stretch;
            BuildStretchGeometry();
            CanvasControl.Invalidate();
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(Math.Max(0, geometryStrokeBounds.Right), 
                                            Math.Max(0, geometryStrokeBounds.Bottom)));
        }

        async void ConvertBrush(ICanvasResourceCreator resourceCreator, Brush xamBrush, BrushWrapper brushWrapper)
        {
            if (xamBrush == null)
            {
                brushWrapper.Brush = null;
            }
              
            else if (xamBrush is SolidColorBrush)
            {
                brushWrapper.Brush = new CanvasSolidColorBrush(resourceCreator, ConvertColor((xamBrush as SolidColorBrush).Color));
            }
            else if (xamBrush is LinearGradientBrush)
            {
                LinearGradientBrush xamGradient = xamBrush as LinearGradientBrush;
                CanvasGradientStop[] gradientStops = new CanvasGradientStop[xamGradient.GradientStops.Count];

                for (int i = 0; i < xamGradient.GradientStops.Count; i++)
                {
                    gradientStops[i] = new CanvasGradientStop
                    {
                        Position = (float)xamGradient.GradientStops[i].Offset,
                        Color = ConvertColor(xamGradient.GradientStops[i].Color)
                    };
                }

                // enumeration values of 0, 1, 2 translated to 0, 2, 1
                CanvasEdgeBehavior edgeBehavior = (CanvasEdgeBehavior)((3 - (int)xamGradient.SpreadMethod) % 3);

                CanvasLinearGradientBrush winGradient = new CanvasLinearGradientBrush(resourceCreator,
                                                                                      gradientStops,
                                                                                      edgeBehavior,
                                                                                      CanvasAlphaMode.Straight);

                // TODO: Opacity and Transform

                brushWrapper.Brush = winGradient;
                brushWrapper.StartPoint = ConvertPoint(xamGradient.StartPoint);
                brushWrapper.EndPoint = ConvertPoint(xamGradient.EndPoint);
            }
            else if (xamBrush is ImageBrush)
            {
                ImageBrush xamImageBrush = (ImageBrush)xamBrush;

                CanvasImageBrush imageBrush = new CanvasImageBrush(resourceCreator);
                imageBrush.ExtendX = CanvasEdgeBehavior.Wrap;
                imageBrush.ExtendY = CanvasEdgeBehavior.Wrap;
                brushWrapper.Brush = imageBrush;
                brushWrapper.Bitmap = null;

                // TODO: Perhaps a few try and catch blocks would be appropriate here. You know. Just in case.

                if (xamImageBrush.ImageSource != null)
                {
                    CanvasBitmap bitmap = null;

                    if (xamImageBrush.ImageSource is UriImageSource)
                    {
                        bitmap = await CanvasBitmap.LoadAsync(resourceCreator, ((UriImageSource)xamImageBrush.ImageSource).Uri.ToString());
                    }
                    else if (xamImageBrush.ImageSource is StreamImageSource)
                    {
                        StreamImageSource streamSource = (StreamImageSource)xamImageBrush.ImageSource;
                        var func = ((StreamImageSource)xamImageBrush.ImageSource).Stream;

                        Task<Stream> task = func(new CancellationToken());

                        // TODO: Is this OK if it's not awaited?
                        // Otherwise we have a problem because the resourceCreator must be valid in the next step
                        Stream stream = task.Result;

                        bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream.AsRandomAccessStream());
                    }
                    else if (xamImageBrush.ImageSource is FileImageSource)
                    {
                        FileImageSource fileSource = (FileImageSource)xamImageBrush.ImageSource;
                        bitmap = await CanvasBitmap.LoadAsync(resourceCreator, fileSource.File);
                    }

                    brushWrapper.Bitmap = bitmap;
                    CanvasControl.Invalidate();
                }
            }
        }

        protected Vector2 ConvertPoint(Point point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }

        protected winUI.Color ConvertColor(Color color)
        {
            return winUI.Color.FromArgb((byte)(255 * color.A), 
                                        (byte)(255 * color.R), 
                                        (byte)(255 * color.G), 
                                        (byte)(255 * color.B));
        }
    }
}
