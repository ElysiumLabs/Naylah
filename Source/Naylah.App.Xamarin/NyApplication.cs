using Naylah.App.Navigation;
using Naylah.App.UI.Style;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.App
{
    public abstract class NyApplication : Application
    {
        public NyApplication()
        {
            Current = this;

            if (Resources == null)
            {
                Resources = new ResourceDictionary();
            }
        }

        public new static NyApplication Current { get; set; }

        #region Style

        private StyleKit _styleKit;

        public StyleKit StyleKit
        {
            get { return _styleKit; }
            set { _styleKit = value; _styleKit?.Apply(this); }
        }

        #endregion Style

        #region Navigation

        public new Page MainPage
        {
            get { return base.MainPage; }
            /*internal*/
            set { base.MainPage = value; }
        }

        public FormsNavigationService NavigationService { get; internal set; }

        /// <summary>
        /// This Function is assigned to when the hardware button needs to be handled in code by the
        /// Views or ViewModels. It should mirror what the back/cancel button does in iOS.
        /// </summary>
        /// <returns>
        /// Three state (nullable) boolean:
        /// - true =&gt; Complete navigation as normal
        /// - false =&gt; Do not navigate
        /// - null =&gt; Exit application (Used on screens that limit access to the app)
        /// </returns>
        public static Func<Task<bool?>> HardwareBackPressed
        {
            private get;
            set;
        }

        /// <summary>
        /// This function is used at the platform level when a hardware back button is pressed. On
        /// occasion we want to prevent/confirm backwards navigation or have the views/viewmodels
        /// perform an action on exit. Since iOS handles this without a hardware button, iOS will not
        /// use this method, but Android and WinPhone may both have hardware buttons that need to be handled.
        /// </summary>
        /// <returns>
        /// Three state (nullable) boolean:
        /// - true =&gt; Complete navigation as normal
        /// - false =&gt; Do not navigate
        /// - null =&gt; Exit application (Used primarily on SignInScreen)
        /// </returns>
        public static async Task<bool?> CallHardwareBackPressed()
        {
            Func<Task<bool?>> backPressed = HardwareBackPressed;
            if (backPressed != null)
            {
                return await backPressed();
            }

            return true;
        }

        public virtual void NavigationServiceFactory(Page shellPage)
        {
            FormsNavigationService.Create(shellPage, this);
        }

        #endregion Navigation
    }
}