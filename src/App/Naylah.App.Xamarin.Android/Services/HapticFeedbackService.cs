﻿using Android.App;
using Android.Views;
using Naylah.App.UX;

namespace Naylah.App.Xamarin.Android.Services
{
    public class HapticFeedbackService
    {
        public static void Init(Activity activity)
        {
            HapticFeedback.Instance = new AndroidHapticFeedback(activity);
        }
    }

    internal class AndroidHapticFeedback : IHapticFeedback
    {
        private readonly Activity activity;

        public AndroidHapticFeedback(Activity activity)
        {
            this.activity = activity;
        }

        public void Run(HapticFeedbackType hapticFeedbackType)
        {
            switch (hapticFeedbackType)
            {
                case HapticFeedbackType.Softy:
                    activity.Window.DecorView.RootView.PerformHapticFeedback(FeedbackConstants.ContextClick);
                    break;

                case HapticFeedbackType.Medium:
                    activity.Window.DecorView.RootView.PerformHapticFeedback(FeedbackConstants.KeyboardPress);
                    break;

                case HapticFeedbackType.Heavy:
                    activity.Window.DecorView.RootView.PerformHapticFeedback(FeedbackConstants.KeyboardPress);
                    break;
            }
        }
    }
}