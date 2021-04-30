using Apollo.ViewModelds;
using Microsoft.AspNetCore.Mvc;
using Apollo.Models;
using Newtonsoft.Json;
using Apollo.Services;
using Apollo.Data;

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
        public string PlayerRegister(PlayerRegisterViewModel playerVM) 
        {
            bool controlResult = _playerService.PlayerRegisterFormDataControl(playerVM);
            if(controlResult)
            {
                bool newUserControl = _playerService.NewAccountControl(playerVM.MailAddress, playerVM.PhoneNumber);
                if(newUserControl)
                {
                    _playerService.CreatePlayer(playerVM);
                    return "true";
                }
                else 
                {
                    ErrorModel errorModel = new ErrorModel("101", "Bu mail adresi veya telefon numarası daha önce kayıt edilmiş.");
                    string jsonObject = JsonConvert.SerializeObject(errorModel);
                    return jsonObject;
                }
            }
            else 
            {
                ErrorModel errorModel = new ErrorModel("100", "Tüm alanlar doğru şekilde doldurulmalı.");
                string jsonObject = JsonConvert.SerializeObject(errorModel);
                return jsonObject;
            }
        }
    }
}