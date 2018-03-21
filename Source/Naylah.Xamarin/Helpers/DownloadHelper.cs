using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Xamarin.Helpers
{
    public class DownloadHelper
    {

        public static async Task<string> DownloadAsString(Uri uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri.ToString());

            return await response.Content.ReadAsStringAsync();
        }


        public static async Task<Stream> DownloadAsStream(Uri uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri.ToString());

            return await response.Content.ReadAsStreamAsync();
        }


        public static async Task<byte[]> DownloadAsByteArray(Uri uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri.ToString());

            return await response.Content.ReadAsByteArrayAsync();
        }

    }
}
