using System.Collections.Generic;

namespace Naylah.Identity.Client.OidcClient
{
    public class OidcSettings
    {
        public OidcSettings(
            string authority,
            string clientId,
            string clientSecret,
            string redirectUri,
            string scope,
            bool loadUserProfile = false,
            bool filterClaims = true
            )

        {
            Authority = authority;
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
            Scope = scope;
            LoadUserProfile = loadUserProfile;
            FilterClaims = filterClaims;
        }

        public string Authority { get; protected set; }
        public string ClientId { get; protected set; }
        public string ClientSecret { get; protected set; }
        public string RedirectUri { get; protected set; }
        public string Scope { get; protected set; }

        public bool LoadUserProfile { get; protected set; } = false;
        public bool FilterClaims { get; protected set; } = true;

        public IEnumerable<string> FilterClaimTypes { get; protected set; } = new List<string>
        {
            "iss",
            "exp",
            "nbf",
            "aud",
            "nonce",
            "c_hash",
            "iat",
            "auth_time"
        };
    }
}