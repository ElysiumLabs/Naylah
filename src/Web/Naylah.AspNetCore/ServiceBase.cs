using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#if NETCOREAPP3_0_OR_GREATER
using Microsoft.Extensions.Hosting;
#endif

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public abstract partial class ServiceBase<TOptions>
        where TOptions : ServiceOptionsBase, new()
    {
        protected IConfiguration Configuration;

#if NETCOREAPP3_0_OR_GREATER
        protected IHostEnvironment Environment;
#endif

#if NETCOREAPP2_1
        protected IHostingEnvironment Environment;
#endif
        public TOptions Options { get; set; }

#if NETCOREAPP3_0_OR_GREATER
        public ServiceBase(IHostEnvironment environment, IConfiguration configuration)
#endif
#if NETCOREAPP2_1
        public Service(IHostingEnvironment environment, IConfiguration configuration)
#endif
        {
            Environment = environment;
            Configuration = configuration;

            Options = ServiceOptionsBase.CreateDefault<TOptions>(GetType().Name);
            Try.Run(() => ConfigureConfiguration(Configuration)?.Bind(Options));
        }

        protected virtual IConfiguration ConfigureConfiguration(IConfiguration configuration)
        {
            return configuration;
        }
    }

    public abstract class ServiceBase : ServiceBase<ServiceOptionsBase>
    {
#if NETCOREAPP3_0_OR_GREATER
        public ServiceBase(IHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
#endif
#if NETCOREAPP2_1
        public Service(IHostingEnvironment environment, IConfiguration configuration): base(environment, configuration)
#endif
        {
        }
    }
}
