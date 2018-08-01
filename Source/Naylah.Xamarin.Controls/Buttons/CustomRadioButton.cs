using Naylah.Xamarin.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Buttons
{
    public class CustomRadioButton : View
    {
        /// <summary>
        /// The checked property
        /// </summary>
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create(nameof(Checked), typeof(bool), typeof(CustomRadioButton), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            EventHandler<CheckedEventArgs> eh = ((CustomRadioButton)bindable).CheckedChanged;
            if (eh != null)
                eh(bindable, new CheckedEventArgs((bool)newValue));
        }, defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        ///     The default text property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomRadioButton), string.Empty);

        /// <summary>
        ///     The default text property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomRadioButton), Color.Default);

        /// <summary>
        /// The font size property
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
          BindableProperty.Create(nameof(FontSize), typeof(object), typeof(CheckBox), -1);

        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty FontNameProperty =
            BindableProperty.Create(nameof(FontName), typeof(string), typeof(CustomRadioButton), string.Empty);

        /// <summary>
        ///     The checked changed event.
        /// </summary>
        public EventHandler<CheckedEventArgs> CheckedChanged;

        /// <summary>
        ///     Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }

            set
            {
                SetValue(CheckedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }

            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName
        {
            get
            {
                return (string)GetValue(FontNameProperty);
            }
            set
            {
                SetValue(FontNameProperty, value);
            }
        }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    }
}