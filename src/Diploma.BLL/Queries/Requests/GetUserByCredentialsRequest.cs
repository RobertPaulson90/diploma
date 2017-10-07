using Diploma.BLL.Queries.Responses;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Requests
{
    public sealed class GetUserByCredentialsRequest : IRequest<OperationResult<UserDataResponse>>
    {
        public string Password { get; set; }

        public string Username { get; set; }
    }
}
