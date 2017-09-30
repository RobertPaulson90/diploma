using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Diploma.DAL;
using Diploma.DAL.Entities;
using Diploma.Framework;
using Diploma.Infrastructure;
using Diploma.Models;
using Diploma.Properties;

namespace Diploma.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        private readonly ICryptoService _cryptoService;

        public UserService(ICryptoService cryptoService, Func<CompanyContext> companyContextFactory)
        {
            _cryptoService = cryptoService;
            _companyContextFactory = companyContextFactory;
        }

        public async Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            try
            {
                using (var context = _companyContextFactory())
                {
                    var isUnique = !await context.Users.AnyAsync(x => username.ToUpper() == x.Username.ToUpper(), cancellationToken);
                    return OperationResult<bool>.CreateSuccess(isUnique);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<UserEntity>> SignInAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var context = _companyContextFactory())
                {
                    var user = await context.Users.AsNoTracking().SingleOrDefaultAsync(
                        x => username.ToUpper() == x.Username.ToUpper(),
                        cancellationToken);

                    if (user == null)
                    {
                        return OperationResult<UserEntity>.CreateFailure(Resources.Authorization_Username_Not_Found);
                    }

                    if (_cryptoService.VerifyPasswordHash(password, user.Password))
                    {
                        return OperationResult<UserEntity>.CreateSuccess(user);
                    }
                }

                return OperationResult<UserEntity>.CreateFailure(Resources.Authorization_Username_Or_Password_Invalid);
            }
            catch (Exception ex)
            {
                return OperationResult<UserEntity>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<UserEntity>> CreateCustomerAsync(
            string username,
            string password,
            string lastName,
            string firstName,
            string middleName,
            DateTime? birthDate,
            GenderType? gender,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateUserAsync(username, password, lastName, firstName, middleName, UserRoleType.Customer, birthDate, gender, cancellationToken);
        }
        
        public async Task<OperationResult<UserEntity>> CreateUserAsync(
            string username,
            string password,
            string lastName,
            string firstName,
            string middleName,
            UserRoleType userRole,
            DateTime? birthDate,
            GenderType? gender,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                UserEntity userEntity;
                switch (userRole)
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
                        return OperationResult<UserEntity>.CreateFailure(Resources.Registration_UserRole_Invalid_Value);
                }

                userEntity.LastName = lastName;
                userEntity.FirstName = firstName;
                userEntity.MiddleName = middleName;
                userEntity.BirthDate = birthDate;
                userEntity.Gender = gender;
                userEntity.Username = username;
                userEntity.Password = _cryptoService.HashPassword(password);

                using (var context = _companyContextFactory())
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var dbUser = context.Users.Add(userEntity);
                        await context.SaveChangesAsync(cancellationToken);
                        transaction.Commit();
                        return OperationResult<UserEntity>.CreateSuccess(dbUser);
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
                return OperationResult<UserEntity>.CreateFailure(ex);
            }
        }
    }
}
