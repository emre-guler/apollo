using System;
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
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public string Description { get; set; }
        public virtual Game Game { get; set;}
    }
}