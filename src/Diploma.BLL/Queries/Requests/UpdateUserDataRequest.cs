using System;
using Diploma.BLL.Queries.Responses;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Requests
{
    public class UpdateUserDataRequest : IRequest<OperationResult<UserDataResponse>>
    {
        public DateTime? BirthDate { get; set; }

        public string FirstName { get; set; }

        public GenderType Gender { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }
    }
}
