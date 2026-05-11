using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;

namespace MobileDevelopment.API.Services.Services.Background
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<TokenCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cleanup Refresh Tokenów został uruchomiony.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<SystemContext>();

                    // Usuwamy tokeny, które wygasły lub zostały unieważnione 1 dzień temu
                    var expiredDate = DateTime.UtcNow;
                    var revokedThreshold = DateTime.UtcNow.AddDays(-1);

                    var tokensToDelete = await context.Set<RefreshToken>()
                        .Where(t => t.ExpiresAt < expiredDate || (t.RevokedAt != null && t.RevokedAt < revokedThreshold))
                        .ToListAsync(stoppingToken);

                    if (tokensToDelete.Count != 0)
                    {
                        context.Set<RefreshToken>().RemoveRange(tokensToDelete);
                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Usunięto {Count} starych tokenów sesji.", tokensToDelete.Count);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Wystąpił błąd podczas czyszczenia tokenów.");
                }

                // wykonaj ponownie co 12h
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}
