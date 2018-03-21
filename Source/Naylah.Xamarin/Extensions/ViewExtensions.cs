using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Extensions
{
    public static class ViewExtensions
    {
        public static ScrollView ToScrollView(this View view, ScrollOrientation orientation = ScrollOrientation.Vertical)
        {
            var scrollView = new ScrollView()
            {
                Orientation = orientation
            };

            scrollView.Content = view;
            return scrollView;
        }
    }
}
