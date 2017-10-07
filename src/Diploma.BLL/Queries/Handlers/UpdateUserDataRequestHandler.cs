using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.BLL.Properties;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.DAL.Contexts;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Handlers
{
    internal sealed class UpdateUserDataRequestHandler : ICancellableAsyncRequestHandler<UpdateUserDataRequest, OperationResult<UserDataResponse>>
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        private readonly IMapper _mapper;

        public UpdateUserDataRequestHandler(Func<CompanyContext> companyContextFactory, IMapper mapper)
        {
            _companyContextFactory = companyContextFactory ?? throw new ArgumentNullException(nameof(companyContextFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResult<UserDataResponse>> Handle(UpdateUserDataRequest message, CancellationToken cancellationToken)
        {
            using (var context = _companyContextFactory())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var entity = await context.Users.SingleAsync(x => x.Id == message.Id, cancellationToken).ConfigureAwait(false);

                    _mapper.Map(message, entity);

                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    transaction.Commit();

                    var response = _mapper.Map<UserDataResponse>(entity);

                    return OperationResultBuilder.CreateSuccess(response);
                }
                catch (TaskCanceledException)
                {
                    transaction.Rollback();
                    return OperationResultBuilder.CreateFailure<UserDataResponse>(Resources.Exception_Update_Canceled);
                }
            }
        }
    }
}
