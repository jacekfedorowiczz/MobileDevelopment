using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.Achievements;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepo;
        private readonly IProfileAchievementRepository _profileAchievementRepo;
        private readonly IProfileRepository _profileRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;

        public AchievementService(
            IAchievementRepository achievementRepo,
            IProfileAchievementRepository profileAchievementRepo,
            IProfileRepository profileRepo,
            IUserContext userContext,
            ICacheService cacheService)
        {
            _achievementRepo = achievementRepo;
            _profileAchievementRepo = profileAchievementRepo;
            _profileRepo = profileRepo;
            _userContext = userContext;
            _cacheService = cacheService;
        }

        public async Task<Result<IEnumerable<AchievementDto>>> GetAllAchievementsAsync(CancellationToken ct = default)
        {
            var achievements = await _cacheService.GetOrSetVersionedAsync(
                "achievements",
                "all",
                async token =>
                {
                    var achievements = await _achievementRepo.GetQueryable().ToListAsync(token);
                    return achievements.Select(a => a.ToDto()).ToList();
                },
                TimeSpan.FromMinutes(60),
                ct);

            return Result<IEnumerable<AchievementDto>>.Success(achievements);
        }

        public async Task<Result<IEnumerable<ProfileAchievementDto>>> GetMyAchievementsAsync(CancellationToken ct = default)
        {
            var userId = _userContext.UserId;
            if (userId is null)
            {
                return Result<IEnumerable<ProfileAchievementDto>>.Failure("Unauthorized");
            }

            var profile = await _profileRepo.GetQueryable()
                .FirstOrDefaultAsync(p => p.UserId == userId, ct);

            if (profile is null)
            {
                return Result<IEnumerable<ProfileAchievementDto>>.Success([]);
            }

            var profileAchievements = await _profileAchievementRepo
                .GetQueryable()
                .Include(pa => pa.Achievement)
                .Where(pa => pa.ProfileId == profile.Id)
                .ToListAsync(ct);

            return Result<IEnumerable<ProfileAchievementDto>>.Success(profileAchievements.Select(pa => pa.ToDto()));
        }
    }
}
