using Foundation;
using Naylah.Xamarin.Behaviors;
using Naylah.Xamarin.Controls.Entrys;
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private UIColor defaultTextColor;
        private NumericEntryBehavior numericBehavior;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var entry = e.NewElement;
            if (entry != null)
            {
                this.SetIsNumeric();
                this.SetPlaceholder();
                this.SetIsPassword();
                this.SetText();
                this.SetTheme();

                this.UpdateKeyboard();

                Control.EditingChanged += delegate (object sender, EventArgs a)
                {
                    entry.Text = Control.Text;
                };
                Control.ShouldReturn = new UITextFieldCondition(this.OnShouldReturn);
                // newView.EditingDidBegin += delegate (object sender, EventArgs args) {
                // jfflEntry.IsFocused = true; }; newView.EditingDidEnd += delegate (object sender,
                // EventArgs args) { jfflEntry.IsFocused = false; };

                this.defaultTextColor = Control.TextColor;

                MessagingCenter.Subscribe<IVisualElementRenderer>(this, "Xamarin.ResignFirstResponder", delegate (IVisualElementRenderer sender)
                {
                    if (Control.IsFirstResponder)
                    {
                        Control.ResignFirstResponder();
                    }
                }, null);
            }
        }

        private bool OnShouldReturn(UITextField view)
        {
            base.Control.ResignFirstResponder();
            // base.Element.SendCompleted ();
            return true;
        }

        private void SetTheme()
        {
            var elementAsEntryBase = Element as NyEntry;

            if (elementAsEntryBase != null)
            {
                //if (elementAsEntryBase.Theme == BasicTheme.Dark)
                //{
                //    Control.TextColor = Color.Black.ToUIColor();
                //}
                //else
                //{
                //    Control.TextColor = Color.White.ToUIColor();
                //}

                Control.BorderStyle = UITextBorderStyle.None;
            }

            NSAttributedString placeholderString = new NSAttributedString(Element.Placeholder, new UIStringAttributes() { ForegroundColor = Control.TextColor });
            Control.AttributedPlaceholder = placeholderString;
        }

        private void UpdateKeyboard()
        {
            base.Control.ApplyKeyboard(base.Element.Keyboard);
        }

        private void SetIsPassword()
        {
            if (base.Element.IsPassword && base.Control.IsFirstResponder)
            {
                base.Control.Enabled = false;
                base.Control.SecureTextEntry = true;
                base.Control.Enabled = base.Element.IsEnabled;
                base.Control.BecomeFirstResponder();
                return;
            }
            base.Control.SecureTextEntry = base.Element.IsPassword;
        }

        private void SetPlaceholder()
        {
            base.Control.Placeholder = base.Element.Placeholder;
        }

        private void SetText()
        {
            if (base.Control.Text != base.Element.Text)
            {
                base.Control.Text = base.Element.Text;
            }
        }

        protected void SetIsNumeric()
        {
            try
            {
                numericBehavior = Element.Behaviors.Where(x => x.GetType() == typeof(NumericEntryBehavior)).FirstOrDefault() as NumericEntryBehavior;

                if (numericBehavior != null)
                {
                    Control.ShouldChangeCharacters = HandleNumericsSchemeGambi;
                    Control.KeyboardType = UIKeyboardType.NumberPad;
                }
            }
            catch (System.Exception)
            {
            }
        }

        private bool HandleNumericsSchemeGambi(UITextField textField, NSRange range, string replacementString)
        {
            try
            {
                var textLenght = Control.Text.Length;

                if (!string.IsNullOrEmpty(replacementString))
                {
                    switch (replacementString.FirstOrDefault())
                    {
                        case '0':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N0);
                            break;

                        case '1':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N1);
                            break;

                        case '2':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N2);
                            break;

                        case '3':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N3);
                            break;

                        case '4':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N4);
                            break;

                        case '5':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N5);
                            break;

                        case '6':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N6);
                            break;

                        case '7':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N7);
                            break;

                        case '8':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N8);
                            break;

                        case '9':
                            numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N9);
                            break;
                    }
                }
                else
                {
                    numericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.Back);
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}