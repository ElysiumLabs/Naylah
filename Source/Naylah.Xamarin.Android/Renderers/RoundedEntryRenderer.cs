//using Android.Graphics.Drawables;
//using Android.Text;
//using Android.Views;
//using Android.Widget;
//using Naylah.Xamarin.Controls.Entrys;
//using System.ComponentModel;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//namespace Naylah.Xamarin.Android.Renderers
//{
//    public class RoundedEntryRenderer : ViewRenderer<RoundedEntry, EditText>
//    {
//        private GradientDrawable gd = null;
//        private EditText edittext;
//        private RoundedEntry view;
//        private string ActualTextGambi;

// protected override void OnElementChanged(ElementChangedEventArgs<RoundedEntry> e) { base.OnElementChanged(e);

// view = (RoundedEntry)Element; if (view == null) return;

// edittext = new EditText(Forms.Context); edittext.LayoutParameters = new
// LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent); SetHintColor();

// if (view.Multiline == true) { edittext.SetLines(20000); edittext.Gravity = GravityFlags.Left |
// GravityFlags.Top; }

// GradientDrawable gd = new GradientDrawable(); gd.SetColor(view.BackgroundColor.ToAndroid());

// gd.SetCornerRadius(float.Parse(view.BorderRadius.ToString()));

// gd.SetStroke(2, view.BorderColor.ToAndroid());

// edittext.SetBackgroundDrawable(gd);

// SetNativeControl(edittext); UpdateTextColor(); SetHintText(); SetText(); SetInKeyboard();
// SetIspassword(); }

// protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
// base.OnElementPropertyChanged(sender, e);

// if (e.PropertyName == Entry.KeyboardProperty.PropertyName) { SetInKeyboard(); }

// if (e.PropertyName == Entry.PlaceholderProperty.PropertyName) { SetHintText(); } if
// (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName) { SetHintColor(); } if
// (e.PropertyName == Entry.TextColorProperty.PropertyName) { UpdateTextColor(); } if (e.PropertyName
// == Entry.TextProperty.PropertyName) { SetText(); } if (e.PropertyName ==
// RoundedEntry.IsPasswordProperty.PropertyName) { SetIspassword(); } }

// private void SetIspassword() { edittext.InputType = Element.IsPassword ?
// InputTypes.TextVariationPassword | InputTypes.ClassText : edittext.InputType; }

// private void SetHintColor() { Color c = Element.PlaceholderColor;

// if (c == Color.Default) { edittext.SetHintTextColor(Color.FromHex("808080").ToAndroid()); } else {
// edittext.SetHintTextColor(Element.PlaceholderColor.ToAndroid()); } }

// private void SetHintText() { edittext.Hint = Element.Placeholder; }

// private void SetText() { edittext.Text = Element.Text; }

// private void UpdateTextColor() { Color c = Element.TextColor;

// if (c == Color.Default) { edittext.SetTextColor(Color.FromHex("808080").ToAndroid()); } else {
// edittext.SetTextColor(c.ToAndroid()); } }

//        private void SetInKeyboard()
//        {
//            if (Element.Keyboard == Keyboard.Url)
//            {
//                edittext.InputType = InputTypes.ClassText | InputTypes.TextVariationUri;
//            }
//            else if (Element.Keyboard == Keyboard.Email)
//            {
//                edittext.InputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
//            }
//            else if (Element.Keyboard == Keyboard.Numeric)
//            {
//                edittext.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
//            }
//            else if (Element.Keyboard == Keyboard.Chat)
//            {
//                edittext.InputType = InputTypes.ClassText | InputTypes.TextVariationShortMessage;
//            }
//            else if (Element.Keyboard == Keyboard.Telephone)
//            {
//                edittext.InputType = InputTypes.ClassPhone;
//            }
//            else if (Element.Keyboard == Keyboard.Text)
//            {
//                edittext.InputType = InputTypes.ClassText | InputTypes.TextFlagNoSuggestions;
//            }
//        }
//    }
//}