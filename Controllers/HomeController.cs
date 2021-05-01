using Microsoft.AspNetCore.Mvc;

namespace Apollo.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase 
    {
        [HttpGet("/")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}