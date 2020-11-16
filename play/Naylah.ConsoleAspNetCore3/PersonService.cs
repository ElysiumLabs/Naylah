using Naylah.ConsoleAspNetCore.Customizations;
using Naylah.ConsoleAspNetCore.DTOs;
using Naylah.ConsoleAspNetCore.Entities;
using Naylah.Data;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore
{
    public class PersonService : StringAppTableDataService<Person, PersonDTO>
    {
        public PersonService(IUnitOfWork _unitOfWork, IRepository<Person, string> repository) : base(_unitOfWork, repository)
        {
        }

        public IQueryable<Person> GetPeopleCustom(string filter1, string filter2)
        {
            var q = GetEntities();

            if (!string.IsNullOrEmpty(filter1))
            {
                q = q.Where(x => x.Id == filter1);
            }

            if (!string.IsNullOrEmpty(filter2))
            {
                q = q.Where(x => x.Name == filter2);
            }

            return q;
        }
    }
}
