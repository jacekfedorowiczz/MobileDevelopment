namespace MobileDevelopment.API.Services.Options
{
    public sealed class BackgroundWorkerOptions
    {
        public const string SectionName = "BackgroundWorkers";

        public int TokenCleanupIntervalHours { get; set; } = 12;
        public int RevokedTokenRetentionDays { get; set; } = 1;
        public int AchievementCheckIntervalMinutes { get; set; } = 5;
    }
}
