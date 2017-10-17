using Diploma.WebAPI.Infrastructure.Entities;

namespace Diploma.WebAPI.Infrastructure.Security
{
    public interface IRoleManager
    {
        UserRole GetUserRole(UserEntity user);
    }
}