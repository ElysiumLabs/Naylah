namespace Naylah.App.Navigation
{
    //public class MasterDetailPageFacade : INavigablePageFacade
    //{
    //    public Page InternalPage { get { return (Page)MasterDetailPageInternal; } }

    // public Xamarin.Forms.INavigation InternalNavigationContext { get { return
    // MasterDetailPageInternal.Detail.Navigation; } }

    // public MasterDetailPage MasterDetailPageInternal { get; }

    // public Page CurrentPage { get { return
    // InternalNavigationContext.NavigationStack[InternalNavigationContext.NavigationStack.Count -
    // 1]; } }

    // public Page ModalCurrentPage { get { return (InternalNavigationContext.ModalStack.Count > 0) ?
    // InternalNavigationContext.ModalStack[InternalNavigationContext.ModalStack.Count() - 1] : null;
    // } }

    // public event EventHandler<NyNavigationEventArgs> Popped;

    // public event EventHandler<NyNavigationEventArgs> PoppedToRoot;

    // public event EventHandler<NyNavigationEventArgs> Pushed;

    // public event EventHandler<NyNavigationEventArgs> ModalPopped;

    // public event EventHandler<NyNavigationEventArgs> ModalPushed;

    // public MasterDetailPageFacade(MasterDetailPage _masterDetailPage) { MasterDetailPageInternal = _masterDetailPage;

    // ((NavigationPage)MasterDetailPageInternal.Detail).Popped += (s, e) => { var popedEv = Popped;
    // popedEv?.Invoke(this, new NyNavigationEventArgs(e.Page, null)); };

    // ((NavigationPage)MasterDetailPageInternal.Detail).PoppedToRoot += (s, e) => { var popedEv =
    // PoppedToRoot; popedEv?.Invoke(this, new NyNavigationEventArgs(e.Page, null)); }; }

    // public async Task ChangeRootAsync(Page page, bool animated = false) { if (page.Parent != null)
    // { throw new Exception("Page has already navigation..."); }

    // InternalNavigationContext.InsertPageBefore(page,
    // InternalNavigationContext.NavigationStack[0]); await
    // InternalNavigationContext.PopToRootAsync(animated); }

    // public async Task<Page> PopAsync(bool animated = false) { var p = await
    // InternalNavigationContext.PopAsync(animated); return p; }

    // public async Task PopToRootAsync(bool animated = false) { await
    // InternalNavigationContext.PopToRootAsync(animated); }

    // public async Task PushAsync(Page page, object parameter = null, bool animated = false) { await
    // InternalNavigationContext.PushAsync(page, animated);

    // var pushedEv = Pushed; pushedEv?.Invoke(this, new NyNavigationEventArgs(page, parameter)); }

    // public async Task PushModalAsync(Page page, object parameter = null, bool animated = false) {
    // await InternalNavigationContext.PushModalAsync(page, animated);

    // var pushedEv = ModalPushed; pushedEv?.Invoke(this, new NyNavigationEventArgs(page,
    // parameter)); }

    // public async Task<Page> PopModalAsync(bool animated = false) { var p = await InternalNavigationContext.PopModalAsync(animated);

    // if (p != null) { var popedEv = ModalPopped; popedEv?.Invoke(this, new NyNavigationEventArgs(p,
    // null)); }

    // return p; }

    // public async Task Refresh(object parameter = null) { var pushedEv = Pushed;
    // pushedEv?.Invoke(this, new NyNavigationEventArgs(CurrentPage, parameter)); }

    //    public async Task RefreshModal(object parameter = null)
    //    {
    //        var pushedEv = ModalPushed;
    //        pushedEv?.Invoke(this, new NyNavigationEventArgs(CurrentPage, parameter));
    //    }
    //}
}