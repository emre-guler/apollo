using Apollo.Data;
using Apollo.Entities;
using Apollo.Enums;
using Apollo.Services;
using Apollo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Apollo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        readonly ApolloContext _db;
        readonly TeamService _teamService;
        public TeamController(
            ApolloContext db,
            TeamService teamService
        )
        {
            _db = db;
            _teamService = teamService;
        }

        [HttpPost("/team-register")]
        public IActionResult TeamRegister([FromForm] TeamRegisterViewModel teamVM)
        {
            bool controlResult = _teamService.TeamRegisterFormDataControl(teamVM);
            if(controlResult)
            {
                bool newUserControl = _teamService.NewAccountControl(teamVM.MailAddress, teamVM.PhoneNumber);
                if(!newUserControl)
                {
                    _teamService.CreateTeam(teamVM);
                    return Ok(true);
                }
                else
                {
                    return BadRequest(error: new { errorCode = ErrorCode.UserExists });
                }
            }
            else
            {
                return BadRequest(error: new { errorCode =  ErrorCode.MustBeFilled });
            }
        }

        [HttpPost("/team-login")]
        public IActionResult TeamLogin([FromForm] LoginViewModel teamVM)
        {
            Team teamControl = _teamService.TeamLoginControl(teamVM);
            if(teamControl != null)
            {
                string teamJWT = _teamService.TeamLogin(teamControl.Id);
                Response.Cookies.Append("apolloJWT", teamJWT, new CookieOptions 
                {
                    HttpOnly = true
                });
                return Ok(true);
            }
            else
            {
                return BadRequest(error: new { errorCode = ErrorCode.InvalidCredentials });
            }
        }

        [HttpPost("/team-logout")]
        public IActionResult TeamLogout()
        {
            Response.Cookies.Delete("apolloJWT");
            return Ok(true);
        }
    }
}