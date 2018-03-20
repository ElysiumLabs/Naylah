using IdentityModel.Client;
using Naylah.Services.Client;
using System;
using System.Threading.Tasks;

namespace Naylah.Identity.Client.Extensions
{
    public static class NaylahServiceClientExtensions
    {
        public async static Task<IServiceUser> NaylahIdentityLogin(TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
            {
                throw new ArgumentNullException("Token response is null");
            }

            if (tokenResponse.IsError)
            {
                throw new Exception("Token response is invalid");
            }
            else
            {
                //IServiceUser user = new NaylahServiceUser(tokenResponse.);
            }
            throw new Exception("Token response is invalid");
        }
    }
}