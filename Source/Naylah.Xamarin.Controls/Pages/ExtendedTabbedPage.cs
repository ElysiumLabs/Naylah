using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Pages
{
    public delegate void CurrentPageChangingEventHandler();

    public delegate void CurrentPageChangedEventHandler();

    public class ExtendedTabbedPage : TabbedPage
    {
        public event CurrentPageChangingEventHandler CurrentPageChanging;

        public new event CurrentPageChangedEventHandler CurrentPageChanged;

        public ExtendedTabbedPage()
        {
            this.PropertyChanging += this.OnPropertyChanging;
            this.PropertyChanged += this.OnPropertyChanged;
        }

        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                this.RaiseCurrentPageChanging();
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                this.RaiseCurrentPageChanged();
            }
        }

        private void RaiseCurrentPageChanging()
        {
            var handler = this.CurrentPageChanging;
            if (handler != null)
            {
                handler();
            }
        }

        private void RaiseCurrentPageChanged()
        {
            var handler = this.CurrentPageChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}