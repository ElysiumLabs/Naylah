using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.App.Navigation
{
    public static class NavigablePageFacadeExtensions
    {
        //public static MasterDetailPageFacade AsMasterDetailPage(this INavigablePageFacade navPageFacade)
        //{
        //    return navPageFacade as MasterDetailPageFacade;
        //}

        public static NavigationPageFacade AsNavigationPage(this INavigablePageFacade navPageFacade)
        {
            return navPageFacade as NavigationPageFacade;
        }
    }

    public interface INavigablePageFacade
    {
        Page ShellPage { get; }

        Xamarin.Forms.INavigation InternalNavigationContext { get; }

        Page CurrentPage { get; }

        Page ModalCurrentPage { get; }

        event EventHandler<ExtNavigationEventArgs> Popped;

        event EventHandler<ExtNavigationEventArgs> PoppedToRoot;

        event EventHandler<ExtNavigationEventArgs> Pushed;

        event EventHandler<ExtNavigationEventArgs> PopRequested;

        event EventHandler<ExtNavigationEventArgs> PopToRootRequested;

        event EventHandler<ExtNavigationEventArgs> PushRequested;

        event EventHandler<ExtNavigationEventArgs> InsertPageBeforeRequested;

        event EventHandler<ExtNavigationEventArgs> RemovePageRequested;

        //

        event EventHandler<ExtNavigationEventArgs> ModalPopped;

        event EventHandler<ExtNavigationEventArgs> ModalPushed;

        Task ChangeRootAsync(Page page, bool animated = false);

        Task PopToRootAsync(bool animated = false);

        Task<Page> PopAsync(bool animated = false);

        Task PushAsync(Page page, object parameter = null, bool animated = false);

        Task PushModalAsync(Page page, object parameter = null, bool animated = false);

        Task<Page> PopModalAsync(bool animated = false);
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