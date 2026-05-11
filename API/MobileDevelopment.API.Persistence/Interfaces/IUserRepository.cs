using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Interfaces
{
    public interface IUserRepository : IBaseEntityRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    }
}
