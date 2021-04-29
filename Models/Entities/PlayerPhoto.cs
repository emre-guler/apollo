using System.ComponentModel.DataAnnotations.Schema;

namespace Apollo.Entities
{
    public class PlayerPhoto : BaseEntity
    {
        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        [ForeignKey("PhotoId")]
        public int PhotoId { get; set; }
        public virtual Photo Photo { get; set; }
    }
}