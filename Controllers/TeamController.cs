using Apollo.Data;
using Apollo.Models;
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
                    ErrorModel errorModel = new ErrorModel("101", "Bu mail adresi veya telefon numarası daha önce kayıt edilmiş.");
                    string jsonObject = JsonConvert.SerializeObject(errorModel);
                    return Ok(jsonObject);
                }
            }
            else
            {
                ErrorModel errorModel = new ErrorModel("101", "Tüm alanlar doğru şekilde doldurulmalı.");
                string jsonObject = JsonConvert.SerializeObject(errorModel);
                return Ok(jsonObject);
            }
        }
    }
}