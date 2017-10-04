using System;
using Diploma.BLL.Contracts.DTO.Enums;

namespace Diploma.BLL.Contracts.DTO
{
    public class CustomerRegistrationDataDto
    {
        public DateTime? BirthDate { get; set; }

        public string FirstName { get; set; }

        public GenderType Gender { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}
