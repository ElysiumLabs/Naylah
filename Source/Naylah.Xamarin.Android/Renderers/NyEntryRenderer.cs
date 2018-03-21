using Android.Content.Res;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Naylah.Xamarin.Android.Extras;
using Naylah.Xamarin.Android.Platform;
using Naylah.Xamarin.Android.Renderers;
using Naylah.Xamarin.Behaviors;
using Naylah.Xamarin.Controls.Entrys;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NyEntry), typeof(NyEntryRenderer))]

namespace Naylah.Xamarin.Android.Renderers
{
    public class NyEntryRenderer : global::Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<NyEntry, TextInputLayout>, ITextWatcher, TextView.IOnEditorActionListener
    {
        private ColorStateList _hintTextColorDefault;
        private ColorStateList _textColorDefault;
        private bool _disposed;

        bool TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            // Fire Completed and dismiss keyboard for hardware / physical keyboards
            if (actionId == ImeAction.Done || (actionId == ImeAction.ImeNull && e.KeyCode == Keycode.Enter))
            {
                Control.ClearFocus();
                v.HideKeyboard();
                ((IEntryController)Element).SendCompleted();
            }

            return true;
        }

        void ITextWatcher.AfterTextChanged(IEditable s)
        {
        }

        void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        void ITextWatcher.OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (string.IsNullOrEmpty(Element.Text) && s.Length() == 0)
                return;

