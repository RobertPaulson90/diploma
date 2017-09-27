using System.ComponentModel.DataAnnotations;

namespace Diploma.Entities
{
    public abstract class Employee : User
    {
        [Required]
        public decimal Salary { get; set; }
    }
}