using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.BLL.Properties;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.DAL.Contexts;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Handlers
{
    internal sealed class GetUserByCredentialsRequestHandler
        : ICancellableAsyncRequestHandler<GetUserByCredentialsRequest, OperationResult<UserDataResponse>>
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        private readonly IMapper _mapper;

        private readonly IPasswordHasher _passwordHasher;

        public GetUserByCredentialsRequestHandler(Func<CompanyContext> companyContextFactory, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _companyContextFactory = companyContextFactory ?? throw new ArgumentNullException(nameof(companyContextFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<OperationResult<UserDataResponse>> Handle(GetUserByCredentialsRequest message, CancellationToken cancellationToken)
        {
            using (var context = _companyContextFactory())
            {
                var username = message.Username.ToUpper();
                var userDb = await context.Users.AsNoTracking()
                    .SingleOrDefaultAsync(x => username == x.Username.ToUpper(), cancellationToken)
                    .ConfigureAwait(false);

                if (userDb == null)
                {
                    return OperationResultBuilder.CreateFailure<UserDataResponse>(Resources.Exception_Authorization_Username_Not_Found);
                }

                if (!_passwordHasher.VerifyPasswordHash(message.Password, userDb.PasswordHash))
                {
                    return OperationResultBuilder.CreateFailure<UserDataResponse>(Resources.Authorization_Message_Validation_Errors);
                }

                var response = _mapper.Map<UserDataResponse>(userDb);
                return OperationResultBuilder.CreateSuccess(response);
            }
        }
    }
}
