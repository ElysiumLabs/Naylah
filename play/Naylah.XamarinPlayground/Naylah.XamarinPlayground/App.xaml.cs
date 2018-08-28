using Naylah.App;
using Naylah.XamarinPlayground.UI.Styles;
using Naylah.XamarinPlayground.Views;
using System.Threading.Tasks;
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

            //var a = new SimpleContainer();
            //a.Register<App>(this);

            //Naylah.App.IoC.DependencyResolver.SetResolver(a.GetResolver());

            //var q = Naylah.App.IoC.DependencyResolver.Resolve<App>();

            NavigationServiceFactory(new NavigationPage(new Page1()));
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