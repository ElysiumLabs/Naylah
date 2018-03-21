using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Helpers
{
    public class DeviceHelper
    {
        public static string SetImageResource(string image)
        {
            return Device.OS == TargetPlatform.Windows ? "Assets/" + image + ".png" : image;
        }
    }
}