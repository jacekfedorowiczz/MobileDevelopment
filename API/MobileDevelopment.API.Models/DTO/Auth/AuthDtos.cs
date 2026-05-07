namespace MobileDevelopment.API.Models.DTO.Auth
{
    public sealed record AuthResponse()
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public DateTime AccessTokenExpiration { get; init; }
    }

    public record LoginRequest
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

    public sealed record RefreshTokenRequest()
    {
        public string RefreshToken { get; init; } = string.Empty;
    }

    public sealed record CreateAccessTokenDto(string Id, string Email, string FullName, string MobilePhone, string Role);
}
