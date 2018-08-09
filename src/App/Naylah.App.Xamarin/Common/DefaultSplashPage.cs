using Xamarin.Forms;

namespace Naylah.App.Common
{
    public class DefaultSplashPage : ContentPage
    {
        public DefaultSplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            BackgroundColor = Color.Accent;

            //// ...
            //// NOTE: use for debugging, not in released app code!
            //var assembly = typeof(DefaultSplashPage).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //}

            //var embeddedImage = new Image { Source = ImageSource.FromResource("Naylah.App.Resources.bg_default.png", typeof(DefaultSplashPage).GetTypeInfo().Assembly) };

            //Content = embeddedImage;
            //BackgroundImage = embeddedImage.Source.ToString();
        }
    }
}