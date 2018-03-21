using Naylah.Xamarin.Controls.Pages;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS.Renderers
{
    public class CustomPageRenderer : PageRenderer
    {
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            var element = Element as IPageBase;
            if (element == null) { return; }

            var navctrl = this.ViewController.NavigationController;
            if (navctrl == null) { return; }

            if (element.HandleBack == true)
            {
                navctrl.InteractivePopGestureRecognizer.Enabled = false;
            }
        }
    }
}