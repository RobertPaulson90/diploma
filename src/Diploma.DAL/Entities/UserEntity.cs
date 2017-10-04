using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diploma.DAL.Entities.Enums;
using SQLite.CodeFirst;

namespace Diploma.DAL.Entities
{
    public abstract class UserEntity
    {
        public DateTime? BirthDate { get; set; }

        [Required]
        public string FirstName { get; set; }

        public GenderType Gender { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [Unique]
        [MinLength(5)]
        [MaxLength(30)]
        public string Username { get; set; }
    }
}
