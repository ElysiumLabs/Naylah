using Naylah.App.Common;
using Naylah.App.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.App.Navigation
{
    public partial class FormsNavigationService :
        Naylah.App.Navigation.INavigation,
        Naylah.App.Navigation.INavigationBackward,
        IDisposable

    {
        private NyApplicationOld App;

        public static FormsNavigationService Create(object shellView, NyApplicationOld app = null)
        {
            return new FormsNavigationService(shellView, app);
        }

        public FormsNavigationService(object shellView) : this(shellView, null)
        {
        }

        public FormsNavigationService(object shellView, NyApplicationOld app)
        {
            InitializeShell(shellView, app);
            HookNavigationEvents();
        }

        private void InitializeShell(object shellView, NyApplicationOld app)
        {
            NavigablePageFacade = null;

            var shell = PageFromViewResolver(shellView);

            NavigablePageFacade = new NavigationPageFacade(shell);

            if (app != null)
            {
                AttachToApplication(app);
            }

            OnNavigatedTo(NavigablePageFacade.ShellPage, null, NavigationMode.New);
        }

        private void AttachToApplication(NyApplicationOld app)
        {
            App = app;

            if (App.NavigationService != null) { App.NavigationService.Dispose(); }

            App.NavigationService = this;
            App.MainPage = NavigablePageFacade.ShellPage;
            NyApplicationOld.HardwareBackPressed = HandleHardwareBackPressed;
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

        public INavigablePageFacade NavigablePageFacade { get; private set; }

        public Page NavigablePageFacadeInternalPage => NavigablePageFacade.ShellPage;

        //

        public object CurrentView { get { return NavigablePageFacade.CurrentPage; } }

        public bool CanGoBack
        {
            get
            {
                return NavigablePageFacade.InternalNavigationContext.NavigationStack.Count > 1;
            }
        }

        public event EventHandler<DataEventArgs<NavigatedEventArgs>> Navigated;

        public event EventHandler<DataEventArgs<NavigatingEventArgs>> Navigating;

        public void RaiseNavigatedEvent()
        {
            var navigated = Navigated;
            navigated?.Invoke(this, new DataEventArgs<NavigatedEventArgs>(new NavigatedEventArgs()));
        }

        public void RaiseNavigatingEvent()
        {
            var navigating = Navigating;
            navigating?.Invoke(this, new DataEventArgs<NavigatingEventArgs>(new NavigatingEventArgs()));
        }

        public async Task ClearHistoryAsync(INavigationOptions options = null)
        {
            if (options?.Animated == true)
            {
                await NavigablePageFacade.PopToRootAsync(true);
            }
            else
            {
                var pagesToRemove = NavigablePageFacade.InternalNavigationContext.NavigationStack.Where(
                    x =>
                    x != NavigablePageFacade.InternalNavigationContext.NavigationStack[0]
                    )
                    .ToList();

                pagesToRemove.ForEach(x =>
                {
                    NavigablePageFacade.InternalNavigationContext.RemovePage(x);
                });
            }
        }

        private void HookNavigationEvents()
        {
            NavigablePageFacade.Pushed += (s, e) =>
            {
                OnNavigatedTo(e.Page, e.Parameter, NavigationMode.New);
                RaiseNavigatedEvent();
            };

            NavigablePageFacade.Popped += (s, e) =>
            {
                OnNavigatedFrom(e.Page, e.Parameter);
                OnNavigatedTo(NavigablePageFacade.CurrentPage, e.Parameter, NavigationMode.Back);
                RaiseNavigatedEvent();
            };

            NavigablePageFacade.PushRequested += (s, e) =>
            {
                RaiseNavigatingEvent();
            };

            NavigablePageFacade.PopRequested += (s, e) =>
            {
                RaiseNavigatingEvent();
            };

            //

            NavigablePageFacade.ModalPushed += (s, e) =>
            {
                OnNavigatedTo(e.Page, e.Parameter, NavigationMode.New);
                RaiseNavigatedEvent();
            };
            NavigablePageFacade.ModalPopped += (s, e) =>
            {
                OnNavigatedFrom(e.Page, e.Parameter);
                OnNavigatedTo(NavigablePageFacade.ModalCurrentPage, e.Parameter, NavigationMode.Back);
                RaiseNavigatedEvent();
            };
        }

        public async Task<INavigationResult> GoBackAsync(INavigationOptions options = null)
        {
            var r = new NavigationResult();

            try
            {
                await NavigablePageFacade.PopAsync(options?.Animated == true);

                r.Success = true;
            }
            catch (Exception ex)
            {
                r.Exception = ex;
                r.Success = false;
            }

            return r;
        }

        public async Task<INavigationResult> NavigateAsync(object view, object parameter = null, INavigationOptions options = null)
        {
            var r = new NavigationResult();

            try
            {
                var page = PageFromViewResolver(view);

                if (page == null)
                    throw new ArgumentNullException(nameof(page));

                OnNavigatingTo(page, parameter, NavigationMode.New);

                OnNavigatedFrom(CurrentView as Page, parameter);

                await NavigablePageFacade.PushAsync(page, parameter, options?.Animated == true);

                r.Success = true;
            }
            catch (Exception ex)
            {
                r.Exception = ex;
                r.Success = false;
            }

            return r;
        }

        public async Task<INavigationResult> NavigateSetRootAsync(object view, object parameter = null, INavigationOptions options = null)
        {
            var r = new NavigationResult();

            try
            {
                var page = PageFromViewResolver(view);

                if (page == null)
                    throw new ArgumentNullException(nameof(page));

                OnNavigatingTo(page, parameter, NavigationMode.New);

                OnNavigatedFrom(CurrentView as Page, parameter);

                await NavigablePageFacade.ChangeRootAsync(page, options?.Animated == true);

                OnNavigatedTo(page, parameter, NavigationMode.New);

                r.Success = true;
            }
            catch (Exception ex)
            {
                r.Exception = ex;
                r.Success = false;
            }

            return r;
        }

        //

        protected static void OnNavigatingTo(Page toPage, object parameters, NavigationMode navigationMode = NavigationMode.New)
        {
            PageUtilities.OnNavigatingTo(toPage, parameters, navigationMode);

            if (toPage is TabbedPage tabbedPage)
            {
                OnNavigatingTo(tabbedPage.CurrentPage, parameters, navigationMode);
            }
            else if (toPage is NavigationPage navigationPage)
            {
                OnNavigatingTo(navigationPage.CurrentPage, parameters, navigationMode);
            }
            else if (toPage is MasterDetailPage masterDetailPage)
            {
                OnNavigatingTo(masterDetailPage.Master, parameters, navigationMode);
                OnNavigatingTo(masterDetailPage.Detail, parameters, navigationMode);
            }
            else if (toPage is CarouselPage carouselPage)
            {
                OnNavigatingTo(carouselPage.CurrentPage, parameters, navigationMode);
            }
        }

        protected static void OnNavigatedTo(Page toPage, object parameters, NavigationMode navigationMode = NavigationMode.New)
        {
            PageUtilities.OnNavigatedTo(toPage, parameters, navigationMode);

            if (toPage is TabbedPage tabbedPage)
            {
                OnNavigatedTo(tabbedPage.CurrentPage, parameters, navigationMode);
            }
            else if (toPage is NavigationPage navigationPage)
            {
                OnNavigatedTo(navigationPage.CurrentPage, parameters, navigationMode);
            }
            else if (toPage is MasterDetailPage masterDetailPage)
            {
                OnNavigatedTo(masterDetailPage.Master, parameters, navigationMode);
                OnNavigatedTo(masterDetailPage.Detail, parameters, navigationMode);
            }
            else if (toPage is CarouselPage carouselPage)
            {
                OnNavigatedTo(carouselPage.CurrentPage, parameters, navigationMode);
            }
        }

        protected static void OnNavigatedFrom(Page fromPage, object parameters)
        {
            PageUtilities.OnNavigatedFrom(fromPage, parameters);

            if (fromPage is TabbedPage tabbedPage)
            {
                OnNavigatedFrom(tabbedPage.CurrentPage, parameters);
            }
            else if (fromPage is NavigationPage navigationPage)
            {
                OnNavigatedFrom(navigationPage.CurrentPage, parameters);
            }
            else if (fromPage is MasterDetailPage masterDetailPage)
            {
                OnNavigatedFrom(masterDetailPage.Master, parameters);
                OnNavigatedFrom(masterDetailPage.Detail, parameters);
            }
            else if (fromPage is CarouselPage carouselPage)
            {
                OnNavigatedFrom(carouselPage.CurrentPage, parameters);
            }
        }

        public Func<object, Page> PageFromViewResolver { get; set; } = PageFromViewResolverDefault;

        public static Page PageFromViewResolverDefault(object view)
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

        public void Dispose()
        {
        }
    }
}