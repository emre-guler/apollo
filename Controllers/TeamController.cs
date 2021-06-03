using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Entities;
using Apollo.Enums;
using Apollo.Services;
using Apollo.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        readonly AuthenticationService _authenticationService;
        public TeamController(
            ApolloContext db,
            TeamService teamService,
            AuthenticationService authenticationService
        )
        {
            _db = db;
            _teamService = teamService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("/team-register")]
        public async Task<IActionResult> TeamRegister([FromForm] TeamRegisterViewModel teamVM)
        {
            bool controlResult = _teamService.TeamRegisterFormDataControl(teamVM);
            if (controlResult)
            {
                bool newUserControl = await _teamService.NewAccountControl(teamVM.MailAddress, teamVM.PhoneNumber);
                if (!newUserControl)
                {
                    await _teamService.CreateTeam(teamVM);
                    return Ok(true);
                }
                else
                {
                    return Ok((int)ErrorCode.UserExists);
                }
            }
            else
            {
                return Ok((int) ErrorCode.MustBeFilled);
            }
        }

        [AllowAnonymous]
        [HttpPost("/team-login")]
        public async Task<IActionResult> TeamLogin([FromForm] LoginViewModel teamVM)
        {
            Team teamControl = await _teamService.TeamLoginControl(teamVM);
            if (teamControl is not null)
            {
                string teamJWT = _teamService.TeamLogin(teamControl);
                Response.Cookies.Append("apolloJWT", teamJWT, new CookieOptions
                {
                    HttpOnly = true
                });
                return Ok(true);
            }
            else
            {
                return Ok((int) ErrorCode.InvalidCredentials);
            }
        }

        [Authorize]
        [HttpPost("/team-logout")]
        public IActionResult TeamLogout()
        {
            Response.Cookies.Delete("apolloJWT");
            return Ok(true);
        }

        [Authorize]
        [HttpGet("/team-mail-verification")]
        public async Task<IActionResult> TeamMailVerificationRequest()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Team)
            {
                bool verifyControl = await _teamService.TeamMailVerificationControl(userData.Id);
                if (verifyControl)
                {
                    await _teamService.SendMailVerification(userData.Id);
                    return Ok(true);
                }
                return Ok((int) ErrorCode.UserNotFind);
            }
            return Ok((int) ErrorCode.Unauthorized);
        }

        [Authorize]
        [HttpGet("/verification/{hashedData}")]
        public async Task<IActionResult> TeamMailVerifyPage([FromQuery] string hashedData)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Team)
            {
                bool confirm = await _teamService.TeamMailVerificationPageControl(hashedData);
                if (confirm)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok((int) ErrorCode.LinkExpired);
                }
            }
            return Ok((int) ErrorCode.Unauthorized);
        }

        [Authorize]
        [HttpPost("/verification/{hashedData}")]
        public async Task<IActionResult> TeamMailVerify([FromForm] int confirmationCode, [FromQuery] string hashedData)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Team)
            {
                bool pageConfirm = await _teamService.TeamMailVerificationPageControl(hashedData);
                if (pageConfirm)
                {
                    bool confirm = await _teamService.TeamMailConfirmation(confirmationCode, hashedData);
                    if (confirm)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return Ok((int) ErrorCode.InvalidCode);
                    }
                }
                else
                {
                    return Ok((int) ErrorCode.LinkExpired);
                }
            }
            return Ok((int) ErrorCode.Unauthorized);
        }

        [Authorize]
        [HttpGet("/team-freeze-account")]
        public async Task<IActionResult> TeamFreezeAccount()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Team)
            {
                bool control = await _teamService.FreezeTeamAccount(userData.Id);
                if (control)
                {
                    return Ok(true);
                }
                return Ok((int) ErrorCode.UserNotFind);
            }
            return Ok((int) ErrorCode.Unauthorized);
        }

        [AllowAnonymous]
        [HttpPost("/team-forget-password")]
        public async Task<IActionResult> TeamForgetPassword([FromBody] string teamMailAdress)
        {
            if (!string.IsNullOrEmpty(teamMailAdress))
            {
                bool mailControl = await _teamService.TeamControlByMail(teamMailAdress);
                if (mailControl)
                {
                    await _teamService.SendPasswordCode(teamMailAdress);
                    return Ok(true);
                }
                else
                {
                    return Ok((int) ErrorCode.UserNotFind);
                }
            }
            else
            {
                return Ok((int) ErrorCode.MustBeFilled);
            }
        }

        [AllowAnonymous]
        [HttpGet("/password-reset/{hashedData}")]
        public async Task<IActionResult> TeamForgetPasswordConfirmation([FromQuery] string hashedData)
        {
            if (!string.IsNullOrEmpty(hashedData))
            {
                bool pageConfirm = await _teamService.TeamForgetPasswordConfirmationPageControl(hashedData);
                if (pageConfirm)
                {
                    return Ok(true);
                }
            }
            return Ok((int) ErrorCode.LinkExpired);
        }


        [AllowAnonymous]
        [HttpPost("/player-reset/{hashedData}")]
        public async Task<IActionResult> TeamFOrgetPasswordConfirmation([FromQuery] string hashedData, [FromBody] UserResetPasswordViewModel teamVM)
        {
            if (!string.IsNullOrEmpty(hashedData))
            {
                bool pageConfirm = await _teamService.TeamForgetPasswordConfirmationPageControl(hashedData);
                if (pageConfirm)
                {
                    bool confirm = await _teamService.TeamResetPassword(teamVM.ConfirmationCode, hashedData, teamVM.Password);
                    if (confirm)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return Ok((int) ErrorCode.InvalidCode);
                    }
                }
            }
            return Ok((int) ErrorCode.LinkExpired);
        }
    }
}