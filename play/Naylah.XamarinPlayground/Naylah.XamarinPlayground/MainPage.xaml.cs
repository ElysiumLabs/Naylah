using Naylah.App.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.XamarinPlayground
{
    public partial class MainPage : ContentPage, INavigable
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async Task OnNavigatedFromAsync(object parameter)
        {
            //throw new NotImplementedException();
        }

        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            //throw new NotImplementedException();
        }

        public async Task OnNavigatingToAsync(object parameter, NavigationMode mode)
        {
            //throw new NotImplementedException();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //App.Current.StyleKit = (App.Current.StyleKit.GetType() == typeof(Style1)) ? new Style2() as StyleKit : new Style1() as StyleKit;
        }
    }
}