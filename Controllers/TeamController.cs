using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> TeamRegister([FromForm] TeamRegisterViewModel teamVM)
        {
            bool controlResult = _teamService.TeamRegisterFormDataControl(teamVM);
            if(controlResult)
            {
                bool newUserControl = await _teamService.NewAccountControl(teamVM.MailAddress, teamVM.PhoneNumber);
                if(!newUserControl)
                {
                    await _teamService.CreateTeam(teamVM);
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
        public async Task<IActionResult> TeamLogin([FromForm] LoginViewModel teamVM)
        {
            Team teamControl = await _teamService.TeamLoginControl(teamVM);
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
        public async Task<IActionResult> TeamMailVerificationRequest()
        {
            string teamJWT = Request.Cookies["apolloJWT"];
            string teamId = Request.Cookies["apolloTeamId"];
            if(!string.IsNullOrEmpty(teamJWT) && !string.IsNullOrEmpty(teamId))
            {
                bool control = _teamService.TeamAuthenticator(teamJWT, Int16.Parse(teamId));
                if(control)
                {
                    bool verifyControl = await _teamService.TeamMailVerificationControl(Int16.Parse(teamId));
                    if(verifyControl)
                    {
                        await _teamService.SendMailVerification(Int16.Parse(teamId));
                        return Ok(true);
                    }
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.Unauthorized });
        }

        [HttpGet("/verification/{hashedData}")]
        public async Task<IActionResult> TeamMailVerifyPage([FromQuery] string hashedData)
        {
            bool confirm = await _teamService.TeamMailVerificationPageControl(hashedData);
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
        public async Task<IActionResult> TeamMailVerify([FromForm] int confirmationCode, [FromQuery] string hashedData)
        {
            bool pageConfirm = await _teamService.TeamMailVerificationPageControl(hashedData);
            if(pageConfirm)
            {
                bool confirm = await _teamService.TeamMailConfirmation(confirmationCode, hashedData);
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

        [HttpGet("/team-freeze-account")]
        public async Task<IActionResult> TeamFreezeAccount()
        {
            string teamJWT = Request.Cookies["apolloJWT"];
            string teamId = Request.Cookies["apolloTeamId"];
            if(!string.IsNullOrEmpty(teamJWT) && !string.IsNullOrEmpty(teamId))
            {
                bool control = await _teamService.FreezeTeamAccount(Int16.Parse(teamId));
                if(control)
                {
                    return Ok(true);   
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.Unauthorized });
        }

        [HttpPost("/team-forget-password")]
        public async Task<IActionResult> TeamForgetPassword([FromBody] string teamMailAdress)
        {
            if(!string.IsNullOrEmpty(teamMailAdress))
            {
                bool mailControl = await _teamService.TeamControlByMail(teamMailAdress);
                if(mailControl)
                {
                    await _teamService.SendPasswordCode(teamMailAdress);
                    return Ok(true);
                }
                else
                {
                    return BadRequest(error: new { errorCode = ErrorCode.UserNotFind });
                }
            }
            else
            {
                return BadRequest(error: new { errorCode = ErrorCode.MustBeFilled });
            }
        }

        [HttpGet("/password-reset/{hashedData}")]
        public async Task<IActionResult> TeamForgetPasswordConfirmation([FromQuery] string hashedData)
        {
            if(!string.IsNullOrEmpty(hashedData))
            {
                bool pageConfirm = await _teamService.TeamForgetPasswordConfirmationPageControl(hashedData);
                if(pageConfirm)
                {
                    return Ok(true);
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
        }

        [HttpPost("/player-reset/{hashedData}")]
        public async Task<IActionResult> TeamFOrgetPasswordConfirmation([FromQuery] string hashedData, [FromBody]  UserResetPasswordViewModel teamVM)
        {
            if(!string.IsNullOrEmpty(hashedData))
            {
                bool pageConfirm = await _teamService.TeamForgetPasswordConfirmationPageControl(hashedData);
                if(pageConfirm)
                {
                    bool confirm = await _teamService.TeamResetPassword(teamVM.ConfirmationCode, hashedData, teamVM.Password);
                    if(confirm)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(error: new { errorCode = ErrorCode.InvalidCode });
                    }
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
        }
    }
}