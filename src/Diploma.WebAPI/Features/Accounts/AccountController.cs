using System.Threading.Tasks;
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

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
