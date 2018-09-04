using Naylah.App.UI.Style;
using Prism;
using Prism.Autofac;
using Prism.Ioc;

namespace Naylah.App
{
    public abstract class NyApplication : PrismApplication
    {
        private StyleKit _styleKit;

        protected NyApplication(IPlatformInitializer platformInitializer = null) : base(platformInitializer)
        {
        }

        public StyleKit StyleKit
        {
            get { return _styleKit; }
            set { _styleKit = value; _styleKit?.Apply(this); }
        }

        protected override void OnInitialized()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}