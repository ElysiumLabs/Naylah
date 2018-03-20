using IdentityModel.Client;
using Naylah.Core.Entities.Identity;
using Naylah.Service.Client.Services;
using Naylah.Services.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.Identity.Client
{
    public class NaylahIdentityService : NaylahIdentityClient, INaylahServiceBase
    {
        public NaylahIdentityService(string clientId, string clientSecret, string scope) : base(clientId, clientSecret, scope)
        {
        }

        public NaylahServiceClient ServiceClient { get; private set; }

        public void Attach(NaylahServiceClient serviceClient)
        {
            ServiceClient = serviceClient;

            ServiceClient.ServiceSettings.AddNaylahIdentityMessageHandler();
        }

        public void Dettach()
        {
        }

        public virtual async Task Login(NTokenResponse tokenResponse)
        {
            var user = new NaylahServiceUser()
            {
                AccessToken = tokenResponse.AccessToken,
                AccessTokenExpiresIn = tokenResponse.AccessTokenExpiresIn,
                IdentityToken = tokenResponse.IdentityToken,
                RefreshToken = tokenResponse.RefreshToken
            };

            if (Settings.LoadUserProfile)
            {
                var userInfo = await GetUserInfo(user.AccessToken);

                if (!userInfo.IsError)
                {
                    user.UpdateClaims(userInfo.Claims.Select(x => new ClaimLite() { Type = x.Type, Value = x.Value }));
                }
            }

            ServiceClient.CurrentUser = user;
        }

        public virtual async Task RefreshLogin()
        {
            try
            {
                if (ServiceClient.CurrentUser == null)
                    throw new Exception("User is not logged in");

                if (string.IsNullOrEmpty(ServiceClient.CurrentUser.RefreshToken))
                    throw new Exception("User not support refresh");

                var tokenResponse = await TokenClient.RequestRefreshTokenAsync(ServiceClient.CurrentUser.RefreshToken);

                if (tokenResponse.IsError)
                    throw new Exception(tokenResponse.Error);

                var ntokenResponse = new NTokenResponse(tokenResponse);

                await Login(ntokenResponse);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task Logout()
        {
            ServiceClient.CurrentUser = null;
        }
    }
}