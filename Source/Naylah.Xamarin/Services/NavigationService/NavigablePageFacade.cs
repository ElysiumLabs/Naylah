using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Services.NavigationService
{
    public class MasterDetailPageFacade : INavigablePageFacade
    {
        public Page InternalPage { get { return (Page)MasterDetailPageInternal; } }

        public INavigation InternalNavigationContext
        {
            get
            {
                return MasterDetailPageInternal.Detail.Navigation;
            }
        }

        public MasterDetailPage MasterDetailPageInternal { get; }

        public Page CurrentPage
        {
            get
            {
                return InternalNavigationContext.NavigationStack[InternalNavigationContext.NavigationStack.Count - 1];
            }
        }

        public Page ModalCurrentPage
        {
            get
            {
                return
                    (InternalNavigationContext.ModalStack.Count > 0) ?
                    InternalNavigationContext.ModalStack[InternalNavigationContext.ModalStack.Count() - 1]
                    :
                    null;
            }
        }

        public event EventHandler<ExtNavigationEventArgs> Popped;

        public event EventHandler<ExtNavigationEventArgs> PoppedToRoot;

        public event EventHandler<ExtNavigationEventArgs> Pushed;

        public event EventHandler<ExtNavigationEventArgs> ModalPopped;

        public event EventHandler<ExtNavigationEventArgs> ModalPushed;

        public MasterDetailPageFacade(MasterDetailPage _masterDetailPage)
        {
            MasterDetailPageInternal = _masterDetailPage;

            ((NavigationPage)MasterDetailPageInternal.Detail).Popped += (s, e) =>
            {
                var popedEv = Popped;
                popedEv?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            ((NavigationPage)MasterDetailPageInternal.Detail).PoppedToRoot += (s, e) =>
            {
                var popedEv = PoppedToRoot;
                popedEv?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };
        }

        public async Task ChangeRootAsync(Page page, bool animated = false)
        {
            if (page.Parent != null)
            {
                throw new Exception("Page has already navigation...");
            }

            InternalNavigationContext.InsertPageBefore(page, InternalNavigationContext.NavigationStack[0]);
            await InternalNavigationContext.PopToRootAsync(animated);
        }

        public async Task<Page> PopAsync(bool animated = false)
        {
            var p = await InternalNavigationContext.PopAsync(animated);
            return p;
        }

        public async Task PopToRootAsync(bool animated = false)
        {
            await InternalNavigationContext.PopToRootAsync(animated);
        }

        public async Task PushAsync(Page page, object parameter = null, bool animated = false)
        {
            await InternalNavigationContext.PushAsync(page, animated);

            var pushedEv = Pushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(page, parameter));
        }

        public async Task PushModalAsync(Page page, object parameter = null, bool animated = false)
        {
            await InternalNavigationContext.PushModalAsync(page, animated);

            var pushedEv = ModalPushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(page, parameter));
        }

        public async Task<Page> PopModalAsync(bool animated = false)
        {
            var p = await InternalNavigationContext.PopModalAsync(animated);

            if (p != null)
            {
                var popedEv = ModalPopped;
                popedEv?.Invoke(this, new ExtNavigationEventArgs(p, null));
            }

            return p;
        }

        public async Task Refresh(object parameter = null)
        {
            var pushedEv = Pushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(CurrentPage, parameter));
        }

        public async Task RefreshModal(object parameter = null)
        {
            var pushedEv = ModalPushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(CurrentPage, parameter));
        }
    }

    public class NavigationPageFacade : INavigablePageFacade
    {
        public Page InternalPage { get { return (Page)NavigationPageInternal; } }

        public INavigation InternalNavigationContext
        {
            get
            {
                return NavigationPageInternal.Navigation;
            }
        }

        public NavigationPage NavigationPageInternal { get; }

        public Page CurrentPage
        {
            get
            {
                return NavigationPageInternal.Navigation.NavigationStack[NavigationPageInternal.Navigation.NavigationStack.Count - 1];
            }
        }

        public Page ModalCurrentPage
        {
            get
            {
                return
                    (InternalNavigationContext.ModalStack.Count > 0) ?
                    InternalNavigationContext.ModalStack[InternalNavigationContext.ModalStack.Count() - 1]
                    :
                    null;
            }
        }

        public event EventHandler<ExtNavigationEventArgs> Popped;

        public event EventHandler<ExtNavigationEventArgs> PoppedToRoot;

        public event EventHandler<ExtNavigationEventArgs> Pushed;

        public event EventHandler<ExtNavigationEventArgs> ModalPopped;

        public event EventHandler<ExtNavigationEventArgs> ModalPushed;

        public NavigationPageFacade(NavigationPage _navigationPage)
        {
            NavigationPageInternal = _navigationPage;

            NavigationPageInternal.Popped += (s, e) =>
            {
                var popedEv = Popped;
                popedEv?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            NavigationPageInternal.PoppedToRoot += (s, e) =>
            {
                var popedEv = PoppedToRoot;
                popedEv?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };
        }

        public async Task ChangeRootAsync(Page page, bool animated = false)
        {
            if (page.Parent != null)
            {
                throw new Exception("Page has already navigation...");
            }

            InternalNavigationContext.InsertPageBefore(page, InternalNavigationContext.NavigationStack[0]);
            await InternalNavigationContext.PopToRootAsync(animated);
        }

        public async Task<Page> PopAsync(bool animated = false)
        {
            var p = await InternalNavigationContext.PopAsync(animated);
            return p;
        }

        public async Task PopToRootAsync(bool animated = false)
        {
            await InternalNavigationContext.PopToRootAsync(animated);
        }

        public async Task PushAsync(Page page, object parameter = null, bool animated = false)
        {
            await InternalNavigationContext.PushAsync(page, animated);

            var pushedEv = Pushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(page, parameter));
        }

        public async Task PushModalAsync(Page page, object parameter = null, bool animated = false)
        {
            await InternalNavigationContext.PushModalAsync(page, animated);

            var pushedEv = ModalPushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(page, parameter));
        }

        public async Task<Page> PopModalAsync(bool animated = false)
        {
            var p = await InternalNavigationContext.PopModalAsync(animated);

            if (p != null)
            {
                var popedEv = ModalPopped;
                popedEv?.Invoke(this, new ExtNavigationEventArgs(p, null));
            }

            return p;
        }

        public async Task Refresh(object parameter = null)
        {
            var pushedEv = Pushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(CurrentPage, parameter));
        }

        public async Task RefreshModal(object parameter = null)
        {
            var pushedEv = ModalPushed;
            pushedEv?.Invoke(this, new ExtNavigationEventArgs(CurrentPage, parameter));
        }
    }

    public interface INavigablePageFacade
    {
        Page InternalPage { get; }

        INavigation InternalNavigationContext { get; }

        Page CurrentPage { get; }
        Page ModalCurrentPage { get; }

        event EventHandler<ExtNavigationEventArgs> Popped;

        event EventHandler<ExtNavigationEventArgs> PoppedToRoot;

        event EventHandler<ExtNavigationEventArgs> Pushed;

        event EventHandler<ExtNavigationEventArgs> ModalPopped;

        event EventHandler<ExtNavigationEventArgs> ModalPushed;

        Task ChangeRootAsync(Page page, bool animated = false);

        Task PopToRootAsync(bool animated = false);

        Task<Page> PopAsync(bool animated = false);

        Task PushAsync(Page page, object parameter = null, bool animated = false);

        Task PushModalAsync(Page page, object parameter = null, bool animated = false);

        Task<Page> PopModalAsync(bool animated = false);

        Task Refresh(object parameter = null);

        Task RefreshModal(object parameter = null);
    }

    public static class NavigablePageFacadeExtensions
    {
        public static MasterDetailPageFacade AsMasterDetailPage(this INavigablePageFacade navPageFacade)
        {
            return navPageFacade as MasterDetailPageFacade;
        }

        public static NavigationPageFacade AsNavigationPage(this INavigablePageFacade navPageFacade)
        {
            return navPageFacade as NavigationPageFacade;
        }
    }

    public class ExtNavigationEventArgs : NavigationEventArgs
    {
        public object Parameter { get; private set; }

        public ExtNavigationEventArgs(Page page, object parameter = null) : base(page)
        {
            Parameter = parameter;
        }
    }
}