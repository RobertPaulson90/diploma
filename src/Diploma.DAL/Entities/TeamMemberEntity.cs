using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.DAL.Entities
{
    public class TeamMemberEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ProgrammerEntity Programmer { get; set; }

        public int ProgrammerId { get; set; }

        public virtual TeamEntity Team { get; set; }

        public int TeamId { get; set; }

        public string TeamRole { get; set; }
    }
}
