using Microsoft.AspNetCore.Mvc;

namespace KiranaStore.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [HttpHead]
        public IActionResult Health()
        {
            return Ok("API Running");
        }
    }
}
