using System.Collections.Generic;

namespace Diploma.WebAPI.Infrastructure.Entities
{
    public class ProgrammerEntity : EmployeeEntity
    {
        public virtual ICollection<TeamMemberEntity> TeamsMemberships { get; set; }
    }
}
