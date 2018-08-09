using Android.Content;
using Naylah.App.UI.Controls;
using Naylah.App.Xamarin.Android.Renderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AMotionEvent = Android.Views.MotionEvent;
using AMotionEventActions = Android.Views.MotionEventActions;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(ContentButtonView), typeof(ContentButtonViewRenderer))]

namespace Naylah.App.Xamarin.Android.Renderers
{
    public class ContentButtonViewRenderer : ViewRenderer
    {
        public ContentButtonViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            ContentButtonView formView = (e.NewElement as ContentButtonView);

            this.SetOnClickListener(ButtonClickListener.Instance.Value);
            this.SetOnTouchListener(ButtonTouchListener.Instance.Value);
            this.Tag = this;
        }

        private class ButtonClickListener : Java.Lang.Object, IOnClickListener
        {
            public static readonly Lazy<ButtonClickListener> Instance = new Lazy<ButtonClickListener>(() => new ButtonClickListener());

            public void OnClick(AView v)
            {
                var renderer = v.Tag as ContentButtonViewRenderer;
                if (renderer != null)
                    ((IButtonController)renderer.Element).SendClicked();
            }
        }

        private class ButtonTouchListener : Java.Lang.Object, IOnTouchListener
        {
            public static readonly Lazy<ButtonTouchListener> Instance = new Lazy<ButtonTouchListener>(() => new ButtonTouchListener());

            public bool OnTouch(AView v, AMotionEvent e)
            {
                var renderer = v.Tag as ContentButtonViewRenderer;
                if (renderer != null)
                {
                    var buttonController = renderer.Element as IButtonController;
                    if (e.Action == AMotionEventActions.Down)
                    {
                        buttonController?.SendPressed();
                    }
                    else if (e.Action == AMotionEventActions.Up)
                    {
                        buttonController?.SendReleased();
                    }
                }
                return false;
            }
        }
    }
}