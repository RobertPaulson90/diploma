using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using JetBrains.Annotations;

namespace Diploma.BLL.Services.Interfaces
{
    public interface IUserService
    {
        [NotNull]
        Task<OperationResult<UserDataResponse>> CreateCustomerAsync(
            [NotNull] RegisterCustomerRequest request,
            CancellationToken cancellationToken = default(CancellationToken));

        [NotNull]
        Task<OperationResult<UserDataResponse>> GetUserByCredentialsAsync(
            [NotNull] GetUserByCredentialsRequest request,
            CancellationToken cancellationToken = default(CancellationToken));

        [NotNull]
        Task<OperationResult<bool>> IsUsernameUniqueAsync(
            [NotNull] VerifyUsernameUniqueRequest request,
            CancellationToken cancellationToken = default(CancellationToken));

        [NotNull]
        Task<OperationResult<UserDataResponse>> UpdateUserAsync(
            [NotNull] UpdateUserDataRequest request,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
