using System.ComponentModel.DataAnnotations;

namespace Diploma.DAL.Entities
{
    public class TeamMemberEntity : BaseEntity
    {
        public virtual ProgrammerEntity Programmer { get; set; }

        public int ProgrammerId { get; set; }

        public virtual TeamEntity Team { get; set; }

        public int TeamId { get; set; }

        [Required]
        public string TeamRole { get; set; }
    }
}
