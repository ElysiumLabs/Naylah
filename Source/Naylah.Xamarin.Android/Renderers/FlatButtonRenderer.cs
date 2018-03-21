using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Naylah.Xamarin.Controls.Buttons;
using Android.Graphics.Drawables;
using System.ComponentModel;
using Android.Graphics;

namespace Naylah.Xamarin.Android.Renderers
{


    public class FlatButtonRenderer : ViewRenderer<FlatButton, Button>
    {
        private Button button;

        protected override void OnElementChanged(ElementChangedEventArgs<FlatButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;
           
            var view = (FlatButton)Element;

            button = new Button(AndroidContext.GetContext());
            button.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            SetText();

            

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(view.BackgroundColor.ToAndroid());

            gd.SetCornerRadius(view.BorderRadius);

            gd.GetPadding(new Rect(10, 10, 10, 10));  
            gd.SetStroke(2, view.BorderColor.ToAndroid());

            button.SetBackground(gd);

            SetNativeControl(button);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == FlatButton.TextProperty.PropertyName)
            {
                SetText();
            }

           
        }

      

        private void SetText()
        {
            button.Text = Element.Text;
        }

        private void UpdateTextColor()
        {
            Color c = Element.TextColor.ToAndroid();

            if (c == Color.Black)
            {
                button.SetTextColor(Color.Black);
            }
            else
            {
                button.SetTextColor(c);
            }
        }
    }
}