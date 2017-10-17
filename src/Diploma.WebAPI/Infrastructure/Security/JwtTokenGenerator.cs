using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Diploma.WebAPI.Infrastructure.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.WebAPI.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IRoleManager _roleManager;

        public JwtTokenGenerator(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public string CreateToken(UserEntity user)
        {
            var role = _roleManager.GetUserRole(user);

            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
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
