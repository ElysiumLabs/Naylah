using Naylah.App.UI.Style;
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
    }
}