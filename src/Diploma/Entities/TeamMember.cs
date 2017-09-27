using System.ComponentModel.DataAnnotations;

namespace Diploma.Entities
{
    public class TeamMember : BaseEntity
    {
        [Required]
        public string TeamRole { get; set; }

        public virtual Programmer Programmer { get; set; }

        public int ProgrammerId { get; set; }

        public virtual Team Team { get; set; }

        public int TeamId { get; set; }
    }
}