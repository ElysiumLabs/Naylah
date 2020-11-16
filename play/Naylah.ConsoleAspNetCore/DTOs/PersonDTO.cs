using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.DTOs
{
    public class PersonDTO : IEntity<string>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
