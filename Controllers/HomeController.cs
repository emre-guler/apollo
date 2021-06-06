using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Apollo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        readonly ApolloContext _db;

        public HomeController(
            ApolloContext db
        )
        {
            _db = db;
        }

        [HttpGet("/")]
        public IActionResult Get()
        {
            return Ok("Welcome to the castle of Apollo Project! Do not try anythig!");
        }

        [HttpGet("/get-cities")]
        public async Task<IActionResult> GetCities()
        {
            var allCities = await _db.Cities.Where(x => !x.DeletedAt.HasValue).Select(x => new { x.Id, x.Name }).ToListAsync();
            string response = JsonConvert.SerializeObject(allCities);
            return Ok(response);
        }
    }
}