namespace Diploma.BLL.Contracts.Services
{
    public interface ICryptoService
    {
        string HashPassword(string password);

        bool VerifyPasswordHash(string password, string hashedPassword);
    }
}
