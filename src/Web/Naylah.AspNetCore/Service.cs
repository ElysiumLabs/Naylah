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
    public abstract partial class Service<TOptions>
        where TOptions : ServiceOptions, new()
    {
        protected IConfiguration Configuration;

#if NETCOREAPP3_0_OR_GREATER
        protected IHostEnvironment Environment;
#endif

#if NETCOREAPP2_1
        protected IHostingEnvironment Environment;
#endif
        public TOptions Options { get; set; }

        private Service()
        {
            Options = ServiceOptions.CreateDefault<TOptions>(GetType().Name);
            Try.Run(() => Configuration?.Bind(Options));
        }

#if NETCOREAPP3_0_OR_GREATER
        public Service(IHostEnvironment environment, IConfiguration configuration) : this()
#endif
#if NETCOREAPP2_1
        public Service(IHostingEnvironment environment, IConfiguration configuration) : this()
#endif
        {
            Environment = environment;
            Configuration = configuration;
        }
    }

    public abstract class Service : Service<ServiceOptions>
    {
#if NETCOREAPP3_0_OR_GREATER
        public Service(IHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
#endif
#if NETCOREAPP2_1
        public Service(IHostingEnvironment environment, IConfiguration configuration): base(environment, configuration)
#endif
        {
        }
    }
}
