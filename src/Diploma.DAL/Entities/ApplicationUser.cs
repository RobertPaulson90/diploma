using Microsoft.AspNetCore.Identity;

namespace Diploma.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserEntity User { get; set; }
    }
}