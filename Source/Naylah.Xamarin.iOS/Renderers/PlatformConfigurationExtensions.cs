#if __MOBILE__

using Xamarin.Forms;
using CurrentPlatform = Xamarin.Forms.PlatformConfiguration.iOS;

namespace Naylah.Xamarin.iOS.Renderers
#else
using CurrentPlatform = Xamarin.Forms.PlatformConfiguration.macOS;

namespace Naylah.Xamarin.iOS.Renderers
#endif
{
    public static class PlatformConfigurationExtensions
    {
        public static IPlatformElementConfiguration<CurrentPlatform, T> OnThisPlatform<T>(this T element)
            where T : Element, IElementConfiguration<T>
        {
            return (element).On<CurrentPlatform>();
        }
    }
}