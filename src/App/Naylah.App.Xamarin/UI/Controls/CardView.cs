using Xamarin.Forms;

namespace Naylah.App.UI.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 0;
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.iOS)
            {
                HasShadow = false;
                BorderColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }
    }
}