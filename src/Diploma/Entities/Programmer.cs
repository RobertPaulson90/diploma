using System.Collections.Generic;

namespace Diploma.Entities
{
    public class Programmer : Employee
    {
        public virtual ICollection<TeamMember> TeamsMemberships { get; set; }
    }
}