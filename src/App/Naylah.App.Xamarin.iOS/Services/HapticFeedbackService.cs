using Naylah.App.UX;
using UIKit;

namespace Naylah.App.Xamarin.iOS.Services
{
    public class HapticFeedbackService
    {
        public static void Init()
        {
            HapticFeedback.Instance = new iOSHapticFeedback();
        }
    }

    public class iOSHapticFeedback : IHapticFeedback
    {
        public void Run(HapticFeedbackType hapticFeedbackType)
        {
            UIImpactFeedbackGenerator impact = null;

            switch (hapticFeedbackType)
            {
                case HapticFeedbackType.Softy:
                    impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
                    break;

                case HapticFeedbackType.Medium:
                    impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Medium);
                    break;

                case HapticFeedbackType.Heavy:
                    impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy);
                    break;
            }

            impact.Prepare();
            impact.ImpactOccurred();
        }
    }
}