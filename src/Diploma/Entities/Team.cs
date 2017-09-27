using System.Collections.Generic;

namespace Diploma.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<TeamMember> InvolvedMembers { get; set; }

        public virtual ICollection<Project> WorkingProjects { get; set; }
    }
}