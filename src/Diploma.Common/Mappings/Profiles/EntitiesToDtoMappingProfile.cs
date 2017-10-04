using AutoMapper;
using Diploma.BLL.Contracts.DTO;
using Diploma.Common.Mappings.Resolvers;
using Diploma.DAL.Entities;

namespace Diploma.Common.Mappings.Profiles
{
    internal class EntitiesToDtoMappingProfile : Profile
    {
        public EntitiesToDtoMappingProfile()
        {
            CreateMap<UserEntity, UserDto>().ForMember(x => x.Role, opt => opt.ResolveUsing<UserRoleTypeResolver>());
        }
    }
}
