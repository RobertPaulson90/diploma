using System.Threading.Tasks;
using Diploma.WebAPI.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.WebAPI.Features.Accounts
{
    [Authorize]
    [Produces("application/json")]
    [Route("api")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        private readonly ICurrentUserAccessor _currentUserAccessor;

        public AccountController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }
        
        [AllowAnonymous]
        [HttpPost("authorize")]
        public Task<Login.Response> Authorize([FromBody] Login.Request request)
        {
            return _mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public Task<Register.Response> Signup([FromBody] Register.Request request)
        {
            return _mediator.Send(request);
        }
        
        [HttpGet("account/details")]
        public Task<Details.Response> GetDetails()
        {
            var request = new Details.Request
            {
                Username = _currentUserAccessor.GetCurrentUsername()
            };

            return _mediator.Send(request);
        }
    }
}
