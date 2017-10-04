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
            if (source is CustomerEntity)
            {
                return UserRoleType.Customer;
            }

            if (source is ProgrammerEntity)
            {
                return UserRoleType.Programmer;
            }

            if (source is ManagerEntity)
            {
                return UserRoleType.Manager;
            }

            if (source is AdminEntity)
            {
                return UserRoleType.Admin;
            }

            throw new NotSupportedException();
        }
    }
}