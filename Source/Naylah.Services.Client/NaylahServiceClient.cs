using Microsoft.WindowsAzure.MobileServices;
using Naylah.Service.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Naylah.Services.Client
{
    public partial class NaylahServiceClient : IDisposable
    {
        protected Func<HttpMessageHandler> DefaultHandlerFactory = GetDefaultHttpClientHandler;

        public NaylahServiceClient(Uri serviceUri, NaylahServiceClientSettings settings)
        {
            Services = new List<INaylahServiceBase>();

            ServiceUri = serviceUri;
            ServiceSettings = NaylahServiceClientSettings.GetDefaultSettings();

            ConfigureServiceClient();
        }

        public NaylahServiceClient(Uri serviceUri) : this(serviceUri, null)
        {
        }

        public IEnumerable<INaylahServiceBase> Services { get; internal set; }

        public Uri ServiceUri { get; private set; }

        public virtual NaylahServiceClientSettings ServiceSettings { get; private set; }

        public IServiceUser CurrentUser { get; set; }

        public virtual MobileServiceClient MobileServiceClient { get; set; }

        public void Dispose()
        {
            if (MobileServiceClient != null)
            {
                MobileServiceClient.Dispose();
            }
        }

        protected internal virtual void ConfigureServiceClient()
        {
            var chainhandler = GetChainMessageHandler();

            var mobileServiceClient = new MobileServiceClient(ServiceUri, chainhandler);

            mobileServiceClient.SerializerSettings.CamelCasePropertyNames = true;

            MobileServiceClient = mobileServiceClient;
        }

        private static HttpMessageHandler GetDefaultHttpClientHandler()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            return handler;
        }

        private HttpMessageHandler GetChainMessageHandler()
        {
            var messageHandlers = ServiceSettings.MessageHandlers;

            //if (!messageHandlers.Any(x => x.GetType() == typeof(NaylahIdentityMessageHandler)))
            //{
            //    messageHandlers.Insert(0, new NaylahIdentityMessageHandler());
            //}

            foreach (var messageHandler in messageHandlers)
            {
                messageHandler.Configure(this);
            }

            return CreatePipeline(messageHandlers);
        }

        private HttpMessageHandler CreatePipeline(IEnumerable<HttpMessageHandler> handlers)
        {
            HttpMessageHandler pipeline = handlers.LastOrDefault() ?? DefaultHandlerFactory();
            DelegatingHandler dHandler = pipeline as DelegatingHandler;
            if (dHandler != null)
            {
                dHandler.InnerHandler = DefaultHandlerFactory();
                pipeline = dHandler;
            }

            // Wire handlers up in reverse order
            IEnumerable<HttpMessageHandler> reversedHandlers = handlers.Reverse().Skip(1);
            foreach (HttpMessageHandler handler in reversedHandlers)
            {
                dHandler = handler as DelegatingHandler;
                if (dHandler == null)
                {
                    throw new ArgumentException(
                        string.Format(
                        "All message handlers except the last must be of the type '{0}'",
                        typeof(DelegatingHandler).Name));
                }

                dHandler.InnerHandler = pipeline;
                pipeline = dHandler;
            }

            return pipeline;
        }
    }
}