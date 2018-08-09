using Naylah.App.Navigation;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Naylah.XamarinPlayground.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage, INavigable
    {
        public Page2()
        {
            InitializeComponent();
        }

        public async Task OnNavigatedFromAsync(object parameter)
        {
        }

        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            throw new Exception("bla");
        }

        public async Task OnNavigatingToAsync(object parameter, NavigationMode mode)
        {
        }
    }
}