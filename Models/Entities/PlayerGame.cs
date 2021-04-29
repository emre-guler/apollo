using System.ComponentModel.DataAnnotations.Schema;
using Apollo.Enums;

namespace Apollo.Entities 
{
    public class PlayerGame : BaseEntity
    {
        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public Game GameType { get; set; }
    }
}