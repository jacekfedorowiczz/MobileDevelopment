using Microsoft.AspNetCore.Http;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using System.Security.Claims;

namespace MobileDevelopment.API.Services.Services.UserContext
{
    public sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser? GetCurrentUser()
        {
            var user = (_httpContextAccessor.HttpContext?.User) ?? throw new InvalidOperationException("Context user is not present.");
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            return new CurrentUser(id, email, roles);
        }

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public int? UserId
        {
            get
            {
                var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(claim, out var id) ? id : null;
            }
        }

        public string? UserEmail => User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? UserRole => User?.FindFirst(ClaimTypes.Role)?.Value;
    }
}
