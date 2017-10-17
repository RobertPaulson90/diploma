using System;
using Diploma.WebAPI.Infrastructure.Entities;

namespace Diploma.WebAPI.Infrastructure.Security
{
    public class RoleManager : IRoleManager
    {
        public UserRole GetUserRole(UserEntity user)
        {
            return UserTypeToRole(user);
        }

        private static UserRole UserTypeToRole(UserEntity source)
        {
            switch (source)
            {
                case CustomerEntity _:
                    return UserRole.Customer;
                case ProgrammerEntity _:
                    return UserRole.Programmer;
                case ManagerEntity _:
                    return UserRole.Manager;
                case AdminEntity _:
                    return UserRole.Admin;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
