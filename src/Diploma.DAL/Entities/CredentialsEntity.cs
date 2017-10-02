using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;

namespace Diploma.DAL.Entities
{
    public class CredentialsEntity
    {
        [Required]
        public string Password { get; set; }

        public virtual UserEntity User { get; set; }

        [Key]
        public int UserId { get; set; }

        [Required]
        [Unique]
        [MinLength(5)]
        [MaxLength(30)]
        public string Username { get; set; }
    }
}
