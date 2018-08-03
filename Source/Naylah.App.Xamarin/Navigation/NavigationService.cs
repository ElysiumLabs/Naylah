using Naylah.App.Common;
using Naylah.App.Events;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.App.Navigation
{
    public partial class NavigationService :
        Naylah.App.Navigation.INavigation,
        Naylah.App.Navigation.INavigationBackward

    {
        private NyApplication App;

        public NavigationService(NyApplication app, Page shellPage)
        {
            App = app;

            NyApplication.HardwareBackPressed = HandleHardwareBackPressed;

            ResetNavigation(shellPage);
        }

        private void ResetNavigation(Page page)
        {
            NavigablePageFacadeInternal = null;

            var masterDetailPage = page as MasterDetailPage;
            var navigationPage = page as NavigationPage;

            if (masterDetailPage != null)
            {
                masterDetailPage.IsPresented = false;
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

            NavigablePageFacadeInternal.Popped += (s, e) =>
            {
                try
                {
                    PageUtilities.OnNavigatedFrom(e.Page, e.Parameter);
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.Pushed += (s, e) =>
            {
                try
                {
                    PageUtilities.OnNavigatedTo(e.Page, e.Parameter, NavigationMode.New);
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.ModalPopped += (s, e) =>
            {
                try
                {
                    PageUtilities.OnNavigatedFrom(e.Page, e.Parameter);
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.ModalPushed += (s, e) =>
            {
                try
                {
                    PageUtilities.OnNavigatedTo(e.Page, e.Parameter, NavigationMode.New);
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            NavigablePageFacadeInternal.Refresh();

            App.MainPage = NavigablePageFacade.InternalPage;
        }

        private Task<bool?> HandleHardwareBackPressed()
        {
            //if (ModalCanGoBack)
            //{
            //    return PageHasHandleBack(NavigablePageFacade.ModalCurrentPage);
            //}

            if (CanGoBack)
            {
                return Task.FromResult(PageUtilities.HandleGoBackForPage(CurrentView as Page));
            }

            return Task.FromResult(default(bool?));
        }

        public Page ModalCurrentPage { get { return NavigablePageFacade.ModalCurrentPage; } }

        public INavigablePageFacade NavigablePageFacadeInternal { get; protected set; }

        public INavigablePageFacade NavigablePageFacade => NavigablePageFacadeInternal;

        public Page NavigablePageFacadeInternalPage => NavigablePageFacade.InternalPage;

        //

        public object CurrentView { get { return NavigablePageFacade.CurrentPage; } }

        public bool CanGoBack => throw new NotImplementedException();

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

        public async Task<INavigationResult> NavigateAsync(object view, object parameter = null, INavigationOptions options = null)
        {
            var r = new NavigationResult();

            try
            {
                var page = ResolvePageFromView(view);

                if (page == null)
                    throw new ArgumentNullException(nameof(page));

                OnNavigatingTo(page, parameter, NavigationMode.New);

                await NavigablePageFacadeInternal.PushAsync(page, parameter, options?.Animated == true);

                r.Success = true;
            }
            catch (Exception ex)
            {
                r.Exception = ex;
                r.Success = false;
                throw;
            }

            return r;
        }

        public virtual Page ResolvePageFromView(object view)
        {
            if ((view as Page != null))
            {
                return view as Page;
            }
            else
            {
                return null;
            }
        }

        public Task NavigateSetRootAsync(object view, object parameter = null, INavigationOptions options = null)
        {
            throw new NotImplementedException();
            NavigablePageFacade.ChangeRootAsync
        }

        //

        private static void OnNavigatingTo(Page toPage, object parameters, NavigationMode navigationMode = NavigationMode.New)
        {
            PageUtilities.OnNavigatingTo(toPage, parameters, navigationMode);

            if (toPage is TabbedPage tabbedPage)
            {
                foreach (var child in tabbedPage.Children)
                {
                    if (child is NavigationPage navigationPage)
                    {
                        PageUtilities.OnNavigatingTo(navigationPage.CurrentPage, parameters, navigationMode);
                    }
                    else
                    {
                        PageUtilities.OnNavigatingTo(child, parameters, navigationMode);
                    }
                }
            }
            else if (toPage is CarouselPage carouselPage)
            {
                foreach (var child in carouselPage.Children)
                {
                    PageUtilities.OnNavigatingTo(child, parameters, navigationMode);
                }
            }
        }

        private static void OnNavigatedTo(Page toPage, object parameters, NavigationMode navigationMode = NavigationMode.New)
        {
            PageUtilities.OnNavigatedTo(toPage, parameters, navigationMode);

            if (toPage is TabbedPage tabbedPage)
            {
                if (tabbedPage.CurrentPage is NavigationPage navigationPage)
                {
                    PageUtilities.OnNavigatedTo(navigationPage.CurrentPage, parameters, navigationMode);
                }
                else
                {
                    if (tabbedPage.BindingContext != tabbedPage.CurrentPage.BindingContext)
                        PageUtilities.OnNavigatedTo(tabbedPage.CurrentPage, parameters, navigationMode);
                }
            }
            else if (toPage is CarouselPage carouselPage)
            {
                PageUtilities.OnNavigatedTo(carouselPage.CurrentPage, parameters);
            }
        }

        private static void OnNavigatedFrom(Page fromPage, object parameters)
        {
            PageUtilities.OnNavigatedFrom(fromPage, parameters);

            if (fromPage is TabbedPage tabbedPage)
            {
                if (tabbedPage.CurrentPage is NavigationPage navigationPage)
                {
                    PageUtilities.OnNavigatedFrom(navigationPage.CurrentPage, parameters);
                }
                else
                {
                    if (tabbedPage.BindingContext != tabbedPage.CurrentPage.BindingContext)
                        PageUtilities.OnNavigatedFrom(tabbedPage.CurrentPage, parameters);
                }
            }
            else if (fromPage is CarouselPage carouselPage)
            {
                PageUtilities.OnNavigatedFrom(carouselPage.CurrentPage, parameters);
            }
        }
    }
}