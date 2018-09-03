using Naylah.App;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Naylah.XamarinPlayground
{
    public partial class App : NyApplication
    {
        public App()
        {
            InitializeComponent();
        }
    }
}