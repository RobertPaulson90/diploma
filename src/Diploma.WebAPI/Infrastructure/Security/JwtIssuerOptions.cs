using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.WebAPI.Infrastructure.Security
{
    public class AuthOptions
    {
        public const string AUDIENCE = "http://localhost:51884/";

        public const string ISSUER = "MyAuthServer";

        public const int LIFETIME = 1 * 60 * 24;

        private const string KEY = "mysupersecret_secretkey!123";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
