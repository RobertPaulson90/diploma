using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.DAL.Entities
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

        public string FirstName { get; set; }

        public GenderType Gender { get; set; }
        
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
    }
}
