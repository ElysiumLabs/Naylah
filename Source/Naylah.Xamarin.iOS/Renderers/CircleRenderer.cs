using CoreGraphics;
using Naylah.Xamarin.Controls.Buttons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS.Renderers
{
    public class CircleRenderer : VisualElementRenderer<CircleContentView>
    {
        private readonly float QuarterTurnCounterClockwise = (float)(-1f * (Math.PI * 0.5f));

        public CircleRenderer()
        {
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            var currentContext = UIGraphics.GetCurrentContext();
            var properRect = AdjustForThickness(rect);
            HandleShapeDraw(currentContext, properRect);
        }

        public static void Init()
        {
        }

        protected RectangleF AdjustForThickness(CGRect rect)
        {
            var x = rect.X + Element.Padding.Left;
            var y = rect.Y + Element.Padding.Top;
            var width = rect.Width - Element.Padding.HorizontalThickness;
            var height = rect.Height - Element.Padding.VerticalThickness;
            return new RectangleF((float)x, (float)y, (float)width, (float)height);
        }

        protected virtual void HandleShapeDraw(CGContext currentContext, RectangleF rect)
        {
            // Only used for circles
            var centerX = rect.X + (rect.Width / 2);
            var centerY = rect.Y + (rect.Height / 2);
            var radius = rect.Width / 2;
            var startAngle = 0;
            var endAngle = (float)(Math.PI * 2);

            HandleStandardDraw(currentContext, rect, () => currentContext.AddArc(centerX, centerY, radius, startAngle, endAngle, true));
        }

        /// <summary>
        /// A simple method for handling our drawing of the shape. This method is called differently for each type of shape
        /// </summary>
        /// <param name="currentContext">Current context.</param>
        /// <param name="rect">Rect.</param>
        /// <param name="createPathForShape">Create path for shape.</param>
        /// <param name="lineWidth">Line width.</param>
        protected virtual void HandleStandardDraw(CGContext currentContext, RectangleF rect, Action createPathForShape, float? lineWidth = null)
        {
            currentContext.SetLineWidth((nfloat)(lineWidth ?? Element.StrokeThickness));
            currentContext.SetFillColor(base.Element.FillColor.ToCGColor());
            currentContext.SetStrokeColor(Element.StrokeColor.ToCGColor());

            createPathForShape();

            currentContext.DrawPath(CGPathDrawingMode.FillStroke);
        }
    }
}