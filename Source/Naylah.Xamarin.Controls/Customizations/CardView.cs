using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 0;
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.iOS)
            {
                HasShadow = false;
                OutlineColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }
    }
}
