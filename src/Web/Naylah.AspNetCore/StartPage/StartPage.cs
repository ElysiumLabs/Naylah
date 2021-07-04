using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

        public virtual async Task<string> GetPageStringAsync()
        {
            var assembly = typeof(StartPage).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("Naylah.Assets.index.html");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public virtual async Task ExecuteAsync(HttpContext context, StartPageOptions _options, HealthCheckService healthyCheck)
        {
            var s = await GetPageStringAsync();

            s = s.Replace("{{ServiceName}}", _options.Title);

            if (healthyCheck!= null)
            {
                var servicesString = "<table style='width: 100 % '>";

                servicesString += "<tr><td> </td><th> Service </th><th> Status </th></tr>";

                var r = await healthyCheck.CheckHealthAsync();
                s = s.Replace("{{Message}}", "Status: " + r.Status);

                foreach (var hi in r.Entries)
                {
                    servicesString += "<tr><td> " + GetHealthStatusEmojiCode(hi.Value.Status) + " </td><td> " + hi.Key + " </td><td> " + hi.Value.Status + " </td></tr>";
                }

                servicesString += "</table>";

                s = s.Replace("{{Services}}", servicesString);
            }
            else
            {
                s = s.Replace("{{Message}}", _options.Message);
                s = s.Replace("{{Services}}", "");
            }

            s = s.Replace("{{CompanyName}}", _options.Organization ?? _options.Title);
            s = s.Replace("{{Year}}", DateTimeOffset.UtcNow.Year.ToString());

            await context.Response.WriteAsync(s);


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
