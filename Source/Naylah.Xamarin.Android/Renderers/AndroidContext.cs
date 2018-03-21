using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Android.Content;

namespace Naylah.Xamarin.Android.Renderers
{
    public class AndroidContext
    {
        public static Context GetContext()
        {
            return Forms.Context;
        }
    }
}