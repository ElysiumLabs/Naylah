using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.ListViews
{
    public class InfiniteListView : ListViewBase
    {
        public static readonly BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create(
                propertyName: "LoadMoreCommand",
                returnType: typeof(ICommand),
                defaultValue: null,
                declaringType: typeof(InfiniteListView),
                defaultBindingMode: BindingMode.Default
            );

        private bool isLoading;

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public InfiniteListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (isLoading) { return; }

                isLoading = true;

                var items = ItemsSource as IList;

                if (items == null) { return; }

                var totalItems = items.Count;
                var percentage = Math.Round(0.9 * totalItems, 0);

                if (items.IndexOf(e.Item) < percentage)
                {
                    return;
                }

                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                {
                    LoadMoreCommand.Execute(null);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
