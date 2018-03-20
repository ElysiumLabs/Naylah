using Naylah.Core.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Naylah.Identity.Client.OidcClient
{
    public class LoginResult
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; }

        public IList<ClaimLite> User { get; set; }

        public string AccessToken { get; set; }
        public string IdentityToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}