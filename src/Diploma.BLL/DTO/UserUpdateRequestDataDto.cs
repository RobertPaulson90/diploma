using System;
using Diploma.BLL.DTO.Enums;

namespace Diploma.BLL.DTO
{
    public class UserUpdateRequestDataDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public GenderType Gender { get; set; }
        
        public DateTime? BirthDate { get; set; }
    }
}