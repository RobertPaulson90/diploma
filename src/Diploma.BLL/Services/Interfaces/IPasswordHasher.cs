using JetBrains.Annotations;

namespace Diploma.BLL.Services.Interfaces
{
    public interface IPasswordHasher
    {
        [NotNull]
        string HashPassword([NotNull] string password);

        bool VerifyPasswordHash([NotNull] string password, [NotNull] string hashedPassword);
    }
}
