using System;
using System.Reflection;

namespace Naylah.App
{
    public static class NyApplicationBuilderExtensions
    {
        /// <summary>
        /// This method is equivalent to LoadApplication in MainActivity, dont use both
        /// </summary>
        /// <param name="application"></param>
        /// <param name="formsAppCompatActivity"></param>
        public static void Run(this NyApplicationOld application, global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate formsApplicationDelegate)
        {
            MethodInfo m = formsApplicationDelegate.GetType().GetMethod("LoadApplication", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(NyApplicationOld) }, null);
            m.Invoke(formsApplicationDelegate, new object[] { application });
        }
    }
}