            ((IElementController)Element).SetValueFromRenderer(Entry.TextProperty, s.ToString());
        }

        public NyEntryRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NyEntry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var textInputLayout = CreateNativeControl();
                //textInputLayout.EditText.ImeOptions = ImeAction.Done;
                textInputLayout.EditText.AddTextChangedListener(this);
                textInputLayout.EditText.SetOnEditorActionListener(this);
                SetNativeControl(textInputLayout);
            }

            ConfigureControl();
        }

        private void ConfigureControl()
        {
            Control.EditText.Text = Element.Text;

            UpdateInputType();
            UpdatePlaceholder();
            UpdateColor();
            UpdateStyle();
            UpdateAlignment();
            UpdateFont();
            UpdatePlaceholderColor();
            UpdateNumericHandle();
        }

        private void UpdateStyle()
        {
            if (Element.EntryStyle == NyEntryStyle.Rounded)
            {
                Control.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

                //if (Control.EditText.Multiline == true)
                //{
                //    edittext.SetLines(20000);
                //    edittext.Gravity = GravityFlags.Left | GravityFlags.Top;
                //}

                global::Android.Graphics.Drawables.GradientDrawable gd = new global::Android.Graphics.Drawables.GradientDrawable();
                gd.SetColor(Element.RoundedBackgroundColor.ToAndroid());

                Control.SetPadding(
                    (int)Element.RoundedPadding.Left,
                    (int)Element.RoundedPadding.Top,
                    (int)Element.RoundedPadding.Right,
                    (int)Element.RoundedPadding.Bottom);

                gd.SetCornerRadius(Element.RoundedBorderRadius);

                gd.SetStroke(2, Element.RoundedBorderColor.ToAndroid());

                Control.SetBackgroundDrawable(gd);

                global::Android.Graphics.Drawables.GradientDrawable gd1 = new global::Android.Graphics.Drawables.GradientDrawable();
                gd1.SetColor(Color.Transparent.ToAndroid());
                Control.EditText.SetBackground(gd1);
            }
        }

        private void UpdateNumericHandle()
        {
            CustomEditText c = Control.EditText as CustomEditText;

            if ((c == null) || (!Element.HasNumericEntryBehavior()))
            {
                return;
            }

            c.UtilizeCustomInputConnection = true;

            Control.EditText.KeyListener = new NumericEntryBehaviorListener(Element.GetNumericEntryBehavior());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
                UpdatePlaceholder();
            else
            if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                UpdateInputType();
            else
            if (e.PropertyName == Entry.TextProperty.PropertyName)
            {
                if (Control.EditText.Text != Element.Text)
                {
                    Control.EditText.Text = Element.Text;
                    if (Control.IsFocused)
                    {
                        Control.EditText.SetSelection(Control.EditText.Text.Length);
                        Control.ShowKeyboard();
                    }
                }
            }
            else if (e.PropertyName == Entry.TextColorProperty.PropertyName)
                UpdateColor();
            else if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
                UpdateInputType();
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
                UpdateAlignment();
            else if (e.PropertyName == Entry.FontAttributesProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontFamilyProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontSizeProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholderColor();
            else if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
                UpdatePlaceholder();

            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdatePlaceholder()
        {
            if (Element.EntryStyle == NyEntryStyle.FloatLabeled)
            {
                Control.HintEnabled = true;
                Control.Hint = Element.Placeholder;
            }
            else
            {
                Control.HintEnabled = false;
                Control.EditText.Hint = Element.Placeholder;
                Control.EditText.SetHintTextColor(Element.PlaceholderColor.ToAndroid());
            }
        }

        private void UpdateAlignment()
        {
            Control.SetGravity(Element.HorizontalTextAlignment.ToHorizontalGravityFlags());
        }

        private void UpdateColor()
        {
            if (Element.TextColor.IsDefault)
            {
                if (_textColorDefault == null)
                {
                    // This control has always had the default colors; nothing to update
                    return;
                }

                // This control is being set back to the default colors
                Control.EditText.SetTextColor(_textColorDefault);
            }
            else
            {
                if (_textColorDefault == null)
                {
                    // Keep track of the default colors so we can return to them later and so we can
                    // preserve the default disabled color
                    _textColorDefault = Control.EditText.TextColors;
                }

                Control.EditText.SetTextColor(Element.TextColor.ToAndroidPreserveDisabled(_textColorDefault));
            }
        }

        private void UpdatePlaceholderColor()
        {
            Color placeholderColor = Element.PlaceholderColor;

            if (placeholderColor.IsDefault)
            {
                if (_hintTextColorDefault == null)
                {
                    // This control has always had the default colors; nothing to update
                    return;
                }

                // This control is being set back to the default colors
                Control.EditText.SetHintTextColor(_hintTextColorDefault);
            }
            else
            {
                if (_hintTextColorDefault == null)
                {
                    // Keep track of the default colors so we can return to them later and so we can
                    // preserve the default disabled color
                    _hintTextColorDefault = Control.EditText.HintTextColors;
                }

                Control.EditText.SetHintTextColor(_hintTextColorDefault);

                TextInputLayoutExtensions.SetExtendColor(Control, placeholderColor.ToAndroid(), false);
            }
        }

        private void UpdateFont()
        {
            Control.Typeface = Element.ToTypeface();
            Control.EditText.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);
        }

        private void UpdateInputType()
        {
            Entry model = Element;
            var keyboard = model.Keyboard;

            Control.EditText.InputType = keyboard.ToInputType();

            if (keyboard == Keyboard.Numeric)
            {
                //Control.EditText.KeyListener = GetDigitsKeyListener(Control.EditText.InputType);
            }

            if (model.IsPassword && ((Control.EditText.InputType & InputTypes.ClassText) == InputTypes.ClassText))
                Control.EditText.InputType = Control.EditText.InputType | InputTypes.TextVariationPassword;
            if (model.IsPassword && ((Control.EditText.InputType & InputTypes.ClassNumber) == InputTypes.ClassNumber))
                Control.EditText.InputType = Control.EditText.InputType | InputTypes.NumberVariationPassword;
        }

        //private void SetText()
        //{
        //    NativeView.EditText.Text = Element.Text;
        //}

        //private void SetIsEnabled()
        //{
        //    NativeView.EditText.Enabled = Element.IsEnabled;
        //}

        protected override TextInputLayout CreateNativeControl()
        {
            return (TextInputLayout)LayoutInflater.From(Context).Inflate(Naylah.Xamarin.Android.Resource.Layout.TextInputLayout, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                //if (Control != null)
                //{
                //    Control.OnKeyboardBackPressed -= OnKeyboardBackPressed;
                //}
            }

            base.Dispose(disposing);
        }
    }
}