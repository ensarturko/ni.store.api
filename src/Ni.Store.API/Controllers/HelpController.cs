using Microsoft.AspNetCore.Mvc;

namespace Ni.Store.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/healthcheck")]
    public class HelpController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}