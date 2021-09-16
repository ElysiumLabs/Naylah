
using Naylah.ConsoleAspNetCore.DTOs;
using Naylah.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Entities
{
    public class Person : IEntity<string>, IModifiable, ISoftDeletable,
        IEntityUpdate<Person>,
        IEntityUpdate<PersonDTO>
    {
        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Version { get; set; }
        public bool Deleted { get; set; }

        public string Name { get; set; }

        public void UpdateFrom(Person source, EntityUpdateOptions options = null)
        {
            Name = source.Name;
        }

        public void UpdateFrom(PersonDTO source, EntityUpdateOptions options = null)
        {
            Name = source.Name;
        }
    }
}
