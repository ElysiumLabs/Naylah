using IdentityModel.Client;
using Newtonsoft.Json;
using System;

namespace Naylah.Identity.Client
{
    public class NTokenResponse
    {
        [JsonConstructor]
        public NTokenResponse(string AccessToken, string IdentityToken, string RefreshToken, string TokenType, DateTime accessTokenExpiresIn)
        {
            this.AccessToken = AccessToken;
            this.AccessTokenExpiresIn = accessTokenExpiresIn;
            this.IdentityToken = IdentityToken;
            this.RefreshToken = RefreshToken;
            this.TokenType = TokenType;
        }

        public NTokenResponse(TokenResponse _tokenResponse)
        {
            AccessToken = _tokenResponse.AccessToken;
            AccessTokenExpiresIn = DateTime.Now.AddSeconds(_tokenResponse.ExpiresIn);
            IdentityToken = _tokenResponse.IdentityToken;
            RefreshToken = _tokenResponse.RefreshToken;
            TokenType = _tokenResponse.TokenType;
        }

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiresIn { get; set; }

        public string IdentityToken { get; set; }

        public string RefreshToken { get; set; }

        public string TokenType { get; set; }
    }
}