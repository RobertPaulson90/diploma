using JetBrains.Annotations;

namespace Diploma.BLL.Contracts.Services
{
    public interface ICryptoService
    {
        [NotNull]
        string HashPassword([NotNull] string password);

        bool VerifyPasswordHash([NotNull] string password, [NotNull] string hashedPassword);
    }
}
