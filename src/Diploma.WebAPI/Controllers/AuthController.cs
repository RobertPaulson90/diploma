using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Diploma.DAL.Entities;
using Diploma.WebAPI.DTOs;
using Diploma.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthController> _logger;

        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        [ValidateForm]
        [HttpPost("Login")]
        [Route("token")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    return Unauthorized();
                }

                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
                {
                    return Unauthorized();
                }

                var userClaims = await _userManager.GetClaimsAsync(user)
                    .ConfigureAwait(false);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName), new Claim(
                        JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }.Union(userClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityToken:Key"]));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["JwtSecurityToken:Issuer"],
                    _configuration["JwtSecurityToken:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signingCredentials);

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        expiration = jwtSecurityToken.ValidTo
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating token: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error while creating token");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password)
                .ConfigureAwait(false);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }

            return BadRequest(result.Errors);
        }
    }
}
