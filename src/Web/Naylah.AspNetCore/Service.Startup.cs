using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Naylah.StartPage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public partial class Service<TOptions>
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Options);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            //app.UseMiddleware<StatsMiddleware>();

            if (Options.UseDefaultStartupPage)
            {
                app.UseStartPage(Options);
            }
        }

    
    }
}
