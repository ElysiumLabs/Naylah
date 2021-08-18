
using Naylah.ConsoleAspNetCore.DTOs;
using Naylah.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Entities
{
    public class PersonGetRequest : IEntity<string>
    {
        public string Id { get; set; }
        public PersonNameFull Name { get; set; } = new PersonNameFull();
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class PersonPostRequest : IEntity<string>
    {
        public string Id { get; set; }
        public PersonNameFull Name { get; set; } = new PersonNameFull();
        public string Version { get; set; }
    }

    public class Person : IEntity<string>, IModifiable,
        IEntityUpdate<Person>,
        IEntityUpdate<PersonDTO>,
        IEntityUpdate<PersonGetRequest>,
        IEntityUpdate<PersonPostRequest>
    {
        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Version { get; set; }
        public bool Deleted { get; set; }

        public PersonNameFull Name { get; set; } = new PersonNameFull();

        public string Partition { get; set; }


        public void UpdateFrom(Person source, EntityUpdateOptions options = null)
        {
            Name = source.Name;
        }

        public void UpdateFrom(PersonDTO source, EntityUpdateOptions options = null)
        {
            Name = source.Name;
        }

        public void UpdateFrom(PersonGetRequest source, EntityUpdateOptions options = null)
        {

        }

        public void UpdateFrom(PersonPostRequest source, EntityUpdateOptions options = null)
        {
            this.Name = source.Name;
            this.Version = source.Version;
        }
    }
}
