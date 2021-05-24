using Apollo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Apollo.Services;
using Apollo.Data;
using Apollo.Entities;
using Apollo.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace Apollo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        readonly ApolloContext _db;
        readonly PlayerService _playerService;
        readonly AuthenticationService _authenticationService;

        public PlayerController(
            ApolloContext db,
            PlayerService playerService,
            AuthenticationService authenticationService
        )
        {
            _db = db;
            _playerService = playerService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("/player-register")]
        public async Task<IActionResult> PlayerRegister([FromForm] PlayerRegisterViewModel playerVM)
        {
            bool controlResult = _playerService.PlayerRegisterFormDataControl(playerVM);
            if (controlResult)
            {
                bool newUserControl = await _playerService.NewAccountControl(playerVM.MailAddress, playerVM.PhoneNumber);
                if (newUserControl)
                {
                    await _playerService.CreatePlayer(playerVM);
                    return Ok(true);
                }
                else
                {
                    return BadRequest(error: new { errorCode = ErrorCode.UserExists });
                }
            }
            else
            {
                return BadRequest(error: new { errorCode = ErrorCode.MustBeFilled });
            }
        }

        [AllowAnonymous]
        [HttpPost("/player-login")]
        public async Task<IActionResult> PlayerLogin([FromForm] LoginViewModel playerVM)
        {
            Player userControl = await _playerService.PlayerLoginControl(playerVM);
            if (userControl is not null)
            {
                string userJWT = _playerService.PlayerLogin(userControl);
                Response.Cookies.Append("apolloJWT", userJWT, new CookieOptions
                {
                    HttpOnly = true
                });
                return Ok(true);
            }
            return BadRequest(error: new { errorCode = ErrorCode.InvalidCredentials });
        }

        [Authorize]
        [HttpPost("/player-logout")]
        public IActionResult PlayerLogout()
        {
            Response.Cookies.Delete("apolloJWT");
            return Ok(true);
        }

        [Authorize]
        [HttpPost("/player-buildup-profile")]
        public async Task<IActionResult> PlayerBuildUpProfile([FromForm] PlayerBuildUpViewModel playerVM)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Player)
            {
                bool result = await _playerService.BuilUpYourProfile(playerVM, userData.Id);
                if (result)
                {
                    return Ok(true);
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.Unauthorized });
        }

        [Authorize]
        [HttpGet("/player-mail-verification")]
        public async Task<IActionResult> PlayerMailVerificationRequest()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Player)
            {
                bool verifyControl = await _playerService.PlayerMailVerificationControl(userData.Id);
                if (verifyControl)
                {
                    await _playerService.SendMailVerification(userData.Id);
                    return Ok(true);
                }
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }

        [Authorize]
        [HttpGet("/verification/{hashedData}")]
        public async Task<IActionResult> PlayerMailVerifyPage([FromQuery] string hashedData)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Player)
            {
                bool confirm = await _playerService.PlayerMailVerificationPageControl(hashedData);
                if (confirm)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
                }
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }

        [Authorize]
        [HttpPost("/verification/{hashedData}")]
        public async Task<IActionResult> PlayerMailVerify([FromForm] int confirmationCode, [FromQuery] string hashedData)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Player)
            {
                bool pageConfirm = await _playerService.PlayerMailVerificationPageControl(hashedData);
                if (pageConfirm)
                {
                    bool confirm = await _playerService.PlayerMailConfirmation(confirmationCode, hashedData);
                    if (confirm)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(error: new { erroCode = ErrorCode.InvalidCode });
                    }
                }
                else
                {
                    return BadRequest(error: new { erroCode = ErrorCode.LinkExpired });
                }
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }

        [Authorize]
        [HttpGet("/player-update-state")]
        public async Task<IActionResult> PlayerUpdateState()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Player)
            {
                bool control = await _playerService.PlayerUpdateState(userData.Id);
                if (control)
                {
                    return Ok(true);
                }
                return BadRequest(error: new { errorCode = ErrorCode.UserNotFind });
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }

        [Authorize]
        [HttpGet("/player-freeze-account")]
        public async Task<IActionResult> PlayerFreezeAccount()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var userData = _authenticationService.GetUserData(claims);
            if (userData is not null && userData.UserType == UserType.Player)
            {
                bool control = await _playerService.FreezePlayerAccount(userData.Id);
                if (control)
                {
                    return Ok(true);
                }
                return BadRequest(error: new { erroCode = ErrorCode.UserNotFind });
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }

        [AllowAnonymous]
        [HttpPost("/player-forget-password")]
        public async Task<IActionResult> PlayerForgetPassword([FromBody] string playerMailAddress)
        {
            if (!string.IsNullOrEmpty(playerMailAddress))
            {
                bool mailControl = await _playerService.PlayerControlByMail(playerMailAddress);
                if (mailControl)
                {
                    await _playerService.SendPasswordCode(playerMailAddress);
                    return Ok(true);
                }
                else
                {
                    return BadRequest(error: new { erroCode = ErrorCode.UserNotFind });
                }
            }
            else
            {
                return BadRequest(error: new { errorCode = ErrorCode.MustBeFilled });
            }
        }

        [AllowAnonymous]
        [HttpGet("/password-reset/{hashedData}")]
        public async Task<IActionResult> PlayerForgetPasswordConfirmation([FromQuery] string hashedData)
        {
            if (!string.IsNullOrEmpty(hashedData))
            {
                bool pageConfirm = await _playerService.PlayerForgetPasswordConfirmationPageControl(hashedData);
                if (pageConfirm)
                {
                    return Ok(true);
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
        }


        [AllowAnonymous]
        [HttpPost("/password-reset/{hashedData}")]
        public async Task<IActionResult> PlayerForgetPasswordConfirmation([FromQuery] string hashedData, [FromBody] UserResetPasswordViewModel playerVM)
        {
            if (!string.IsNullOrEmpty(hashedData))
            {
                bool pageConfirm = await _playerService.PlayerForgetPasswordConfirmationPageControl(hashedData);
                if (pageConfirm)
                {
                    bool confirm = await _playerService.PlayerResetPassword(playerVM.ConfirmationCode, hashedData, playerVM.Password);
                    if (confirm)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(error: new { errorCode = ErrorCode.InvalidCode });
                    }
                }
            }
            return BadRequest(error: new { erroCode = ErrorCode.LinkExpired });
        }
    }
}