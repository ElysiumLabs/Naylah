using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public abstract class ServiceBase
    {
        protected readonly NaylahRestClient client;

        public ServiceBase(NaylahRestClient naylahRestClient)
        {
            client = naylahRestClient;
        }
    }
}
