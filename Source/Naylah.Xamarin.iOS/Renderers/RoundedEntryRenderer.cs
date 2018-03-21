//using Naylah.Xamarin.Controls.Entrys;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin.Forms.Platform.iOS;
//using UIKit;
//using System.ComponentModel;
//using Xamarin.Forms;
//using CoreGraphics;
//using Foundation;

//namespace Naylah.Xamarin.iOS.Renderers
//{
//    public class RoundedEntryRenderer : ViewRenderer<RoundedEntry,UITextField>
//    {
//        private UITextField textfield;

// protected override void OnElementChanged(ElementChangedEventArgs<RoundedEntry> e) { base.OnElementChanged(e);

// var view = (RoundedEntry)Element;

// textfield = new UITextField();

// textfield.Layer.BorderWidth = 1;

// if (view.Multiline == true) { var tv = new UITextView();

// }

// var paddingview = new UIView(frame: new CGRect(0,0,15,textfield.Frame.Height)); textfield.LeftView
// = paddingview; textfield.LeftViewMode = UITextFieldViewMode.Always;

// ConfigureControl(); SetNativeControl(textfield);

// } protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
// base.OnElementPropertyChanged(sender, e);

// if (e.PropertyName == RoundedEntry.TextProperty.PropertyName) { SetText(); } if (e.PropertyName ==
// RoundedEntry.PlaceholderProperty.PropertyName) { SetPlaceholder(); } if (e.PropertyName ==
// RoundedEntry.PlaceholderColorProperty.PropertyName) { SetPlaceholderColor(); } if (e.PropertyName
// == RoundedEntry.TextColorProperty.PropertyName) { UpdateTextColor(); } if (e.PropertyName ==
// RoundedEntry.KeyboardProperty.PropertyName) { SetIsKeyboard(); } if (e.PropertyName ==
// RoundedEntry.IsPasswordProperty.PropertyName) { SetIspassword(); }

// }

// private void SetIspassword() { textfield.SecureTextEntry = Element.IsPassword; }

// private void SetPlaceholderColor() { textfield.AttributedPlaceholder = new
// NSAttributedString(Element.Placeholder, null, Element.PlaceholderColor.ToUIColor()); }

// private void UpdateBgColor() { Color c = Element.BackgroundColor;

// if (c == Color.Default) { textfield.BackgroundColor = Color.FromHex("808080").ToUIColor(); } else
// { textfield.BackgroundColor = c.ToUIColor(); }; }

// private void ConfigureControl() { SetText(); SetPlaceholder(); UpdateTextColor(); SetIsKeyboard();
// UpdateBgColor(); SetBorderColor(); SetCornerRadius(); SetIspassword(); }

// private void SetCornerRadius() { textfield.Layer.CornerRadius = Element.BorderRadius; }

// private void SetBorderColor() { textfield.Layer.BorderColor = Element.BorderColor.ToCGColor(); }

// private void SetIsKeyboard() { if (Element.Keyboard == Keyboard.Email) { textfield.KeyboardType =
// UIKeyboardType.EmailAddress; } if (Element.Keyboard == Keyboard.Numeric) { textfield.KeyboardType
// = UIKeyboardType.NumbersAndPunctuation; } if (Element.Keyboard == Keyboard.Telephone) {
// textfield.KeyboardType = UIKeyboardType.PhonePad; } if (Element.Keyboard == Keyboard.Chat) {
// textfield.KeyboardType = UIKeyboardType.Default; } if (Element.Keyboard == Keyboard.Default) {
// textfield.KeyboardType = UIKeyboardType.Default; } }

// private void UpdateTextColor() { Color c = Element.TextColor;

// if (c == Color.Default) { textfield.TextColor = Color.FromHex("808080").ToUIColor(); } else {
// textfield.TextColor = c.ToUIColor(); } }

// private void SetPlaceholder() { textfield.Placeholder = Element.Placeholder; }

//        private void SetText()
//        {
//            textfield.Text = Element.Text;
//        }
//    }
//}