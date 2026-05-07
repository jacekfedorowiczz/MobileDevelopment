using System.Security.Claims;

namespace MobileDevelopment.API.Domain.Interfaces.Auth
{
    public interface IUserContext
    {
        int? UserId { get; }
        string? UserEmail { get; }
        string? UserRole { get; }
        bool IsAuthenticated { get; }
        ClaimsPrincipal? User { get; }
    }
}
