using Naylah.App.Navigation;
using Naylah.App.UI.UX;
using System;
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

        private void leve_Clicked(object sender, EventArgs e)
        {
            HapticFeedback.Instance.Run(HapticFeedbackType.Softy);
        }

        private void medio_Clicked(object sender, EventArgs e)
        {
            HapticFeedback.Instance.Run(HapticFeedbackType.Medium);
        }

        private void pesado_Clicked(object sender, EventArgs e)
        {
            HapticFeedback.Instance.Run(HapticFeedbackType.Heavy);
        }
    }
}