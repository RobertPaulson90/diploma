using System;
using System.ComponentModel.DataAnnotations;

namespace Diploma.DAL.Entities
{
    public class UserEntity : BaseEntity
    {
        public DateTime? BirthDate { get; set; }

        public CredentialsEntity Credentials { get; set; }

        [Required]
        public string FirstName { get; set; }

        public GenderType Gender { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }
    }
}
