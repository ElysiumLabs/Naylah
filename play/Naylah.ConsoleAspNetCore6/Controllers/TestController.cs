using Microsoft.AspNetCore.Mvc;

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
    }
}
