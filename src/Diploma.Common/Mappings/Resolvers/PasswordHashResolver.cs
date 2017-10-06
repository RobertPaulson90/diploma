using AutoMapper;
using Diploma.BLL.Contracts.DTO;
using Diploma.BLL.Contracts.Services;
using Diploma.DAL.Entities;

namespace Diploma.Common.Mappings.Resolvers
{
    internal sealed class PasswordHashResolver : IValueResolver<CustomerRegistrationDataDto, UserEntity, string>
    {
        private readonly IPasswordHasher _passwordHasher;

        public PasswordHashResolver(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string Resolve(CustomerRegistrationDataDto source, UserEntity destination, string destMember, ResolutionContext context)
        {
            return _passwordHasher.HashPassword(source.Password);
        }
    }
}
