using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Diploma.BLL.Queries.Requests;
using Diploma.DAL.Contexts;
using Diploma.Infrastructure.Data;
using MediatR;

namespace Diploma.BLL.Queries.Handlers
{
    internal sealed class VerifyUsernameUniqueRequestHandler : ICancellableAsyncRequestHandler<VerifyUsernameUniqueRequest, OperationResult<bool>>
    {
        private readonly Func<CompanyContext> _companyContextFactory;

        public VerifyUsernameUniqueRequestHandler(Func<CompanyContext> companyContextFactory)
        {
            _companyContextFactory = companyContextFactory ?? throw new ArgumentNullException(nameof(companyContextFactory));
        }

        public async Task<OperationResult<bool>> Handle(VerifyUsernameUniqueRequest message, CancellationToken cancellationToken)
        {
            using (var context = _companyContextFactory())
            {
                var username = message.Username.ToUpper();
                var isUnique = !await context.Users.AnyAsync(x => username == x.Username.ToUpper(), cancellationToken)
                    .ConfigureAwait(false);
                return OperationResultBuilder.CreateSuccess(isUnique);
            }
        }
    }
}
