using System.ComponentModel.DataAnnotations.Schema;
using Apollo.Enums;

namespace Apollo.Entities 
{
    public class OldTeam : BaseEntity
    {
        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public string TeamName { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        [ForeignKey("GameId")]
        public int GameId { get; set; }
        public virtual Game Game { get; set;}
    }
}