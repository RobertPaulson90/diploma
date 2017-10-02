using System.Collections.Generic;

namespace Diploma.DAL.Entities
{
    public class ProgrammerEntity : EmployeeEntity
    {
        public virtual ICollection<TeamMemberEntity> TeamsMemberships { get; set; }
    }
}
