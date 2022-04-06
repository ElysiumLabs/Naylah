using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Naylah.HealthChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.StartPage
{
    public class StartPage
    {
        public StartPage()
        {

        }

        protected virtual async Task<string> GetPageStringAsync()
        {
            var assembly = typeof(StartPage).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("Naylah.Assets.index.html");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public virtual async Task ExecuteAsync(HttpContext context, StartPageOptions _options, HealthCheckService healthyCheck, CachedHealthCheckService cachedHealthCheckService)
        {
            var s = await GetPageStringAsync();

            s = s.Replace("{{ServiceName}}", _options.Title);

            if (_options.HealthCheckEnable && (healthyCheck != null))
            {
                var servicesString = "";

                var r = await cachedHealthCheckService.GetOrCheckHealthAsync(healthyCheck, context.RequestAborted);

                if (r != null)
                {
                    s = s.Replace("{{Message}}",  GetHealthStatusEmojiCode(r.Report.Status)+ "  " + r.Report.Status + " - " + GetHealthDateTimeString(r.Date));

                    servicesString = "<table style='width: 100 % '>";
                    servicesString += "<tr><td> </td><th> Service </th><th> Status </th></tr>";

                    foreach (var hi in r.Report.Entries)
                    {
                        servicesString += "<tr><td> " + GetHealthStatusEmojiCode(hi.Value.Status) + " </td><td> " + hi.Key + " </td><td> " + hi.Value.Status + " </td></tr>";
                    }

                    servicesString += "</table>";
                }
                else
                {
                    s = s.Replace("{{Message}}", "Status: " + "Evaluating...");
                }
                

                s = s.Replace("{{Services}}", servicesString);
            }
            else
            {
                s = s.Replace("{{Message}}", _options.Message);
                s = s.Replace("{{Services}}", "");
            }

            s = s.Replace("{{Version}}", _options.Version);
            s = s.Replace("{{OrganizationName}}", _options.Organization ?? _options.Title);
            s = s.Replace("{{Year}}", DateTimeOffset.UtcNow.Year.ToString());

            await context.Response.WriteAsync(s);


        }

        private string GetHealthDateTimeString(DateTimeOffset date)
        {
            return "<script> document.write(new Date('" + 
                date.ToString("o")+
                "').toLocaleString()); </script>";
        }

        protected string GetHealthStatusEmojiCode(HealthStatus status)
        {
            switch (status)
            {
                case HealthStatus.Unhealthy: 
                    return "&#128308;";
                case HealthStatus.Degraded:
                    return "&#128992;";
                case HealthStatus.Healthy:
                    return "&#128994;";
                default:
                    return "&#128217;";
            }
        }
    }
}
