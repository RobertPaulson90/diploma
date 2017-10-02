using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.DTO;
using Diploma.BLL.DTO.Enums;
using Diploma.BLL.Interfaces.Services;
using Diploma.Common;
using Diploma.Common.Properties;
using Diploma.DAL.Contexts;
using Diploma.DAL.Entities;

namespace Diploma.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        private readonly ICryptoService _cryptoService;

        public UserService(Func<CompanyContext> companyContextFactory, ICryptoService cryptoService)
        {
            _companyContextFactory = companyContextFactory;
            _cryptoService = cryptoService;
        }

        public async Task<OperationResult<UserDto>> CreateUserAsync(
            UserRegistrationDataDto userRegistrationData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var userEntity = UserRegistrationDataDtoToUserEntity(userRegistrationData);

                using (var context = _companyContextFactory())
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var dbUser = context.Users.Add(userEntity);
                        await context.SaveChangesAsync(cancellationToken);
                        transaction.Commit();

                        var userDto = UserEntityToUserDto(dbUser);

                        return OperationResult<UserDto>.CreateSuccess(userDto);
                    }
                    catch (TaskCanceledException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult<UserDto>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<UserDto>> GetUserByCredentialsAsync(
            UserAuthorizationDataDto userAuthorizationData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var context = _companyContextFactory())
                {
                    var credentialsEntity = await context.Credentials.AsNoTracking().SingleOrDefaultAsync(
                        UserNameEquals(userAuthorizationData.Username),
                        cancellationToken);

                    if (credentialsEntity == null)
                    {
                        return OperationResult<UserDto>.CreateFailure(Resources.Authorization_Username_Not_Found);
                    }

                    if (_cryptoService.VerifyPasswordHash(userAuthorizationData.Password, credentialsEntity.PasswordHash))
                    {
                        var dbUser = credentialsEntity.User;
                        var userDto = UserEntityToUserDto(dbUser);
                        return OperationResult<UserDto>.CreateSuccess(userDto);
                    }

                    return OperationResult<UserDto>.CreateFailure(Resources.Authorization_Username_Or_Password_Invalid);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<UserDto>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            try
            {
                using (var context = _companyContextFactory())
                {
                    var isUnique = !await context.Credentials.AnyAsync(UserNameEquals(username), cancellationToken);
                    return OperationResult<bool>.CreateSuccess(isUnique);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex);
            }
        }

        private static UserEntity GetUserEntityByRole(UserRegistrationDataDto userRegistrationData)
        {
            UserEntity userEntity;
            switch (userRegistrationData.Role)
            {
                case UserRoleType.Customer:
                    userEntity = new CustomerEntity();
                    break;
                case UserRoleType.Programmer:
                    userEntity = new ProgrammerEntity();
                    break;
                case UserRoleType.Manager:
                    userEntity = new ManagerEntity();
                    break;
                case UserRoleType.Admin:
                    userEntity = new AdminEntity();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return userEntity;
        }

        private static UserDto UserEntityToUserDto(UserEntity dbUser)
        {
            return new UserDto
            {
                Username = dbUser.Credentials.Username,
                PasswordHash = dbUser.Credentials.PasswordHash,
                BirthDate = dbUser.BirthDate,
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                MiddleName = dbUser.MiddleName,
                Gender = (GenderType)dbUser.Gender,
                Role = UserEntityToUserRoleType(dbUser),
                Id = dbUser.Id
            };
        }

        private static UserRoleType UserEntityToUserRoleType(UserEntity dbUser)
        {
            if (dbUser is CustomerEntity)
            {
                return UserRoleType.Customer;
            }

            if (dbUser is ProgrammerEntity)
            {
                return UserRoleType.Programmer;
            }

            if (dbUser is ManagerEntity)
            {
                return UserRoleType.Manager;
            }

            if (dbUser is AdminEntity)
            {
                return UserRoleType.Admin;
            }

            throw new ArgumentException();
        }

        [SuppressMessage("ReSharper", "SpecifyStringComparison", Justification = "This must be used explicit cuz of LINQ to entities.")]
        private static Expression<Func<CredentialsEntity, bool>> UserNameEquals(string username)
        {
            return x => username.ToUpper() == x.Username.ToUpper();
        }

        private UserEntity UserRegistrationDataDtoToUserEntity(UserRegistrationDataDto userRegistrationData)
        {
            var userEntity = GetUserEntityByRole(userRegistrationData);
            userEntity.LastName = userRegistrationData.LastName;
            userEntity.FirstName = userRegistrationData.FirstName;
            userEntity.MiddleName = userRegistrationData.MiddleName;
            userEntity.BirthDate = userRegistrationData.BirthDate;
            userEntity.Gender = (DAL.Entities.Enums.GenderType)(int)userRegistrationData.Gender;
            userEntity.Credentials = new CredentialsEntity
            {
                Username = userRegistrationData.Username,
                PasswordHash = _cryptoService.HashPassword(userRegistrationData.Password)
            };
            return userEntity;
        }
    }
}
