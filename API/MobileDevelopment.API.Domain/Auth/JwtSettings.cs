namespace MobileDevelopment.API.Domain.Auth
{
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";
        public required string SecretKey { get; set; }
        public string Issuer { get; set; } = "FitTrackerAPI";
        public string Audience { get; set; } = "FitTrackerClient";
        public int AccessTokenExpirationMinutes { get; set; } = 15;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
