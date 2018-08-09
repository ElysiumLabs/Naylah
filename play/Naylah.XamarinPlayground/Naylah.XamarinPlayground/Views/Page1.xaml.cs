using Naylah.App.Navigation;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Naylah.XamarinPlayground.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage, INavigable
    {
        public Page1()
        {
            InitializeComponent();
        }

        public async Task OnNavigatedFromAsync(object parameter)
        {
        }

        public async Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
        }

        public async Task OnNavigatingToAsync(object parameter, NavigationMode mode)
        {
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var r = await App.Current.NavigationService.NavigateSetRootAsync((new Page2() { BackgroundColor = Color.Purple }));
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}