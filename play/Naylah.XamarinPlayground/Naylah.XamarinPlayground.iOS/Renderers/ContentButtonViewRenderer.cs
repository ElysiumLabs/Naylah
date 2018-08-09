using CoreGraphics;
using Foundation;
using Naylah.App.UI.Controls;
using Naylah.XamarinPlayground.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentButtonView), typeof(ContentButtonViewRenderer))]

namespace Naylah.XamarinPlayground.iOS.Renderers
{
    public class ContentButtonViewRenderer : VisualElementRenderer<ContentButtonView>
    {
        private ContentButtonView cvb;
        private CGPoint touched;

        protected override void OnElementChanged(ElementChangedEventArgs<ContentButtonView> e)
        {
            base.OnElementChanged(e);

            cvb = Element as ContentButtonView;
        }

        //protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        //{
        //    base.OnElementChanged(e);

        // UserInteractionEnabled = true;

        //    if (Element != null)
        //    {
        //        cvb = Element as ContentButtonView;
        //    }
        //}

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            cvb?.SendPressed();

            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                touched = touch.LocationInView(this);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            cvb?.SendReleased();

            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                CGPoint location = touch.LocationInView(this);
                if (new CGRect(new CGPoint(0.0, 0.0), this.Frame.Size).Contains(location))
                {
                    cvb?.SendClicked();
                }
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            cvb?.SendReleased();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
        }
    }
}