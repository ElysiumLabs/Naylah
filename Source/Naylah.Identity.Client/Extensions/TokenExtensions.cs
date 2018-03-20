using IdentityModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Identity.Client.Extensions
{
    public static class TokenExtensions
    {
        public static string[] DecodeJwToken(string token)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var claims = parts[1];

            return new string[] 
            {
                JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header), 0, Base64Url.Decode(header).Length)).ToString(),
                JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims), 0,  Base64Url.Decode(claims).Length)).ToString()
            };
            
        }
    }
}
