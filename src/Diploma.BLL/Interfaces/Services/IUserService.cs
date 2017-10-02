using System;
using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.DTO.Enums;
using Diploma.Common;
using Diploma.DAL.Entities;
using Diploma.DAL.Entities.Enums;

namespace Diploma.BLL.Interfaces.Services
{
    public interface IUserService
    {
        Task<OperationResult<UserEntity>> CreateUserAsync(
            string username,
            string password,
            string lastName,
            string firstName,
            string middleName,
            UserRoleType userRole,
            DateTime? birthDate,
            GenderType gender,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<UserEntity>> GetUserByCredentialsAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
    }
}
