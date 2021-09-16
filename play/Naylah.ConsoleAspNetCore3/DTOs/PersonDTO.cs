using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.DTOs
{
    public class PersonDTO : IEntity<string>
    {
        public string Id { get; set; }

        public PersonNameFull Name { get; set; } = new PersonNameFull();

        public PersonName Test { get; set; } = new PersonName();

        public string Version { get; set; }

    }

    public class PersonDTO2 : IEntity<string>
    {
        public string Id { get; set; }

        public PersonNameFull Name { get; set; } = new PersonNameFull();
    }
}
