using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public partial class ServiceBase<TOptions>
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesDefault(services);
            ConfigureServicesApp(services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            ConfigureDefault(app);
            ConfigureApp(app);
        }
        

        protected abstract void ConfigureServicesApp(IServiceCollection services);

        protected abstract void ConfigureApp(IApplicationBuilder app);

        protected virtual void ConfigureServicesDefault(IServiceCollection services)
        {
            services.AddSingleton(Options);
            services.AddProblemDetails(opts => ConfigureProblemDetails(opts, Environment));
        }

        protected virtual void ConfigureDefault(IApplicationBuilder app)
        {
            //first thing is problemdetails... then rest...
            app.UseProblemDetails();
            ConfigureStartupPage(app);
        }

    }
}
