using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest
{
    public abstract class ServiceBase
    {
        protected readonly NaylahRestClient2 client;

        public ServiceBase(NaylahRestClient2 naylahRestClient)
        {
            client = naylahRestClient;
        }
    }

    public abstract class ServiceBaseOld
    {
        protected readonly NaylahRestClient client;

        public ServiceBaseOld(NaylahRestClient naylahRestClient)
        {
            client = naylahRestClient;
        }
    }
}
