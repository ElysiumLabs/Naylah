using Microsoft.AspNet.OData;
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
    //public class PersonController : TableDataController<Person, PersonDTO, string>
    public class PersonController : CustomTableDataController<Person, PersonDTO, string>
    {
        private readonly StringAppTableDataService<Person, PersonDTO> tableDataService;

        public PersonController(StringAppTableDataService<Person, PersonDTO> tableDataService) : base(tableDataService)
        {
            this.tableDataService = tableDataService;
        }

        //public override PageResult<PersonDTO> GetAll()
        //{
        //    var q = tableDataService.CreateWrapper().GetEntities().Where(x => x.Id == "1");
        //    return tableDataService.CreateODataWrapper().GetPaged(Request, q);
        //}
    }
}
