using System;
using System.Net;
using System.Text.RegularExpressions;
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
    public class Register
    {
        public class Handler : ICancellableAsyncRequestHandler<Request, Response>
        {
            private readonly CompanyContext _companyContext;

            private readonly IMapper _mapper;

            public Handler(CompanyContext companyContext, IMapper mapper)
            {
                _companyContext = companyContext;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request message, CancellationToken cancellationToken = default(CancellationToken))
            {
                var customerEntity = _mapper.Map<CustomerEntity>(message);

                using (var transaction = await _companyContext.Database.BeginTransactionAsync(cancellationToken)
                    .ConfigureAwait(false))
                {
                    try
                    {
                        var username = message.Username.ToUpper();
                        var userExists = await _companyContext.Users.AnyAsync(x => username == x.Username.ToUpper(), cancellationToken)
                            .ConfigureAwait(false);

                        if (userExists)
                        {
                            throw new RestException(HttpStatusCode.Conflict, "Username already taken");
                        }

                        var user = await _companyContext.Users.AddAsync(customerEntity, cancellationToken)
                            .ConfigureAwait(false);

                        await _companyContext.SaveChangesAsync(cancellationToken)
                            .ConfigureAwait(false);

                        transaction.Commit();

                        var response = _mapper.Map<Response>(user.Entity);

                        return response;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Request, CustomerEntity>(MemberList.Destination)
                    .ForMember(x => x.PasswordHash, opt => opt.ResolveUsing<PasswordHashResolver>());

                CreateMap<UserEntity, Response>(MemberList.Destination);
            }
        }

        public class PasswordHashResolver : IValueResolver<Request, UserEntity, string>
        {
            private readonly IPasswordHasher _passwordHasher;

            public PasswordHashResolver(IPasswordHasher passwordHasher)
            {
                _passwordHasher = passwordHasher;
            }

            public string Resolve(Request source, UserEntity destination, string destMember, ResolutionContext context)
            {
                return _passwordHasher.HashPassword(source.Password);
            }
        }

        public class Request : IRequest<Response>
        {
            public DateTime? BirthDate { get; set; }

            public string FirstName { get; set; }

            public GenderType Gender { get; set; }

            public string LastName { get; set; }

            public string MiddleName { get; set; }

            public string Password { get; set; }

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

            public DateTime? BirthDate { get; set; }

            public string FirstName { get; set; }

            public GenderType? Gender { get; set; }
            
            public string LastName { get; set; }

            public string MiddleName { get; set; }

            public string PasswordHash { get; set; }

            public string Username { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            private static readonly Regex PasswordValidationRegex = new Regex("^[ -~]*$", RegexOptions.Compiled);

            private static readonly Regex UsernameValidationRegex = new Regex("^[a-zA-Z0-9_.-]*$", RegexOptions.Compiled);

            public Validator()
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;

                RuleFor(x => x.FirstName)
                    .NotEmpty();

                RuleFor(x => x.LastName)
                    .NotEmpty();

                RuleFor(x => x.Username)
                    .NotEmpty()
                    .Length(5, 30)
                    .Matches(UsernameValidationRegex);

                RuleFor(x => x.BirthDate)
                    .Must(BeValidBirthDate);

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Length(6, -1)
                    .Matches(PasswordValidationRegex);

                RuleFor(x => x.Gender)
                    .IsInEnum();
            }

            private static bool BeValidBirthDate(DateTime? dateTime)
            {
                if (dateTime == null)
                {
                    return true;
                }

                const int MaximumAge = 131;
                const int MinimumAge = 2;

                var currentDate = DateTime.Today;
                var minimumBirthDate = currentDate.AddYears(-MaximumAge);
                var maximumBirthDate = currentDate.AddYears(-MinimumAge);

                return dateTime >= minimumBirthDate && dateTime < maximumBirthDate;
            }
        }
    }
}
