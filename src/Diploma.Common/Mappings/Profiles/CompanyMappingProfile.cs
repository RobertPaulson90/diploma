using AutoMapper;
using Diploma.BLL.Contracts.DTO;
using Diploma.Common.Mappings.Resolvers;
using Diploma.DAL.Entities;

namespace Diploma.Common.Mappings.Profiles
{
    internal class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<UserEntity, UserDto>()
                .ForMember(x => x.Role, opt => opt.ResolveUsing<UserRoleTypeResolver>());

            CreateMap<UserUpdateRequestDataDto, UserEntity>(MemberList.Source);

            CreateMap<CustomerRegistrationDataDto, CustomerEntity>(MemberList.None)
                .ForMember(x => x.PasswordHash, opt => opt.ResolveUsing<PasswordHashResolver>());
        }
    }
}
