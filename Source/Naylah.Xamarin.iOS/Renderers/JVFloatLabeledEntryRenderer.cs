//using FloatLabeledEntry;
//using Foundation;
//using Naylah.Xamarin.Behaviors;
//using System;
//using System.ComponentModel;

//using System.Linq;
//using UIKit;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;

//namespace Naylah.Xamarin.iOS.Renderers
//{
//    public class JVFloatLabeledEntryRenderer : ViewRenderer<Entry, FloatLabeledTextField>
//    {
//        public static float JVFieldHMargin = 10.0f;
//        public static float JVFieldFontSize = 16.0f;
//        public static float JVFieldFloatingLabelFontSize = 11.0f;

// private NumericEntryBehavior numericBehavior;

// private UIColor defaultTextColor;

// public JVFloatLabeledEntryRenderer() { }

// protected override void OnElementChanged(ElementChangedEventArgs<Entry> e) { base.OnElementChanged(e);

// var jfflEntry = e.NewElement; if (jfflEntry != null) { //var color = jfflEntry.Theme ==
// EntryTheme.Dark ? Color.Black.ToUIColor() : Color.White.ToUIColor();

// var newView = new FloatLabeledTextField( new System.Drawing.RectangleF(JVFieldHMargin, 0,
// (float)jfflEntry.WidthRequest, (float)jfflEntry.HeightRequest)) { Text = jfflEntry.Text,
// Placeholder = jfflEntry.Placeholder, Font = UIFont.SystemFontOfSize(JVFieldFontSize),
// ClearButtonMode = UITextFieldViewMode.WhileEditing, FloatingLabelFont =
// UIFont.BoldSystemFontOfSize(JVFieldFloatingLabelFontSize), //FloatingLabelTextColor = color,
// //FloatingLabelActiveTextColor = color, BorderStyle = UITextBorderStyle.None, };

// SetNativeControl(newView);

// this.SetIsNumeric(); this.SetPlaceholder(); this.SetIsPassword(); this.SetText(); this.SetTheme();

// this.UpdateKeyboard();

// newView.EditingChanged += delegate (object sender, EventArgs a) { jfflEntry.Text = newView.Text;
// }; newView.ShouldReturn = new UITextFieldCondition(this.OnShouldReturn); //
// newView.EditingDidBegin += delegate (object sender, EventArgs args) { // jfflEntry.IsFocused =
// true; }; newView.EditingDidEnd += delegate (object sender, // EventArgs args) {
// jfflEntry.IsFocused = false; };

// this.defaultTextColor = newView.TextColor;

// MessagingCenter.Subscribe<IVisualElementRenderer>(this, "Xamarin.ResignFirstResponder", delegate
// (IVisualElementRenderer sender) { if (newView.IsFirstResponder) { newView.ResignFirstResponder();
// } }, null); } }

// protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
// Entry arg_06_0 = base.Element;

// if (e.PropertyName == Entry.PlaceholderProperty.PropertyName) { this.SetPlaceholder(); } else { if
// (e.PropertyName == Entry.IsPasswordProperty.PropertyName) { this.SetIsPassword(); } else { if
// (e.PropertyName == Entry.TextProperty.PropertyName) { this.SetText(); } else { if (e.PropertyName
// == Entry.TextColorProperty.PropertyName) { this.SetTheme(); } else { if (e.PropertyName ==
// global::Xamarin.Forms.InputView.KeyboardProperty.PropertyName) { this.UpdateKeyboard(); } } } } }
// base.OnElementPropertyChanged(sender, e); }

// private bool OnShouldReturn(UITextField view) { base.Control.ResignFirstResponder(); //
// base.Element.SendCompleted (); return true; }

// private void SetTheme() { //if (Element.Theme == BasicTheme.Dark) //{ // Control.TextColor =
// Color.Black.ToUIColor(); //} //else //{ // Control.TextColor = Color.White.ToUIColor(); //}

// Control.FloatingLabelTextColor = Control.TextColor; Control.FloatingLabelActiveTextColor = Control.TextColor;

// NSAttributedString placeholderString = new NSAttributedString(Element.Placeholder, new
// UIStringAttributes() { ForegroundColor = Control.TextColor }); Control.AttributedPlaceholder = placeholderString;

// //if (base.Element.TextColor == Color.Default) //{ // base.Control.TextColor =
// this.defaultTextColor; // return; //} //base.Control.TextColor =
// base.Element.TextColor.ToUIColor(); }

// private void UpdateKeyboard() { base.Control.ApplyKeyboard(base.Element.Keyboard); }

// private void SetIsPassword() { if (base.Element.IsPassword && base.Control.IsFirstResponder) {
// base.Control.Enabled = false; base.Control.SecureTextEntry = true; base.Control.Enabled =
// base.Element.IsEnabled; base.Control.BecomeFirstResponder(); return; }
// base.Control.SecureTextEntry = base.Element.IsPassword; }

// private void SetPlaceholder() { base.Control.Placeholder = base.Element.Placeholder; }

// private void SetText() { if (base.Control.Text != base.Element.Text) { base.Control.Text =
// base.Element.Text; } }

// protected void SetIsNumeric() { try { numericBehavior = Element.Behaviors.Where(x => x.GetType()
// == typeof(NumericEntryBehavior)).FirstOrDefault() as NumericEntryBehavior;

// if (numericBehavior != null) { Control.ShouldChangeCharacters = HandleNumericsSchemeGambi;
// Control.KeyboardType = UIKeyboardType.NumberPad; } } catch (System.Exception) { } }

// private bool HandleNumericsSchemeGambi(UITextField textField, NSRange range, string
// replacementString) { try { var textLenght = Control.Text.Length;

// if (!string.IsNullOrEmpty(replacementString)) { switch (replacementString.FirstOrDefault()) { case
// '0': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N0); break;

// case '1': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N1); break;

// case '2': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N2); break;

// case '3': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N3); break;

// case '4': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N4); break;

// case '5': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N5); break;

// case '6': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N6); break;

// case '7': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N7); break;

// case '8': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N8); break;

// case '9': numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N9); break; } } else {
// numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.Back); } } catch (Exception) { }

//            return false;
//        }
//    }
//}