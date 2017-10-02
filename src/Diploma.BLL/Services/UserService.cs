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
using GenderType = Diploma.DAL.Entities.Enums.GenderType;

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
                        var userDb = context.Users.Add(userEntity);
                        await context.SaveChangesAsync(cancellationToken);
                        transaction.Commit();

                        var userDto = UserEntityToUserDto(userDb);

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
                        var userDb = credentialsEntity.User;
                        var userDto = UserEntityToUserDto(userDb);
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

        public async Task<OperationResult<UserDto>> UpdateUserAsync(
            UserUpdateRequestDataDto userUpdateRequestData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var context = _companyContextFactory())
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var userDb = await context.Users.SingleAsync(x => x.Id == userUpdateRequestData.Id, cancellationToken);
                        UpdateUserEntityWithData(userDb, userUpdateRequestData);

                        await context.SaveChangesAsync(cancellationToken);
                        transaction.Commit();

                        var userDto = UserEntityToUserDto(userDb);

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

        private static GenderType DtoGenderTypeToEntityGenderType(DTO.Enums.GenderType gender)
        {
            return (GenderType)(int)gender;
        }

        private static DTO.Enums.GenderType EntityGenderTypeToDtoGenderType(GenderType gender)
        {
            return (DTO.Enums.GenderType)(int)gender;
        }

        private static UserEntity GetUserEntityByRole(UserRoleType role)
        {
            UserEntity userEntity;
            switch (role)
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
            var dto = new UserDto
            {
                Username = dbUser.Credentials.Username,
                PasswordHash = dbUser.Credentials.PasswordHash,
                BirthDate = dbUser.BirthDate,
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                MiddleName = dbUser.MiddleName,
                Role = UserEntityToUserRoleType(dbUser),
                Id = dbUser.Id,
                Gender = EntityGenderTypeToDtoGenderType(dbUser.Gender)
            };
            return dto;
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

        [SuppressMessage("ReSharper", "SpecifyStringComparison", Justification = "This must be used explicit cuz LINQ to entities doesn't support string.Equals(...).")]
        private static Expression<Func<CredentialsEntity, bool>> UserNameEquals(string username)
        {
            return x => username.ToUpper() == x.Username.ToUpper();
        }

        private void UpdateUserEntityWithData(UserEntity userDb, UserUpdateRequestDataDto userUpdateRequestData)
        {
            userDb.Gender = DtoGenderTypeToEntityGenderType(userUpdateRequestData.Gender);
            userDb.BirthDate = userUpdateRequestData.BirthDate;
            userDb.FirstName = userUpdateRequestData.FirstName;
            userDb.LastName = userUpdateRequestData.LastName;
            userDb.MiddleName = userUpdateRequestData.MiddleName;
        }

        private UserEntity UserRegistrationDataDtoToUserEntity(UserRegistrationDataDto userRegistrationData)
        {
            var userEntity = new CustomerEntity
            {
                LastName = userRegistrationData.LastName,
                FirstName = userRegistrationData.FirstName,
                MiddleName = userRegistrationData.MiddleName,
                BirthDate = userRegistrationData.BirthDate,
                Gender = (GenderType)(int)userRegistrationData.Gender,
                Credentials = new CredentialsEntity
                {
                    Username = userRegistrationData.Username,
                    PasswordHash = _cryptoService.HashPassword(userRegistrationData.Password)
                }
            };
            return userEntity;
        }
    }
}
