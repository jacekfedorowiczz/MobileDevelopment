using MobileDevelopment.API.Domain.Entities;
using System.Security.Claims;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
