using System;
using AutoMapper;
using Diploma.BLL.Contracts.DTO;
using Diploma.BLL.Contracts.DTO.Enums;
using Diploma.DAL.Entities;

namespace Diploma.Common.Mappings.Resolvers
{
    internal sealed class UserRoleTypeResolver : IValueResolver<UserEntity, UserDto, UserRoleType>
    {
        public UserRoleType Resolve(UserEntity source, UserDto destination, UserRoleType destMember, ResolutionContext context)
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
