using AutoMapper;
using Diploma.BLL.Contracts.DTO;
using Diploma.Common.Mappings.Resolvers;
using Diploma.DAL.Entities;

namespace Diploma.Common.Mappings.Profiles
{
    internal class DtoToEntitiesMappingProfile : Profile
    {
        public DtoToEntitiesMappingProfile()
        {
            CreateMap<UserPersonalInfoDto, UserEntity>(MemberList.Source);

            CreateMap<CustomerRegistrationDataDto, CustomerEntity>(MemberList.None).ForMember(
                x => x.PasswordHash,
                opt => opt.ResolveUsing<PasswordHashResolver>());
        }
    }
}
