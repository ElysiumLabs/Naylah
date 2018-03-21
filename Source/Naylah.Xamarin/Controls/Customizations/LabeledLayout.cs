using Naylah.Xamarin.Controls.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public abstract class LabeledLayout : StackLayout
    {
        //public static BindableProperty ThemeProperty =
        //    BindableProperty.Create(nameof(Theme), typeof(BasicTheme), typeof(LabeledLayout), default(BasicTheme));

        //public BasicTheme Theme
        //{
        //    get { return (BasicTheme)GetValue(ThemeProperty); }
        //    set { SetValue(ThemeProperty, value); }
        //}

        public string Placeholder
        {
            get { return Header.Text; }
            set
            {
                Header.Text = value;
                HeaderLayout.IsVisible = !string.IsNullOrEmpty(Header.Text);
            }
        }

        public Label Header { get; private set; }
        public StackLayout HeaderLayout { get; private set; }

        public virtual View Content { get; set; }

        public LabeledLayout()
        {
            Header = new Label();
            HeaderLayout = new StackLayout();

            //Header.TextColor = Theme == BasicTheme.Dark ? Color.Black : Color.White;

            if (Device.OS == TargetPlatform.iOS)
            {
                Header.FontSize = 11;
                Header.FontAttributes = FontAttributes.Bold;
                Spacing = 0;
            }
            else
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    Header.FontSize = 12;
                    HeaderLayout.Padding = new Thickness(4, 0, 0, 0);
                    Header.TextColor = Color.FromHex("#666666");
                    Spacing = 0;
                }
            }

            HeaderLayout.Children.Add(Header);
            Children.Add(HeaderLayout);
        }
    }

    public class CustomLabeledLayout : LabeledLayout
    {
        public CustomLabeledLayout(View view) : base()
        {
            Content = view;
            Children.Add(view);
        }
    }

    public static class CustomLabeledLayoutExtensions
    {
        public static CustomLabeledLayout AsCustomLabeled(this View view)
        {
            return new CustomLabeledLayout(view);
        }
    }
}