using Naylah.App;
using Naylah.XamarinPlayground.Views;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using System.Diagnostics;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Naylah.XamarinPlayground
{
    public partial class App : NyApplication
    {
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeComponent();

            NavigationService.NavigateAsync(nameof(Page1));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterSingleton<ILoggerFacade, Log>();

            containerRegistry.RegisterForNavigation<Page1>();
        }
    }

    public class Log : ILoggerFacade
    {
        void ILoggerFacade.Log(string message, Category category, Priority priority)
        {
            Debug.Write(message);
            //UserDialogs.Instance.Alert(message);
        }
    }
}