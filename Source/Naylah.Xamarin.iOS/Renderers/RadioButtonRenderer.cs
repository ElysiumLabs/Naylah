using CoreGraphics;
using Naylah.Xamarin.Controls.Buttons;
using Naylah.Xamarin.iOS.Controls;
using Naylah.Xamarin.iOS.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS.Renderers
{
    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, RadioButtonView>
    {
        private UIColor _defaultTextColor;

        /// <summary>
        /// Handles the Element Changed event
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var checkBox = new RadioButtonView(Bounds);
                _defaultTextColor = checkBox.TitleColor(UIControlState.Normal);

                checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;

                SetNativeControl(checkBox);
            }

            if (this.Element == null) return;

            BackgroundColor = this.Element.BackgroundColor.ToUIColor();
            UpdateFont();

            Control.LineBreakMode = UILineBreakMode.CharacterWrap;
            Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            Control.Text = this.Element.Text;
            Control.Checked = this.Element.Checked;
            UpdateTextColor();
        }

        /// <summary>
        /// Resizes the text.
        /// </summary>
        private void ResizeText()
        {
            var text = Element.Text;

            var bounds = Control.Bounds;

            var width = Control.TitleLabel.Bounds.Width;

            var height = StringExtensions.Empty.StringHeight(Control.Font, width);

            var minHeight = string.Empty.StringHeight(Control.Font, width);

            var requiredLines = Math.Round(height / minHeight, MidpointRounding.AwayFromZero);

            var supportedLines = Math.Round(bounds.Height / minHeight, MidpointRounding.ToEven);

            if (supportedLines == requiredLines)
            {
                return;
            }

            bounds.Height += (float)(minHeight * (requiredLines - supportedLines));

            Control.Bounds = bounds;
            Element.HeightRequest = bounds.Height;
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            ResizeText();
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (string.IsNullOrEmpty(Element.FontName))
            {
                return;
            }

            var font = UIFont.FromName(Element.FontName, (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f);

            if (font != null)
            {
                Control.Font = font;
            }
        }

        /// <summary>
        /// Updates the color of the text.
        /// </summary>
        private void UpdateTextColor()
        {
            Control.SetTitleColor(Element.TextColor.ToUIColor(_defaultTextColor), UIControlState.Normal);
            Control.SetTitleColor(Element.TextColor.ToUIColor(_defaultTextColor), UIControlState.Selected);
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;

                case "Text":
                    Control.Text = Element.Text;
                    break;

                case "TextColor":
                    UpdateTextColor();
                    break;

                case "Element":
                    break;

                case "FontSize":
                    UpdateFont();
                    break;

                case "FontName":
                    UpdateFont();
                    break;

                default:
                    //                    Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    return;
            }
        }
    }
}