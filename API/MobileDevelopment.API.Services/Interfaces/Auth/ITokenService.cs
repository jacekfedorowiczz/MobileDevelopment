using MobileDevelopment.API.Models.DTO.Auth;
using System.Security.Claims;

namespace MobileDevelopment.API.Services.Interfaces.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(CreateAccessTokenDto dto);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
