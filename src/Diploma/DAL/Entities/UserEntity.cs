using System;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;

namespace Diploma.DAL.Entities
{
    public abstract class UserEntity : BaseEntity
    {
        public DateTime? BirthDate { get; set; }

        [Required]
        public string FirstName { get; set; }

        public GenderType Gender { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Unique]
        [MinLength(5)]
        [MaxLength(30)]
        public string Username { get; set; }
    }
}
