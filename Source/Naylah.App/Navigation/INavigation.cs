using Naylah.App.Events;
using System;
using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public interface INavigation
    {
        Task NavigateSetRootAsync(object view, object parameter = null, INavigationOptions options = null);

        Task<INavigationResult> NavigateAsync(object view, object parameter = null, INavigationOptions options = null);

        object CurrentView { get; }

        Task ClearHistoryAsync(INavigationOptions options = null);

        event EventHandler<DataEventArgs<NavigatedEventArgs>> Navigated;

        event EventHandler<DataEventArgs<NavigatingEventArgs>> Navigating;
    }
}