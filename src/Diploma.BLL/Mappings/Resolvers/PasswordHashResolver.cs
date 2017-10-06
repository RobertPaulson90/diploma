using AutoMapper;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Services.Interfaces;
using Diploma.DAL.Entities;

namespace Diploma.BLL.Mappings.Resolvers
{
    internal sealed class PasswordHashResolver : IValueResolver<RegisterCustomerRequest, UserEntity, string>
    {
        private readonly IPasswordHasher _passwordHasher;

        public PasswordHashResolver(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string Resolve(RegisterCustomerRequest source, UserEntity destination, string destMember, ResolutionContext context)
        {
            return _passwordHasher.HashPassword(source.Password);
        }
    }
}
