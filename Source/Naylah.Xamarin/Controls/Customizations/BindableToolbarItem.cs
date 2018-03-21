using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public class BindableToolbarItem : ToolbarItem
    {
        public BindableToolbarItem() : base()
        {
            InitVisibility();
        }

        private async void InitVisibility()
        {
            await Task.Delay(100);
            OnIsVisibleChanged(this, false, IsVisible);
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(BindableToolbarItem), false, propertyChanged: OnIsVisibleChanged);

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = bindable as BindableToolbarItem;

            if (item.Parent == null)
                return;

            var parent = item.Parent as ContentPage;
            var items = parent.ToolbarItems;

            if ((bool)newValue && !items.Contains(item))
            {
                items.Add(item);
            }
            else if (!(bool)newValue && items.Contains(item))
            {
                items.Remove(item);
            }
        }
    }
}
