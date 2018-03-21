using Naylah.Xamarin.Common;
using Naylah.Xamarin.Controls.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public class SectionDivider : ContentView
    {
        public SectionDivider()
        {
            BackgroundColor = Color.White;

            Padding = Device.OnPlatform(new Thickness(16, 0, 0, 0), new Thickness(0), new Thickness(0));

            Content = new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = BootStrapper.CurrentApp.StyleKit.DividerColor,
                HeightRequest = Device.OnPlatform(.5, 1, 1)
            };

            
        }
    }
}
