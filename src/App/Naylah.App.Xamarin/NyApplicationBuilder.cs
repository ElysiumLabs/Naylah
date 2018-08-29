using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naylah.Domain;
using System;
using System.Collections.Generic;

namespace Naylah.App
{
    public class NyApplicationBuilder<TApplication> where TApplication : NyApplication
    {
        private List<Action<IConfigurationBuilder>> _configureHostConfigActions = new List<Action<IConfigurationBuilder>>();
        private List<Action<ApplicationBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new List<Action<ApplicationBuilderContext, IConfigurationBuilder>>();
        private List<Action<ApplicationBuilderContext, IServiceCollection>> _configureServicesActions = new List<Action<ApplicationBuilderContext, IServiceCollection>>();
        private List<IConfigureContainerAdapter> _configureContainerActions = new List<IConfigureContainerAdapter>();
        private IServiceFactoryAdapter _serviceProviderFactory = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private bool _hostBuilt;
        private IConfiguration _hostConfiguration;
        private IConfiguration _appConfiguration;
        private ApplicationBuilderContext _applicationBuilderContext;
        private IApplicationEnvironment _applicationEnvironment;

        private IServiceProvider _appServices;

        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public NyApplicationBuilder<TApplication> ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureHostConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public NyApplicationBuilder<TApplication> ConfigureAppConfiguration(Action<ApplicationBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureAppConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public NyApplicationBuilder<TApplication> ConfigureServices(Action<ApplicationBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public NyApplicationBuilder<TApplication> UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        public NyApplicationBuilder<TApplication> ConfigureContainer<TContainerBuilder>(Action<ApplicationBuilderContext, TContainerBuilder> configureDelegate)
        {
            _configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate
                ?? throw new ArgumentNullException(nameof(configureDelegate))));
            return this;
        }

        public TApplication Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }
            _hostBuilt = true;

            BuildHostConfiguration();
            CreateHostingEnvironment();
            CreateHostBuilderContext();
            BuildAppConfiguration();
            CreateServiceProvider();

            var app = _appServices.GetRequiredService<TApplication>();

            app.Services = _appServices;

            DomainEvent.Resolver = new

            return app;
        }

        private void BuildHostConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            foreach (var buildAction in _configureHostConfigActions)
            {
                buildAction(configBuilder);
            }
            _hostConfiguration = configBuilder.Build();
        }

        private void CreateHostingEnvironment()
        {
            _applicationEnvironment = new ApplicationEnvironment()
            {
                //ApplicationName = _hostConfiguration[HostDefaults.ApplicationKey],
                //EnvironmentName = _hostConfiguration[HostDefaults.EnvironmentKey] ?? EnvironmentName.Production,
                //ContentRootPath = ResolveContentRootPath(_hostConfiguration[HostDefaults.ContentRootKey], AppContext.BaseDirectory),
            };
            //_applicationEnvironment.ContentRootFileProvider = new PhysicalFileProvider(_hostingEnvironment.ContentRootPath);
        }

        private void CreateHostBuilderContext()
        {
            _applicationBuilderContext = new ApplicationBuilderContext(Properties)
            {
                HostingEnvironment = _applicationEnvironment,
                Configuration = _hostConfiguration
            };
        }

        private void BuildAppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddConfiguration(_hostConfiguration);
            foreach (var buildAction in _configureAppConfigActions)
            {
                buildAction(_applicationBuilderContext, configBuilder);
            }
            _appConfiguration = configBuilder.Build();
            _applicationBuilderContext.Configuration = _appConfiguration;
        }

        private void CreateServiceProvider()
        {
            var services = new ServiceCollection();
            //services.AddSingleton(_hostingEnvironment);
            //services.AddSingleton(_hostBuilderContext);
            //services.AddSingleton(_appConfiguration);
            //services.AddSingleton<IApplicationLifetime, ApplicationLifetime>();
            //services.AddSingleton<IHostLifetime, ConsoleLifetime>();
            services.AddSingleton<TApplication, TApplication>();
            services.AddSingleton<NyApplication, TApplication>();
            services.AddOptions();
            services.AddLogging();

            foreach (var configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_applicationBuilderContext, services);
            }

            var containerBuilder = _serviceProviderFactory.CreateBuilder(services);

            foreach (var containerAction in _configureContainerActions)
            {
                containerAction.ConfigureContainer(_applicationBuilderContext, containerBuilder);
            }

            _appServices = _serviceProviderFactory.CreateServiceProvider(containerBuilder);

            if (_appServices == null)
            {
                throw new InvalidOperationException($"The IServiceProviderFactory returned a null IServiceProvider.");
            }
        }
    }

    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(ApplicationBuilderContext hostContext, object containerBuilder);
    }

    public class ApplicationBuilderContext
    {
        public ApplicationBuilderContext(IDictionary<object, object> properties)
        {
            Properties = properties ?? throw new System.ArgumentNullException(nameof(properties));
        }

        public IApplicationEnvironment HostingEnvironment { get; set; }

        public IConfiguration Configuration { get; set; }

        public IDictionary<object, object> Properties { get; }
    }

    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);

        IServiceProvider CreateServiceProvider(object containerBuilder);
    }

    internal class ServiceFactoryAdapter<TContainerBuilder> : IServiceFactoryAdapter
    {
        private IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;

        public ServiceFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory ?? throw new System.ArgumentNullException(nameof(serviceProviderFactory));
        }

        public object CreateBuilder(IServiceCollection services)
        {
            return _serviceProviderFactory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder)
        {
            return _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }

    internal class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private Action<ApplicationBuilderContext, TContainerBuilder> _action;

        public ConfigureContainerAdapter(Action<ApplicationBuilderContext, TContainerBuilder> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(ApplicationBuilderContext hostContext, object containerBuilder)
        {
            _action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }

    public interface IApplicationEnvironment
    {
        string EnvironmentName { get; set; }

        string ApplicationName { get; set; }
        string ContentRootPath { get; set; }

        //IFileProvider ContentRootFileProvider { get; set; }
    }

    public class ApplicationEnvironment : IApplicationEnvironment
    {
        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; }

        public string ContentRootPath { get; set; }

        //public IFileProvider ContentRootFileProvider { get; set; }
    }
}