using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Services.NavigationService
{
    public interface INavigationService : IDisposable
    {
        Task GoBack(bool animated = false);
        bool CanGoBack { get; }

        Task ModalGoBack(bool animated = false);
        bool ModalCanGoBack { get; }

        //void GoForward();
        //bool CanGoForward { get; }

        //void Refresh();

        Task NavigateSetRootAsync(Page page, object parameter = null, bool animated = false);
        Task<bool> NavigateAsync(Page page, object parameter = null, bool animated = false);
        Task NavigateModalAsync(Page page, object parameter = null, bool animated = false);

        //bool Navigate<T>(T key, object parameter = null) where T : struct;//, IConvertible;
        //Task OpenAsync(Type page, object parameter = null);

        //object CurrentPageParam { get; }
        //Type CurrentPageType { get; }
        //DispatcherWrapper Dispatcher { get; }

        Page CurrentPage { get; }
        Page ModalCurrentPage { get; }

        event EventHandler Navigated;
        event EventHandler Navigating;

        ////Task SaveNavigationAsync();
        ////Task<bool> RestoreSavedNavigationAsync();
        ////event TypedEventHandler<Type> AfterRestoreSavedNavigation;

        Task ClearHistory(bool animated = false);
        Task ModalClearHistory(bool animated = false);
        ////void ClearCache(bool removeCachedPagesInBackStack = false);

        ////Task SuspendingAsync();
        ////void Resuming();

        //MasterDetailPage NavigationMasterDetailPage { get; }
        //MasterDetailPageFacade NavigationPageFacade { get; }

        INavigablePageFacade NavigablePageFacade { get; }
        Page NavigablePageFacadeInternalPage { get; }


    }
}
