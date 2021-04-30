using System.ComponentModel.DataAnnotations.Schema;

namespace Apollo.Entities
{
    public class PlayerVideo : BaseEntity 
    {
        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        [ForeignKey("VideoId")]
        public int VideoId { get; set; }
        public virtual Video Video { get; set; }
    }
}