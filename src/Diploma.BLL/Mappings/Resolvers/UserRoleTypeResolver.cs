using System;
using AutoMapper;
using Diploma.BLL.Queries.Responses;
using Diploma.DAL.Entities;

namespace Diploma.BLL.Mappings.Resolvers
{
    internal sealed class UserRoleTypeResolver : IValueResolver<UserEntity, UserDataResponse, UserRoleType>
    {
        public UserRoleType Resolve(UserEntity source, UserDataResponse destination, UserRoleType destMember, ResolutionContext context)
        {
            switch (source)
            {
                case CustomerEntity _:
                    return UserRoleType.Customer;
                case ProgrammerEntity _:
                    return UserRoleType.Programmer;
                case ManagerEntity _:
                    return UserRoleType.Manager;
                case AdminEntity _:
                    return UserRoleType.Admin;
            }

            throw new NotSupportedException();
        }
    }
}
