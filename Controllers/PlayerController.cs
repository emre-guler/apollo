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

        [HttpPost("/player-register")]
        public async Task<IActionResult> PlayerRegister([FromForm] PlayerRegisterViewModel playerVM) 
        {
            bool controlResult = _playerService.PlayerRegisterFormDataControl(playerVM);
            if(controlResult)
            {
                bool newUserControl = await _playerService.NewAccountControl(playerVM.MailAddress, playerVM.PhoneNumber);
                if(newUserControl)
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

        [HttpPost("/player-login")]
        public async Task<IActionResult> PlayerLogin([FromForm] LoginViewModel playerVM)
        {
            Player userControl = await _playerService.PlayerLoginControl(playerVM);
            if(userControl is not null)
            { 
                string userJWT = _playerService.PlayerLogin(userControl.Id);
                Response.Cookies.Append("apolloJWT", userJWT, new CookieOptions 
                {
                    HttpOnly = true
                });
                Response.Cookies.Append("apolloPlayerId", userControl.Id.ToString(), new CookieOptions 
                {
                    HttpOnly = true
                });
                return Ok(true);
            }
            return BadRequest(error: new { errorCode = ErrorCode.InvalidCredentials });
        }

        [HttpPost("/player-logout")]
        public IActionResult PlayerLogout()
        {
            Response.Cookies.Delete("apolloJWT");
            Response.Cookies.Delete("apolloPlayerId");
            return Ok(true);
        }

        [HttpPost("/player-buildup-profile")]
        public async Task<IActionResult> PlayerBuildUpProfile([FromForm] PlayerBuildUpViewModel playerVM)
        {
            string userJWT = Request.Cookies["apolloJWT"];
            string userId = Request.Cookies["apolloPlayerId"];
            if(!string.IsNullOrEmpty(userJWT) && !string.IsNullOrEmpty(userId))
            {
                bool control = _playerService.PlayerAuthenticator(userJWT, Int16.Parse(userId));
                if(control)
                {
                    bool result = await _playerService.BuilUpYourProfile(playerVM, userJWT, userId);
                    if(result)
                    {
                        return Ok(true);
                    }
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.Unauthorized });
        }

        [HttpGet("/player-mail-verification")]
        public async Task<IActionResult> PlayerMailVerificationRequest()
        {
            string userJWT = Request.Cookies["apolloJWT"];
            string userId = Request.Cookies["apolloPlayerId"];
            if(!string.IsNullOrEmpty(userJWT) && !string.IsNullOrEmpty(userId))
            {
                bool control = _playerService.PlayerAuthenticator(userJWT, Int16.Parse(userId));
                if(control)
                {
                    bool verifyControl = await _playerService.PlayerMailVerificationControl(Int16.Parse(userId));
                    if(verifyControl)
                    {
                        await _playerService.SendMailVerification(Int16.Parse(userId));
                        return Ok(true);
                    }
                }
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }

        [HttpGet("verification/{hashedData}")]
        public async Task<IActionResult> PlayerMailVerifyPage([FromQuery] string hashedData)
        {
            bool confirm = await _playerService.PlayerMailVerificationPageControl(hashedData);
            if(confirm)
            {
                return Ok(true);
            }
            else 
            {
                return BadRequest(error: new { errorCode = ErrorCode.LinkExpired });
            }
        }

        [HttpPost("/verification/{hashedData}")]
        public async Task<IActionResult> PlayerMailVerify([FromForm] int confirmationCode, [FromQuery] string hashedData)
        {
            bool pageConfirm = await _playerService.PlayerMailVerificationPageControl(hashedData);
            if(pageConfirm)
            {
                bool confirm = await _playerService.PlayerMailConfirmation(confirmationCode, hashedData);
                if(confirm)
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

        [HttpGet("/player-update-state")]
        public async Task<IActionResult> PlayerUpdateState()
        {
            string userJWT = Request.Cookies["apolloJWT"];
            string userId = Request.Cookies["apolloPlayerId"];
            if(!string.IsNullOrEmpty(userJWT) && !string.IsNullOrEmpty(userId))
            {
                bool control = await _playerService.PlayerUpdateState(Int16.Parse(userId));
                if(control)
                {
                    return Ok(true);
                }
            }
            return BadRequest(error: new { errorCode = ErrorCode.Unauthorized });
        }

        [HttpGet("/player-freeze-account")]
        public async Task<IActionResult> PlayerFreezeAccount()
        {
            string userJWT = Request.Cookies["apolloJWT"];
            string userId = Request.Cookies["apolloPlayerId"];
            if(!string.IsNullOrEmpty(userJWT) && !string.IsNullOrEmpty(userId))
            {
                bool control = await _playerService.FreezePlayerAccount(Int16.Parse(userId));
                if(control)
                {
                    return Ok(true);
                }
            }
            return BadRequest(error: new { erroCode = ErrorCode.Unauthorized });
        }
    }
}