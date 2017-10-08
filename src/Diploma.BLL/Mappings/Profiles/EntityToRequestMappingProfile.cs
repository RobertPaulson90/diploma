using AutoMapper;
using Diploma.BLL.Mappings.Resolvers;
using Diploma.BLL.Queries.Responses;
using Diploma.DAL.Entities;

namespace Diploma.BLL.Mappings.Profiles
{
    internal sealed class EntityToRequestMappingProfile : Profile
    {
        public EntityToRequestMappingProfile()
        {
            CreateMap<UserEntity, UserDataResponse>()
                .ForMember(x => x.Role, opt => opt.ResolveUsing<UserRoleTypeResolver>());
        }
    }
}
