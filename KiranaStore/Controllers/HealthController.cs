using Microsoft.AspNetCore.Mvc;

namespace KiranaStore.Controllers
{
    public class HealthController : Controller
    {
        [HttpGet]
        [Route("health")]
        public IActionResult Health()
        {
            return Ok("API Running");
        }
    }
}
