using Microsoft.AspNetCore.Mvc;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "API is running" });
        }
    }
}
