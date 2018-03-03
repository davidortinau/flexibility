using System;

using CoreAnimation;
using CoreGraphics;
using UIKit;

using Xamarin.Forms.Media;

namespace Xamarin.Forms.Shapes.iOS
{
    // This CALayer derivative is necessary to render negative coordinates
    public class ShapeDrawingLayer : CALayer
    {
        UIView view;

        // Path and sizes
        CGPath basicPath;
        CGRect basicPathBounds;
        CGRect basicPathStrokeBounds;

        CGPath renderPath;
        CGRect renderPathBounds;
        CGRect renderPathStrokeBounds;

        bool isNonzeroFill = false;

        // Brushes
        object stroke;  // either CGColor or LinearGradientBrush or ImageBrush
        object fill;    // ditto

        // Stroke attributes
        nfloat dashPhase;
        nfloat[] dashLengths;
        nfloat strokeWidth;
        CGLineCap strokeLineCap = CGLineCap.Butt;
        CGLineJoin strokeLineJoin = CGLineJoin.Miter;
        nfloat strokeMiterLimit = 10;
        Stretch stretch = Stretch.None;

        public ShapeDrawingLayer(UIView view)
        {
            this.view = view;
        }

        public override void DrawInContext(CGContext ctx)
        {
            base.DrawInContext(ctx);
            RenderPath(ctx, Bounds);
        }

        // Called by ShapeUIView derivatives
        public void SetIsNonzeroFill(bool isNonzeroFill)
        {
            this.isNonzeroFill = isNonzeroFill;
            SetNeedsDisplay();
        }

        // Called by ShapeUIView derivatives
        public void SetBasicPath(CGPath path)
        {
            basicPath = path;

            if (basicPath != null)
            {
                basicPathBounds = basicPath.PathBoundingBox;
            }
            else
            {
                basicPathBounds = new CGRect();
            }

            CalculateBasicPathStrokeBounds();
        }

        // Called by above and when stroke properties change
        void CalculateBasicPathStrokeBounds()
        {
            if (basicPath != null)
            {
                basicPathStrokeBounds = basicPath.CopyByStrokingPath(strokeWidth,
                                                                     strokeLineCap,
                                                                     strokeLineJoin,
                                                                     strokeMiterLimit).PathBoundingBox;
            }
            else
            {
                basicPathStrokeBounds = new CGRect();
            }

            BuildRenderPath();
        }

        // Called by above and when Stretch and render-size changes.
        void BuildRenderPath()
        {
            if (basicPath == null)
            {
                renderPath = null;
                renderPathBounds = new CGRect();
                renderPathStrokeBounds = new CGRect();
                return;
            }

            // Disable animations.
            CATransaction.Begin();
            CATransaction.DisableActions = true;

            if (stretch != Stretch.None)
            {
                // Adjust for Stretch setting
                CGRect viewBounds = Bounds;
                viewBounds.X += strokeWidth / 2;
                viewBounds.Y += strokeWidth / 2;
                viewBounds.Width -= strokeWidth;
                viewBounds.Height -= strokeWidth;

                // Compare with path geometry only (i.e., basicPathBounds rather than basicPathStrokeBounds)
                nfloat widthScale = viewBounds.Width / basicPathBounds.Width;
                nfloat heightScale = viewBounds.Height / basicPathBounds.Height;
                CGAffineTransform stretchTransform = CGAffineTransform.MakeIdentity();

                switch (stretch)
                {
                    case Stretch.None:
                        break;

                    case Stretch.Fill:
                        stretchTransform.Scale(widthScale, heightScale);

                        stretchTransform.Translate(viewBounds.Left - widthScale * basicPathBounds.Left,
                                                    viewBounds.Top - heightScale * basicPathBounds.Top);
                        break;

                    case Stretch.Uniform:
                        nfloat minScale = NMath.Min(widthScale, heightScale);

                        stretchTransform.Scale(minScale, minScale);

                        stretchTransform.Translate(viewBounds.Left - minScale * basicPathBounds.Left +                  // scale and position
                                                        (viewBounds.Width - minScale * basicPathBounds.Width) / 2,       //  and center horizontally
                                                    viewBounds.Top - minScale * basicPathBounds.Top +                    // scale and position
                                                        (viewBounds.Height - minScale * basicPathBounds.Height) / 2);    //  and center vertically
                        break;

                    case Stretch.UniformToFill:
                        nfloat maxScale = NMath.Max(widthScale, heightScale);

                        stretchTransform.Scale(maxScale, maxScale);

                        stretchTransform.Translate(viewBounds.Left - maxScale * basicPathBounds.Left,
                                                    viewBounds.Top - maxScale * basicPathBounds.Top);
                        break;
                }

                Frame = Bounds;
                renderPath = basicPath.CopyByTransformingPath(stretchTransform);
            }
            else
            {
                // Adjust non-stretched paths for negative coordinates.
                nfloat adjustX = NMath.Min(0, basicPathStrokeBounds.X);
                nfloat adjustY = NMath.Min(0, basicPathStrokeBounds.Y);

                if (adjustX < 0 || adjustY < 0)
                {
                    nfloat width = Bounds.Width;
                    nfloat height = Bounds.Height;

                    if (basicPathStrokeBounds.Width > Bounds.Width)
                    {
                        width = Bounds.Width - adjustX;
                    }
                    if (basicPathStrokeBounds.Height > Bounds.Height)
                    {
                        height = Bounds.Height - adjustY;
                    }

                    Frame = new CGRect(adjustX, adjustY, width, height);
                    CGAffineTransform xform = new CGAffineTransform(Bounds.Width / width, 0,
                                                                    0, Bounds.Height / height,
                                                                    -adjustX, -adjustY);
                    renderPath = basicPath.CopyByTransformingPath(xform);
                }
                else
                {
                    Frame = Bounds;
                    renderPath = basicPath.CopyByTransformingPath(CGAffineTransform.MakeIdentity());
                }
            }

            renderPathBounds = renderPath.PathBoundingBox;

            renderPathStrokeBounds = renderPath.CopyByStrokingPath(strokeWidth,
                                                                   strokeLineCap,
                                                                   strokeLineJoin,
                                                                   strokeMiterLimit).PathBoundingBox;
            CATransaction.Commit();

            SetNeedsDisplay();
        }

