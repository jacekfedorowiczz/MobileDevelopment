using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Interfaces.Base;

namespace MobileDevelopment.API.Persistence.Interfaces
{
    public interface IUserRepository : IBaseEntityRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    }
}
