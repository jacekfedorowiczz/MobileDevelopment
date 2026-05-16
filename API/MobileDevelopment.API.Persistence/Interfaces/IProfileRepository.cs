using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Interfaces.Base;

namespace MobileDevelopment.API.Persistence.Interfaces
{
    public interface IProfileRepository : IBaseEntityRepository<Profile>
    {
        Task<Profile?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<Profile?> GetByIdWithUserAsync(int id, CancellationToken cancellationToken = default);
        Task<Profile?> GetByUserIdWithUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<Profile?> GetByUserIdWithUserAndAchievementsAsync(int userId, CancellationToken cancellationToken = default);
    }
}
