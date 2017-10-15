using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.DAL.Entities
{
    public class TeamEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ICollection<TeamMemberEntity> InvolvedMembers { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ProjectTeamEntity> WorkingProjects { get; set; }
    }
}
