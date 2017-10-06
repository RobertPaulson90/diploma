using System;

namespace Diploma.BLL.Queries.Responses
{
    public class UserDataResponse
    {
        public DateTime? BirthDate { get; set; }

        public string FirstName { get; set; }

        public GenderType Gender { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string PasswordHash { get; set; }

        public UserRoleType Role { get; set; }

        public string Username { get; set; }
    }
}
