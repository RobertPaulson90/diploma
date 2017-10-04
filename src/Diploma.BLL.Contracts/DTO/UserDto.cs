using System;
using Diploma.BLL.Contracts.DTO.Enums;

namespace Diploma.BLL.Contracts.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public GenderType Gender { get; set; }

        public UserRoleType Role { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }
    }
}