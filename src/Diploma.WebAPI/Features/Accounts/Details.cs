using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.WebAPI.Infrastructure.Contexts;
using Diploma.WebAPI.Infrastructure.Entities;
using Diploma.WebAPI.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Diploma.WebAPI.Features.Accounts
{
    public class Details
    {
        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
        }

        public class Response
        {
            public enum GenderType
            {
                Unspecified,

                Female,

                Male,

                Other
            }

            public int Id { get; set; }

            public string Username { get; set; }

            public string LastName { get; set; }

            public string FirstName { get; set; }

            public string MiddleName { get; set; }

            public GenderType Gender { get; set; }

            public DateTime? BirthDate { get; set; }
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<UserEntity, Response>(MemberList.Destination);
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .Length(5, 30);
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Request, Response>
        {
            private readonly CompanyContext _context;

            private readonly IMapper _mapper;

            public QueryHandler(CompanyContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request message)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }

                var response = _mapper.Map<UserEntity, Response>(user);

                return response;
            }
        }
    }
}