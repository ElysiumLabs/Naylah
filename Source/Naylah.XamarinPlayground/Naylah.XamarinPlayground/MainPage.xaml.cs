using Naylah.App.UI.Style;
using Naylah.XamarinPlayground.UI.Styles;
using System;
using Xamarin.Forms;

namespace Naylah.XamarinPlayground
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.StyleKit = (App.Current.StyleKit.GetType() == typeof(Style1)) ? new Style2() as StyleKit : new Style1() as StyleKit;
        }
    }
}