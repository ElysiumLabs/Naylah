using System;

namespace Naylah.Core
{
    public class NaylahReactor : IDisposable
    {
        public string InstanceId;

        public NaylahReactor()
        {
            InstanceId = Guid.NewGuid().ToString();
            DomainManager = new NaylahDomainManager();
        }

        public NaylahDomainManager DomainManager { get; set; }

        public void Dispose()
        {
            DomainManager.Dispose();
        }
    }
}