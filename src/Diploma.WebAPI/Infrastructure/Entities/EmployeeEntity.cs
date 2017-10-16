using System.ComponentModel.DataAnnotations;

namespace Diploma.WebAPI.Infrastructure.Entities
{
    public abstract class EmployeeEntity : UserEntity
    {
        [Required]
        public decimal Salary { get; set; }
    }
}
