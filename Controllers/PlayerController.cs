using Apollo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Apollo.Models;
using Newtonsoft.Json;
using Apollo.Services;
using Apollo.Data;
using Apollo.Entities;

namespace Apollo.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase 
    {
        readonly ApolloContext _db;
        readonly PlayerService _playerService;

        public PlayerController(
            ApolloContext db,
            PlayerService playerService
        )
        {
            _db = db;
            _playerService = playerService;
        }

        [HttpPost("/player-register")]
        public IActionResult PlayerRegister(PlayerRegisterViewModel playerVM) 
        {
            bool controlResult = _playerService.PlayerRegisterFormDataControl(playerVM);
            if(controlResult)
            {
                bool newUserControl = _playerService.NewAccountControl(playerVM.MailAddress, playerVM.PhoneNumber);
                if(newUserControl)
                {
                    _playerService.CreatePlayer(playerVM);
                    return Ok(true);
                }
                else 
                {
                    ErrorModel errorModel = new ErrorModel("101", "Bu mail adresi veya telefon numarası daha önce kayıt edilmiş.");
                    string jsonObject = JsonConvert.SerializeObject(errorModel);
                    return BadRequest(jsonObject);
                }
            }
            else 
            {
                ErrorModel errorModel = new ErrorModel("100", "Tüm alanlar doğru şekilde doldurulmalı.");
                string jsonObject = JsonConvert.SerializeObject(errorModel);
                return BadRequest(jsonObject);
            }
        }

        [HttpPost("/player-login")]
        public IActionResult PlayerLogin(PlayerLoginViewModel playerVM)
        {
            Player userControl =  _playerService.PlayerLoginControl(playerVM);
            if(userControl != null)
            {
               PlayerViewModel allUserData =  _playerService.PlayerLogin(userControl);
               string jsonObject = JsonConvert.SerializeObject(allUserData);
               return Ok(jsonObject);
            }
            else
            {
                ErrorModel errorModel = new ErrorModel("102", "Böyle bir kullanıcı yok veya şifre yanlış.");
                string jsonObject = JsonConvert.SerializeObject(errorModel);
                return BadRequest(jsonObject);
            }
        }
    }
}