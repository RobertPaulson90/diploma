namespace Diploma.WebAPI.Infrastructure.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPasswordHash(string password, string hashedPassword);
    }
}