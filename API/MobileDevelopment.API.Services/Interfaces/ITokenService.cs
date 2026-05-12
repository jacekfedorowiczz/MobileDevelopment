using MobileDevelopment.API.Domain.Entities;
using System.Security.Claims;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface ITokenService
    {
        Task AddRefreshToken(int userId, string refreshToken, CancellationToken cancellationToken = default);
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<bool> RevokeTokenAsync(string token, CancellationToken ct = default);
    }
}
