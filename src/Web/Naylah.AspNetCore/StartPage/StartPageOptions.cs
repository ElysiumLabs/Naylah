using Microsoft.AspNetCore.Http;
using System;

namespace Naylah.StartPage
{
    public class StartPageOptions
    {
        public string Title { get; set; } = "Service";

        public string Message { get; set; } = "This service is up and running ;)";

        public string Organization { get; set; } = "";

        public string Version { get; set; } = "1";

        public PathString Path { get; set; } = "/";

        public bool HealthCheckEnabled { get; set; } = true;

        public string[] HealthCheckTags { get; set; }

        public TimeSpan HealtyCheckCacheDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}