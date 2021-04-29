using Apollo.Enums;

namespace Apollo.Entities
{
    public class Tournament : BaseEntity
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public Game Game { get; set; }
    }
}