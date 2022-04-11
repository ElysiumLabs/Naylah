using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Naylah.AspNetCore.Tests
{
    public class TestService : ServiceBase<TestServiceOptions>
    {
        public TestService(IHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {

        }


        protected override void ConfigureApp(IApplicationBuilder app)
        {
        }

        protected override void ConfigureServicesApp(IServiceCollection services)
        {
        }
    }

}