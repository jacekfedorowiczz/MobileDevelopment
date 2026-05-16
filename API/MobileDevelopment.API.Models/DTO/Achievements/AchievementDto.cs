using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Models.DTO.Achievements
{
    public sealed record AchievementDto(
        int Id,
        string Name,
        string Description,
        string IconCode,
        AchievementType AchievementType,
        int TargetValue
    );

    public sealed record ProfileAchievementDto(
        int Id,
        AchievementDto Achievement,
        DateTime UnlockedAt
    );
}
