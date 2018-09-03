using Naylah.App;
using Naylah.XamarinPlayground.UI.Styles;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Naylah.XamarinPlayground
{
    public partial class App : NyApplicationOld
    {
        public App()
        {
            InitializeComponent();

            StyleKit = new Style1();

            //var a = new SimpleContainer();
            //a.Register<App>(this);

            //Naylah.App.IoC.DependencyResolver.SetResolver(a.GetResolver());

            //var q = Naylah.App.IoC.DependencyResolver.Resolve<App>();

            NavigationServiceFactory(new NavigationPage(new MainPage()));
        }

        //public override object GetIntialViewPage()
        //{
        //    return new SplashPage();
        //}

        public override async Task LoadAppAsync()
        {
        }
    }
}