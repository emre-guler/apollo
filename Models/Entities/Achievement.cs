using System.ComponentModel.DataAnnotations.Schema;
using Apollo.Enums;

namespace Apollo.Entities
{
    public class Achievement : BaseEntity 
    {
        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        [ForeignKey("TournamentId")]
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
        public Rank Rank { get; set; }
    }
}