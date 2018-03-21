using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public class LabelSection : ContentView
    {

        public static readonly BindableProperty TextProperty =
           BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelSection), string.Empty);
        private Label section;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == TextProperty.PropertyName)
            {
                section.Text = Device.OS == TargetPlatform.iOS ? Text.ToUpperInvariant() : Text;
            }
        }

        public LabelSection()
        {
            Padding = Device.OnPlatform(new Thickness(16, 16, 16, 4), new Thickness(16, 24, 16, 4), new Thickness(16, 16, 16, 4));

            section = new Label()
            {
                FontSize = Device.OnPlatform(13, 18, 13),
                TextColor = Color.FromHex("#6D6D72"),
                Style = Device.Styles.ListItemDetailTextStyle
            };

            Content = section;

            if (Device.OS == TargetPlatform.Android)
            {
                section.FontFamily = "sans-serif-light";
            }
        }
    }
}
