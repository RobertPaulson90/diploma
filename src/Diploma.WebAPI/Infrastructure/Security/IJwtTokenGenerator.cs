namespace Diploma.WebAPI.Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(string username);
    }
}
