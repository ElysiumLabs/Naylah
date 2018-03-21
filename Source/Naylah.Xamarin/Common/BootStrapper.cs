using Naylah.Xamarin.Controls.Style;
using Naylah.Xamarin.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Common
{
    public class BootStrapper : Application
    {
        
        internal static BootStrapper CurrentApp;

        private StyleKitBase _styleKit;
        public StyleKitBase StyleKit
        {
            get { return _styleKit; }
            set { _styleKit = value; _styleKit?.Apply(); }
        }

        public Action PlataformSpecifyInitializationExtension;

        public bool Started { get; private set; }
        public bool Initialized { get; private set; }

        public BootStrapper()
        {
            if (Resources == null)
            {
                Resources = new ResourceDictionary();
            }
            
            CurrentApp = this;
            
        }

        public new Page MainPage
        {
            get { return base.MainPage; }
            internal set { base.MainPage = value;  }
        }


        public virtual async Task InitializeApp()
        {
            try
            {
                PlataformSpecifyInitializationExtension?.Invoke();
                Initialized = true;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Started = true;
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        public virtual async Task LoadApp()
        {
        }

        public INavigationService NavigationService { get; protected set; }



        #region dependency injection

        /// <summary>
        /// If a developer overrides this method, the developer can resolve DataContext or unwrap DataContext 
        /// available for the Page object when using a MVVM pattern that relies on a wrapped/porxy around ViewModels
        /// </summary>
        public virtual INavigable ResolveForPage(Page page, INavigationService navigationService) => null;

        #endregion


        /// <summary>
        /// This Function is assigned to when the hardware button needs to be handled in code
        /// by the Views or ViewModels. It should mirror what the back/cancel button does in
        /// iOS.
        /// </summary>
        /// <returns>
        /// Three state (nullable) boolean:
        ///  - true  => Complete navigation as normal
        ///  - false => Do not navigate
        ///  - null  => Exit application (Used on screens that limit access to the app)
        /// </returns>
        public static Func<Task<bool?>> HardwareBackPressed
        {
            private get;
            set;
        }

        /// <summary>
        /// This function is used at the platform level when a hardware back button is pressed.
        /// On occasion we want to prevent/confirm backwards navigation or have the views/viewmodels
        /// perform an action on exit. Since iOS handles this without a hardware button, iOS will not
        /// use this method, but Android and WinPhone may both have hardware buttons that need to be
        /// handled.
        /// </summary>
        /// <returns>
        /// Three state (nullable) boolean:
        ///  - true  => Complete navigation as normal
        ///  - false => Do not navigate
        ///  - null  => Exit application (Used primarily on SignInScreen)
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

        public virtual INavigationService NavigationServiceFactory(MasterDetailPage page)
        {
            var navigationService = CreateNavigationService(page);

            if (NavigationService != null)
            {
                NavigationService.Dispose();
            }

            NavigationService = navigationService;

            try // try invoke rootFacadePage viewModel stuffs
            {
                navigationService.NavigatedToAsync(NavigationMode.New, null, navigationService.NavigablePageFacadeInternalPage).ConfigureAwait(false); }
            catch
            {
            }

            return navigationService;
        }

        public virtual INavigationService NavigationServiceFactory(NavigationPage page)
        {
            var navigationService = CreateNavigationService(page);

            if (NavigationService != null)
            {
                NavigationService.Dispose();
            }

            NavigationService = navigationService;

            try // try invoke rootFacadePage viewModel stuffs
            {
                navigationService.NavigatedToAsync(NavigationMode.New, null, navigationService.NavigablePageFacadeInternalPage).ConfigureAwait(false); }
            catch
            {
            }

            return navigationService;
        }

        protected virtual NavigationService CreateNavigationService(Page page)
        {
            return new NavigationService(this, page);
        }


    }
}
