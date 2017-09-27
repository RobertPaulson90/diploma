using System.Collections.Generic;

namespace Diploma.Entities
{
    public class Manager : Employee
    {
        public virtual ICollection<Project> ManagedProjects { get; set; }
    }
}