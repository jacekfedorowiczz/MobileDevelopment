using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Interfaces
{
    public interface IRefreshTokenRepository : IBaseEntityRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken cancellationToken = default);
    }
}
