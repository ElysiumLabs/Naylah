using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.App.Navigation
{
    public class NavigationPageFacade : INavigablePageFacade
    {
        public Page InternalPage { get { return (Page)NavigationPageInternal; } }

        public Xamarin.Forms.INavigation InternalNavigationContext
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
}