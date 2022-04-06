using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Xunit;

namespace Naylah.AspNetCore.Tests
{

    public class ServiceStartupWebHostTest
    {
        [Fact]
        public IWebHost WebHostBuildUseStartupBuild()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<TestService>().Build();
            Assert.NotNull(webHost);
            return webHost;
        }
    }

    public class ServiceStartupTest : IClassFixture<ServiceStartupWebHostTest>
    {
        private IWebHost webHost;

        public ServiceStartupTest(ServiceStartupWebHostTest testServiceStartupWebHostTest)
        {
            webHost = testServiceStartupWebHostTest.WebHostBuildUseStartupBuild();
        }

        protected IServiceScope CreateScope()
        {
            return webHost.Services.CreateScope();
        }

        [Fact]
        public void GetCustomServiceOptions()
        {
            using (var scope = CreateScope())
            {
                Assert.NotNull(scope.ServiceProvider.GetService<TestServiceOptions>());
            }
        }
    }

}