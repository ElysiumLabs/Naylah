using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Naylah.Data;

namespace Naylah.ConsoleAspNetCore6.Controllers
{
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet("")]
        public object Get()
        {
            return "ok";
        }

        [HttpGet("list")]
        public async Task<PageResult<Person>> GetCustom()
        {
            var list = new List<Person>();

            foreach (var item in Enumerable.Range(1, 10000))
            {
                list.Add(new Person { Id = item.ToString(), Name = "Name " + item });
            }
            var q = list.AsQueryable();

            var wrapper = EntityODataRequestWrapperExtensions.CreateODataWrapper<Person>(Request);
            
            var q2 = wrapper.ApplyTo(q, asd => asd.Cast<Person>());

            return await wrapper.Paged(q2);

        }
    }

    public class Person
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

    }
}
