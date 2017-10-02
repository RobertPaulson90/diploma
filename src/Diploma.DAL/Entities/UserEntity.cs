using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diploma.DAL.Entities.Enums;

namespace Diploma.DAL.Entities
{
    public abstract class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public GenderType Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public virtual CredentialsEntity Credentials { get; set; }
    }
}
