using Naylah.Core.Entities.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naylah.Services.Client
{
    public interface IServiceUser
    {
        string UserName { get; set; }
        string AccessToken { get; set; }
        DateTime AccessTokenExpiresIn { get; set; }
        string IdentityToken { get; set; }
        string RefreshToken { get; set; }
    }

    public class NaylahServiceUser : User, IServiceUser
    {
        public NaylahServiceUser()
        {
        }

        public NaylahServiceUser(string userName) : this()
        {
            UserName = userName;
        }



        [JsonIgnore]
        public new IEnumerable<UserRole> Roles
        {
            get { return Claims.Where(x => x.ClaimType == "role").Select(x => new UserRole() { Name = x?.ClaimValue }).AsEnumerable(); }
        }



        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresIn { get; set; }
        public string IdentityToken { get; set; }
        public string RefreshToken { get; set; }


        public void UpdateClaims(IEnumerable<ClaimLite> claims)
        {
            Claims = claims.Select(x => new UserClaim() { ClaimType = x.Type, ClaimValue = x.Value }).ToList();

            Id = claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
            FirstName = claims.Where(x => x.Type == "given_name").FirstOrDefault()?.Value;
            LastName = claims.Where(x => x.Type == "family_name").FirstOrDefault()?.Value;

            UserName = claims.Where(x => x.Type == "preferred_username").FirstOrDefault()?.Value;
            Email = claims.Where(x => x.Type == "email").FirstOrDefault()?.Value;
            EmailConfirmed = claims.Where(x => x.Type == "email_verified").FirstOrDefault()?.Value == "true";

            ImageUri = claims.Where(x => x.Type == "picture").FirstOrDefault()?.Value;
        }
    }
}