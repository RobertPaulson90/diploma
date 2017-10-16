using System.Collections.Generic;

namespace Diploma.WebAPI.Infrastructure.Entities
{
    public class ManagerEntity : EmployeeEntity
    {
        public virtual ICollection<ProjectEntity> ManagedProjects { get; set; }
    }
}
