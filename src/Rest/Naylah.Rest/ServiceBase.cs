using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public abstract class ServiceBase
    {
        protected readonly RestClient client;

        public ServiceBase(RestClient naylahRestClient)
        {
            client = naylahRestClient;
        }
    }
}
