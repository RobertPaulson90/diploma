using System;
using System.Threading;
using System.Threading.Tasks;
using Diploma.DAL.Entities;
using Diploma.Framework;
using Diploma.Models;

namespace Diploma.Infrastructure
{
    public interface IUserService
    {
        Task<OperationResult<UserEntity>> CreateCustomerAsync(
            string username,
            string password,
            string lastName,
            string firstName,
            string middleName,
            DateTime? birthDate,
            GenderType gender,
            CancellationToken cancellationToken = default(CancellationToken));

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

        Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);

        Task<OperationResult<UserEntity>> GetUserByCredentialsAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
