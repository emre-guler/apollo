using Microsoft.AspNetCore.Mvc;

namespace Apollo.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase 
    {
        [HttpGet("/sayfalar")]
        public ActionResult Get()
        {
            return NoContent();
        }
    }
}