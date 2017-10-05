using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.BLL.Contracts;
using Diploma.BLL.Contracts.DTO;
using Diploma.BLL.Contracts.Services;
using Diploma.BLL.Properties;
using Diploma.DAL.Contexts;
using Diploma.DAL.Entities;

namespace Diploma.BLL.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        private readonly ICryptoService _cryptoService;

        private readonly IMapper _mapper;

        public UserService(Func<CompanyContext> companyContextFactory, ICryptoService cryptoService, IMapper mapper)
        {
            _companyContextFactory = companyContextFactory ?? throw new ArgumentNullException(nameof(companyContextFactory));
            _cryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResult<UserDto>> CreateUserAsync(
            CustomerRegistrationDataDto customerRegistrationData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (customerRegistrationData == null)
            {
                throw new ArgumentNullException(nameof(customerRegistrationData));
            }

            try
            {
                var customerEntity = _mapper.Map<CustomerEntity>(customerRegistrationData);

                using (var context = _companyContextFactory())
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var userDb = context.Users.Add(customerEntity);
                        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        transaction.Commit();

                        var userDto = _mapper.Map<UserDto>(userDb);

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
            UserCredentialsDto userCredentials,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userCredentials == null)
            {
                throw new ArgumentNullException(nameof(userCredentials));
            }

            try
            {
                using (var context = _companyContextFactory())
                {
                    var userDb = await context.Users.AsNoTracking()
                        .SingleOrDefaultAsync(UserNameEquals(userCredentials.Username), cancellationToken).ConfigureAwait(false);

                    if (userDb == null)
                    {
                        return OperationResult<UserDto>.CreateFailure(Resources.Exception_Authorization_Username_Not_Found);
                    }

                    if (!_cryptoService.VerifyPasswordHash(userCredentials.Password, userDb.PasswordHash))
                    {
                        return OperationResult<UserDto>.CreateFailure(Resources.Exception_Authorization_Username_Or_Password_Invalid);
                    }

                    var userDto = _mapper.Map<UserDto>(userDb);
                    return OperationResult<UserDto>.CreateSuccess(userDto);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<UserDto>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<bool>> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(nameof(username));
            }

            try
            {
                using (var context = _companyContextFactory())
                {
                    var isUnique = !await context.Users.AnyAsync(UserNameEquals(username), cancellationToken).ConfigureAwait(false);
                    return OperationResult<bool>.CreateSuccess(isUnique);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<UserDto>> UpdateUserAsync(
            UserPersonalInfoDto userPersonalInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userPersonalInfo == null)
            {
                throw new ArgumentNullException(nameof(userPersonalInfo));
            }

            try
            {
                using (var context = _companyContextFactory())
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var userDb = await context.Users.SingleAsync(x => x.Id == userPersonalInfo.Id, cancellationToken).ConfigureAwait(false);

                        _mapper.Map(userPersonalInfo, userDb);

                        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        transaction.Commit();

                        var userDto = _mapper.Map<UserDto>(userDb);

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

        [SuppressMessage("ReSharper", "SpecifyStringComparison", Justification = "This must be used as is cuz of LINQ to entities.")]
        private static Expression<Func<UserEntity, bool>> UserNameEquals(string username)
        {
            return x => username.ToUpper() == x.Username.ToUpper();
        }
    }
}
