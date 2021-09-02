using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Naylah.Rest
{
    public class NaylahRestClientSettings
    {
        public string AppName { get; set; }

        public string AppVersion { get; set; }

        public Uri BaseUri { get; set; }

        public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(30);

        public string DefaultContentType { get; set; } = MediaTypeNames2.Application.Json;

        public Func<HttpClient> HttpClientFactory { get; set; }

        public Dictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>();

        public ILogger Logger { get; set; }

        public NaylahRestClientSettings()
        {
            var ass = Assembly.GetExecutingAssembly();
            AppName = ass.GetName().Name;
            AppVersion = ass.GetName().Version.Major.ToString();
        }
    }
}
