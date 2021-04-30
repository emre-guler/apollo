using Microsoft.AspNetCore.Mvc;

namespace Apollo.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase 
    {
        [HttpGet("/")]
        public ActionResult Get()
        {
            return NoContent();
        }
    }
}