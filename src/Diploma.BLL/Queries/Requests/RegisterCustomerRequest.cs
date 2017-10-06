using System;
using Diploma.BLL.Queries.Responses;
using MediatR;

namespace Diploma.BLL.Queries.Requests
{
    public sealed class RegisterCustomerRequest : IRequest<OperationResult<UserDataResponse>>
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
