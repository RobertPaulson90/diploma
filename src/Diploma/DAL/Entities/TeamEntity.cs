using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diploma.DAL.Entities
{
    public class TeamEntity : BaseEntity
    {
        public virtual ICollection<TeamMemberEntity> InvolvedMembers { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ProjectEntity> WorkingProjects { get; set; }
    }
}
