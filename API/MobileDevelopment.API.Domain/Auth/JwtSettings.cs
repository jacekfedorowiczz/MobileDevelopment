namespace MobileDevelopment.API.Domain.Auth
{
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";

        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; } = 30;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
