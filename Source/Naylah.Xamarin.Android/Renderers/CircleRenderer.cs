using Naylah.Xamarin.Android.Extensions;
using Naylah.Xamarin.Controls.Buttons;
using Xamarin.Forms.Platform.Android;

namespace Naylah.Xamarin.Android.Renderers
{
    public class CircleRenderer : ViewRenderer<CircleContentView, CircleViewAndroid>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CircleContentView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                SetNativeControl(new CircleViewAndroid(Resources.DisplayMetrics.Density, Context)
                {
                    ShapeView = Element,
                });
            }
        }
    }
}