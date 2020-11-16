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
    public class PersonController : CustomTableDataController<Person, PersonDTO, string>
    {
        private readonly StringAppTableDataService<Person, PersonDTO> tableDataService;

        public PersonController(StringAppTableDataService<Person, PersonDTO> tableDataService) : base(tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        [HttpGet("custom")]
        public PageResult<PersonDTO> GetCustom([FromServices] PersonService service, string filter1, string filter2)
        {
            var q = service.GetPeopleCustom(filter1, filter2);
            return tableDataService.CreateODataWrapper(new ODataQuerySettings() { PageSize = 75}).GetPaged(Request, q);
        }
    }
}
