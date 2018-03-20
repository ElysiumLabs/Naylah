using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Entities.Identity
{
    public class UserClaim : EntityBase
    {

        public static UserClaim Create(
           string userId,
           string claimType,
           string claimValue
            )
        {
            var uClaim = Create();

            uClaim.UserId = userId;
            uClaim.ClaimType = claimType;
            uClaim.ClaimValue = claimValue;

            return uClaim;
        }

        private static UserClaim Create()
        {
            var uClaim = new UserClaim();

            uClaim.GenerateId();

            return uClaim;
        }

        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual User User { get; set; }

       
    }
}
