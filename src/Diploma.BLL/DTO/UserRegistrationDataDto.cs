using System;
using Diploma.BLL.DTO.Enums;

namespace Diploma.BLL.DTO
{
    public class UserRegistrationDataDto
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public GenderType Gender { get; set; }

        public UserRoleType Role { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
