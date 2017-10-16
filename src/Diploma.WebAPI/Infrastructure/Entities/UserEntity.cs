using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.WebAPI.Infrastructure.Entities
{
    public enum GenderType
    {
        Unspecified,

        Female,

        Male,

        Other
    }
    
    public abstract class UserEntity
    {
        public DateTime? BirthDate { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Username { get; set; }
    }
}
