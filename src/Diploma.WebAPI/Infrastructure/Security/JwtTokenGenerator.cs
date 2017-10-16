using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.WebAPI.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string CreateToken(string username)
        {
            var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, username) };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var encodedJwt = jwtSecurityTokenHandler.WriteToken(jwt);
            return encodedJwt;
        }
    }
}
