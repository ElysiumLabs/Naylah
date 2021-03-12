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
        //private readonly PersonServiceV2 tableDataService;

        //public PersonControllerV2(
        //    PersonServiceV2 tableDataService)
        //{
        //    this.tableDataService = tableDataService;
        //}

        //[HttpGet("")]
        //public IEnumerable<PersonGetRequest> GetPeople()
        //{
        //    return tableDataService.GetAll<PersonGetRequest>();
        //}

        //[HttpPost("SameType")]
        //public async Task<PersonPostRequest> PostReturningSameType(
        //    [FromBody] PersonPostRequest person)
        //{
        //    return await tableDataService.UpsertAsync<Person, PersonPostRequest>(person);
        //}

        //[HttpPost("OtherType")]
        //public async Task<PersonPostRequest> PostReturningOtherType(
        //    [FromBody] PersonPostRequest person)
        //{
        //    return await tableDataService.UpsertAsync<Person, PersonPostRequest>(person);
        //}
    }
}
