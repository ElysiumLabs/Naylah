using AutoMapper;
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
    //public class PersonServiceV2 : StringTableDataServiceV2<Person>
    //{
    //    public PersonServiceV2(
    //        IRepository<Person, string> repository,
    //        IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    //    {
    //    }
    //}

    public class PersonService : CosmosStringAppTableDataService<Person, PersonDTO>
    {
        public PersonService(IMapper mapper,IUnitOfWork _unitOfWork, IRepository<Person> repository) : base(mapper, _unitOfWork, repository)
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
                //q = q.Where(x => x.Name == filter2);
            }

            return q;
        }

        public IQueryable<PersonDTO> GetProjection()
        {
            return Project(GetEntities());
        }
    }
}
