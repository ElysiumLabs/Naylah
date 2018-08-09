using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.App.Navigation
{
    public class NavigationPageFacade : INavigablePageFacade
    {
        public Page ShellPage { get; internal set; }

        public Xamarin.Forms.INavigation InternalNavigationContext
        {
            get
            {
                return NavigationPageInternal.Navigation;
            }
        }

        public NavigationPage NavigationPageInternal { get; internal set; }

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

        public event EventHandler<ExtNavigationEventArgs> PopRequested;

        public event EventHandler<ExtNavigationEventArgs> PopToRootRequested;

        public event EventHandler<ExtNavigationEventArgs> PushRequested;

        public event EventHandler<ExtNavigationEventArgs> InsertPageBeforeRequested;

        public event EventHandler<ExtNavigationEventArgs> RemovePageRequested;

        //

        public event EventHandler<ExtNavigationEventArgs> ModalPopped;

        public event EventHandler<ExtNavigationEventArgs> ModalPushed;

        public NavigationPageFacade(Page shellPage)
        {
            PrepareNavigationPage(shellPage);

            NavigationPageInternal.Popped += (s, e) =>
            {
                var ev = Popped;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page));
            };

            NavigationPageInternal.PoppedToRoot += (s, e) =>
            {
                var ev = PoppedToRoot;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            NavigationPageInternal.PopRequested += (s, e) =>
            {
                var ev = PopRequested;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            NavigationPageInternal.PopToRootRequested += (s, e) =>
            {
                var ev = PopToRootRequested;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            NavigationPageInternal.PushRequested += (s, e) =>
            {
                var ev = PushRequested;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            NavigationPageInternal.InsertPageBeforeRequested += (s, e) =>
            {
                var ev = InsertPageBeforeRequested;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };

            NavigationPageInternal.RemovePageRequested += (s, e) =>
            {
                var ev = RemovePageRequested;
                ev?.Invoke(this, new ExtNavigationEventArgs(e.Page, null));
            };
        }

        private void PrepareNavigationPage(Page shellPage)
        {
            NavigationPage navPage = null;

            if (shellPage is MasterDetailPage masterDetailPage)
            {
                if (masterDetailPage.Detail is NavigationPage navigationPage)
                {
                    navPage = navigationPage;
                    ShellPage = shellPage;
                }
            }
            else if (shellPage is NavigationPage navigationPage)
            {
                navPage = navigationPage;
                ShellPage = shellPage;
            }

            if (navPage == null)
            {
                navPage = new NavigationPage(shellPage);
                ShellPage = navPage;
            }

            NavigationPageInternal = navPage;
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
            if (animated)
            {
                await InternalNavigationContext.PopToRootAsync(animated);
            }
            else
            {
                var pagesToRemove = InternalNavigationContext.NavigationStack.Skip(1).ToList();

                pagesToRemove.ForEach(x =>
                {
                    InternalNavigationContext.RemovePage(x);
                });
            }
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
    }
}