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
        public static void Run(this NyApplication application, global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity formsAppCompatActivity)
        {
            MethodInfo m = formsAppCompatActivity.GetType().GetMethod("LoadApplication", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(NyApplication) }, null);
            m.Invoke(formsAppCompatActivity, new object[] { application });
        }
    }
}