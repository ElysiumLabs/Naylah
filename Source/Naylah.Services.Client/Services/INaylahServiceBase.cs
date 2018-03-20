using Naylah.Services.Client;

namespace Naylah.Service.Client.Services
{
    public interface INaylahServiceBase
    {
        NaylahServiceClient ServiceClient { get; }

        void Attach(NaylahServiceClient serviceClient);

        void Dettach();
    }
}