using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.Contracts.DTO;
using JetBrains.Annotations;

namespace Diploma.BLL.Contracts.Services
{
    public interface IUserService
    {
        [NotNull]
        Task<OperationResult<UserDto>> CreateUserAsync(
            [NotNull] CustomerRegistrationDataDto customerRegistrationData,
            CancellationToken cancellationToken = default(CancellationToken));

        [NotNull]
        Task<OperationResult<UserDto>> GetUserByCredentialsAsync(
            [NotNull] UserCredentialsDto userCredentials,
            CancellationToken cancellationToken = default(CancellationToken));

        [NotNull]
        Task<OperationResult<bool>> IsUsernameUniqueAsync(
            [NotNull] string username,
            CancellationToken cancellationToken = default(CancellationToken));

        [NotNull]
        Task<OperationResult<UserDto>> UpdateUserAsync(
            [NotNull] UserPersonalInfoDto userPersonalInfo,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
