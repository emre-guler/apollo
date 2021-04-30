using Microsoft.EntityFrameworkCore;
using Apollo.Entities;

namespace Apollo.Data 
{
    public class ApolloContext : DbContext 
    {
        public ApolloContext (DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Özel veri girişi
        }

        public DbSet<Achievement> Achievements { get; set; } 
        public DbSet<City> Cities { get; set; }
        public DbSet<PlayerGame> PlayerGames { get; set; }
        public DbSet<OldTeam> OldTeams { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerCity> PlayerCities { get; set; }
        public DbSet<PlayerPhoto> PlayerPhotos { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<PlayerVideo> PlayerVideos { get; set; }
    }
}