using System.Collections.Generic;

namespace Diploma.DAL.Entities
{
    public class ManagerEntity : EmployeeEntity
    {
        public virtual ICollection<ProjectEntity> ManagedProjects { get; set; }
    }
}
