using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Pages
{
    public class MasterDetailNavigationPage : MasterDetailPage
    {
        public MasterDetailNavigationPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Detail = new NavigationPage(new ContentPage());
        }
    }
}