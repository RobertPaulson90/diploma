using Microsoft.AspNetCore.Http;

namespace Diploma.WebAPI.Infrastructure.Security
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var claims = principal?.Identity;
            var currentUsername = claims?.Name;
            return currentUsername;
        }
    }
}