        // Drawing method
        void RenderPath(CGContext graphics, CGRect bounds)
        {
            if (basicPath == null)
                return;

            if (stroke == null && fill == null)
                return;

            // Disable animations.
            CATransaction.Begin();
            CATransaction.DisableActions = true;

            // If it's a Rectangle, RadiusX and RadiusY must be independent of Stretch
            if (view is RectangleUIView)
            {
                // BoundingBox is the same as PathBoundingBox for Rectangle but probably cheaper
                CGRect renderPathBounds = renderPath.BoundingBox;      
                nfloat radiusX = (view as RectangleUIView).RadiusX;
                nfloat radiusY = (view as RectangleUIView).RadiusY;

                renderPath = new CGPath();
                renderPath.AddRoundedRect(renderPathBounds, radiusX, radiusY);
            }

            // Set various graphics attributes for stroking
            graphics.SetLineWidth(strokeWidth);
            graphics.SetLineDash(dashPhase * strokeWidth, dashLengths);
            graphics.SetLineCap(strokeLineCap);
            graphics.SetLineJoin(strokeLineJoin);
            graphics.SetMiterLimit(strokeMiterLimit * strokeWidth / 4);     // TODO: Check if this is right

            if (stroke is CGColor && fill is CGColor)
            {
                graphics.AddPath(renderPath);
                graphics.SetStrokeColor((CGColor)stroke);
                graphics.SetFillColor((CGColor)fill);
                graphics.DrawPath(isNonzeroFill ? CGPathDrawingMode.FillStroke : CGPathDrawingMode.EOFillStroke);
            }
            else
            {
                if (fill is CGColor)
                {
                    graphics.AddPath(renderPath);
                    graphics.SetFillColor((CGColor)fill);
                    graphics.DrawPath(isNonzeroFill ? CGPathDrawingMode.Fill : CGPathDrawingMode.EOFill);
                }
                else if (fill != null)
                {
                    graphics.AddPath(renderPath);

                    if (fill is LinearGradientBrush)
                    {
                        if (isNonzeroFill)
                        {
                            graphics.Clip();
                        }
                        else
                        {
                            graphics.EOClip();
                        }

                        // Fill the area with a gradient
                        RenderLinearGradient(graphics, renderPathBounds, fill as LinearGradientBrush);
                    }
                    else if (fill is ImageBrush)
                    {
                        RenderImageBrush(graphics, fill as ImageBrush, true);
                    }
                }

                if (stroke is CGColor)
                {
                    graphics.AddPath(renderPath);
                    graphics.SetStrokeColor((CGColor)stroke);
                    graphics.DrawPath(CGPathDrawingMode.Stroke);
                }
                else if (stroke != null)
                {
                    graphics.AddPath(renderPath);

                    if (stroke is LinearGradientBrush)
                    {
                        graphics.ReplacePathWithStrokedPath();
                        graphics.Clip();

                        // Stroke with a gradient
                        RenderLinearGradient(graphics, renderPathStrokeBounds, stroke as LinearGradientBrush);
                    }
                    else if (stroke is ImageBrush)
                    {
                        RenderImageBrush(graphics, stroke as ImageBrush, false);
                    }
                }
            }

            CATransaction.Commit();
        }

