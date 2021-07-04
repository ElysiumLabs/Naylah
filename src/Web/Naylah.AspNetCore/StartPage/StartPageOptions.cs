using System;

namespace Naylah.StartPage
{
    public class StartPageOptions
    {
        public string Title { get; set; } = "Service";

        public string Message { get; set; } = "This service is up and running ;)";

        public string Organization { get; set; } = "";

        public TimeSpan HealtyCheckCacheDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}