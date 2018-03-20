using IdentityModel.Client;
using Naylah.Identity.Client.Constants;
using Naylah.Identity.Client.OidcClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Identity.Client
{
    public class NaylahIdentityClient
    {
        private const string BaseAddress = "https://sso.identity.naylah.co/core";

        public NaylahIdentityClient(
            string clientId,
            string clientSecret,
            string scope
            )
        {
            Settings = new OidcSettings(
                BaseAddress,
                clientId,
                clientSecret,
                "https://naylahidentity/redirect",
                scope,
                true
                );

            TokenClient = new TokenClient(BaseAddress + Endpoints.TokenEndpoint, Settings.ClientId, Settings.ClientSecret);
        }

        public OidcSettings Settings { get; private set; }

        public TokenClient TokenClient { get; private set; }

        public async Task<UserInfoResponse> GetUserInfo(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userInfoClient = new UserInfoClient(new Uri(BaseAddress + Endpoints.UserInfoEndpoint).ToString());
            return await userInfoClient.GetAsync(accessToken, cancellationToken);
        }

        public string GetAuthorizeUrl()
        {
            var request = new RequestUrl(BaseAddress + Endpoints.AuthorizeEndpoint);

            var _nonce = Guid.NewGuid().ToString();

            return request.CreateAuthorizeUrl(
                clientId: Settings.ClientId,
                responseType: "code id_token token",
                scope: Settings.Scope,
                redirectUri: Settings.RedirectUri,
                nonce: _nonce);
        }

        public async Task<NTokenResponse> ValidateAuthorizeResponse(string responseUri)
        {
            AuthorizeResponse response = new AuthorizeResponse(responseUri);

            var tokenResponse = await TokenClient.RequestAuthorizationCodeAsync(
                code: response.Code,
                redirectUri: Settings.RedirectUri);

            return new NTokenResponse(tokenResponse);
            //await SetCurrentUser(tokenResponse);
        }
    }
}