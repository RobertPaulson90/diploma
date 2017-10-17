using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.WebAPI.Infrastructure.Contexts;
using Diploma.WebAPI.Infrastructure.Entities;
using Diploma.WebAPI.Infrastructure.Errors;
using Diploma.WebAPI.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Diploma.WebAPI.Features.Accounts
{
    public class Login
    {
        public class Handler : ICancellableAsyncRequestHandler<Request, Response>
        {
            private readonly CompanyContext _companyContext;

            private readonly IMapper _mapper;

            private readonly IPasswordHasher _passwordHasher;

            public Handler(CompanyContext companyContext, IMapper mapper, IPasswordHasher passwordHasher)
            {
                _companyContext = companyContext;
                _mapper = mapper;
                _passwordHasher = passwordHasher;
            }

            public async Task<Response> Handle(Request message, CancellationToken cancellationToken = default(CancellationToken))
            {
                var username = message.Username.ToUpper();

                var user = await _companyContext.Users.SingleOrDefaultAsync(x => username == x.Username.ToUpper(), cancellationToken)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, "User not found");
                }

                if (!_passwordHasher.VerifyPasswordHash(message.Password, user.PasswordHash))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, "Wrong password");
                }

                var response = _mapper.Map<Response>(user);

                return response;
            }
        }

        public class UserRoleTypeResolver : IValueResolver<UserEntity, Response, UserRole>
        {
            private readonly IRoleManager _roleManager;

            public UserRoleTypeResolver(IRoleManager roleManager)
            {
                _roleManager = roleManager;
            }

            public UserRole Resolve(UserEntity source, Response destination, UserRole destMember, ResolutionContext context)
            {
               return _roleManager.GetUserRole(source);
            }
        }

        public class JwtTokenResolver : IValueResolver<UserEntity, Response, string>
        {
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            
            public JwtTokenResolver(IJwtTokenGenerator jwtTokenGenerator)
            {
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public string Resolve(UserEntity source, Response destination, string destMember, ResolutionContext context)
            {
                return _jwtTokenGenerator.CreateToken(source);
            }
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Request, UserEntity>(MemberList.Destination);

                CreateMap<UserEntity, Response>(MemberList.Destination)
                    .ForMember(x => x.Role, opt => opt.ResolveUsing<UserRoleTypeResolver>())
                    .ForMember(x => x.Token, opt => opt.ResolveUsing<JwtTokenResolver>());
            }
        }

        public class Request : IRequest<Response>
        {
            public string Password { get; set; }

            public string Username { get; set; }
        }

        public class Response
        {
            public string Username { get; set; }

            public UserRole Role { get; set; }

            public string Token { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .Length(5, 30);

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Length(6, -1);
            }
        }
    }
}
