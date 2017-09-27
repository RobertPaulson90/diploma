using System.Collections.Generic;

namespace Diploma.Entities
{
    public class Customer : User
    {
        public virtual ICollection<Project> Projects { get; set; }
    }
}