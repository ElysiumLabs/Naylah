using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public class HeaderLayout : StackLayout
    {
        public CachedImage Image;
        public Label Line1, Line2, Line3;

        public event EventHandler Tapped;

        public static BindableProperty Line1TextProperty = BindableProperty.Create(nameof(Line1Text), typeof(string), typeof(HeaderLayout), default(string));

        public string Line1Text
        {
            get { return (string)GetValue(Line1TextProperty); }
            set { SetValue(Line1TextProperty, value); }
        }

        public static BindableProperty Line2TextProperty = BindableProperty.Create(nameof(Line2Text), typeof(string), typeof(HeaderLayout), default(string));

        public string Line2Text
        {
            get { return (string)GetValue(Line2TextProperty); }
            set { SetValue(Line2TextProperty, value); }
        }

        public static BindableProperty Line3TextProperty = BindableProperty.Create(nameof(Line3Text), typeof(string), typeof(HeaderLayout), default(string));

        public string Line3Text
        {
            get { return (string)GetValue(Line3TextProperty); }
            set { SetValue(Line3TextProperty, value); }
        }

        public static BindableProperty ErrorPlaceholderProperty = BindableProperty.Create(nameof(ErrorPlaceholder), typeof(string), typeof(HeaderLayout), default(string));

        public string ErrorPlaceholder
        {
            get { return (string)GetValue(ErrorPlaceholderProperty); }
            set { SetValue(ErrorPlaceholderProperty, value); }
        }

        public static BindableProperty LoadingPlaceholderProperty = BindableProperty.Create(nameof(LoadingPlaceholder), typeof(string), typeof(HeaderLayout), default(string));

        public string LoadingPlaceholder
        {
            get { return (string)GetValue(LoadingPlaceholderProperty); }
            set { SetValue(LoadingPlaceholderProperty, value); }
        }

        public static BindableProperty ImageBorderSizeProperty = BindableProperty.Create(nameof(ImageBorderSize), typeof(int), typeof(HeaderLayout), 0);

        public int ImageBorderSize
        {
            get { return (int)GetValue(ImageBorderSizeProperty); }
            set { SetValue(ImageBorderSizeProperty, value); }
        }

        public static BindableProperty ImageBorderColorProperty = BindableProperty.Create(nameof(ImageBorderColor), typeof(Color), typeof(HeaderLayout), default(Color));

        public Color ImageBorderColor
        {
            get { return (Color)GetValue(ImageBorderColorProperty); }
            set { SetValue(ImageBorderColorProperty, value); }
        }

        public static BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(HeaderLayout), default(ICommand));

        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }

        public HeaderLayout()
        {
            CreateLayout();
            Tapped += DataLayout_Tapped;
        }

        private void DataLayout_Tapped(object sender, EventArgs e)
        {
            TappedCommand?.Execute(null);
        }

        private void CreateLayout()
        {
            Image = new CachedImage()
            {
                HeightRequest = 90,
                WidthRequest = 90,
                DownsampleToViewSize = true,
                DownsampleUseDipUnits = true,
                TransparencyEnabled = false,
                Aspect = Aspect.AspectFit,
                RetryCount = 3,
                RetryDelay = 500,
                ErrorPlaceholder = ErrorPlaceholder,
                LoadingPlaceholder = LoadingPlaceholder,
                Transformations = new List<FFImageLoading.Work.ITransformation>
                {
                    //new CircleTransformation(ImageBorderSize, ImageBorderColor.ToString()) /transfer to control
                }
            };

            Line1 = new Label
            {
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                Text = Line1Text
            };

            Line2 = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                Text = Line2Text
            };

            Line3 = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                Text = Line3Text
            };

            var data = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    Line1,
                    Line2,
                    Line3
                }
            };

            var info = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 0,
                Spacing = 10,
                Children =
                {
                    Image,
                    data
                }
            };

            var headerTapGestureRecognizer = new TapGestureRecognizer();
            headerTapGestureRecognizer.Tapped += (s, e) => { var tapped = Tapped; tapped?.Invoke(s, e); };

            var infoView = new ContentView
            {
                Content = info,
                Padding = Padding,
                BackgroundColor = BackgroundColor
            };
            infoView.GestureRecognizers.Add(headerTapGestureRecognizer);

            Children.Add(infoView);
        }
    }
}