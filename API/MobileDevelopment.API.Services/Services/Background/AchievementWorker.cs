using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Services.Options;

namespace MobileDevelopment.API.Services.Services.Background
{
    public class AchievementWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundWorkerOptions _options;
        private readonly ILogger<AchievementWorker> _logger;

        public AchievementWorker(
            IServiceScopeFactory scopeFactory,
            IOptions<BackgroundWorkerOptions> options,
            ILogger<AchievementWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromMinutes(Math.Max(1, _options.AchievementCheckIntervalMinutes));
            _logger.LogInformation("AchievementWorker is starting. Interval: {Interval}.", interval);

            try
            {
                await CheckAchievementsAsync(stoppingToken);

                using var timer = new PeriodicTimer(interval);
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CheckAchievementsAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("AchievementWorker is stopping.");
            }
        }

        private async Task CheckAchievementsAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SystemContext>();

            try
            {
                var profiles = await dbContext.Profiles
                    .Include(p => p.ProfileAchievements)
                    .ToListAsync(stoppingToken);

                if (profiles.Count == 0)
                {
                    return;
                }

                var achievements = await dbContext.Achievements
                    .AsNoTracking()
                    .ToListAsync(stoppingToken);

                if (achievements.Count == 0)
                {
                    return;
                }

                var userIds = profiles.Select(p => p.UserId).Distinct().ToList();
                var workoutCounts = await dbContext.WorkoutSessions
                    .Where(session => userIds.Contains(session.UserId))
                    .GroupBy(session => session.UserId)
                    .Select(group => new { UserId = group.Key, Count = group.Count() })
                    .ToDictionaryAsync(group => group.UserId, group => group.Count, stoppingToken);

                var postCounts = await dbContext.Posts
                    .Where(post => userIds.Contains(post.UserId))
                    .GroupBy(post => post.UserId)
                    .Select(group => new { UserId = group.Key, Count = group.Count() })
                    .ToDictionaryAsync(group => group.UserId, group => group.Count, stoppingToken);

                var now = DateTime.UtcNow;
                var newAchievementsGranted = 0;

                foreach (var profile in profiles)
                {
                    var unlockedIds = profile.ProfileAchievements.Select(pa => pa.AchievementId).ToHashSet();
                    var workoutCount = workoutCounts.GetValueOrDefault(profile.UserId);
                    var postCount = postCounts.GetValueOrDefault(profile.UserId);

                    foreach (var achievement in achievements)
                    {
                        if (unlockedIds.Contains(achievement.Id))
                            continue;

                        var isUnlocked = achievement.AchievementType switch
                        {
                            AchievementType.WorkoutCount => workoutCount >= achievement.TargetValue,
                            AchievementType.PostCount => postCount >= achievement.TargetValue,
                            _ => false
                        };

                        if (!isUnlocked)
                            continue;

                        dbContext.ProfileAchievements.Add(new ProfileAchievement
                        {
                            ProfileId = profile.Id,
                            AchievementId = achievement.Id,
                            UnlockedAt = now
                        });
                        unlockedIds.Add(achievement.Id);
                        newAchievementsGranted++;
                        _logger.LogInformation(
                            "Profile {ProfileId} unlocked achievement '{AchievementName}'",
                            profile.Id, achievement.Name);
                    }
                }

                if (newAchievementsGranted > 0)
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("AchievementWorker granted {Count} new achievements.", newAchievementsGranted);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("AchievementWorker is stopping.");
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                _logger.LogWarning(ex, "AchievementWorker detected duplicate achievement unlock during concurrent execution.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing AchievementWorker.");
            }
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException?.Message.Contains("IX_ProfileAchievements_ProfileId_AchievementId", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
