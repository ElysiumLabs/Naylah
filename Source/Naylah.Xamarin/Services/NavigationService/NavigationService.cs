using Naylah.Xamarin.Common;
using Naylah.Xamarin.Controls.Pages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Services.NavigationService
{
    public class NavigationService : INavigationService, IDisposable
    {
        private BootStrapper App;

        public Page CurrentPage { get { return NavigablePageFacade.CurrentPage; } }
        public Page ModalCurrentPage { get { return NavigablePageFacade.ModalCurrentPage; } }

        public INavigablePageFacade NavigablePageFacadeInternal { get; protected set; }

        public INavigablePageFacade NavigablePageFacade => NavigablePageFacadeInternal;
        public Page NavigablePageFacadeInternalPage => NavigablePageFacade.InternalPage;

        public bool CanGoBack
        {
            get
            {
                return NavigablePageFacadeInternal.InternalNavigationContext.NavigationStack.Count > 1;
            }
        }

        public bool ModalCanGoBack
        {
            get
            {
                var modalCanB = NavigablePageFacadeInternal.InternalNavigationContext.ModalStack.Count > 0;

                //In xamarin android the navigation page shows up on modalstack :/
                if (NavigablePageFacadeInternal.InternalNavigationContext.ModalStack.Count == 1)
                {
                    modalCanB = NavigablePageFacadeInternal.InternalNavigationContext.ModalStack[0] != NavigablePageFacadeInternal.InternalPage;
                }

                return modalCanB;
            }
        }

        public event EventHandler Navigated;

        public void RaiseNavigatedEvent()
        {
            var navigated = Navigated;
            navigated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Navigating;

        public void RaiseNavigatingEvent()
        {
            var navigating = Navigating;
            navigating?.Invoke(this, EventArgs.Empty);
        }

        protected internal NavigationService(BootStrapper app, Page page)
        {
            App = app;

            BootStrapper.HardwareBackPressed = NavigationServiceHardwarePressed;

            ResetNavigation(page);
        }

        private async Task<bool?> NavigationServiceHardwarePressed()
        {
            return !await BackHandled();
        }

        private async Task<bool?> BackHandled()
        {
            if (ModalCanGoBack)
            {
                return PageHasHandleBack(NavigablePageFacade.ModalCurrentPage);
            }

            if (CanGoBack)
            {
                return PageHasHandleBack(NavigablePageFacade.CurrentPage);
            }

            return null;
        }

        public static bool? PageHasHandleBack(Page page)
        {
            var ppage = page as PageBase;
            var contentpage = page as ContentPageBase;
            var tabbedpage = page as TabbedPageBase;

            if (ppage != null)
            {
                return ppage.HandleBack;
            }

            if (contentpage != null)
            {
                return contentpage.HandleBack;
            }

            if (tabbedpage != null)
            {
                return tabbedpage.HandleBack;
            }

            return false;
        }

        private void ResetNavigation(Page page)
        {
            NavigablePageFacadeInternal = null;

            var masterDetailPage = page as MasterDetailPage;
            var navigationPage = page as NavigationPage;

            if (masterDetailPage != null)
            {
                masterDetailPage.IsPresented = false;
                //masterDetailPage.IsGestureEnabled = false;

                NavigablePageFacadeInternal = new MasterDetailPageFacade(masterDetailPage);
            }

            if (navigationPage != null)
            {
                NavigablePageFacadeInternal = new NavigationPageFacade(navigationPage);
            }

            if (NavigablePageFacadeInternal == null)
            {
                throw new Exception("Must be a MasterDetail or Navigation Page.");
            }

            NavigablePageFacadeInternal.Popped += async (s, e) =>
            {
                try
                {
                    await NavigateFromAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.Pushed += async (s, e) =>
            {
                try
                {
                    await NavigatedToAsync(NavigationMode.New, e.Parameter, NavigablePageFacadeInternal.CurrentPage);
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.ModalPopped += async (s, e) =>
            {
                try
                {
                    await ModalNavigateFromAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.ModalPushed += async (s, e) =>
            {
                try
                {
                    await NavigatedToAsync(NavigationMode.New, e.Parameter, NavigablePageFacadeInternal.ModalCurrentPage);
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.Refresh();

            App.MainPage = NavigablePageFacade.InternalPage;
        }

        private async Task NavigateFromAsync()
        {
            var page = NavigablePageFacadeInternal.CurrentPage as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = ResolveForPage(page);
                if (dataContext != null)
                {
                    await dataContext.OnNavigatedFromAsync();
                }
            }

            RaiseNavigatedEvent();
        }

        internal async Task ModalNavigateFromAsync()
        {
            var page = NavigablePageFacadeInternal.ModalCurrentPage as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = ResolveForPage(page);
                if (dataContext != null)
                {
                    await dataContext.OnNavigatedFromAsync();
                }
            }

            RaiseNavigatedEvent();
        }

        internal async Task NavigatedToAsync(NavigationMode mode, object parameter, object pageContent = null)
        {
            var page = pageContent as Page;
            if (page != null)
            {
                var dataContext = ResolveForPage(page);

                if (dataContext != null)
                {
                    await dataContext.OnNavigatedToAsync(parameter, mode);
                }
            }

            RaiseNavigatedEvent();
        }

        internal async Task NavigatingToAsync(NavigationMode mode, object parameter, object pageContent = null)
        {
            var page = pageContent as Page;
            if (page != null)
            {
                var dataContext = ResolveForPage(page);

                if (dataContext != null)
                {
                    await dataContext.OnNavigatingToAsync(parameter, mode);
                }
            }

            RaiseNavigatingEvent();
        }

        public async Task NavigateModalAsync(Page page, object parameter = null, bool animated = false)
        {
            if (page != null)
            {
                await NavigatingToAsync(NavigationMode.New, parameter, page);
                await NavigablePageFacadeInternal.PushModalAsync(page, parameter, animated);
            }
        }

        public async Task<bool> NavigateAsync(Page page, object parameter = null, bool animated = true)
        {
            if (App.MainPage == null) { return false; }

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            await NavigatingToAsync(NavigationMode.New, parameter, page);
            await NavigablePageFacadeInternal.PushAsync(page, parameter, animated);

            return true;
        }

        public async Task GoBack(bool animated = true)
        {
            await NavigatingToAsync(NavigationMode.Back, null, CurrentPage);
            await NavigablePageFacadeInternal.PopAsync(animated);
        }

        public async Task ModalGoBack(bool animated = false)
        {
            await NavigatingToAsync(NavigationMode.Back, null, ModalCurrentPage);
            await NavigablePageFacadeInternal.PopModalAsync(animated);
        }

        public async Task ClearHistory(bool animated = false)
        {
            try
            {
                while (CanGoBack)
                {
                    await GoBack(animated);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task ModalClearHistory(bool animated = false)
        {
            try
            {
                while (ModalCanGoBack)
                {
                    await ModalGoBack(animated);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private INavigable ResolveForPage(Page page)
        {
            if (!(page.BindingContext is INavigable) | page.BindingContext == null)
            {
                var viewModel = ((BootStrapper)Application.Current).ResolveForPage(page, this);
                if ((viewModel != null))
                {
                    return viewModel;
                }
            }

            return page.BindingContext as INavigable;
        }

        public async Task NavigateSetRootAsync(Page page, object parameter = null, bool animated = false)
        {
            await NavigatingToAsync(NavigationMode.New, parameter, page);
            await NavigablePageFacade.ChangeRootAsync(page, animated);
            await NavigatedToAsync(NavigationMode.New, parameter, page);
        }

        public void Dispose()
        {
            Navigated = null;
            Navigating = null;
        }

        public enum NavigationType
        {
            NavigationPage,
            MasterDetail
        }
    }
}