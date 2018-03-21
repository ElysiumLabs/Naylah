using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Helpers
{
    public class LocationHelper
    {
        public static void SearchRouteFromMyPosition(string address, string latitude, string longitude)
        {
            try
            {
                var lat = latitude.Replace(",", ".");
                var lng = longitude.Replace(",", ".");

                if (!string.IsNullOrEmpty(address))
                {
                    switch (Device.OS)
                    {
                        case TargetPlatform.iOS:
                            Device.OpenUri(new Uri(string.Format("http://maps.apple.com/?saddr="
                                + WebUtility.UrlEncode(lat)
                                + ","
                                + WebUtility.UrlEncode(lng)
                                + "&daddr=" + address)));
                            break;

                        case TargetPlatform.Android:

                            Device.OpenUri(new Uri(string.Format("https://maps.google.com/?saddr="
                                + WebUtility.UrlEncode(lat)
                                + ","
                                + WebUtility.UrlEncode(lng)
                                + "&daddr=" + address)));
                            break;

                        case TargetPlatform.Windows:
                            Device.OpenUri(new Uri(string.Format("https://www.bing.com/mapspreview?rtp=pos.50_2~pos.48_5~pos."
                                + WebUtility.UrlEncode(lat)
                                + "_"
                                + WebUtility.UrlEncode(lng)
                                + "_Rtp=adr." + address)));
                            break;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void SearchRoute(string origin, string destination)
        {
            try
            {
                if (!string.IsNullOrEmpty(origin) && !string.IsNullOrEmpty(destination))
                {
                    switch (Device.OS)
                    {
                        case TargetPlatform.iOS:
                            Device.OpenUri(new Uri(string.Format("http://maps.apple.com/?saddr="
                                + WebUtility.UrlEncode(origin)
                                + ","
                                + "&daddr=" + destination)));
                            break;

                        case TargetPlatform.Android:

                            Device.OpenUri(new Uri(string.Format("https://maps.google.com/?saddr="
                                + WebUtility.UrlEncode(origin)
                                + ","
                                + "&daddr=" + destination)));
                            break;

                        case TargetPlatform.Windows:
                            Device.OpenUri(new Uri(string.Format("https://www.bing.com/mapspreview?rtp=pos.50_2~pos.48_5~pos."
                                + WebUtility.UrlEncode(origin)
                                + "_"
                                + "_Rtp=adr." + destination)));
                            break;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void SearchAddress(string address)
        {
            try
            {
                if (!string.IsNullOrEmpty(address))
                {
                    switch (Device.OS)
                    {
                        case TargetPlatform.iOS:
                            Device.OpenUri(new Uri(string.Format("http://maps.apple.com/?q={0}", address)));
                            break;

                        case TargetPlatform.Android:
                            Device.OpenUri(new Uri(string.Format("geo:0,0?q={0}", address)));
                            break;

                        case TargetPlatform.Windows:
                            Device.OpenUri(new Uri(string.Format("bingmaps:?where={0}", address)));
                            break;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}