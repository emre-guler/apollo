using System.ComponentModel.DataAnnotations.Schema;

namespace Apollo.Entities 
{
    public class PlayerCity : BaseEntity
    {
        [ForeignKey("PlayerId")]
        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }
        [ForeignKey("CityId")]
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}