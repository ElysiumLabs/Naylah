using Naylah.ConsoleAspNetCore.Entities;
using Naylah.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Customizations
{
    public class SomeRepository : IRepository<Person>
    {
        private readonly List<Person> people;

        public SomeRepository(List<Person> people)
        {
            this.people = people;
        }

        public IQueryable<Person> Entities => people.AsQueryable();

        public ValueTask<Person> AddAsync(Person entity)
        {
            people.Add(entity);
            return new ValueTask<Person>(entity);
        }

        public ValueTask<Person> EditAsync(Person entity)
        {
            return new ValueTask<Person>(entity);
        }

        public Task RemoveAsync(Person entity)
        {
            people.Remove(entity);
            return Task.FromResult(1);
        }
    }
}
