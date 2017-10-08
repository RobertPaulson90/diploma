using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult<UserDataResponse>> CreateCustomerAsync(
            RegisterCustomerRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _mediator.Send(request, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        public async Task<OperationResult<UserDataResponse>> GetUserByCredentialsAsync(
            GetUserByCredentialsRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _mediator.Send(request, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        public async Task<OperationResult<bool>> IsUsernameUniqueAsync(VerifyUsernameUniqueRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        public async Task<OperationResult<UserDataResponse>> UpdateUserAsync(
            UpdateUserDataRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _mediator.Send(request, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }
    }
}
