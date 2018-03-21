using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS.Renderers
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<global::Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.ScrollEnabled = false;
            }

        }
    }
}