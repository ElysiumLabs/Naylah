using CoreGraphics;
using FloatLabeledEntry;
using Foundation;
using Naylah.Xamarin.Behaviors;
using Naylah.Xamarin.Controls.Entrys;
using Naylah.Xamarin.iOS.Renderers;
using System;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS
{
    public class NyEntryRenderer : ViewRenderer<NyEntry, UITextField>
    {
        public static float JVFieldHMargin = 10.0f;
        public static float JVFieldFontSize = 16.0f;
        public static float JVFieldFloatingLabelFontSize = 11.0f;

        private UIColor _defaultTextColor;
        private bool _disposed;

        public NyEntryRenderer()
        {
            //Frame = new RectangleF(0, 20, 320, 40);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NyEntry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            var ent = e.NewElement;

            if (Control == null)
            {
                UITextField textField = null;

                switch (ent.EntryStyle)
                {
                    case NyEntryStyle.FloatLabeled:
                        textField = new FloatLabeledTextField(
                          new System.Drawing.RectangleF(JVFieldHMargin, 0, (float)ent.WidthRequest, (float)ent.HeightRequest)
                          )
                        {
                            FloatingLabelFont = UIFont.BoldSystemFontOfSize(JVFieldFloatingLabelFontSize),
                            BorderStyle = UITextBorderStyle.None,
                        };

                        textField.BorderStyle = UITextBorderStyle.None;

                        break;

                    case NyEntryStyle.Rounded:
                        textField = new UITextFieldWithPadding()
                        {
                            Padding = ent.RoundedPadding
                        };

                        textField.BorderStyle = UITextBorderStyle.None;
                        textField.Layer.BorderColor = ent.RoundedBorderColor.ToCGColor();
                        textField.Layer.CornerRadius = ent.RoundedBorderRadius / 2;
                        textField.Layer.BackgroundColor = ent.RoundedBackgroundColor.ToCGColor();
                        break;

                    default:
                        textField = new UITextField();
                        textField.BorderStyle = UITextBorderStyle.RoundedRect;
                        break;
                }

                SetNativeControl(textField);

                _defaultTextColor = textField.TextColor;
                //textField.BorderStyle = UITextBorderStyle.RoundedRect;
                textField.ClipsToBounds = true;

                textField.EditingChanged += OnEditingChanged;

                textField.ShouldReturn = OnShouldReturn;

                textField.EditingDidBegin += OnEditingBegan;
                textField.EditingDidEnd += OnEditingEnded;
            }

            UpdatePlaceholder();
            UpdatePassword();
            UpdateText();
            UpdateColor();
            UpdateFont();
            UpdateKeyboard();
            UpdateAlignment();
            UpdateAdjustsFontSizeToFitWidth();
            UpdateNumericHandle();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Entry.PlaceholderProperty.PropertyName || e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholder();
            else if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                UpdatePassword();
            else if (e.PropertyName == Entry.TextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Entry.TextColorProperty.PropertyName)
                UpdateColor();
            else if (e.PropertyName == global::Xamarin.Forms.InputView.KeyboardProperty.PropertyName)
                UpdateKeyboard();
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
                UpdateAlignment();
            else if (e.PropertyName == Entry.FontAttributesProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontFamilyProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontSizeProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                UpdateColor();
                UpdatePlaceholder();
            }
            else if (e.PropertyName == global::Xamarin.Forms.PlatformConfiguration.iOSSpecific.Entry.AdjustsFontSizeToFitWidthProperty.PropertyName)
                UpdateAdjustsFontSizeToFitWidth();

            base.OnElementPropertyChanged(sender, e);
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            //with borderStyle set to RoundedRect, iOS always returns a height of 30
            //https://stackoverflow.com/a/36569247/1063783
            //we get the current value, and restor it, to allow custom renderers to change the border style
            var borderStyle = Control.BorderStyle;
            Control.BorderStyle = UITextBorderStyle.None;
            var size = Control.GetSizeRequest(widthConstraint, double.PositiveInfinity);
            Control.BorderStyle = borderStyle;
            return size;
        }

        private IElementController ElementController => Element as IElementController;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                _defaultTextColor = null;

                if (Control != null)
                {
                    Control.EditingDidBegin -= OnEditingBegan;
                    Control.EditingChanged -= OnEditingChanged;
                    Control.EditingDidEnd -= OnEditingEnded;
                }
            }

            base.Dispose(disposing);
        }

        private void OnEditingBegan(object sender, EventArgs e)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        private void OnEditingChanged(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(Entry.TextProperty, Control.Text);
        }

        private void OnEditingEnded(object sender, EventArgs e)
        {
            // Typing aid changes don't always raise EditingChanged event
            if (Control.Text != Element.Text)
            {
                ElementController.SetValueFromRenderer(Entry.TextProperty, Control.Text);
            }

            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        protected virtual bool OnShouldReturn(UITextField view)
        {
            Control.ResignFirstResponder();
            ((IEntryController)Element).SendCompleted();
            return false;
        }

        private void UpdateAlignment()
        {
            Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
        }

        private void UpdateColor()
        {
            var textColor = Element.TextColor;

            if (textColor.IsDefault || !Element.IsEnabled)
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = textColor.ToUIColor();
        }

        private void UpdateAdjustsFontSizeToFitWidth()
        {
            //Control.AdjustsFontSizeToFitWidth = Naylah.Xamarin.iOS.Renderers.PlatformConfigurationExtensions.OnThisPlatform(Element).AdjustsFontSizeToFitWidth();
        }

        private void UpdateFont()
        {
            Control.Font = Element.ToUIFont();
        }

        private void UpdateKeyboard()
        {
            Control.ApplyKeyboard(Element.Keyboard);
            Control.ReloadInputViews();
        }

        private void UpdatePassword()
        {
            if (Element.IsPassword && Control.IsFirstResponder)
            {
                Control.Enabled = false;
                Control.SecureTextEntry = true;
                Control.Enabled = Element.IsEnabled;
                Control.BecomeFirstResponder();
            }
            else
                Control.SecureTextEntry = Element.IsPassword;
        }

        private void UpdatePlaceholder()
        {
            var formatted = (FormattedString)Element.Placeholder;

            if (formatted == null)
                return;

            var targetColor = Element.PlaceholderColor;

            // Placeholder default color is 70% gray https://developer.apple.com/library/prerelease/ios/documentation/UIKit/Reference/UITextField_Class/index.html#//apple_ref/occ/instp/UITextField/placeholder

            var color = Element.IsEnabled && !targetColor.IsDefault ? targetColor : Naylah.Xamarin.iOS.Renderers.ColorExtensions.SeventyPercentGrey.ToColor();

            Control.AttributedPlaceholder = formatted.ToAttributed(Element, Element.PlaceholderColor != Color.Default ? Element.PlaceholderColor : color);

            //Control.FloatingLabelActiveTextColor = Color.Accent.ToUIColor();

            if (Control is FloatLabeledTextField)
            {
                ((FloatLabeledTextField)Control).FloatingLabelTextColor = Element.PlaceholderColor.ToUIColor();
                Control.Placeholder = formatted.ToString();
            }
        }

        private void UpdateText()
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (Control.Text != Element.Text)
                Control.Text = Element.Text;
        }

        private void UpdateNumericHandle()
        {
            if ((!Element.HasNumericEntryBehavior()))
            {
                return;
            }

            Control.ShouldChangeCharacters = HandleNumericsScheme;
        }

        private bool HandleNumericsScheme(UITextField textField, NSRange range, string replacementString)
        {
            try
            {
                var numericBehavior = Element.GetNumericEntryBehavior();

                if (numericBehavior == null)
                {
                    Control.ShouldChangeCharacters = null;
                    return false;
                }

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

    public static class XamarinInternalExtensions
    {
        internal static NSAttributedString ToAttributed(this FormattedString formattedString, Element owner,
            global::Xamarin.Forms.Color defaultForegroundColor)
        {
            if (formattedString == null)
                return null;
            var attributed = new NSMutableAttributedString();
            foreach (var span in formattedString.Spans)
            {
                if (span.Text == null)
                    continue;

                attributed.Append(span.ToAttributed(owner, defaultForegroundColor));
            }

            return attributed;
        }

        internal static NSAttributedString ToAttributed(this Span span, Element owner, global::Xamarin.Forms.Color defaultForegroundColor)
        {
            if (span == null)
                return null;

#if __MOBILE__
            UIFont targetFont;
            if (span.IsDefault())
                targetFont = ((IFontElement)owner).ToUIFont();
            else
                targetFont = span.ToUIFont();

            var fgcolor = span.ForegroundColor;
            if (fgcolor.IsDefault)
                fgcolor = defaultForegroundColor;
            if (fgcolor.IsDefault)
                fgcolor = global::Xamarin.Forms.Color.Black; // as defined by apple docs

            return new NSAttributedString(span.Text, targetFont, fgcolor.ToUIColor(), span.BackgroundColor.ToUIColor());
#else
			NSFont targetFont;
			if (span.IsDefault())
				targetFont = ((IFontElement)owner).ToNSFont();
			else
				targetFont = span.ToNSFont();

			var fgcolor = span.ForegroundColor;
			if (fgcolor.IsDefault)
				fgcolor = defaultForegroundColor;
			if (fgcolor.IsDefault)
				fgcolor = Color.Black; // as defined by apple docs

			return new NSAttributedString(span.Text, targetFont, fgcolor.ToNSColor(), span.BackgroundColor.ToNSColor());
#endif
        }
    }

    public class UITextFieldWithPadding : UITextField
    {
        public Thickness Padding { get; set; }

        public UITextFieldWithPadding()
        {
        }

        private static CGRect InsetRect(CGRect rect, UIEdgeInsets insets) =>
            new CGRect(
                rect.X + insets.Left,
                rect.Y + insets.Top,
                rect.Width - insets.Left - insets.Right,
                rect.Height - insets.Top - insets.Bottom);

        public override CGRect TextRect(CGRect forBounds)
        {
            //if (_floatingLabel == null)
            //    return base.TextRect(forBounds);

            return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(new nfloat(Padding.Left), new nfloat(Padding.Top), new nfloat(Padding.Right), new nfloat(Padding.Bottom)));
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            //if (_floatingLabel == null)
            //    return base.EditingRect(forBounds);

            return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(new nfloat(Padding.Left), new nfloat(Padding.Top), new nfloat(Padding.Right), new nfloat(Padding.Bottom)));
        }
    }
}