using CoreGraphics;
using FloatLabeledEntry;
using Foundation;
using Naylah.App.UI.Behaviors;
using Naylah.App.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using PointF = CoreGraphics.CGPoint;
using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;

namespace Naylah.App.Xamarin.iOS.Renderers
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

            var color = Element.IsEnabled && !targetColor.IsDefault ? targetColor : Naylah.App.Xamarin.iOS.Renderers.ColorExtensions.SeventyPercentGrey.ToColor();

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

    public static class NyEntryRendererExtensions
    {
        internal static UITextAlignment ToNativeTextAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UITextAlignment.Center;

                case TextAlignment.End:
                    return UITextAlignment.Right;

                default:
                    return UITextAlignment.Left;
            }
        }

        private static readonly Dictionary<ToUIFontKey, UIFont> ToUiFont = new Dictionary<ToUIFontKey, UIFont>();
#if __MOBILE__

        public static UIFont ToUIFont(this Font self)

#else
		public static UIFont ToNSFont(this Font self)
#endif
        {
            var size = (float)self.FontSize;
            if (self.UseNamedSize)
            {
                switch (self.NamedSize)
                {
                    case NamedSize.Micro:
                        size = 12;
                        break;

                    case NamedSize.Small:
                        size = 14;
                        break;

                    case NamedSize.Medium:
                        size = 17; // as defined by iOS documentation
                        break;

                    case NamedSize.Large:
                        size = 22;
                        break;

                    default:
                        size = 17;
                        break;
                }
            }

            var bold = self.FontAttributes.HasFlag(FontAttributes.Bold);
            var italic = self.FontAttributes.HasFlag(FontAttributes.Italic);

            if (self.FontFamily != null)
            {
                try
                {
#if __MOBILE__
                    if (UIFont.FamilyNames.Contains(self.FontFamily))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(self.FontFamily);

                        if (bold || italic)
                        {
                            var traits = (UIFontDescriptorSymbolicTraits)0;
                            if (bold)
                                traits = traits | UIFontDescriptorSymbolicTraits.Bold;
                            if (italic)
                                traits = traits | UIFontDescriptorSymbolicTraits.Italic;

                            descriptor = descriptor.CreateWithTraits(traits);
                            return UIFont.FromDescriptor(descriptor, size);
                        }
                    }

                    return UIFont.FromName(self.FontFamily, size);

#else

					var descriptor = new NSFontDescriptor().FontDescriptorWithFamily(self.FontFamily);

					if (bold || italic)
					{
						var traits = (NSFontSymbolicTraits)0;
						if (bold)
							traits = traits | NSFontSymbolicTraits.BoldTrait;
						if (italic)
							traits = traits | NSFontSymbolicTraits.ItalicTrait;

						descriptor = descriptor.FontDescriptorWithSymbolicTraits(traits);
						return NSFont.FromDescription(descriptor, size);
					}

					return NSFont.FromFontName(self.FontFamily, size);
#endif
                }
                catch
                {
                    Debug.WriteLine("Could not load font named: {0}", self.FontFamily);
                }
            }
            if (bold && italic)
            {
                var defaultFont = UIFont.SystemFontOfSize(size);
#if __MOBILE__
                var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return UIFont.FromDescriptor(descriptor, 0);
            }
            if (italic)
                return UIFont.ItalicSystemFontOfSize(size);
#else
				var descriptor = defaultFont.FontDescriptor.FontDescriptorWithSymbolicTraits(
					NSFontSymbolicTraits.BoldTrait |
					NSFontSymbolicTraits.ItalicTrait);

				return NSFont.FromDescription(descriptor, 0);
			}
			if (italic)
			{
				Debug.WriteLine("Italic font requested, passing regular one");
				return NSFont.UserFontOfSize(size);
			}
#endif

            if (bold)
                return UIFont.BoldSystemFontOfSize(size);

            return UIFont.SystemFontOfSize(size);
        }

        internal static bool IsDefault(this Span self)
        {
            return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) &&
                    self.FontAttributes == FontAttributes.None;
        }

#if __MOBILE__

        internal static UIFont ToUIFont(this Label label)
#else
		internal static UIFont ToNSFont(this Label label)
#endif
        {
            var values = label.GetValues(Label.FontFamilyProperty, Label.FontSizeProperty, Label.FontAttributesProperty);
            return ToUIFont((string)values[0], (float)(double)values[1], (FontAttributes)values[2]) ??
                    UIFont.SystemFontOfSize(UIFont.LabelFontSize);
        }

#if __MOBILE__

        internal static UIFont ToUIFont(this IFontElement element)
#else
		internal static NSFont ToNSFont(this IFontElement element)
