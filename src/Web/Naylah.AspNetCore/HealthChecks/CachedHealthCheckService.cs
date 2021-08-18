using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.HealthChecks
{
    public class CachedHealthCheckServiceOptions
    {
        public TimeSpan CacheDuration { get; set; }
    }

    public class CachedHealthCheckService
    {
        private readonly CachedHealthCheckServiceOptions options;
        private DateHealthReport Result;
        private bool Evaluating = false;

        public CachedHealthCheckService(CachedHealthCheckServiceOptions options)
        {
            this.options = options;
        }

        public async Task<DateHealthReport> GetOrCheckHealthAsync(HealthCheckService healthCheckService, CancellationToken cancellationToken = default)
        {
            return null;
//            try
//            {
//                if ((Evaluating) || (Result?.Date.Add(options.CacheDuration) > DateTimeOffset.UtcNow))
//                {
//                    return Result;
//                }

//                Evaluating = true;

//#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//                healthCheckService.CheckHealthAsync(cancellationToken).ContinueWith(report => 
//                {
//                    Result = new DateHealthReport()
//                    {
//                        Date = DateTimeOffset.UtcNow,
//                        Report = report.Result
//                    };

//                    Evaluating = false;
//                });
//#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


//                //var report = await healthCheckService.CheckHealthAsync(cancellationToken);
//                return Result;
//            }
//            catch (Exception)
//            {
//                Evaluating = false;
//                return Result;
//            }
        }
    }

    public class DateHealthReport
    {
        public DateTimeOffset Date { get; set; }
        public HealthReport Report { get; set; }
    }
}
