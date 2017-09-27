using System;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;

namespace Diploma.Entities
{
    public abstract class User : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string MiddleName { get; set; }

        public GenderType? Gender { get; set; }
        
        public DateTime? BirthDate { get; set; }

        [Required]
        [Unique]
        [MinLength(6)]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}