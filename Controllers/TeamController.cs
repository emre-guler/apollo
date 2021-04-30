using Apollo.Data;
using Apollo.Models;
using Apollo.Services;
using Apollo.ViewModelds;
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
        public string TeamRegister(TeamRegisterViewModel teamVM)
        {
            bool controlResult = _teamService.TeamRegisterFormDataControl(teamVM);
            if(controlResult)
            {
                _teamService.CreateTeam(teamVM);
                return "true";
            }
            else
            {
                ErrorModel errorModel = new ErrorModel("101", "Tüm alanlar doğru şekilde doldurulmalı.");
                string jsonObject = JsonConvert.SerializeObject(errorModel);
                return jsonObject;
            }
        }
    }
}