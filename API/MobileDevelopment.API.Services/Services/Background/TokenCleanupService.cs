using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Services.Options;

namespace MobileDevelopment.API.Services.Services.Background
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundWorkerOptions _options;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(
            IServiceScopeFactory scopeFactory,
            IOptions<BackgroundWorkerOptions> options,
            ILogger<TokenCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromHours(Math.Max(1, _options.TokenCleanupIntervalHours));
            _logger.LogInformation("Refresh token cleanup service started. Interval: {Interval}.", interval);

            try
            {
                await RunCleanupAsync(stoppingToken);

                using var timer = new PeriodicTimer(interval);
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await RunCleanupAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Refresh token cleanup service stopped.");
            }
        }

        private async Task RunCleanupAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SystemContext>();

                var expiredDate = DateTime.UtcNow;
                var revokedThreshold = DateTime.UtcNow.AddDays(-Math.Max(0, _options.RevokedTokenRetentionDays));

                var deletedCount = await context.Set<RefreshToken>()
                    .Where(t => t.ExpiresAt < expiredDate || (t.RevokedAt != null && t.RevokedAt < revokedThreshold))
                    .ExecuteDeleteAsync(stoppingToken);

                if (deletedCount > 0)
                {
                    _logger.LogInformation("Deleted {Count} expired or revoked refresh tokens.", deletedCount);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Refresh token cleanup service stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cleaning up refresh tokens.");
            }
        }
    }
}
