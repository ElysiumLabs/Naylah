using Naylah.App.Events;
using System;
using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public class NavigationService :
        INavigation,
        INavigationBackward
    {
        public object CurrentView => throw new NotImplementedException();

        public Task<bool> CanGoBack => throw new NotImplementedException();

        public event EventHandler<DataEventArgs<NavigatedEventArgs>> Navigated;

        public event EventHandler<DataEventArgs<NavigatingEventArgs>> Navigating;

        public Task ClearHistoryAsync(INavigationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<INavigationResult> GoBackAsync(INavigationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<INavigationResult> NavigateAsync(object view, object parameter = null, INavigationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task NavigateSetRootAsync(object view, object parameter = null, INavigationOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}