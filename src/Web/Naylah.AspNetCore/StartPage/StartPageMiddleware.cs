using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Naylah.HealthChecks;

namespace Naylah.StartPage
{
    public class StartPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly StartPageOptions _options;
        private CachedHealthCheckService cachedHealthCheckService;

        public StartPageMiddleware(RequestDelegate next, IOptions<StartPageOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options.Value;
        }

        public Task InvokeAsync(HttpContext context)
        {

            HttpRequest request = context.Request;

            //not asking for a homepage
            if (!(request.Path == "" || request.Path == "/"))
            {
                return _next(context);
            }

            var startPageProvider = context.RequestServices.GetService<IStartPageProvider>();
            var healthyCheck = context.RequestServices.GetService<HealthCheckService>();

            cachedHealthCheckService =
                cachedHealthCheckService ??
                context.RequestServices.GetService<CachedHealthCheckService>()
                ?? new CachedHealthCheckService(new CachedHealthCheckServiceOptions()
                {
                    CacheDuration = _options.HealtyCheckCacheDuration
                });

            var welcomePage = startPageProvider?.GetStartPage() ?? new StartPage();
            return welcomePage.ExecuteAsync(context, _options, healthyCheck, cachedHealthCheckService);

        }

    }
}