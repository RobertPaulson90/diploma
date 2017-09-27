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
        Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);

        Task<OperationResult<UserEntity>> SignInAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<UserEntity>> SignUp(
            string username,
            string password,
            string lastName,
            string firstName,
            string middleName,
            UserRoleType userRole,
            DateTime? birthDate,
            GenderType? gender,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
