using Apollo.Data;
using Apollo.Enums;
using Apollo.Services;
using Apollo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult TeamRegister(TeamRegisterViewModel teamVM)
        {
            bool controlResult = _teamService.TeamRegisterFormDataControl(teamVM);
            if(controlResult)
            {
                bool newUserControl = _teamService.NewAccountControl(teamVM.MailAddress, teamVM.PhoneNumber);
                if(newUserControl)
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
    }
}