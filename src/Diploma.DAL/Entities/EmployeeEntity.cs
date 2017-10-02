using System.ComponentModel.DataAnnotations;

namespace Diploma.DAL.Entities
{
    public abstract class EmployeeEntity : UserEntity
    {
        [Required]
        public decimal Salary { get; set; }
    }
}
