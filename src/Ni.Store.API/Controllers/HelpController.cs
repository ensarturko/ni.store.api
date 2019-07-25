using Microsoft.AspNetCore.Mvc;

namespace Ni.Store.Api.Controllers
{
    public class HelpController : Controller
    {
        [HttpGet("/")]
        [Produces("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Root()
        {
            return Ok("Warmed up successfully");
        }
    }
}