#endif
        {
            return ToUIFont(element.FontFamily, (float)element.FontSize, element.FontAttributes);
        }

        private static UIFont _ToUIFont(string family, float size, FontAttributes attributes)
        {
            var bold = (attributes & FontAttributes.Bold) != 0;
            var italic = (attributes & FontAttributes.Italic) != 0;

            if (family != null)
            {
                try
                {
                    UIFont result;
#if __MOBILE__
                    if (UIFont.FamilyNames.Contains(family))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(family);

                        if (bold || italic)
                        {
                            var traits = (UIFontDescriptorSymbolicTraits)0;
                            if (bold)
                                traits = traits | UIFontDescriptorSymbolicTraits.Bold;
                            if (italic)
                                traits = traits | UIFontDescriptorSymbolicTraits.Italic;

                            descriptor = descriptor.CreateWithTraits(traits);
                            result = UIFont.FromDescriptor(descriptor, size);
                            if (result != null)
                                return result;
                        }
                    }

                    result = UIFont.FromName(family, size);
#else

					var descriptor = new NSFontDescriptor().FontDescriptorWithFamily(family);

					if (bold || italic)
					{
						var traits = (NSFontSymbolicTraits)0;
						if (bold)
							traits = traits | NSFontSymbolicTraits.BoldTrait;
						if (italic)
							traits = traits | NSFontSymbolicTraits.ItalicTrait;

						descriptor = descriptor.FontDescriptorWithSymbolicTraits(traits);
						result = NSFont.FromDescription(descriptor, size);
						if (result != null)
							return result;
					}

					result = NSFont.FromFontName(family, size);
#endif
                    if (result != null)
                        return result;
                }
                catch
                {
                    Debug.WriteLine("Could not load font named: {0}", family);
                }
            }

            if (bold && italic)
            {
                var defaultFont = UIFont.SystemFontOfSize(size);

#if __MOBILE__
                var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return UIFont.FromDescriptor(descriptor, 0);
            }
            if (italic)
                return UIFont.ItalicSystemFontOfSize(size);
#else
				var descriptor = defaultFont.FontDescriptor.FontDescriptorWithSymbolicTraits(
					NSFontSymbolicTraits.BoldTrait |
					NSFontSymbolicTraits.ItalicTrait);

				return NSFont.FromDescription(descriptor, 0);
			}
			if (italic)
			{
				Debug.WriteLine("Italic font requested, passing regular one");
				return NSFont.UserFontOfSize(size);
			}
#endif
            if (bold)
                return UIFont.BoldSystemFontOfSize(size);

            return UIFont.SystemFontOfSize(size);
        }

        private static UIFont ToUIFont(string family, float size, FontAttributes attributes)
        {
            var key = new ToUIFontKey(family, size, attributes);

            lock (ToUiFont)
            {
                UIFont value;
                if (ToUiFont.TryGetValue(key, out value))
                    return value;
            }

            var generatedValue = _ToUIFont(family, size, attributes);

            lock (ToUiFont)
            {
                UIFont value;
                if (!ToUiFont.TryGetValue(key, out value))
                    ToUiFont.Add(key, value = generatedValue);
                return value;
            }
        }

        private struct ToUIFontKey
        {
            internal ToUIFontKey(string family, float size, FontAttributes attributes)
            {
                _family = family;
                _size = size;
                _attributes = attributes;
            }

#pragma warning disable 0414 // these are not called explicitly, but they are used to establish uniqueness. allow it!
            private string _family;
            private float _size;
            private FontAttributes _attributes;
#pragma warning restore 0414
        }
    }

    public static class ColorExtensions
    {
#if __MOBILE__
        internal static readonly UIColor Black = UIColor.Black;
        internal static readonly UIColor SeventyPercentGrey = new UIColor(0.7f, 0.7f, 0.7f, 1);
#else
		internal static readonly NSColor Black = NSColor.Black;
		internal static readonly NSColor SeventyPercentGrey = NSColor.FromRgba(0.7f, 0.7f, 0.7f, 1);
#endif

        //public static CGColor ToCGColor(this Color color)
        //{
        //    return new CGColor((float)color.R, (float)color.G, (float)color.B, (float)color.A);
        //}

        //        public static Color ToColor(this UIColor color)
        //        {
        //            nfloat red;
        //            nfloat green;
        //            nfloat blue;
        //            nfloat alpha;
        //#if __MOBILE__
        //            color.GetRGBA(out red, out green, out blue, out alpha);
        //#else
        //			color.GetRgba(out red, out green, out blue, out alpha);
        //#endif
        //            return new Color(red, green, blue, alpha);
        //        }

        //#if __MOBILE__

        // public static UIColor ToUIColor(this Color color) { return new UIColor((float)color.R,
        // (float)color.G, (float)color.B, (float)color.A); }

        // public static UIColor ToUIColor(this Color color, Color defaultColor) { if
        // (color.IsDefault) return defaultColor.ToUIColor();

        // return color.ToUIColor(); }

        // public static UIColor ToUIColor(this Color color, UIColor defaultColor) { if
        // (color.IsDefault) return defaultColor;

        // return color.ToUIColor(); }

        //#else
        //		public static NSColor ToNSColor(this Color color)
        //		{
        //			return NSColor.FromRgba((float)color.R, (float)color.G, (float)color.B, (float)color.A);
        //		}

        // public static NSColor ToNSColor(this Color color, Color defaultColor) { if
        // (color.IsDefault) return defaultColor.ToNSColor();

        // return color.ToNSColor(); }

        // public static NSColor ToNSColor(this Color color, NSColor defaultColor) { if
        // (color.IsDefault) return defaultColor;

        //			return color.ToNSColor();
        //		}
        //#endif
    }

    public static class PointExtensions
    {
        public static Point ToPoint(this PointF point)
        {
            return new Point(point.X, point.Y);
        }

        public static PointF ToPointF(this Point point)
        {
            return new PointF(point.X, point.Y);
        }
    }

    public static class SizeExtensions
    {
        public static SizeF ToSizeF(this Size size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }
    }

    public static class RectangleExtensions
    {
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static RectangleF ToRectangleF(this Rectangle rect)
        {
            return new RectangleF((nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height);
        }
    }
}