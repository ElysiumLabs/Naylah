using CoreGraphics;
using Naylah.Xamarin.Controls.Labels;
using Naylah.Xamarin.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace Naylah.Xamarin.iOS.Renderers
{
    public class HyperLinkLabelRenderer : ViewRenderer<HyperLinkLabel, UITextView>
    {
        private UITextView uilabelleftside;

        protected override void OnElementChanged(ElementChangedEventArgs<HyperLinkLabel> e)
        {
            base.OnElementChanged(e);

            var view = (HyperLinkLabel)Element;
            if (view == null) return;

            uilabelleftside = new UITextView(new CGRect(0, 0, view.Width, view.Height));
            uilabelleftside.Text = view.Text;
            uilabelleftside.Font = UIFont.SystemFontOfSize((float)view.FontSize);
            uilabelleftside.Editable = false;

            // Setting the data detector types mask to capture all types of link-able data
            uilabelleftside.DataDetectorTypes = UIDataDetectorType.All;
            uilabelleftside.BackgroundColor = UIColor.Clear;

            // overriding Xamarin Forms Label and replace with our native control
            SetNativeControl(uilabelleftside);
            UpdateTextColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                uilabelleftside.Text = Element.Text;
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
                uilabelleftside.TextColor = Color.FromHex("808080").ToUIColor();
            }
            else
            {
                uilabelleftside.TextColor = c.ToUIColor();
            }
        }
    }
}