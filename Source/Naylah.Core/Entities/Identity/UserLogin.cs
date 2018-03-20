using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Entities.Identity
{
    public class UserLogin : EntityBase
    {

        public static UserLogin Create()
        {
            var ul = new UserLogin();

            ul.GenerateId();

            return ul;
        }

        public static UserLogin Create(
            string userId,
            string loginProvider,
            string providerKey
            )
        {
            var ul = Create();

            ul.UserId = userId;
            ul.LoginProvider = loginProvider;
            ul.ProviderKey = providerKey;

            return ul;
        }

        
        public UserLogin()
        {

        }

        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public virtual User User { get; set; }
    }
}
