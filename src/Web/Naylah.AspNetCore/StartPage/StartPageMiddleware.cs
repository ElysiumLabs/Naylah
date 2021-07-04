using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Naylah.StartPage
{
    public class StartPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly StartPageOptions _options;


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

            // Dynamically generated for LOC.
            var startPageProvider = context.RequestServices.GetService<IStartPageProvider>();
            var healthyCheck = context.RequestServices.GetService<HealthCheckService>();

            var welcomePage = startPageProvider?.GetStartPage() ?? new StartPage();
            return welcomePage.ExecuteAsync(context, _options, healthyCheck);

        }

    }
}