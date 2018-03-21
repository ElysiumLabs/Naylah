//using Android.Runtime;
//using Android.Support.Design.Widget;
//using Android.Text;
//using Android.Text.Method;
//using Android.Views;
//using Naylah.Xamarin.Android.Extras;
//using Naylah.Xamarin.Behaviors;
//using Naylah.Xamarin.Controls.Entrys;
//using System.ComponentModel;
//using System.Linq;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//namespace Naylah.Xamarin.Android.Renderers
//{
//    public class FloatLabeledEntryRenderer : global::Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<FloatLabeledEntry, TextInputLayout>
//    {
//        public bool IsNumeric => Element.Behaviors.Where(x => x.GetType() == typeof(NumericEntryBehavior)).Any();

// private string ActualTextGambi;

// private TextInputLayout _nativeView;

// private TextInputLayout NativeView { get { return _nativeView ?? (_nativeView =
// InitializeNativeView()); } }

// protected override void OnElementChanged(ElementChangedEventArgs<FloatLabeledEntry> e) { base.OnElementChanged(e);

// if (e.OldElement == null) { SetNativeControl(CreateNativeControl()); ConfigureControl(); } }

// private void ConfigureControl() { SetIsKeyboard(); SetText(); SetHintText(); SetTextColor();
// SetIsPassword(); SetIsEnabled(); SetIsNumeric(); }

// private void SetIsKeyboard() { if (Element.Keyboard == Keyboard.Url) {
// NativeView.EditText.InputType = InputTypes.ClassText | InputTypes.TextVariationUri; } else if
// (Element.Keyboard == Keyboard.Email) { NativeView.EditText.InputType = InputTypes.ClassText |
// InputTypes.TextVariationEmailAddress; } else if (Element.Keyboard == Keyboard.Numeric) {
// NativeView.EditText.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal; } else if
// (Element.Keyboard == Keyboard.Chat) { NativeView.EditText.InputType = InputTypes.ClassText |
// InputTypes.TextVariationShortMessage; } else if (Element.Keyboard == Keyboard.Telephone) {
// NativeView.EditText.InputType = InputTypes.ClassPhone; } else if (Element.Keyboard ==
// Keyboard.Text) { NativeView.EditText.InputType = InputTypes.ClassText |
// InputTypes.TextFlagNoSuggestions; } }

// protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
// base.OnElementPropertyChanged(sender, e);

// if ( (e.PropertyName == Entry.PlaceholderProperty.PropertyName) || (e.PropertyName ==
// Entry.PlaceholderColorProperty.PropertyName) ) { SetHintText(); }

// if (e.PropertyName == Entry.IsPasswordProperty.PropertyName) { SetIsPassword(); }

// if (e.PropertyName == Entry.TextProperty.PropertyName) { SetText(); }

// if (e.PropertyName == Entry.IsEnabledProperty.PropertyName) { SetIsEnabled(); } if (e.PropertyName
// == Entry.KeyboardProperty.PropertyName) { SetIsKeyboard(); } }

// private void SetText() { NativeView.EditText.Text = Element.Text; }

// private void SetIsPassword() { NativeView.EditText.InputType = Element.IsPassword ?
// InputTypes.TextVariationPassword | InputTypes.ClassText : NativeView.EditText.InputType; }

// private void SetHintText() { NativeView.Hint = Element.Placeholder;

// //if (Element.Theme == BasicTheme.Dark) //{ //
// NativeView.EditText.SetHintTextColor(StyleKit.Current.PrimaryTextColor.ToAndroid()); //} //else
// //{ // NativeView.EditText.SetHintTextColor(StyleKit.Current.SecondaryTextColor.ToAndroid()); //} }

// private void SetTextColor() { //if (Element.Theme == BasicTheme.Dark) //{ //
// NativeView.EditText.SetTextColor(StyleKit.Current.PrimaryTextColor.ToAndroid()); //} //else //{ //
// NativeView.EditText.SetTextColor(StyleKit.Current.SecondaryTextColor.ToAndroid()); //} }

// private void SetIsEnabled() { NativeView.EditText.Enabled = Element.IsEnabled; }

// private TextInputLayout InitializeNativeView() { var view = FindViewById<TextInputLayout>(Naylah.Xamarin.Android.Resource.Id.textInputLayout);

// view.EditText.TextChanged += EditTextOnTextChanged; view.EditText.FocusChange += EditText_FocusChange;

// return view; }

// private void EditText_FocusChange(object sender, FocusChangeEventArgs e) { Element.Text =
// ActualTextGambi; }

// private void EditTextOnTextChanged(object sender, global::Android.Text.TextChangedEventArgs e) {
// ActualTextGambi = e.Text.ToString(); }

// protected override TextInputLayout CreateNativeControl() { //if (Element.Theme == BasicTheme.Dark)
// //{ if (IsNumeric) { return
// (TextInputLayout)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.HandleCustomTextInputLayout,
// null); } else { return
// (TextInputLayout)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.TextInputLayout,
// null); } //} //else //{ // if (IsNumeric) // { // return
// (TextInputLayout)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.HandleCustomTextInputLayoutLight,
// null); // } // else // { // return
// (TextInputLayout)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.TextInputLayoutLight,
// null); // } //} }

// protected void SetIsNumeric() { try { var numericBehavior = Element.Behaviors.Where(x =>
// x.GetType() == typeof(NumericEntryBehavior)).FirstOrDefault() as NumericEntryBehavior;

// if (numericBehavior != null) { NativeView.EditText.KeyListener = new
// NumericEntryBehaviorListener(numericBehavior); } } catch (System.Exception) { } } }

// public class CTextKeyListener : BaseKeyListener { public override InputTypes InputType { get {
// return InputTypes.ClassText; } }

//        public override bool OnKeyDown(global::Android.Views.View view, IEditable content, [GeneratedEnum] Keycode keyCode, KeyEvent e)
//        {
//            return true;
//        }
//    }
//}