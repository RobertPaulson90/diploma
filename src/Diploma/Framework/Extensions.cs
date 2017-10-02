using System;
using Diploma.BLL.DTO.Enums;
using Diploma.DAL.Entities;

namespace Diploma.Framework
{
    public static class Extensions
    {
        public static string GetUserRole(this UserEntity userEntity)
        {
            if (userEntity is ManagerEntity)
            {
                return UserRoleType.Manager.ToString();
            }

            if (userEntity is ProgrammerEntity)
            {
                return UserRoleType.Programmer.ToString();
            }

            if (userEntity is CustomerEntity)
            {
                return UserRoleType.Customer.ToString();
            }

            if (userEntity is AdminEntity)
            {
                return UserRoleType.Admin.ToString();
            }

            throw new NotSupportedException();
        }
    }
}
