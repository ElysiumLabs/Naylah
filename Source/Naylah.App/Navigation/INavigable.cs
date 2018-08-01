using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public interface INavigable
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode);

        Task OnNavigatedFromAsync(object parameter);

        Task OnNavigatingToAsync(object parameter, NavigationMode mode);
    }
}