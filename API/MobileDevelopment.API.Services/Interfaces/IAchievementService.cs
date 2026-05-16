using MobileDevelopment.API.Models.DTO.Achievements;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IAchievementService
    {
        Task<Result<IEnumerable<AchievementDto>>> GetAllAchievementsAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<ProfileAchievementDto>>> GetMyAchievementsAsync(CancellationToken ct = default);
    }
}
