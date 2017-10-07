using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Requests
{
    public sealed class VerifyUsernameUniqueRequest : IRequest<OperationResult<bool>>
    {
        public string Username { get; set; }
    }
}
