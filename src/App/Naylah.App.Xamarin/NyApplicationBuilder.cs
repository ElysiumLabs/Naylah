using Naylah.App.IoC;
using System;

namespace Naylah.App
{
    public class NyApplicationBuilder
    {
        internal Type appType;
        internal IDependencyContainer container;

        public NyApplicationBuilder()
        {
        }

        public NyApplicationBuilder UseApplication<T>() where T : NyApplication
        {
            appType = typeof(T);
            return this;
        }

        public NyApplication Build()
        {
            if (container == null)
            {
                this.UseDependencyContainer();
            }

            if (appType == null)
            {
                throw new Exception("UseApplication not configured");
            }

            container.Register(appType, appType);

            DependencyResolver.SetResolver(container.GetResolver());

            var app = DependencyResolver.Resolve(appType) as NyApplication;

            return app;
        }
    }

    public static class SimpleContainerExtensions
    {
        public static NyApplicationBuilder UseDependencyContainer(this NyApplicationBuilder builder, Action<IDependencyContainer> configure = null, Func<IDependencyContainer> createContainer = null)
        {
            builder.container = createContainer != null ? createContainer() : new SimpleContainer();

            configure?.Invoke(builder.container);

            return builder;
        }
    }
}