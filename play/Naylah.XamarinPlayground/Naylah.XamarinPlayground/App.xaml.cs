using Naylah.App;
using Naylah.App.IoC;
using Naylah.XamarinPlayground.UI.Styles;
using System.Threading.Tasks;
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

            var a = new SimpleContainer();
            a.Register<App>(this);

            Resolver.SetResolver(a.GetResolver());

            var q = Resolver.Resolve<App>();
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