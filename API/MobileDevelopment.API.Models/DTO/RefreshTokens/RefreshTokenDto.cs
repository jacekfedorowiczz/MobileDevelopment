namespace MobileDevelopment.API.Models.DTO.RefreshTokens
{
    public sealed record RefreshTokenDto(
        int Id,
        int UserId,
        string Token,
        DateTime ExpiresAt,
        DateTime CreatedAt,
        DateTime? RevokedAt,
        bool IsRevoked,
        bool IsExpired,
        bool IsActive,
        MobileDevelopment.API.Models.DTO.Users.UserDto? User = null
    );

    public sealed record CreateEditRefreshTokenDto(
        int UserId,
        string Token,
        DateTime ExpiresAt
    );
}
