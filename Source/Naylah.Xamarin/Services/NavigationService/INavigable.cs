using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Xamarin.Services.NavigationService
{
    public interface INavigable
    {

        Task OnNavigatedToAsync(object parameter, NavigationMode mode);

        Task OnNavigatedFromAsync();



        Task OnNavigatingToAsync(object parameter, NavigationMode mode);

    }

    public enum NavigationMode
    {
        New,
        Back,
        Forward,
        Refresh
    }
}
