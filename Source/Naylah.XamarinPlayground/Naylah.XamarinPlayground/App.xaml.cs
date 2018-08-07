using Naylah.App;
using Naylah.XamarinPlayground.UI.Styles;
using Naylah.XamarinPlayground.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Naylah.XamarinPlayground
{
    public partial class App : NyApplication
    {
        public App()
        {
            InitializeComponent();

            StyleKit = new Style1();

            var v2 = new MasterDetailPage()
            {
                Detail = new NavigationPage(new Page1()) { },
                Master = new ContentPage() { Title = "blablah", BackgroundColor = Color.Green, Icon = "Menu.png" },
                Title = "qwdqwd"
            };

            //NavigationServiceFactory(v2);

            //var navService2 = Naylah.App.Navigation.FormsNavigationService.Create(v2.Detail, null);

            //navService2.NavigateAsync(new ContentPage() { BackgroundColor = Color.Black }).Wait();

            //var shellPage = new TabbedPage()
            //{
            //    Children =
            //    {
            //        new NavigationPage(new Page1()),
            //        new NavigationPage(new ContentPage(){BackgroundColor = Color.Yellow}),
            //        new NavigationPage(new ContentPage(){BackgroundColor = Color.Purple}),
            //    }
            //};

            NavigationServiceFactory(new Page1());

            //var nav1 = FormsNavigationService.Create(((TabbedPage)MainPage).Children[0]);
            //var nav2 = FormsNavigationService.Create(((TabbedPage)MainPage).Children[1]);
            //var nav3 = FormsNavigationService.Create(((TabbedPage)MainPage).Children[2]);

            //nav1.NavigateAsync(new ContentPage() { BackgroundColor = Color.Fuchsia }).Wait();
            //nav2.NavigateAsync(new ContentPage() { BackgroundColor = Color.Green }).Wait();

            //((TabbedPage)MainPage).Children[0].Navigation.PushModalAsync(new ContentPage() { BackgroundColor = Color.Blue });
            //((TabbedPage)MainPage).Children[1].Navigation.PushModalAsync(new ContentPage() { BackgroundColor = Color.Aquamarine });

            //var b = ((TabbedPage)MainPage).Children[0].Navigation == ((TabbedPage)MainPage).Children[1].Navigation;
            //MainPage.Navigation.PushAsync(new ContentPage() { BackgroundColor = Color.Purple });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}