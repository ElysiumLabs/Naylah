using Android.Content;
using Naylah.App.UI.Behaviors;
using Naylah.App.Xamarin.Android.Renderers;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ListView), typeof(CustomListViewRenderer))]

namespace Naylah.App.Xamarin.Android.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        public CustomListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<global::Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                if (e.NewElement?.Behaviors?.OfType<NonScrollableListViewBehavior>()?.Any() == true)
                {
                    Control.VerticalScrollBarEnabled = false;
                    Control.HorizontalScrollBarEnabled = false;
                }
            }
        }
    }
}