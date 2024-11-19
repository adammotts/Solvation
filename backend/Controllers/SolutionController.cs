using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolutionController : ControllerBase
    {
        [HttpGet("/test")]
        public IActionResult GetTestResponse()
        {
            return Ok(new { message = "Test route" });
        }
    }
}
