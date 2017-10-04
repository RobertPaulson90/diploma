using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.Contracts.DTO;

namespace Diploma.BLL.Contracts.Services
{
    public interface IUserService
    {
        Task<OperationResult<UserDto>> CreateUserAsync(
            CustomerRegistrationDataDto customerRegistrationData,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<UserDto>> GetUserByCredentialsAsync(
            UserAuthorizationDataDto userAuthorizationData,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);

        Task<OperationResult<UserDto>> UpdateUserAsync(
            UserUpdateRequestDataDto userUpdateRequestData,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
