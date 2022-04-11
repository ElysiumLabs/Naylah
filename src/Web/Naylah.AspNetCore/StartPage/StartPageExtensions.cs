using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.StartPage
{
    public static class StartPageExtensions
    {
        public static IApplicationBuilder UseStartPage(this IApplicationBuilder app, Action<StartPageOptions> optionsAction = null)
        {
            var option = new StartPageOptions();

            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            optionsAction?.Invoke(option);

            return app.UseMiddleware<StartPageMiddleware>(Options.Create(option));
        }

        public static IApplicationBuilder UseStartPage<TOptions>(this IApplicationBuilder app, TOptions options)
            where TOptions : ServiceOptionsBase
        {
            var optionsAssembly = typeof(TOptions).Assembly;
            return app.UseStartPage(x =>
            {
                x.Title = options.Name ?? optionsAssembly.GetName().Name;
                x.Organization = options.Organization;
                x.Version = optionsAssembly.GetName().Version.ToString();
            });
        }
    }
}
