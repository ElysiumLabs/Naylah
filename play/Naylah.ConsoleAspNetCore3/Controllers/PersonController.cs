using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Naylah.ConsoleAspNetCore.Customizations;
using Naylah.ConsoleAspNetCore.DTOs;
using Naylah.ConsoleAspNetCore.Entities;
using Naylah.Data;
using Naylah.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naylah.Data.Providers.CosmosDB;
using Microsoft.Azure.Cosmos.Linq;

namespace Naylah.ConsoleAspNetCore.Controllers
{
    public class PersonController : CustomTableDataController<Person, PersonDTO, string>
    {
        private readonly StringAppTableDataService<Person, PersonDTO> tableDataService;
        private readonly IMapper mapper;

        public PersonController(StringAppTableDataService<Person, PersonDTO> tableDataService, IMapper mapper) : base(tableDataService)
        {
            this.tableDataService = tableDataService;
            this.mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IQueryable<PersonDTO>> GetCustom([FromServices] PersonService service)
        {
            var item = await service.GetByIdAsync("1D793280-8E94-4D99-8C9C-B47482EDEB40");

            var wrapper = service.CreateWrapper();
            var entities = wrapper.GetEntities().Where(x => x.Id == "1D793280-8E94-4D99-8C9C-B47482EDEB40");
            var proj = await wrapper.Project(entities).ToFeedIterator().ToCosmosListAsync();

            return null;
            //return service.GetProjection();
        }

        [HttpGet("custom")]
        public async Task<PageResult<PersonDTO>> GetCustom([FromServices] PersonService service, string filter1, string filter2)
        {
            var q = service.GetPeopleCustom(filter1, filter2);

            var odataWrapper = Request.CreateODataWrapper<Person>(new ODataQuerySettings() { PageSize = 75 });
            var applyedQuery = odataWrapper.ApplyTo(q, (q) => q.ProjectTo<PersonDTO>(mapper.ConfigurationProvider));
            return await odataWrapper.Paged(applyedQuery);
        }

        [HttpGet("custom2")]
        public virtual async Task<PageResult<object>> GetAllCustom2()
        {
            var odataWrapper = Request.CreateODataWrapper<Person>();
            var e = odataWrapper.ApplyTo(GetEntities());
            return await odataWrapper.Paged(e);
        }

        [HttpGet("custom3")]
        public async Task<PageResult<PersonDTO2>> GetAllCustom3()
        {
            var odataWrapper = Request.CreateODataWrapper<Person>();
            var e1 = odataWrapper.ApplyTo(GetEntities(), (q) => q.Select(x => new PersonDTO2() { Id = x.Id, Name = x.Name }));
            var e2 = odataWrapper.ApplyTo(GetEntities(), (q) => q.ProjectTo<PersonDTO2>(mapper.ConfigurationProvider));
            return await odataWrapper.Paged(e1);
        }

        [HttpGet("custom4")]
        public async Task<PersonDTO> GetAllCustom4(string id)
        {
            return await tableDataService.GetByIdAsync(id);
        }

        [HttpGet("raiseexception")]
        public async Task<string> RaiseException()
        {
            PersonDTO p = null;
            return p.Name.FirstName;

        }

    }
}
