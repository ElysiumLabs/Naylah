using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Naylah.Core.Entities.Identity
{
    public class User : Person
    {
        public User()
        {
            this.Claims = new Collection<UserClaim>();
            this.Logins = new Collection<UserLogin>();
            this.Roles = new Collection<UserRole>();
        }


        public string UserName { get; set; }


        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }



        public virtual ICollection<UserRole> Roles { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
    }
}