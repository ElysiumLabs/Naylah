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

        public Func<HealthCheckRegistration, bool> Predicate { get; set; } = x => true;
    }

    public class CachedHealthCheckService
    {
        private readonly CachedHealthCheckServiceOptions options;
        private DateHealthReport Result;
        private bool Evaluating = false;

        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        public CachedHealthCheckService(CachedHealthCheckServiceOptions options)
        {
            this.options = options;
        }

        public async Task<DateHealthReport> GetOrCheckHealthAsync(HealthCheckService healthCheckService, CancellationToken cancellationToken = default)
        {
            try
            {
                if ((Evaluating) || (Result?.Date.Add(options.CacheDuration) > DateTimeOffset.UtcNow))
                {
                    return Result;
                }

                await _mutex.WaitAsync(cancellationToken);

                Evaluating = true;

                var report = await healthCheckService.CheckHealthAsync(options.Predicate, cancellationToken);

                Result = new DateHealthReport()
                {
                    Date = DateTimeOffset.UtcNow,
                    Report = report
                };

                Evaluating = false;

                return Result;
            }
            catch (Exception)
            {
                Evaluating = false;
                return Result;
            }
            finally
            {
                _mutex.Release();
            }
        }
    }

    public class DateHealthReport
    {
        public DateTimeOffset Date { get; set; }
        public HealthReport Report { get; set; }
    }
}