        void RenderLinearGradient(CGContext graphics, CGRect pathBounds, LinearGradientBrush brush)
        {
            using (CGColorSpace rgb = CGColorSpace.CreateDeviceRGB())
            {
                CGColor[] colors = new CGColor[brush.GradientStops.Count];
                nfloat[] locations = new nfloat[brush.GradientStops.Count];

                for (int index = 0; index < brush.GradientStops.Count; index++)
                {
                    Color color = brush.GradientStops[index].Color;

                    colors[index] = new CGColor(new nfloat(color.R), new nfloat(color.G), new nfloat(color.B), new nfloat(color.A));
                    locations[index] = new nfloat(brush.GradientStops[index].Offset);
                }

                CGGradient gradient = new CGGradient(rgb, colors, locations);

                if (brush.Transform != null)
                {
                    CGAffineTransform transform = (CGAffineTransform)brush.Transform.GetNativeObject();

                    // TODO: This does not adequately work.
                    //  There is no real SpreadMethod in iOS.
                    pathBounds = transform.TransformRect(pathBounds);
                }

                graphics.DrawLinearGradient(gradient, new CGPoint(pathBounds.Left + brush.StartPoint.X * pathBounds.Width,
                                                                  pathBounds.Top + brush.StartPoint.Y * pathBounds.Height),
                                                      new CGPoint(pathBounds.Left + brush.EndPoint.X * pathBounds.Width,
                                                                  pathBounds.Top + brush.EndPoint.Y * pathBounds.Height),
                                                      CGGradientDrawingOptions.DrawsBeforeStartLocation |
                                                      CGGradientDrawingOptions.DrawsAfterEndLocation);
            }
        }


        // TODO: Want to create and retain UIImage so it doesn't have to be reloaded!!!!

        void RenderImageBrush(CGContext context, ImageBrush imageBrush, bool isFill)
        {
            UIImage image = imageBrush.NativeBitmap as UIImage;

            if (image == null)
                return;

            System.Diagnostics.Debug.WriteLine("Image: {0} x {1}", image.CGImage.Width, image.CGImage.Height);

            using (CGColorSpace space = CGColorSpace.CreatePattern(null))
            {
                if (isFill)
                {
                    context.SetFillColorSpace(space);
                }
                else
                {
                    context.SetStrokeColorSpace(space);
                }
            }

            nfloat scale = UIScreen.MainScreen.NativeScale;
            CGRect cell = new CGRect(0, 0, image.Size.Width / scale, image.Size.Height / scale);

            using (CGPattern pattern = new CGPattern(cell,
                                                CGAffineTransform.MakeIdentity(),
                                                cell.Width, cell.Height,
                                                CGPatternTiling.NoDistortion,
                                                true,
                                                (CGContext localContext) =>
                                                {
                                                    localContext.DrawImage(cell, image.CGImage);
                                                }))
            {
                nfloat[] alpha = { 1 };

                if (isFill)
                {
                    context.SetFillPattern(pattern, alpha);
                    context.FillPath();
                }
                else
                {
                    context.SetStrokePattern(pattern, alpha);
                    context.StrokePath();
                }
            }
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(Math.Max(0, basicPathStrokeBounds.Right), 
                                            Math.Max(0, basicPathStrokeBounds.Bottom)));
        }

        public void SetSize(CGSize size)
        {
            Bounds = new CGRect(new CGPoint(), size);           // Set bounds of sub-layer

            // Required when switching between switching 
            //  between portrait and landscape
            BuildRenderPath();
        }

        public void SetFill(object obj)
        {
            fill = obj;
            SetNeedsDisplay();
        }

        public void SetStroke(object obj)
        {
            stroke = obj;
            SetNeedsDisplay();
        }

        public void SetStrokeThickness(double thickness)
        {
            strokeWidth = new nfloat(thickness);
            CalculateBasicPathStrokeBounds();
            SetNeedsDisplay();
        }

        public void SetDashLengths(nfloat[] dashLengths)
        {
            this.dashLengths = dashLengths;
            SetNeedsDisplay();
        }

        public void SetDashPhase(nfloat dashPhase)
        {
            this.dashPhase = dashPhase;
            SetNeedsDisplay();
        }

        public void SetStrokeLineCap(CGLineCap strokeLineCap)
        {
            this.strokeLineCap = strokeLineCap;
            CalculateBasicPathStrokeBounds();
            SetNeedsDisplay();
        }

        public void SetStrokeLineJoin(CGLineJoin strokeLineJoin)
        {
            this.strokeLineJoin = strokeLineJoin;
            CalculateBasicPathStrokeBounds();
            SetNeedsDisplay();
        }

        public void SetStrokeMiterLimit(nfloat strokeMiterLimit)
        {
            this.strokeMiterLimit = strokeMiterLimit;
            CalculateBasicPathStrokeBounds();
            SetNeedsDisplay();
        }

        public void SetStretch(Stretch stretch)
        {
            this.stretch = stretch;
            BuildRenderPath();
        }
    }
}
