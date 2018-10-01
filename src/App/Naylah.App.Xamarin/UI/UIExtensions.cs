using System;
using Xamarin.Forms;

namespace Naylah.App.UI
{
    public static class UIExtensions
    {
    }

    public static class ListViewExtensions
    {
        public static void AsNoSelectable<T, U>(this ListView listView, Action<T, U> itemSelected = null)
            where T : class
            where U : class
        {
            listView.ItemTapped += (s, e) =>
            {
                if (e == null) return;

                itemSelected?.Invoke(e.Item as T, e.Group as U);

                ((ListView)s).SelectedItem = null;
            };
        }

        public static void AsNoSelectable(this ListView listView, Action<object, object> itemSelected = null)
        {
            listView.AsNoSelectable<object, object>(itemSelected);
        }
    }
}