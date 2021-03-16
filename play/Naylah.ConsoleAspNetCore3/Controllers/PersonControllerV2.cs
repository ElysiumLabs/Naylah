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

namespace Naylah.ConsoleAspNetCore.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class PersonControllerV2 : ControllerBase
    {
        private readonly StringTableDataServiceCustom<Person> tableDataService;

        public PersonControllerV2(
           StringTableDataServiceCustom<Person> tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        [HttpGet("")]
        public IEnumerable<PersonGetRequest> GetPeople()
        {
            return tableDataService.GetAll<PersonGetRequest>();
        }

        [HttpPost("SameType")]
        public async Task<PersonPostRequest> PostReturningSameType(
           [FromBody] PersonPostRequest person)
        {
            return await tableDataService.UpsertEntityAsync(person).ConfigureAwait(false);
        }

        [HttpPost("OtherType")]
        public async Task<PersonGetRequest> PostReturningOtherType(
           [FromBody] PersonPostRequest person)
        {
            return await tableDataService.UpsertEntityAsync<PersonPostRequest, PersonGetRequest>(person)
                .ConfigureAwait(false);
        }
    }
}
