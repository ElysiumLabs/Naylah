//using Android.Runtime;
//using Android.Support.Design.Widget;
//using Android.Text;
//using Android.Text.Method;
//using Android.Views;
//using Android.Widget;
//using Naylah.Xamarin.Android.Extras;
//using Naylah.Xamarin.Behaviors;
//using Naylah.Xamarin.Controls.Entrys;
//using Naylah.Xamarin.Controls.Style;
//using System.ComponentModel;
//using System.Linq;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//namespace Naylah.Xamarin.Android.Renderers
//{
//    public class EntryCustomRenderer : global::Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<NyEntryBase, EditText>
//    {
//        public bool IsNumeric => Element.Behaviors.Where(x => x.GetType() == typeof(NumericEntryBehavior)).Any();

// private string ActualTextGambi;

// private EditText _nativeView;

// private EditText NativeView { get { return _nativeView ?? (_nativeView = InitializeNativeView());
// } }

// protected override void OnElementChanged(ElementChangedEventArgs<NyEntryBase> e) { base.OnElementChanged(e);

// if (e.OldElement == null) { SetNativeControl(CreateNativeControl()); ConfigureControl(); } }

// private void ConfigureControl() { SetIsKeyboard(); SetText(); SetHintText(); SetTextColor();
// SetIsPassword(); SetIsEnabled(); SetIsNumeric(); }

// protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
// base.OnElementPropertyChanged(sender, e);

// if ( (e.PropertyName == Entry.PlaceholderProperty.PropertyName) || (e.PropertyName ==
// Entry.PlaceholderColorProperty.PropertyName) ) { SetHintText(); }

// if (e.PropertyName == Entry.IsPasswordProperty.PropertyName) { SetIsPassword(); }

// if (e.PropertyName == Entry.TextProperty.PropertyName) { SetText(); }

// if (e.PropertyName == Entry.IsEnabledProperty.PropertyName) { SetIsEnabled(); }

// if (e.PropertyName == Entry.KeyboardProperty.PropertyName) { SetIsKeyboard(); } }

// private void SetIsKeyboard() { if (Element.Keyboard == Keyboard.Url) { NativeView.InputType =
// InputTypes.ClassText | InputTypes.TextVariationUri; } else if (Element.Keyboard == Keyboard.Email)
// { NativeView.InputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress; } else if
// (Element.Keyboard == Keyboard.Numeric) { NativeView.InputType = InputTypes.ClassNumber |
// InputTypes.NumberFlagDecimal; } else if (Element.Keyboard == Keyboard.Chat) { NativeView.InputType
// = InputTypes.ClassText | InputTypes.TextVariationShortMessage; } else if (Element.Keyboard ==
// Keyboard.Telephone) { NativeView.InputType = InputTypes.ClassPhone; } else if (Element.Keyboard ==
// Keyboard.Text) { NativeView.InputType = InputTypes.ClassText | InputTypes.TextFlagNoSuggestions; } }

// private void SetText() { NativeView.Text = Element.Text; }

// private void SetIsPassword() { NativeView.InputType = Element.IsPassword ?
// InputTypes.TextVariationPassword | InputTypes.ClassText : NativeView.InputType; }

// private void SetHintText() { NativeView.Hint = Element.Placeholder;

// //if (Element.Theme == BasicTheme.Dark) //{ //
// NativeView.SetHintTextColor(StyleKit.Current.PrimaryTextColor.ToAndroid()); //} //else //{ //
// NativeView.SetHintTextColor(StyleKit.Current.SecondaryTextColor.ToAndroid()); //} }

// private void SetTextColor() { //if (Element.Theme == BasicTheme.Dark) //{ //
// NativeView.SetTextColor(StyleKit.Current.PrimaryTextColor.ToAndroid()); //} //else //{ //
// NativeView.SetTextColor(StyleKit.Current.SecondaryTextColor.ToAndroid()); //} }

// private void SetIsEnabled() { NativeView.Enabled = Element.IsEnabled; }

// private EditText InitializeNativeView() { var view = FindViewById<EditText>(Naylah.Xamarin.Android.Resource.Id.editText);

// view.TextChanged += EditTextOnTextChanged; view.FocusChange += EditText_FocusChange;

// return view; }

// private void EditText_FocusChange(object sender, FocusChangeEventArgs e) { Element.Text =
// ActualTextGambi; }

// private void EditTextOnTextChanged(object sender, global::Android.Text.TextChangedEventArgs e) {
// ActualTextGambi = e.Text.ToString(); }

// protected override EditText CreateNativeControl() { //if (Element.Theme == BasicTheme.Dark) //{ if
// (IsNumeric) { return
// (EditText)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.HandleCustomEditText,
// null); } else { return
// (EditText)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.EditText,
// null); } //} //else //{ // if (IsNumeric) // { // return
// (EditText)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.HandleCustomEditTextLight,
// null); // } // else // { // return
// (EditText)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.EditTextLight,
// null); // } //} }

// protected void SetIsNumeric() { try { var numericBehavior = Element.Behaviors.Where(x =>
// x.GetType() == typeof(NumericEntryBehavior)).FirstOrDefault() as NumericEntryBehavior;

//                if (numericBehavior != null)
//                {
//                    NativeView.KeyListener = new NumericEntryBehaviorListener(numericBehavior);
//                }
//            }
//            catch (System.Exception)
//            {
//            }
//        }
//    }
//}

//public class CTextKeyListener : BaseKeyListener
//{
//    public override InputTypes InputType
//    {
//        get
//        {
//            return InputTypes.ClassText;
//        }
//    }

//    public override bool OnKeyDown(Android.Views.View view, IEditable content, [GeneratedEnum] Keycode keyCode, KeyEvent e)
//    {
//        return true;
//    }
//}