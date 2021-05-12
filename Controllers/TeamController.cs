using System;
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
            if(teamControl is not null)
            {
                string teamJWT = _teamService.TeamLogin(teamControl.Id);
                Response.Cookies.Append("apolloJWT", teamJWT, new CookieOptions 
                {
                    HttpOnly = true
                });
                Response.Cookies.Append("apolloTeamId", teamControl.Id.ToString(), new CookieOptions 
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

        [HttpGet("/team-mail-verification")]
        public IActionResult TeamMailVerificationRequest()
        {
            string teamJWT = Request.Cookies["apolloJWT"];
            string teamId = Request.Cookies["apolloTeamId"];
            if(!string.IsNullOrEmpty(teamJWT) && !string.IsNullOrEmpty(teamId))
            {
                bool control = _teamService.TeamAuthenticator(teamJWT, Int16.Parse(teamId));
                if(control)
                {
                    bool verifyControl = _teamService.TeamMailVerificationControl(Int16.Parse(teamId));
                    if(verifyControl)
                    {
                        _teamService.SendMailVerification(Int16.Parse(teamId));
                        return Ok(true);
                    }
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.Unauthorized });
        }

        [HttpGet("/verification/{hashedData}")]
        public IActionResult TeamMailVerifyPage([FromQuery] string hashedData)
        {
            bool confirm = _teamService.TeamMailVerificationPageControl(hashedData);
            if(confirm)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
            }
        }

        [HttpPost("/verification/{hashedData")]
        public IActionResult TeamMailVerify([FromForm] int confirmationCode, [FromQuery] string hashedData)
        {
            bool pageConfirm = _teamService.TeamMailVerificationPageControl(hashedData);
            if(pageConfirm)
            {
                bool confirm = _teamService.TeamMailConfirmation(confirmationCode, hashedData);
                if(confirm)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(error: new { errorCode = ErrorCode.InvalidCode });
                }
            }
            else
            {
                return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
            }
        }
    }
}