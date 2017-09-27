using System.Collections.Generic;

namespace Diploma.DAL.Entities
{
    public class CustomerEntity : UserEntity
    {
        public virtual ICollection<ProjectEntity> Projects { get; set; }
    }
}
