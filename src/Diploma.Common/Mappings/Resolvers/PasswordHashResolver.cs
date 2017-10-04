using AutoMapper;
using Diploma.BLL.Contracts.DTO;
using Diploma.BLL.Contracts.Services;
using Diploma.DAL.Entities;

namespace Diploma.Common.Mappings.Resolvers
{
    internal sealed class PasswordHashResolver : IValueResolver<CustomerRegistrationDataDto, UserEntity, string>
    {
        private readonly ICryptoService _cryptoService;

        public PasswordHashResolver(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        public string Resolve(CustomerRegistrationDataDto source, UserEntity destination, string destMember, ResolutionContext context)
        {
            return _cryptoService.HashPassword(source.Password);
        }
    }
}