using System;
using Xamarin.Forms;

namespace Naylah.App.UI
{
    public class UIUtils
    {
        public static Action<VisualElement> DefaultButtonPressedAnimation { get; set; }

        public static Action<VisualElement> DefaultButtonReleasedAnimation { get; set; }

        static UIUtils()
        {
            DefaultButtonPressedAnimation = async (c) =>
            {
                c.AnchorX = 0.5;
                c.AnchorY = 0.5;

                await c.ScaleTo(0.92, 50, Easing.SpringOut);
            };

            DefaultButtonReleasedAnimation = async (c) =>
            {
                await c.ScaleTo(1, 50, Easing.SpringOut);
            };
        }
    }
}