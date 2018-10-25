using System;
using Xamarin.Forms;

namespace Naylah.App.UX
{
    public class UXUtils
    {
        public static Action<VisualElement> DefaultButtonHapticFeedback { get; set; }

        static UXUtils()
        {
            DefaultButtonHapticFeedback = (c) =>
            {
                HapticFeedback.Instance.Run(HapticFeedbackType.Medium);
            };
        }
    }
}