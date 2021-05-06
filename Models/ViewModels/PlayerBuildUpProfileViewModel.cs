using System;
using System.Collections.Generic;
using Apollo.Enums;
using Microsoft.AspNetCore.Http;

namespace Apollo.ViewModels
{
    public class PlayerBuildUpViewModel 
    {
        public string TwitterContact { get; set; }
        public string FacebookContact { get; set; }
        public string YoutubeContact { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public int SalaryException { get; set; }
        public bool IsActiveForTeam { get; set; }
        public List<int> AvailableCities { get; set; }
        public List<Game> PlayerGames { get; set; }
        public List<IFormFile> UserPhotos { get; set; }
        public List<IFormFile> UserVideos { get; set; }
        public List<TournamentAchievement> TournamentAchievements { get; set; } 
        public List<OldTeam> OldTeams { get; set; }
    }

    public class TournamentAchievement 
    {
        public int TournamentId { get; set; }
        public Rank Rank { get; set; }
    }
    
    public class OldTeam
    {
        public string TeamName { get; set; }
        public string Description { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public Game Game { get; set; }
    }
}