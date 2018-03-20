using Naylah.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Naylah.Services.Client.MessageHandlers
{
    public class ServiceClientBaseDelegatingHandler : DelegatingHandler
    {
        public virtual NaylahServiceClient ServiceClient { get; private set; }

        public ServiceClientBaseDelegatingHandler()
        {

        }

        protected internal void Configure(NaylahServiceClient _serviceClient)
        {
            ServiceClient = _serviceClient;
        }

        
    }
}
