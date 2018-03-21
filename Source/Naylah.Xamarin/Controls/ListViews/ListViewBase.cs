using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.ListViews
{
    public class ListViewBase : ListView
    {
        public ListViewBase(ListViewCachingStrategy cachingStrategy = ListViewCachingStrategy.RecycleElement) : base(cachingStrategy)
        {
        }
    }
}
