using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.BLL.Properties;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.DAL.Contexts;
using Diploma.DAL.Entities;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Handlers
{
    internal sealed class RegisterCustomerRequestHandler : ICancellableAsyncRequestHandler<RegisterCustomerRequest, OperationResult<UserDataResponse>>
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        private readonly IMapper _mapper;

        public RegisterCustomerRequestHandler(Func<CompanyContext> companyContextFactory, IMapper mapper)
        {
            _companyContextFactory = companyContextFactory ?? throw new ArgumentNullException(nameof(companyContextFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResult<UserDataResponse>> Handle(RegisterCustomerRequest message, CancellationToken cancellationToken)
        {
            var customerEntity = _mapper.Map<CustomerEntity>(message);

            using (var context = _companyContextFactory())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var username = message.Username.ToUpper();
                    var isUnique = !await context.Users.AnyAsync(x => username == x.Username.ToUpper(), cancellationToken).ConfigureAwait(false);

                    if (!isUnique)
                    {
                        return OperationResultBuilder.CreateFailure<UserDataResponse>(Resources.Exception_Registration_Username_Already_Taken);
                    }

                    var userDb = context.Users.Add(customerEntity);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    transaction.Commit();

                    var userDto = _mapper.Map<UserDataResponse>(userDb);

                    return OperationResultBuilder.CreateSuccess(userDto);
                }
                catch (TaskCanceledException)
                {
                    transaction.Rollback();
                    return OperationResultBuilder.CreateFailure<UserDataResponse>(Resources.Exception_Registration_Canceled);
                }
            }
        }
    }
}
