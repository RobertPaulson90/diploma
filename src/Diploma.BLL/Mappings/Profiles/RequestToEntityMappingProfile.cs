using AutoMapper;
using Diploma.BLL.Mappings.Resolvers;
using Diploma.BLL.Queries.Requests;
using Diploma.DAL.Entities;

namespace Diploma.BLL.Mappings.Profiles
{
    internal sealed class RequestToEntityMappingProfile : Profile
    {
        public RequestToEntityMappingProfile()
        {
            CreateMap<UpdateUserDataRequest, UserEntity>(MemberList.Source);

            CreateMap<RegisterCustomerRequest, CustomerEntity>(MemberList.None).ForMember(
                x => x.PasswordHash,
                opt => opt.ResolveUsing<PasswordHashResolver>());
        }
    }
}
