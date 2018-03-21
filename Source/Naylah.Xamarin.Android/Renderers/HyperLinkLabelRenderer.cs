using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Util;
using Android.Util;
using Android.Views;
using Android.Widget;
using Naylah.Xamarin.Android.Renderers;
using Naylah.Xamarin.Controls.Labels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace Naylah.Xamarin.Android.Renderers
{
    public class HyperLinkLabelRenderer : ViewRenderer<HyperLinkLabel, TextView>
    {
        private TextView textView;

        protected override void OnElementChanged(ElementChangedEventArgs<HyperLinkLabel> e)
        {
            base.OnElementChanged(e);

            var view = (HyperLinkLabel)Element;
            if (view == null) return;

            textView = new TextView(Forms.Context);
            textView.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            // Setting the auto link mask to capture all types of link-able data
            textView.AutoLinkMask = MatchOptions.All;
            // Make sure to set text after setting the mask
            textView.Text = view.Text;
            textView.SetTextSize(ComplexUnitType.Dip, (float)view.FontSize);

            // overriding Xamarin Forms Label and replace with our native control
            SetNativeControl(textView);
            UpdateTextColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                textView.Text = Element.Text;
            }
            if (e.PropertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }
        }

        private void UpdateTextColor()
        {
            Color c = Element.TextColor;

            if (c == Color.Default)
            {
                textView.SetTextColor(Color.FromHex("808080").ToAndroid());
            }
            else
            {
                textView.SetTextColor(c.ToAndroid());
            }
        }
    }
}