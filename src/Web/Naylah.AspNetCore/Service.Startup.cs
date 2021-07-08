using Hellang.Middleware.ProblemDetails;
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
            
            services.AddProblemDetails(opts => ConfigureProblemDetails(opts, Environment));
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();

            if (Options.UseDefaultStartupPage)
            {
                app.UseStartPage(Options);
            }
        }

    
    }
}
